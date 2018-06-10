using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static TrackWebsite.WSTask;
using System.Data.SQLite;
using System.Threading;
using System.Diagnostics;
using Microsoft.Win32;

namespace TrackWebsite
{
    public partial class MainForm : Form
    {
        List<WSTask> tasks;
        int current;
        DateTime stopTime;
        int wait;
        bool allowToWork;
        Settings set;
        public static string dbFile = "tasks.db";
        SQLiteConnection connection;
        SQLiteCommand command;
        SQLiteDataReader read;
        Dictionary<ResultColors, Color> RColorsDict;
        Dictionary<MoveResultTo, Button> MoveResultButtons;
        DisplayResult disRes;
        FormWindowState wLastState, stateBeforeTray;
        bool isWorking = false;

        bool silentBoot = false;
        
        enum MoveResultTo { First, Last, Previous, Next }
        enum ResultColors { Running = -1, Error, Done, OldUnread, New }
        
        public MainForm(string[] args)
        {
            InitializeComponent();
            lLoadTasks.Parent = DGVTasks;

            dbFile = Application.StartupPath.TrimEnd('\\') + "\\" + dbFile;
            Text = $"{ProductName} v{ProductVersion}";
            tasks = new List<WSTask>();
            disRes = new DisplayResult();
            set = new Settings();
            allowToWork = false;
            current = 0;
            wLastState = stateBeforeTray = WindowState;

            //Обработка аргументов
            foreach (string arg in args)
            {
                switch (arg)
                {
                    case "/back":
                        silentBoot = true;
                        HideToTray();
                        break;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Init();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            if (silentBoot)
            {
                wait = 60;
                cbStartStop.Checked = true;
            }
            else if (set.AutoStart)
            {
                Application.DoEvents();
                wait = 10;
                cbStartStop.Checked = true;
            }
        }

        void Init()
        {
            if (!silentBoot)
            {
                ShowFromTray();
            }
            Application.DoEvents();

            RColorsDict = new Dictionary<ResultColors, Color>();
            RColorsDict.Add(ResultColors.Done, Color.FromArgb(240, 240, 240));
            RColorsDict.Add(ResultColors.Error, Color.FromArgb(251, 160, 164));
            RColorsDict.Add(ResultColors.New, Color.FromArgb(143, 225, 153));
            RColorsDict.Add(ResultColors.OldUnread, Color.Honeydew);
            RColorsDict.Add(ResultColors.Running, Color.FromArgb(253, 246, 160));

            MoveResultButtons = new Dictionary<MoveResultTo, Button>();
            MoveResultButtons.Add(MoveResultTo.First, bFirst);
            MoveResultButtons.Add(MoveResultTo.Last, bLast);
            MoveResultButtons.Add(MoveResultTo.Previous, bPrevious);
            MoveResultButtons.Add(MoveResultTo.Next, bNext);

            PrepareSqlite(out connection, out command, dbFile);

            if (!System.IO.File.Exists(dbFile))
            {
                CreateDB();
            }
            
            LoadSetting();

            LoadTasksFromDB();
            lLoadTasks.Hide();
            FillTaskList();
        }

        public bool SetAutorunValue(bool autorun)
        {
            string name = Application.ProductName;
            string ExePath = Application.ExecutablePath;
            RegistryKey reg;
            reg = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run\\");
            try
            {
                if (autorun)
                {
                    reg.SetValue(name, ExePath + " /back");
                }
                else
                {
                    reg.DeleteValue(name);
                }

                reg.Close();
            }
            catch
            {
                return false;
            }
            return true;
        }

        private void UpdateTasksStatus()
        {
            foreach (WSTask task in tasks)
            {
                UpdateTaskStatus(task.Id);
            }
        }

        private void UpdateTaskStatus(int id)
        {
            string tip = "";
            ResultColors color = GetTaskStatus(id, ref tip);

            int rowId = FindTask(id);
            if (rowId == -1) return;

            if (DGVTasks.Rows[rowId].Cells["progress"].Style.BackColor != GetRealColor(color))
            {
                DGVTasks.Rows[rowId].Cells["progress"].Style.BackColor = GetRealColor(color);
                DGVTasks.Rows[rowId].Cells["progress"].Style.SelectionBackColor = GetRealColor(color);
                UpdateNotifyIconText();
            }
            DGVTasks.Rows[rowId].Cells["progress"].ToolTipText = tip;
        }

        private ResultColors GetTaskStatus(int task_id, ref string tip)
        {
            PrepareSqlite(ref connection, out command);
            tip = "Новых результатов нет";
            ResultColors status = ResultColors.Done;

            command.Parameters.AddWithValue("@tid", task_id);
            // Последний результат не ошибка и непрочитан
            command.CommandText = @"SELECT COUNT(*) FROM records re JOIN result_record rre ON rre.record_id = re.id JOIN results r ON r.id = rre.result_id WHERE r.task_id = @tid AND r.isReaded = 0 AND re.isError = 0 AND r.date = (SELECT MAX(r.date) FROM records re JOIN result_record rre ON rre.record_id = re.id JOIN results r ON r.id = rre.result_id WHERE r.task_id = @tid LIMIT 1)";
            if (ExecuteScalar() != "0")
            {
                tip = "Новый результат";
                status = ResultColors.New;
                return status;
            }
            // Последний результат - ошибка
            command.CommandText = @"SELECT COUNT(*) FROM records re JOIN result_record rre ON rre.record_id = re.id JOIN results r ON r.id = rre.result_id WHERE r.task_id = @tid AND re.isError = 1 AND r.date = (SELECT MAX(r.date) FROM records re JOIN result_record rre ON rre.record_id = re.id JOIN results r ON r.id = rre.result_id WHERE r.task_id = @tid LIMIT 1)";
            if (ExecuteScalar() != "0")
            {
                tip = "Ошибка при выполнении задания";
                status = ResultColors.Error;
                return status;
            }
            // Есть непрочитанные результаты, которые не содержат ошибок
            command.CommandText = @"SELECT COUNT(*) FROM records re JOIN result_record rre ON rre.record_id = re.id JOIN results r ON r.id = rre.result_id WHERE r.task_id = @tid AND r.isReaded = 0 AND re.isError = 0";
            string oldUnread = ExecuteScalar();
            if (oldUnread != "0")
            {
                tip = "Есть старые непрочитанные результаты";
                status = ResultColors.OldUnread;
            }
            return status;
        }

        #region Tasks
        void AddFindAraNews()
        {
            tasks.Add(new WSTask(
                name: "Новости Project Ara",
                link: "http://hi-news.ru/tag/project-ara",
                container: new Element("div", "class", "item type-post"),
                results: new List<Pathway>() {
                    //Заголовок и ссылка 
                    new Pathway(new List<Element>() {
                            new Element("h2"),
                            new Element("a",
                                new List<Data>() {
                                    new Data(),
                                    new Data("href")
                                })
                    }),
                    //Дата
                    new Pathway(new Element("li", new List<Data>() {
                        new Data("class", "lowercase"),
                        new Data()
                    }))
                }
            ));
        }

        void AddFindAraNews2()
        {
            tasks.Add(new WSTask(
                name: "Новости Project Ara 2",
                link: "http://project-ara.net",
                container: new Element("article"),
                results: new List<Pathway>() {
                    //Заголовок и ссылка
                    new Pathway(new List<Element>() {
                            new Element("h2", "class", "post-entry-headline"),
                            new Element("a",
                                new List<Data>() {
                                    new Data(),
                                    new Data("href")
                                })
                    }),
                    //Дата
                    new Pathway(new Element("span", new List<Data>() {
                                    new Data("class", "post-info-date"),
                                    new Data()
                                })
                    )
                }
            ));
        }

        void AddFindZNONews()
        {
            tasks.Add(new WSTask(
                name: "Новости ЗНО",
                link: "http://testportal.gov.ua",
                container: new Element("td", "id", "page_content"), //Check
                results: new List<Pathway>() {
                    //Заголовок
                    new Pathway(new Element("h3", new Data())),
                    //Ссылка
                    new Pathway(new List<Element>() {
                        new Element("div"),                         //Check
                        new Element("a", "href")                    //Get
                    })
                }
            ));
        }

        void AddLostFilmNews()
        {
            tasks.Add(new WSTask(
                name: "Новости LostFilm",
                link: "https://www.lostfilm.tv/browse.php",
                container: new Element("div", "class", "content_body"),
                results: new List<Pathway>() {
                    //Заголовок
                    new Pathway(new Element("span", new Data("Подозреваемый", HowSearch.Contains, DataType.Get))),
                    //Ссылка
                    //new Result(new Element("a"), new Data("href"), true)
                }
            ));
        }

        void AddCBookWait()
        {
            tasks.Add(new WSTask(
                name: "Ожидание книги по C#",
                link: "http://nnm-club.me/?q=C%23+6.0.+%D1%EF%F0%E0%E2%EE%F7%ED%E8%EA&w=title",
                container: new Element("td", "width", "80%"),
                result: new Pathway(new List<Element>() {
                    new Element("table", new List<Data>() { new Data("class", "pline"), new Data("width", "100%") }, EDistance.Any),
                    new Element("h2", new Data("Полное описание языка", HowSearch.Contains, DataType.Get)),
                    new Element("a", "href")
                })
            ));
        }

        void AddGFNewPost()
        {
            tasks.Add(new WSTask(
                name: "Новости группы GF",
                link: "https://vk.com/gravityfallsrus",
                container: new Element("div", "id", "page_wall_posts"),
                results: new List<Pathway>() {
                    new Pathway(new List<Element>() {
                        new Element("div", "class", "post_table"),
                        new Element("div", new List<Data>() { new Data("id", "wpt"), new Data("id") }),
                        new Element("div", new List<Data>() { new Data("class", "wall_post_text"), new Data() })
                    }),
                    new Pathway(new List<Element>() {
                        new Element("div", "class", "post_table", EDistance.Any),
                        new Element("div", new List<Data>() { new Data("id", "wpt", HowSearch.StartsWith), new Data("id") }),
                        new Element("div", new List<Data>() { new Data("class", "wall_post_text", 0, DataType.Get), new Data("#Cipher_time", HowSearch.StartsWith) })
                    })
                }
            ));
        }

        void AddPOINewSiries()
        {
            tasks.Add(new WSTask(
                name: "Новая серия POI",
                link: "http://kinoyad.ru/boeviki/716-v-pole-zreniya-2016-5-sezon.html",
                container: new Element("div", "id", "adv2"),
                result: new Pathway(new Element("li", new Data(), EDistance.Last))
            ));
        }

        void AddNarutoNewSiries()
        {
            tasks.Add(new WSTask(
                name: "Новая серия Naruto",
                link: "http://hokage.tv",
                container: new Element("td", "valign", "top", EDistance.Num2),
                result: new Pathway(new List<Element>() {
                    new Element("div", new Data("id", "entryID", HowSearch.StartsWith), EDistance.Any),
                    new Element("div", "class", "narhoknews"),
                    new Element("a", new List<Data>() { new Data("Naruto", HowSearch.Contains), new Data(), new Data("href") })
                })
            ));
        }

        void AddAlloVmeste()
        {
            tasks.Add(new WSTask(
                name: "Алло покупаем вместе",
                link: "http://vmeste.allo.ua",
                container: new Element("div", "class", "cw"),
                results: new List<Pathway>() {
                    new Pathway(new List<Element>() {
                        new Element("h2", new Data()),
                        new Element("a", new Data("href"))
                    }),
                    new Pathway(new List<Element>() {
                        new Element("div", new Data("class", "progress", HowSearch.StartsWith)),
                        new Element("div", new Data("class", "status", HowSearch.StartsWith), EDistance.Last),
                        new Element("h4", new Data())
                    }),
                    new Pathway(new List<Element>() {
                        new Element("div", "class", "price"),
                        new Element("h4", new Data())
                    }),
                    new Pathway(new Element("div", new List<Data>() { new Data("class", "time-left"), new Data() }))
                }
            ));
        }

        void AddGetPriorityStatus()
        {
            tasks.Add(new WSTask(
                name: "ИПЗ в ХНУРЭ",
                link: "http://vstup.info/2016/92/i2016i92p301233.html#list",
                container: new Element("tbody", EDistance.Num3),
                result: new Pathway(new List<Element>() {
                    new Element("tr", new List<Data>() { new Data("title"), new Data() }, EDistance.Any),
                    new Element("td", new Data("Кошовий М. Ю.", HowSearch.ExactMatch), EDistance.Num2),
                    //new Element("td", EDistance.First)
                })
            ));
        }
        #endregion

        #region Sqlite
        public static void PrepareSqlite(ref SQLiteConnection connection, out SQLiteCommand command)
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Cancel();
                connection.Close();
            }
            connection = (SQLiteConnection)connection.Clone();
            command = connection.CreateCommand();
        }

        public static void PrepareSqlite(out SQLiteConnection connection, out SQLiteCommand command, string dbName)
        {
            connection = new SQLiteConnection("Data Source = " + dbName);
            PrepareSqlite(ref connection, out command);
        }

        private void CreateDB()
        {
            SQLiteConnection.CreateFile(dbFile);
            PrepareSqlite(ref connection, out command);
            command.CommandText = @"
                    CREATE TABLE tasks (
                        id integer PRIMARY KEY AUTOINCREMENT NOT NULL,
                        name TEXT NOT NULL,
                        link TEXT NOT NULL,
                        ContainerEl_id int,
                        period DATETIME NOT NULL,
                        lastDone DATETIME
                    );
                    CREATE TABLE pathways (
                        id integer PRIMARY KEY AUTOINCREMENT NOT NULL,
                        task_id INT NOT NULL,
                        isFromCont INT NOT NULL DEFAULT 1
                    );
                    CREATE TABLE elements (
                        id integer PRIMARY KEY AUTOINCREMENT NOT NULL,
                        pathway_id INT,
                        tag TEXT,
                        ED INT NOT NULL DEFAULT 0, 
                        EL INT NUT NULL DEFAULT 0
                    );
                    CREATE TABLE data (
                        id integer PRIMARY KEY AUTOINCREMENT NOT NULL,
                        element_id INT NOT NULL,
                        prop TEXT,
                        value TEXT, 
                        type INT NOT NULL DEFAULT 0,
                        howS INT NOT NULL DEFAULT 0
                    );
                    CREATE TABLE results (
                        id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                        task_id INT NOT NULL,
                        isReaded INT NOT NULL DEFAULT 0,
                        isFavorite INT NOT NULL DEFAULT 0,
                        date DATETIME DEFAULT CURRENT_TIMESTAMP
                    );
                    CREATE TABLE records (
                        id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                        data_id INT NOT NULL,
                        isError INT NOT NULL DEFAULT 0,
                        text TEXT
                    );
                    CREATE TABLE result_record (
                        result_id INT NOT NULL,
                        record_id INT NOT NULL
                    );
                    CREATE TABLE settings (
                        name TEXT PRIMARY KEY NOT NULL,
                        value TEXT NOT NULL
                    );";

            ExecuteNonQuery();
        }

        private void LoadSetting()
        {
            PrepareSqlite(ref connection, out command);
            command.CommandText = "SELECT name, value FROM settings";
            connection.Open();
            read = command.ExecuteReader();
            while (read.Read())
            {
                switch (read.GetString(0))
                {
                    case "AutoStart":
                        set.AutoStart = Convert.ToBoolean(read.GetString(1));
                        break;
                    case "LoadWithWindows":
                        set.LoadWithWindows = Convert.ToBoolean(read.GetString(1));
                        break;
                    case "PauseBeforeLoop":
                        set.PauseBeforeLoop = int.Parse(read.GetString(1));
                        break;
                    case "PauseBetweenTasks":
                        set.PauseBetweenTasks = int.Parse(read.GetString(1));
                        break;
                }
            }
            connection.Close();

            numRest.Value = set.PauseBeforeLoop;
            numTaskChange.Value = set.PauseBetweenTasks;
            cbAutoGo.Checked = set.AutoStart;
            cbLoadWithWin.Checked = set.LoadWithWindows;
            SetAutorunValue(set.LoadWithWindows);
        }

        private void SaveSettings()
        {
            set.PauseBeforeLoop = (int)numRest.Value;
            set.PauseBetweenTasks = (int)numTaskChange.Value;
            set.AutoStart = cbAutoGo.Checked;
            set.LoadWithWindows = cbLoadWithWin.Checked;
            SetAutorunValue(set.LoadWithWindows);

            PrepareSqlite(ref connection, out command);
            SaveValue("AutoStart", set.AutoStart.ToString());
            SaveValue("LoadWithWindows", set.LoadWithWindows.ToString());
            SaveValue("PauseBeforeLoop", set.PauseBeforeLoop);
            SaveValue("PauseBetweenTasks", set.PauseBetweenTasks);
        }

        void SaveValue(string name, object value)
        {
            command.Parameters.AddWithValue("@n", name);
            command.Parameters.AddWithValue("@v", value);
            command.CommandText = "SELECT COUNT(*) FROM settings WHERE name = @n";
            connection.Open();
            int count = Convert.ToInt32(command.ExecuteScalar());
            connection.Close();
            if (count == 0)
            {
                command.CommandText = "INSERT INTO settings VALUES (@n, @v)";
            }
            else
            {
                command.CommandText = "UPDATE settings SET value = @v WHERE name = @n";
            }
            ExecuteNonQuery();
        }

        private void LoadTasksFromDB()
        {
            List<WSTask> tasks = new List<WSTask>();
            PrepareSqlite(ref connection, out command);

            command.CommandText = "SELECT id FROM tasks";
            connection.Open();
            read = command.ExecuteReader();
            while (read.Read())
            {
                WSTask task = new WSTask();
                task.Id = read.GetInt32(0);

                tasks.Add(task);
            }
            connection.Close();

            for (int i = 0; i < tasks.Count; i++)
            {
                tasks[i] = LoadTaskFromDB(tasks[i].Id, dbFile);
            }

            this.tasks = tasks;
        }
        
        #region GetSetTaskData

        #region LoadTask
        public static WSTask LoadTaskFromDB(int id, string dbName)
        {
            SQLiteConnection connection;
            SQLiteCommand command;

            bool isExists = false;

            WSTask task = new WSTask();
            task.Id = id;

            //Загрузка информации о задании
            PrepareSqlite(out connection, out command, dbName);
            command.Parameters.AddWithValue("@tid", task.Id);

            command.CommandText = "SELECT name, link, period, lastDone, ContainerEl_id FROM tasks WHERE id = @tid";
            connection.Open();
            SQLiteDataReader read = command.ExecuteReader();
            while (read.Read())
            {
                isExists = true;

                task.Name = read.GetString(0);
                task.Link = read.GetString(1);
                if (!read.IsDBNull(4))
                {
                    task.Container = new Element();
                    task.Container.Id = read.GetInt32(4);
                }
                //добавить период и дату последнего выполнения
            }
            connection.Close();

            //Загрузка контейнера
            if (task.Container != null)
            {
                task.Container = LoadElementFromDB(task.Container.Id, dbName);
            }

            //Загрузка путей
            PrepareSqlite(ref connection, out command);
            command.Parameters.AddWithValue("@tid", task.Id);

            command.CommandText = "SELECT id FROM pathways WHERE task_id = @tid";
            connection.Open();
            read = command.ExecuteReader();
            while (read.Read())
            {
                Pathway path = new Pathway();
                path.Id = read.GetInt32(0);

                task.Pathways.Add(path);
            }
            connection.Close();

            for (int i = 0; i < task.Pathways.Count; i++)
            {
                task.Pathways[i] = LoadPathwayFromDB(task.Pathways[i].Id, dbName);
            }

            return isExists ? task : null;
        }

        private static Pathway LoadPathwayFromDB(int id, string dbName)
        {
            SQLiteConnection connection;
            SQLiteCommand command;

            Pathway way = new Pathway();
            way.Id = id;

            //Загрузка данный о пути
            PrepareSqlite(out connection, out command, dbName);
            command.Parameters.AddWithValue("pid", way.Id);

            command.CommandText = "SELECT isFromCont FROM pathways WHERE id = @pid";
            connection.Open();
            SQLiteDataReader read = command.ExecuteReader();
            while (read.Read())
            {
                way.isGetDataFromContainer = read.GetBoolean(0);
            }
            connection.Close();

            //Загрузка элементов пути
            PrepareSqlite(ref connection, out command);
            command.Parameters.AddWithValue("pid", way.Id);

            command.CommandText = "SELECT id FROM elements WHERE pathway_id = @pid";
            connection.Open();
            read = command.ExecuteReader();
            while (read.Read())
            {
                Element element = new Element();
                element.Id = read.GetInt32(0);

                way.elements.Add(element);
            }
            connection.Close();

            for (int i = 0; i < way.elements.Count; i++)
            {
                way.elements[i] = LoadElementFromDB(way.elements[i].Id, dbName);
            }

            return way;
        }

        private static Element LoadElementFromDB(int id, string dbName)
        {
            SQLiteConnection connection;
            SQLiteCommand command;

            bool isExists = false;

            Element element = new Element();
            element.Id = id;

            //Загрузка данных об элементе
            PrepareSqlite(out connection, out command, dbName);
            command.Parameters.AddWithValue("eid", element.Id);

            command.CommandText = "SELECT tag, ED, EL FROM elements WHERE id = @eid";
            connection.Open();
            SQLiteDataReader read = command.ExecuteReader();
            while (read.Read())
            {
                isExists = true;

                if (!read.IsDBNull(0))
                {
                    element.Tag = read.GetString(0);
                }
                element.WhichOne = (EDistance)read.GetInt32(1);
                element.Where = (ELocation)read.GetInt32(2);
            }
            connection.Close();

            //Загрузка данных элемента
            PrepareSqlite(ref connection, out command);
            command.Parameters.AddWithValue("eid", element.Id);

            command.CommandText = "SELECT id, prop, value, type, howS FROM data WHERE element_id = @eid";
            connection.Open();
            read = command.ExecuteReader();
            while (read.Read())
            {
                Data data = new Data();
                data.Id = read.GetInt32(0);
                if (!read.IsDBNull(1))
                {
                    data.Prop = read.GetString(1);
                }
                if (!read.IsDBNull(2))
                {
                    data.Value = read.GetString(2);
                }
                data.Type = (DataType)read.GetInt32(3);
                data.howSearch = (HowSearch)read.GetInt32(4);

                element.Data.Add(data);
            }
            connection.Close();

            for (int i = 0; i < element.Data.Count; i++)
            {
                element.Data[i] = LoadDataFromDB(element.Data[i].Id, dbName);
            }

            return isExists ? element : null;
        }

        private static Data LoadDataFromDB(int id, string dbName)
        {
            SQLiteConnection connection;
            SQLiteCommand command;

            Data data = new Data();
            data.Id = id;

            //Загрузка данных о Data
            PrepareSqlite(out connection, out command, dbName);
            command.Parameters.AddWithValue("did", data.Id);

            command.CommandText = "SELECT prop, value, type, howS FROM data WHERE id = @did";
            connection.Open();
            SQLiteDataReader read = command.ExecuteReader();
            while (read.Read())
            {
                if (!read.IsDBNull(0))
                {
                    data.Prop = read.GetString(0);
                }
                if (!read.IsDBNull(1))
                {
                    data.Value = read.GetString(1);
                }
                data.Type = (DataType)read.GetInt32(2);
                data.howSearch = (HowSearch)read.GetInt32(3);
            }
            connection.Close();

            return data;
        }
        #endregion

        #region SaveTask
        public static int SaveTask(WSTask task)
        {
            SQLiteConnection connection;
            SQLiteCommand command;

            int t_id = task.Id, cont_id = -1;
            PrepareSqlite(out connection, out command, dbFile);
            
            //Обновление контейнера
            if (task.Container != null)
            {
                cont_id = SaveElement(task.Container);
            }

            // Проверка, если ли уже такой элемент в базе данных
            command.CommandText = "SELECT COUNT(*) FROM tasks WHERE id = " + t_id;
            string isExists = ExecuteScalar(connection, command);

            command.Parameters.AddWithValue("@name", task.Name);
            command.Parameters.AddWithValue("@link", task.Link);
            command.Parameters.AddWithValue("@cont_id", cont_id == -1 ? null : cont_id.ToString());
            command.Parameters.AddWithValue("@period", new TimeSpan(1, 0, 0, 0));
            command.Parameters.AddWithValue("@lastDone", null);

            if (isExists == "0")
            {
                command.CommandText = $"INSERT INTO tasks (name, link, ContainerEl_id, period, lastDone) VALUES (@name, @link, @cont_id, @period, @lastDone);SELECT last_insert_rowid();";
                t_id = int.Parse(ExecuteScalar(connection, command));
            }
            else
            {
                command.CommandText = "UPDATE tasks SET name = @name, link = @link, ContainerEl_id = @cont_id, period = @period, lastDone = @lastDone WHERE id = " + t_id;
                ExecuteNonQuery(connection, command);
            }

            foreach (Pathway path in task.Pathways)
            {
                SavePath(path, t_id);
            }

            return t_id;
        }

        public static int SavePath(Pathway path, int t_id)
        {
            SQLiteConnection connection;
            SQLiteCommand command;

            int p_id = path.Id;
            PrepareSqlite(out connection, out command, dbFile);

            // Проверка, если ли уже такой элемент в базе данных
            command.CommandText = "SELECT COUNT(*) FROM pathways WHERE id = " + p_id;
            string isExists = ExecuteScalar(connection, command);

            //Добавление или обновление элемента
            command.Parameters.AddWithValue("@tid", t_id);
            command.Parameters.AddWithValue("@isfc", path.isGetDataFromContainer);

            if (isExists == "0")
            {
                command.CommandText = "INSERT INTO pathways (task_id, isFromCont) VALUES (@tid, @isfc);SELECT last_insert_rowid();";
                p_id = int.Parse(ExecuteScalar(connection, command));
            }
            else
            {
                command.CommandText = "UPDATE pathways SET task_id = @tid, isFromCont = @isfc WHERE id = " + p_id;
                ExecuteNonQuery(connection, command);
            }

            foreach (Element el in path.elements)
            {
                SaveElement(el, p_id);
            }

            return p_id;
        }

        public static int SaveElement(Element el, int p_id = -1)
        {
            SQLiteConnection connection;
            SQLiteCommand command;

            int e_id = el.Id;
            PrepareSqlite(out connection, out command, dbFile);

            // Проверка, если ли уже такой элемент в базе данных
            command.CommandText = "SELECT COUNT(*) FROM elements WHERE id = " + e_id;
            string isExists = ExecuteScalar(connection, command);

            //Добавление или обновление элемента
            command.Parameters.AddWithValue("@pid", p_id == -1 ? null : p_id.ToString());
            command.Parameters.AddWithValue("@tag", el.Tag);
            command.Parameters.AddWithValue("@ed", el.WhichOne);
            command.Parameters.AddWithValue("@el", el.Where);

            if (isExists == "0")
            {
                command.CommandText = "INSERT INTO elements (pathway_id, tag, ED, EL) VALUES (@pid, @tag, @ed, @el); SELECT last_insert_rowid();";
                e_id = int.Parse(ExecuteScalar(connection, command));
            }
            else
            {
                command.CommandText = "UPDATE elements SET pathway_id = @pid, tag = @tag, ED = @ed, EL = @el WHERE id = " + e_id;
                ExecuteNonQuery(connection, command);
            }

            foreach (Data data in el.Data)
            {
                SaveData(data, e_id);
            }

            return e_id;
        }

        public static int SaveData(Data data, int e_id)
        {
            SQLiteConnection connection;
            SQLiteCommand command;

            int d_id = data.Id;
            PrepareSqlite(out connection, out command, dbFile);

            //Проверка, если ли уже такой элемент в базе данных
            command.CommandText = "SELECT COUNT(*) FROM data WHERE id = " + d_id;
            string isExists = ExecuteScalar(connection, command);

            //Добавление или обновление элемента
            command.Parameters.AddWithValue("@eid", e_id);
            command.Parameters.AddWithValue("@prop", data.Prop);
            command.Parameters.AddWithValue("@val", data.Value);
            command.Parameters.AddWithValue("@type", data.Type);
            command.Parameters.AddWithValue("@howS", data.howSearch);

            if (isExists == "0")
            {
                command.CommandText = "INSERT INTO data (element_id, prop, value, type, howS) VALUES (@eid, @prop, @val, @type, @howS); SELECT last_insert_rowid();";
                d_id = int.Parse(ExecuteScalar(connection, command));
            }
            else
            {
                command.CommandText = "UPDATE data SET element_id = @eid, prop = @prop, value = @val, type = @type, howS = @howS WHERE id = " + d_id;
                ExecuteNonQuery(connection, command);
            }

            return d_id;
        }
        #endregion

        #region SaveTaskResult
        int UploadResults(WSTask task, out bool isNeedInsert)
        {
            string res_id = "-1";

            bool isNew = false; isNeedInsert = false;
            List<string> IDs = new List<string>();

            if (task.Container != null)
            {
                bool isNeedInsertOld = isNeedInsert;
                isNew = UploadElementResult(task.Container, ref IDs, out isNeedInsert);
                isNeedInsert = isNeedInsert || isNeedInsertOld;
            }
            foreach (Pathway way in task.Pathways)
            {
                foreach (Element el in way.elements)
                {
                    bool isNeedInsertOld = isNeedInsert;
                    isNew = isNew | UploadElementResult(el, ref IDs, out isNeedInsert);
                    isNeedInsert = isNeedInsert || isNeedInsertOld;
                }
            }

            if (isNeedInsert)
            {
                command.Parameters.AddWithValue("@tid", task.Id.ToString());
                command.CommandText = "INSERT INTO results (task_id) VALUES (@tid); SELECT last_insert_rowid();";
                res_id = ExecuteScalar();
                command.Parameters.Clear();
                command.CommandText = "INSERT INTO result_record VALUES (@rid, @reid)";
                foreach (string reid in IDs.Where(x => x != null))
                {
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@rid", res_id);
                    command.Parameters.AddWithValue("@reid", reid);
                    ExecuteNonQuery();
                }
            }
            return isNew ? int.Parse(res_id) : -1;
        }

        bool UploadElementResult(Element element, ref List<string> record_ids, out bool isNeedInsert)
        {
            bool isNew = false;
            isNeedInsert = false;

            PrepareSqlite(ref connection, out command);
            foreach (Data data in element.Data)
            {
                if (data.GetRealType() == DataType.Get || data.Type == DataType.Get)
                {
                    string id;
                    bool isNeedInsertOld = isNeedInsert;
                    isNew = UploadDataResult(data, out id, out isNeedInsert);
                    isNeedInsert = isNeedInsert || isNeedInsertOld;
                    record_ids.Add(id);
                }
            }
            return isNew;
        }

        bool UploadDataResult(Data data, out string record_id, out bool isNeedInsert)
        {
            bool isNew = false;
            isNeedInsert = false;
            record_id = null;
            string result = (data.result.HasValue) ? data.result.Value : data.result.Error;

            if (result == null) return false;

            command.Parameters.AddWithValue("@did", data.Id);
            command.Parameters.AddWithValue("@ise", !data.result.HasValue);
            command.Parameters.AddWithValue("@text", result);

            command.CommandText = "SELECT id FROM records WHERE data_id = @did AND text = @text AND isError = @ise LIMIT 1;";
            string isExists = ExecuteScalar();

            if (isExists == null)
            {
                isNew = isNeedInsert = true;
                command.CommandText = "INSERT INTO records (data_id, isError, text) VALUES(@did, @ise, @text); SELECT last_insert_rowid();";
                record_id = ExecuteScalar();
            }
            else
            {
                
                string cmd = "SELECT re.text FROM records re JOIN result_record rre ON re.id = rre.record_id JOIN results r ON r.id = rre.result_id WHERE re.data_id = @did{0} ORDER BY r.date DESC LIMIT 1";
                command.CommandText = string.Format(cmd, "");
                string last = ExecuteScalar();

                isNew = isNeedInsert = last != result;

                string where = "";
                if (data.result.HasValue)
                {
                    where = " AND re.isError = 0";
                    command.CommandText = string.Format(cmd, where);
                    string lastNotError = ExecuteScalar();
                    isNew = lastNotError != result;
                }
                record_id = isExists;
            }
            command.Parameters.Clear();
            return isNew;
        }
        #endregion

        #region LoadTaskResult
        private void LoadResult(int result_id)
        {
            disRes = new DisplayResult();
            disRes.Id = result_id;

            PrepareSqlite(ref connection, out command);
            command.Parameters.AddWithValue("@rid", result_id);

            //Проверяем, существует ли результат
            command.CommandText = "SELECT date FROM results WHERE id = @rid";
            connection.Open();
            object date = command.ExecuteScalar();
            connection.Close();
            if (date == null)
            {
                //Загрузка информации о задании
                disRes.Task_id = GetTaskIdFromGrid();
                command.CommandText = "SELECT name FROM tasks WHERE id = " + disRes.Task_id;
                disRes.TaskName = ExecuteScalar();

                disRes.Total = 0;
                disRes.Place = 0;

                ShowResult();
                return;
            }
            else
            {
                //Загрузка информации о задании
                command.CommandText = "SELECT t.id, t.name FROM tasks t JOIN results r ON r.task_id = t.id WHERE r.id = @rid";
                connection.Open();
                read = command.ExecuteReader();
                while (read.Read())
                {
                    disRes.Task_id = read.GetInt32(0);
                    disRes.TaskName = read.GetString(1);
                }
                connection.Close();
            }

            PrepareSqlite(ref connection, out command);
            command.Parameters.AddWithValue("@rid", result_id);
            command.Parameters.AddWithValue("@tid", disRes.Task_id);
            command.Parameters.AddWithValue("@date", (DateTime)date);

            //Загрузка места результата
            command.CommandText = "SELECT COUNT(*) FROM results WHERE task_id = @tid";
            disRes.Total = int.Parse(ExecuteScalar());
            command.CommandText = "SELECT COUNT(*) + 1 FROM results WHERE task_id = @tid AND date < @date";
            disRes.Place = int.Parse(ExecuteScalar());

            //Загрузка результата
            command.CommandText = "SELECT text, isError, r.id, r.isReaded, r.date FROM records re JOIN result_record rre ON rre.record_id = re.id JOIN results r ON r.id = rre.result_id WHERE r.id = @rid GROUP BY data_id ORDER BY re.data_id";
            connection.Open();
            read = command.ExecuteReader();
            while (read.Read())
            {
                disRes.IsRead = Convert.ToBoolean(read.GetInt32(3));
                disRes.Id = read.GetInt32(2);
                disRes.Date = read.GetDateTime(4);

                disRes.Text += read.GetString(0) + Environment.NewLine;
            }
            connection.Close();

            ShowResult();
        }
        #endregion

        #endregion

        int IsThereIsUnread()
        {
            PrepareSqlite(ref connection, out command);
            command.CommandText = "SELECT COUNT(*) FROM results WHERE isReaded = 0";
            string count = ExecuteScalar();
            return int.Parse(count);
        }

        void MarkRead(int r_id, bool isReaded = true)
        {
            PrepareSqlite(ref connection, out command);

            command.CommandText = "UPDATE results SET isReaded = @isread WHERE id = @id;";
            command.Parameters.AddWithValue("@isread", isReaded);
            command.Parameters.AddWithValue("@id", r_id);
            ExecuteNonQuery();
        }

        /// <summary>
        /// Обновляет все результаты, к которым можно переместиться
        /// </summary>
        void UpdateMoveResults()
        {
            UpdateMoveResults(MoveResultTo.First, MoveResultTo.Last, MoveResultTo.Next, MoveResultTo.Previous);
        }

        /// <summary>
        /// Обновляет выбранные результаты, из тех, к которым можно переместиться
        /// </summary>
        void UpdateMoveResults(params MoveResultTo[] move)
        {
            foreach (var moveto in move)
            {
                int dt = LoadMoveResultId(disRes.Task_id, disRes.Id, moveto);
                if (dt == -1)
                {
                    MoveResultButtons[moveto].Enabled = false;
                    MoveResultButtons[moveto].Tag = null;
                }
                else
                {
                    MoveResultButtons[moveto].Enabled = true;
                    MoveResultButtons[moveto].Tag = dt;
                }
            }
        }

        int LoadMoveResultId(int task_id, int result_id, MoveResultTo move)
        {
            PrepareSqlite(ref connection, out command);
            command.Parameters.AddWithValue("@tid", task_id);
            command.Parameters.AddWithValue("@rid", result_id);
            
            switch (move)
            {
                case MoveResultTo.First: command.CommandText = $"SELECT id FROM results WHERE task_id = @tid AND date = (SELECT MIN(date) FROM results WHERE task_id = @tid) LIMIT 1;"; break;
                case MoveResultTo.Last: command.CommandText = $"SELECT id FROM results WHERE task_id = @tid AND date = (SELECT MAX(date) FROM results WHERE task_id = @tid) LIMIT 1;"; break;
                case MoveResultTo.Previous: command.CommandText = $"SELECT id FROM results WHERE task_id = @tid AND date < (SELECT date FROM results WHERE id = @rid) ORDER BY date DESC LIMIT 1;"; break;
                case MoveResultTo.Next: command.CommandText = $"SELECT id FROM results WHERE task_id = @tid AND date > (SELECT date FROM results WHERE id = @rid) ORDER BY date LIMIT 1;"; break;
                default: return -1;
            }
            connection.Open();
            object dt = command.ExecuteScalar();
            connection.Close();

            if (dt == null)
            {
                return -1;
            }
            else
            {
                return Convert.ToInt32(dt);
            }
        }

        private int GetTaskIdFromGrid()
        {
            if (DGVTasks.SelectedRows.Count != 0)
            {
                return int.Parse(DGVTasks.SelectedRows[0].Cells["Id"].Value.ToString());
            }
            else if (current >= 0 && current < tasks.Count)
            {
                return tasks[current].Id;
            }
            else
            {
                return -1;
            }
        }

        void ShowResult()
        {
            bIsNew.Enabled = false;
            lTaskName.Text = disRes.TaskName;
            tbResult.Text = disRes.Text;
            lDate.Text = disRes.Date.ToString();
            lResultNum.Text = string.Format(lResultNum.Tag.ToString(), disRes.Place, disRes.Total);
            SetIsRead(disRes.IsRead);
            bIsNew.Visible = disRes.Place > 0;
            bIsNew.Enabled = true;


            UpdateMoveResults(MoveResultTo.Next, MoveResultTo.Previous);
        }

        string ExecuteScalar()
        {
            return ExecuteScalar(connection, command);
        }

        public static string ExecuteScalar(SQLiteConnection connection, SQLiteCommand command)
        {
            connection.Open();
            object result = command.ExecuteScalar();
            connection.Close();
            if (result == null)
            {
                return null;
            }
            else
            {
                return result.ToString();
            }
        }

        int ExecuteNonQuery()
        {
            return ExecuteNonQuery(connection, command);
        }

        public static int ExecuteNonQuery(SQLiteConnection connection, SQLiteCommand command)
        {
            connection.Open();
            int result = command.ExecuteNonQuery();
            connection.Close();
            return result;
        }
        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            Stopwatch sw = new Stopwatch();
            sw.Reset();
            SaveTask(tasks[0]);
            sw.Stop();
            MessageBox.Show(sw.ElapsedMilliseconds.ToString());
        }

        private void cbStartStop_CheckedChanged(object sender, EventArgs e)
        {
            cbStartStop.Enabled = false;
            if (cbStartStop.Checked)
            {
                allowToWork = true;
                cbStartStop.BackColor = Color.LightGreen;
                cbStartStop.Text = "Stop";
                cbStartStop.Enabled = true;
                if (wait == 0)
                {
                    current = 0;
                    Go(current++);
                    wait = (int)numTaskChange.Value;
                }
                StartTimer();
            }
            else
            {
                allowToWork = false;
                wait = 0;
            }
        }

        private void MoveResult_Click(object sender, EventArgs e)
        {
            object r_id = (sender as Button).Tag;
            if (r_id == null)
            {
                UpdateMoveResults();
                LoadResult(-1);
            }
            else
            {
                LoadResult((int)r_id);
                UpdateMoveResults(MoveResultTo.Previous, MoveResultTo.Next);
            }
        }

        private void DGVTasks_SelectionChanged(object sender, EventArgs e)
        {
            ChangeTaskDisplayResult(GetTaskIdFromGrid());
        }

        private void ChangeTaskDisplayResult(int task_id)
        {
            disRes.Task_id = task_id;
            UpdateMoveResults();
            object r_id = MoveResultButtons[MoveResultTo.Last].Tag;
            LoadResult(r_id == null ? -1 : (int)r_id);
        }

        private void MenuItemChangeTask_Click(object sender, EventArgs e)
        {
            int id = GetTaskIdFromGrid();
            EditTask(id);
        }

        void EditTask(int id)
        {
            bool isRun = cbStartStop.Checked;
            cbStartStop.Enabled = false;
            cbStartStop.Checked = false;
            Hide();

            EditTask et = new EditTask(id);
            et.ShowDialog();

            Show();
            UpdateTaskList();

            cbStartStop.Enabled = true;
            cbStartStop.Checked = isRun;
        }

        private void UpdateTaskList()
        {
            LoadTasksFromDB();
            FillTaskList();
        }

        private void bAddNewTask_Click(object sender, EventArgs e)
        {
            StartASettings();
            CreateNewTask();
        }

        private void MenuItemAddNewTask_Click(object sender, EventArgs e)
        {
            CreateNewTask();
        }

        private void miAddTaskByBase_Click(object sender, EventArgs e)
        {
            int id = GetTaskIdFromGrid();
            CreateNewTask(id);
        }

        private void CreateNewTask(int id = -1)
        {
            bool isRun = cbStartStop.Checked;
            cbStartStop.Enabled = false;
            cbStartStop.Checked = false;
            Hide();

            EditTask et;
            if (id == -1)
            {
                et = new EditTask();
            }
            else
            {
                et = new EditTask(id, true);
            }
            et.ShowDialog();

            Show();
            UpdateTaskList();

            cbStartStop.Enabled = true;
            cbStartStop.Checked = isRun;
        }

        private void MenuItemDeleteTask_Click(object sender, EventArgs e)
        {
            DeleteTaskById(GetTaskIdFromGrid());
        }

        private void DeleteTaskById(int id)
        {
            command.CommandText = "DELETE FROM tasks WHERE id = " + id;
            ExecuteNonQuery();

            UpdateTaskList();
        }

        private void DGVTasks_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
            {
                DGVTasks.Rows[e.RowIndex].Selected = true;
                if (e.Button == MouseButtons.Right)
                {
                    cmTask.Show(Cursor.Position);
                }
            }
        }

        private void cmTask_Opening(object sender, CancelEventArgs e)
        {
            miRunTask.Enabled = (wait == 0 && !isWorking);
        }

        private void bAddTask_Click(object sender, EventArgs e)
        {
            CreateNewTask();
        }

        #region Other boring stuff
        private void webBrowser1_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            if (webBrowser1.Url == null) return;

            tbAdress.Text = webBrowser1.Url.AbsoluteUri;
        }

        private void webBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            tbAdress.Text = webBrowser1.Url.AbsoluteUri;
        }

        void FillTaskList()
        {
            int selected_id = GetTaskIdFromGrid();
            DGVTasks.Rows.Clear();
            foreach (WSTask task in tasks)
            {
                DGVTasks.Rows.Add(task.Id, task.Name);
                UpdateTaskStatus(task.Id);
                Application.DoEvents();
            }
            bAddTask.Visible = DGVTasks.RowCount == 0;

            for (int i = 0; i < DGVTasks.RowCount; i++)
            {
                if (int.Parse(DGVTasks.Rows[i].Cells["Id"].Value.ToString()) == selected_id)
                {
                    DGVTasks.Rows[i].Selected = true;
                    break;
                }
            }
        }

        private void bIsNew_Click(object sender, EventArgs e)
        {
            if (bIsNew.ForeColor == Color.ForestGreen)
            {
                SetResultReadState(disRes.Id, true);
            }
            else
            {
                SetResultReadState(disRes.Id, false);
            }
        }

        void SetResultReadState(int rid, bool isRead)
        {
            bIsNew.Enabled = false;
            SetIsRead(isRead);
            MarkRead(rid, isRead);
            bIsNew.Enabled = true;

            UpdateTaskStatus(disRes.Task_id);
        }

        private void SetIsRead(bool isRead)
        {
            if (isRead)
            {
                bIsNew.Text = "Просмотрен";
                bIsNew.ForeColor = Color.Black;
            }
            else
            {
                bIsNew.Text = "Новый";
                bIsNew.ForeColor = Color.ForestGreen;
            }
        }
        #endregion

        #region Timer and work
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (allowToWork == false)
            {
                StopTimer();
                cbStartStop.BackColor = Color.Transparent;
                cbStartStop.Text = "Start";
                cbStartStop.Enabled = true;
                return;
            }
            TimeSpan ts = (new TimeSpan(0, 0, wait) - (DateTime.Now - stopTime));
            lProgress.Text = lProgress.Tag + ts.Minutes.ToString("00") + ":" + ts.Seconds.ToString("00");
            if (ts.TotalSeconds <= 0)
            {
                StopTimer();
                Go(current++);
                wait = (int)numTaskChange.Value;
                if (current >= tasks.Count)
                {
                    current = 0;
                    wait = (int)numRest.Value * 60;
                }
                StartTimer();
            }
        }

        void StartTimer()
        {
            stopTime = DateTime.Now;
            label1.Enabled = false;
            lProgress.Text = lProgress.Tag.ToString();
            lProgress.Visible = true;
            timerBreak.Start();
        }

        void StopTimer()
        {
            timerBreak.Stop();
            wait = 0;
            lProgress.Visible = false;
            label1.Enabled = true;
        }

        void Go(int task_num)
        {
            isWorking = true;
            if (task_num >= 0 && task_num < tasks.Count)
            {
                TaskSetColor(tasks[task_num].Id, ResultColors.Running, "Выполняется");
                bool isOk = PrepareToDoWork(tasks[task_num].Link, webBrowser1);
                if (isOk)
                {
                    isOk = tasks[task_num].DoWork(webBrowser1.Document);
                    if (isOk)
                    {
                        bool isNeedInsert;
                        int r_id = UploadResults(tasks[task_num], out isNeedInsert);

                        UpdateTaskStatus(tasks[task_num].Id);
                        if (isNeedInsert)
                        {
                            SetTaskInGrid(tasks[task_num].Id);
                            if (r_id > 0)
                            {
                                string nl = Environment.NewLine;
                                SetNotification(tasks[task_num].Name, tbResult.Text.Trim(Environment.NewLine.ToCharArray()), ToolTipIcon.Info, new NotifyTag(NotifyTag.NotifyType.NewResult, tasks[task_num].Id, r_id));
                            }
                        }
                    }
                    else
                    {
                        TaskSetColor(tasks[task_num].Id, ResultColors.Error, "Непредвиденная ошибка при выполнении задания");
                    }
                }
                else
                {
                    TaskSetColor(tasks[task_num].Id, ResultColors.Error, "Не удалось получить доступ к сайту");
                }
            }
            isWorking = false;
        }

        private void SetTaskInGrid(int id)
        {
            for (int r = 0; r < DGVTasks.RowCount; r++)
            {
                if (int.Parse(DGVTasks.Rows[r].Cells["Id"].Value.ToString()) == id)
                {
                    DGVTasks.SelectionChanged -= DGVTasks_SelectionChanged;
                    DGVTasks.Rows[r].Selected = true;
                    DGVTasks.SelectionChanged += DGVTasks_SelectionChanged;
                    ChangeTaskDisplayResult(id);
                }
            }
        }

        private void TaskSetColor(int id, ResultColors color, string tip = null)
        {
            int row = FindTask(id);

            int colNow = (int)GetResColor(DGVTasks.Rows[row].Cells["progress"].Style.BackColor);
            int colComing = (int)color;
            int colBefore = Convert.ToInt32(DGVTasks.Rows[row].Cells["progress"].Tag);
            string tipNow = DGVTasks.Rows[row].Cells["progress"].ToolTipText;

            if (colComing < 0 || colNow < colComing || colNow < 0)
            {
                if (colNow < 0 && colComing >= 0 && (colBefore >= 10 || colComing >= 10) && colComing <= colBefore)
                {
                    color = (ResultColors)colBefore;
                    tip = tipNow;
                }
                Color realColor = GetRealColor(color);
                DGVTasks.Rows[row].Cells["progress"].Tag = GetResColor(DGVTasks.Rows[row].Cells["progress"].Style.BackColor);
                DGVTasks.Rows[row].Cells["progress"].Style.BackColor =
                DGVTasks.Rows[row].Cells["progress"].Style.SelectionBackColor = realColor;
                DGVTasks.Rows[row].Cells["progress"].ToolTipText = tip;
            }
        }

        Color GetRealColor(ResultColors color)
        {
            Color col = RColorsDict[color];
            return col;
        }

        ResultColors GetResColor(Color color)
        {
            foreach (var record in RColorsDict)
            {
                if (record.Value.Equals(color))
                {
                    return record.Key;
                }
            }
            return ResultColors.Done;
        }

        int FindTask(int id)
        {
            for (int i = 0; i < DGVTasks.RowCount; i++)
            {
                if (DGVTasks.Rows[i].Cells["id"].Value.ToString() == id.ToString())
                {
                    return i;
                }
            }
            return -1;
        }

        public static bool PrepareToDoWork(string link, WebBrowser browser)
        {
            HtmlDocument document;
            //Проверяем есть ли интерет и доступен ли сайт
            if (!isInternetConnected(link))
            {
                return false;
            }
            //Открываем ссылку
            try
            {
                OpenUri(browser, link);
                document = browser.Document;

                if (document.Domain == null)
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
        #endregion

        #region Settings Animation
        private void bSet_Click(object sender, EventArgs e)
        {
            bSet.Enabled = false;
            StartASettings();
        }

        Thread us;
        int ChangeUS = -60;
        private void ASettings()
        {
            if (ChangeUS > 0)
            {
                Invoke((Action)(() => { bSet.Enabled = false; pSet.Left = ClientRectangle.Width; pSet.Visible = true; }));
            }
            int target = ClientRectangle.Width - pSet.Width;
            do
            {
                Invoke((Action)(() =>
                {
                    pSet.Left -= ChangeUS;
                }));
                Thread.Sleep(30);
            } while (pSet.Left - ChangeUS > target && pSet.Left - ChangeUS < ClientRectangle.Width);

            if (ChangeUS > 0)
            {
                Invoke((Action)(() => { pSet.Left = target; }));
            }
            else
            {
                Invoke((Action)(() => { pSet.Left = ClientRectangle.Width; pSet.Visible = false; bSet.Enabled = true; }));
            }
        }

        private void bCancelSet_Click(object sender, EventArgs e)
        {
            StartASettings();
        }

        void StartASettings()
        {
            ChangeUS = -ChangeUS;
            if (us == null || !us.IsAlive)
            {
                us = new Thread(new ThreadStart(ASettings));
                us.IsBackground = true;
                us.Start();
            }
        }

        private void bSaveSet_Click(object sender, EventArgs e)
        {
            if (ChangeUS > 0)
            {
                ChangeUS = -ChangeUS;
            }
            if (us == null || !us.IsAlive)
            {
                us = new Thread(new ThreadStart(ASettings));
                us.IsBackground = true;
                us.Start();
            }
            SaveSettings();
        }

        private void bReloadSet_Click(object sender, EventArgs e)
        {
            bReloadSet.Enabled = false;
            LoadSetting();
            bReloadSet.Enabled = true;
        }
        #endregion

        #region Tray
        struct NotifyTag
        {
            public enum NotifyType { None, NewResult, Error }

            public NotifyType Type;
            public int Task_id;
            public int Result_id;

            public NotifyTag(NotifyType type, int tid, int rid)
            {
                Type = type;
                Task_id = tid;
                Result_id = rid;
            }
        }

        protected override void OnClientSizeChanged(EventArgs e)
        {
            base.OnClientSizeChanged(e);
            if (WindowState != wLastState)
            {
                MainForm_OnWindowStateChanged(e);
                wLastState = WindowState;
            }
        }

        void HideToTray()
        {
            stateBeforeTray = wLastState;
            WindowState = FormWindowState.Minimized;
            ShowInTaskbar = false;
            notifyIcon1.Visible = true;

            if (Visible)
            {
                Hide();
            }
        }

        void ShowFromTray()
        {
            if (!Visible)
            {
                Show();
            }

            ShowInTaskbar = true;
            WindowState = stateBeforeTray;
            //notifyIcon1.Visible = false;
            Activate();
        }

        void SetNotification(string title, string text, ToolTipIcon icon = ToolTipIcon.None, NotifyTag tag = new NotifyTag(), int time = 7000)
        {
            notifyIcon1.SetBalloonTipTag(tag);
            notifyIcon1.ShowBalloonTip(time, title, text, icon);
        }

        void UpdateNotifyIconText(string text = null)
        {
            string nl = Environment.NewLine;
            if (text == null)
            {
                text = Text + nl;

                int newCount = 0, errorCount = 0;
                foreach (DataGridViewRow row in DGVTasks.Rows)
                {
                    Color color = row.Cells["progress"].Style.BackColor;
                    if (color == GetRealColor(ResultColors.New))
                    {
                        newCount++;
                    }
                    else if (color == GetRealColor(ResultColors.Error))
                    {
                        errorCount++;
                    }
                }

                text += $"Новых результатов: {newCount}, Ошибок: {errorCount}";
            }
            notifyIcon1.Text = text;
        }

        private void MainForm_OnWindowStateChanged(EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                HideToTray();
            }
        }

        private void contextMenuTray_Opening(object sender, CancelEventArgs e)
        {
            MenuItemStartStop.Enabled = cbStartStop.Enabled;
            if (cbStartStop.Checked)
            {
                MenuItemStartStop.Text = "Stop";
            }
            else
            {
                MenuItemStartStop.Text = "Start";
            }

            if (WindowState == FormWindowState.Minimized)
            {
                открытьОкноToolStripMenuItem.Text = "Открыть окно";
            }
            else
            {
                открытьОкноToolStripMenuItem.Text = "Свернуть окно";
            }
        }
        
        private void MainForm_Activated(object sender, EventArgs e)
        {
            Application.DoEvents();
            if (WindowState != FormWindowState.Minimized && !ShowInTaskbar)
            {
                ShowFromTray();
            }
        }

        private void notifyIcon1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (WindowState == FormWindowState.Minimized)
                {
                    ShowFromTray();
                }
                else
                {
                    HideToTray();
                }
            }
        }

        private void открытьОкноToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                ShowFromTray();
            }
            else
            {
                HideToTray();
            }
        }

        private void MenuItemStartStop_Click(object sender, EventArgs e)
        {
            cbStartStop.Checked = !cbStartStop.Checked;
        }

        private void закрытьПрограммуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void выполнитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (wait == 0 && !isWorking)
            {
                if (DGVTasks.SelectedRows.Count == 0) return;

                int num = DGVTasks.SelectedRows[0].Index;
                Go(num);
            }
        }

        private void notifyIcon1_BalloonTipClicked(object sender, EventArgs e)
        {
            ShowFromTray();

            object tag = notifyIcon1.GetBalloonTipTag();
            if (tag == null)
            {
                return;
            }
            NotifyTag nTag = (NotifyTag)tag;
            switch (nTag.Type)
            {
                case NotifyTag.NotifyType.NewResult:
                {
                    SetTaskInGrid(nTag.Task_id);
                    SetResultReadState(nTag.Result_id, true);
                    break;
                }
            }
        }
        #endregion
    }

    public class Settings
    {
        public int PauseBetweenTasks { get; set; } = 10;
        public int PauseBeforeLoop { get; set; } = 60;
        public bool AutoStart { get; set; } = false;
        public bool LoadWithWindows { get; set; } = false;
    }

    public struct DisplayResult
    {
        public int Id { get; set; }
        public int Task_id { get; set; }
        public DateTime Date { get; set; }
        public bool IsRead { get; set; }
        public string Text { get; set; }
        public string TaskName { get; set; }
        public int Total { get; set; }
        public int Place { get; set; }
    }

    #region Extension methods
    static class MyBalloonTipTag
    {
        public static object Tag { get; set; }

        public static object GetBalloonTipTag(this NotifyIcon element)
        {
            return Tag;
        }

        public static void SetBalloonTipTag(this NotifyIcon element, object tag)
        {
            Tag = tag;
        }
    }
    #endregion
}
