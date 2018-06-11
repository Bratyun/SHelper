using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using static TrackWebsite.WSTask;

namespace TrackWebsite
{
    public partial class EditTask : Form
    {
        delegate void DUpdateData();
        struct StepLink
        {
            public string Name;
            public string PanelName;
            public DUpdateData LoadId;

            public StepLink(string Name, string PName, DUpdateData howLoad = null)
            {
                this.Name = Name;
                PanelName = PName;
                LoadId = howLoad;
            }
        }

        public WSTask task;
        Panel currentPanel;
        StepLink[] stepLinks;

        SQLiteConnection connection;
        SQLiteCommand command;
        private const string DefaultName = "Редактирование задачи";
        bool isBigSteps = true;
        int MinimumWidth;

        public EditTask(int task_id = -1, bool isBaseToNew = false)
        {
            InitializeComponent();
            task = MainForm.LoadTaskFromDB(task_id, MainForm.dbFile);
            if (task != null && isBaseToNew)
            {
                task.Name += " - Копия";
                task.Link = "";
                DropTaskId(task);
            }
            DialogResult = DialogResult.None;
        }

        private void EditTask_Load(object sender, EventArgs e)
        {
            int vertScrollWidth = SystemInformation.VerticalScrollBarWidth;
            tableData.Padding = new Padding(0, 0, vertScrollWidth, 0);
            
            Init();
        }

        private void Init()
        {
            //Проверка есть ли задание
            if (task == null)
            {
                task = new WSTask();
                task.Id = -1;
            }

            //Подключаемся в БД
            MainForm.PrepareSqlite(out connection, out command, MainForm.dbFile);

            //Формируем список панелей и отображаем основные шаги
            stepLinks = new StepLink[]
            {
                new StepLink("Шаг 1-2: Ссылка и контейнер", "pStep1", new DUpdateData(UpdateTaskInfo)),
                new StepLink("Шаг 3: Пути", "pPathways", new DUpdateData(UpdatePathways)),
                new StepLink("Шаг 4: Проверка", "pFinish"),

                new StepLink("Редактор пути", "pPath", new DUpdateData(UpdatePath)),
                new StepLink("Редактор элемента", "pElement", new DUpdateData(UpdateElement))
            };
            for (int i = 0; i < 3; i++)
            {
                tableSmallStepButtons.Controls[i].Tag = stepLinks[i].PanelName;
                treeSteps.Nodes.Add(stepLinks[i].PanelName, stepLinks[i].Name);
            }

            if (task.Id != -1)
            {
                RefreshPathways();
            }
            
            ShowStepMenu(false, false);

            if (isBigSteps)
            {
                treeSteps.SelectedNode = treeSteps.Nodes[0];
            }
            else
            {
                (tableSmallStepButtons.Controls[0] as RadioButton).Checked = true;
            }
        }

        private void RefreshPathways()
        {
            if (!isBigSteps) return;

            //Отображаем пути
            treeSteps.BeginUpdate();

            while (treeSteps.Nodes[1].GetNodeCount(false) > 0)
            {
                treeSteps.Nodes[1].Nodes[0].Remove();
            }
            for (int i = 0; i < task.Pathways.Count; i++)
            {
                treeSteps.Nodes[1].Nodes.Add(task.Pathways[i].Id.ToString(), $"Путь {i + 1}");
            }
            treeSteps.Nodes[1].Expand();

            treeSteps.EndUpdate();
        }

        private void ReloadTask()
        {
            if (currentPanel != null)
            {
                ShowPanel(currentPanel.Name, currentPanel.Tag);
            }
        }
        
        private void DropTaskId(WSTask task)
        {
            int minP = -1, minE = -1, minD = -1;

            task.Id = -1;
            if (task.Container != null)
            {
                DropElementId(task.Container, minE--, ref minD);
            }
            for (int p = 0; p < task.Pathways.Count; p++)
            {
                task.Pathways[p].Id = minP--;
                for (int e = 0; e < task.Pathways[p].elements.Count; e++)
                {
                    DropElementId(task.Pathways[p].elements[e], minE--, ref minD);
                }
            }
        }

        void DropElementId(Element el, int minE, ref int minD)
        {
            el.Id = minE;
            for (int i = 0; i < el.Data.Count; i++)
            {
                el.Data[i].Id = minD--;
            }
        }

        #region StepMenu
        private bool ChangeStep(string name, string id)
        {
            //Проверка, можно ли менять шаг
            if (currentPanel != null && currentPanel.Name == "pElement" && !SaveElementInTask())
            {
                return false;
            }
            //Смена шага
            ShowPanel(name, int.Parse(id));

            return true;
        }

        private void ShowStepMenu(bool isBig, bool isVisible = true)
        {
            if (isBig == isBigSteps) return;
            isBigSteps = isBig;
            
            if (us != null && us.IsAlive)
            {
                us.Abort();
            }
            us = new Thread(new ParameterizedThreadStart(AStepMenu));
            us.IsBackground = true;
            if (isBigSteps)
            {
                us.Start(new object[] { tableSmallSteps, tableBigSteps, isVisible });
            }
            else
            {
                us.Start(new object[] { tableBigSteps, tableSmallSteps, isVisible });
            }
        }

        Thread us;
        int ChangeUS = 10;
        private void AStepMenu(object controls)
        {
            Control[] targets = new Control[] { (Control)(controls as object[])[0], (Control)(controls as object[])[1] };
            bool isAnimate = (bool)(controls as object[])[2];

            Invoke((Action)(() =>
            {
                SizeChanged -= EditTask_SizeChanged;

                tableMain.SuspendLayout();
                pStepMenu.MaximumSize = new Size(targets.Select(c => c.MaximumSize.Width).Max(), pStepMenu.MaximumSize.Height);
                pStepMenu.AutoSize = false;
                pStepMenu.Width = pStepMenu.MaximumSize.Width;
                if (isAnimate)
                {
                    tableMain.PerformLayout();
                    tableMain.ResumeLayout(true);
                }
            }));

            while (targets[0].Width - ChangeUS > 0 && isAnimate)
            {
                Invoke((Action)(() =>
                {
                    targets[0].Width -= ChangeUS;
                }));
                Thread.Sleep(5);
            }
            
            Invoke((Action)(() =>
            {
                targets[0].Width = 0;
                targets[0].Dock = DockStyle.None;
                targets[1].BringToFront();
                targets[1].Dock = DockStyle.Left;
                targets[1].Show();
                targets[0].Hide();
            }));

            while (targets[1].Width + ChangeUS < targets[1].MaximumSize.Width && isAnimate)
            {
                Invoke((Action)(() =>
                {
                    targets[1].Width += ChangeUS;
                }));
                Thread.Sleep(5);
            }

            Invoke((Action)(() => {
                targets[1].Width = targets[1].MaximumSize.Width;

                pStepMenu.AutoSize = true;
                pStepMenu.MaximumSize = new Size(0, pStepMenu.MaximumSize.Height);

                if (!isAnimate)
                {
                    tableMain.PerformLayout();
                    tableMain.ResumeLayout(true);
                }

                int maxMinWidth = pSteps.Controls[0].MinimumSize.Width;
                for (int i = 1; i < pSteps.Controls.Count; i++)
                {
                    if (pSteps.Controls[i].MinimumSize.Width > maxMinWidth)
                    {
                        maxMinWidth = pSteps.Controls[i].MinimumSize.Width;
                    }
                }
                MinimumSize = new Size(0, MinimumSize.Height);
                MinimumWidth = pStepMenu.Width + maxMinWidth + (Width - ClientRectangle.Width);
                if (Width < MinimumWidth)
                {
                    Width = MinimumWidth;
                }

                SizeChanged += EditTask_SizeChanged;
            }));
        }

        private void UpdateBackNextStepBig(TreeNode node = null)
        {
            if (node == null)
            {
                node = treeSteps.SelectedNode;
            }
            int step = node.Index;

            if (node.Level > 0)
            {
                step = node.Parent.Index;
            }

            bBackB.Enabled = step > 0;
            bNextB.Enabled = step < treeSteps.Nodes.Count - 1;
        }

        private void UpdateBackNextStepSmall()
        {
            int step = GetCurSamllStep();

            bPrevStepS.Enabled = step > 0;
            bNextStepS.Enabled = step < tableSmallStepButtons.Controls.Count - 1;
        }

        int GetCurSamllStep()
        {
            for (int i = 0; i < tableSmallStepButtons.Controls.Count; i++)
            {
                if ((tableSmallStepButtons.Controls[i] as RadioButton).Checked)
                {
                    return i;
                }
            }
            return -1;
        }

        void SetSmallStep(int step, bool checkOnly = false)
        {
            RadioButton rb = (tableSmallStepButtons.Controls[step] as RadioButton);
            SetCheck(rb, checkOnly);
        }

        void SetCheck(RadioButton rb, bool checkOnly = false)
        {
            if (checkOnly) rb.CheckedChanged -= rbStep_CheckedChanged;
            rb.Checked = true;
            if (checkOnly) rb.CheckedChanged += rbStep_CheckedChanged;
        }

        private void ChangeStepBig_Click(object sender, EventArgs e)
        {
            //Меняем шаг
            Button button = (sender as Button);
            int step = treeSteps.SelectedNode.Index + int.Parse(button.Tag.ToString());

            if (step >= 0 && step < treeSteps.Nodes.Count)
            {
                treeSteps.SelectedNode = treeSteps.Nodes[step];
            }

            currentPanel.Focus();
        }

        private void ChangeStepSmall_Click(object sender, EventArgs e)
        {
            //Меняем шаг
            Button button = (sender as Button);
            int step = GetCurSamllStep() + int.Parse(button.Tag.ToString());

            if (step >= 0 && step < tableSmallStepButtons.Controls.Count)
            {
                SetSmallStep(step);
            }
        }

        private void bToBigSteps_Click(object sender, EventArgs e)
        {
            int index = GetCurSamllStep();
            treeSteps.BeforeSelect -= treeSteps_BeforeSelect;
            treeSteps.SelectedNode = treeSteps.Nodes[index];
            treeSteps.BeforeSelect += treeSteps_BeforeSelect;
            UpdateBackNextStepBig();

            ShowStepMenu(true);
        }

        private void bToSmallSteps_Click(object sender, EventArgs e)
        {
            int index = (treeSteps.SelectedNode.Level == 0) ? treeSteps.SelectedNode.Index : treeSteps.SelectedNode.Parent.Index;
            SetSmallStep(index);
            UpdateBackNextStepSmall();
            
            ShowStepMenu(false);
        }

        private void button3_EnabledChanged(object sender, EventArgs e)
        {
            Button b = (sender as Button);
            if (b.Enabled)
            {
                b.BackColor = Color.FromArgb(138, 235, 230);
            }
            else
            {
                b.BackColor = Color.FromArgb(190, 240, 240);
            }
        }

        private void treeSteps_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            string data = e.Node.Name;
            string name, id = "0";
            if (e.Node.Level == 0)
            {
                name = data;
            }
            else
            {
                name = e.Node.Parent.Name;
                switch (name)
                {
                    case "pPathways": name = "pPath"; break;
                }
                id = data;
            }

            if (ChangeStep(name, id))
            {
                UpdateBackNextStepBig(e.Node);
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void rbStep_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (sender as RadioButton);
            if (rb.Checked)
            {
                if (ChangeStep(rb.Tag.ToString(), "0"))
                {
                    UpdateBackNextStepSmall();
                }
                else
                {
                    bool found = false;
                    foreach (Control item in tableSmallStepButtons.Controls)
                    {
                        if (item.Tag.ToString() == currentPanel.Name)
                        {
                            SetCheck(item as RadioButton, true);
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        SetSmallStep(1, true);
                    }
                }
            }
        }
        #endregion

        void ShowPanel(string name, object data = null)
        {
            Panel lastPanel = currentPanel;
            currentPanel = (Panel)pSteps.Controls.Find(name, false)[0];
            currentPanel.Location = new Point(0, 0);
            currentPanel.Size = pSteps.Size;
            //currentPanel.Anchor = (AnchorStyles)(AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right);
            currentPanel.Tag = data;
            currentPanel.BringToFront();
            if (lastPanel != null)
            {
                lastPanel.Dock = DockStyle.None;
            }
            currentPanel.Dock = DockStyle.Fill;

            UpdateData();
            currentPanel.Show();
            if (currentPanel != lastPanel)
            {
                HidePanel(lastPanel);
            }
        }

        void HidePanel(Panel panel)
        {
            if (panel == null) return;
            panel.Visible = false;
        }

        private void UpdateData()
        {
            foreach (StepLink step in stepLinks)
            {
                if (step.PanelName == currentPanel.Name)
                {
                    step.LoadId?.Invoke();
                }
            }
        }

        #region TaskEdit
        private void UpdateTaskInfo()
        {
            tbName.Text = task.Name;
            tbLink.Text = task.Link;

            if (task.Container == null)
            {
                pContSetted.Hide();
  
                bEditCont.Enabled = false;
                bDelCont.Enabled = false;
            }
            else
            {
                lContSetted.Text = string.Format(lContSetted.Tag.ToString(), task.Container.Tag, task.Container.Data.Where(d => d.GetRealType() == DataType.Check).Count());
                pContSetted.Show();
                
                bEditCont.Enabled = true;
                bDelCont.Enabled = true;
            }
        }

        private void bSetCont_Click(object sender, EventArgs e)
        {
            task.Container = new Element();
            ShowPanel("pElement", new object[] { task.Container, currentPanel.Name, currentPanel.Tag, null });
        }

        private void bEditCont_Click(object sender, EventArgs e)
        {
            ShowPanel("pElement", new object[] { task.Container, currentPanel.Name, currentPanel.Tag, null });
        }

        private void bDelCont_Click(object sender, EventArgs e)
        {
            DeleteElementById(task.Container.Id);
            UpdateTaskInfo();
        }

        private void tbName_TextChanged(object sender, EventArgs e)
        {
            task.Name = tbName.Text;
            this.Text = DefaultName + ": " + tbName.Text;
        }
        private void tbLink_TextChanged(object sender, EventArgs e)
        {
            task.Link = tbLink.Text;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bCheckLink.Enabled = false;

            string result = "";
            bool isOk = MainForm.PrepareToDoWork(tbLink.Text, webBrowser1);

            if (isOk)
            {
                result = "Проверка прошла успешно! Программе удалось получить доступ к сайту.";
            }
            else
            {
                result = "Проверка провалилась. Не удалось получить доступ к сайту. Проверьте правильность написания и интернет соединение, а затем повторите попытку.";
            }

            bCheckLink.Enabled = true;
            MessageBox.Show(result, "Результат проверки сайта", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        #endregion

        #region PathwaysEdit
        void UpdatePathways()
        {
            DGVPathways.Rows.Clear();
            foreach (Pathway path in task.Pathways)
            {
                int getCount = 0, checkCount = 0;
                foreach (Element el in path.elements)
                {
                    getCount += el.Data.Where(d => d.Type == DataType.Get || d.GetRealType() == DataType.Get).Count();
                    checkCount += el.Data.Where(d => d.Type == DataType.Check || d.GetRealType() == DataType.Check).Count();
                }
                string startPoint = "Верх страницы";
                if (path.isGetDataFromContainer)
                {
                    startPoint = "Контейнер";
                }
                DGVPathways.Rows.Add(path.Id, path.elements.Count, getCount, startPoint);
            }
        }

        private int GetPathIdFromGrid()
        {
            if (DGVPathways.SelectedRows.Count == 0) return -1;

            int id = int.Parse(DGVPathways.SelectedRows[0].Cells["colPId"].Value.ToString());
            return id;
        }

        private int GetMinPathId(int start = -1)
        {
            while (task.GetPathwayById(start) != null)
            {
                start--;
            }
            return start;
        }

        private void bEditPath_Click(object sender, EventArgs e)
        {
            if (DGVPathways.SelectedRows.Count == 0) return;
            int id = int.Parse(DGVPathways.SelectedRows[0].Cells["colPId"].Value.ToString());

            ShowPanel("pPath", id);

            if (treeSteps.Nodes[1].IsExpanded && treeSteps.Nodes[1].Nodes.Count > DGVPathways.SelectedRows[0].Index)
            {
                treeSteps.BeforeSelect -= treeSteps_BeforeSelect;
                treeSteps.SelectedNode = treeSteps.Nodes[1].Nodes[DGVPathways.SelectedRows[0].Index];
                treeSteps.BeforeSelect += treeSteps_BeforeSelect;
            }
        }

        private void bDelPath_Click(object sender, EventArgs e)
        {
            int id = GetPathIdFromGrid();

            DeletePathById(id);
            RefreshPathways();
        }

        private void bAddPath_Click(object sender, EventArgs e)
        {
            Pathway pw = new Pathway(new Element());
            pw.Id = GetMinPathId();
            task.Pathways.Add(pw);

            RefreshPathways();
            ShowPanel("pElement", new object[] { pw.elements[0], currentPanel.Name, currentPanel.Tag, pw });
        }
        #endregion

        #region PathwayEdit
        private void UpdatePath()
        {
            int pid = (int)pPath.Tag;

            DGVElements.Rows.Clear();
            var ways = task.Pathways.Where(x => x.Id == pid).ToArray();
            if (ways.Length == 0) return;

            Pathway pw = ways[0];
            foreach (Element element in pw.elements)
            {
                int getCount = element.Data.Where(d => d.Type == DataType.Get || d.GetRealType() == DataType.Get).Count();
                int checkCount = element.Data.Where(d => d.Type == DataType.Check || d.GetRealType() == DataType.Check).Count();
                DGVElements.Rows.Add(element.Id, element.Tag, getCount, checkCount);
            }

            cbFromCont.Checked = pw.isGetDataFromContainer;
        }

        private int GetElementFromGrid()
        {
            if (DGVElements.SelectedRows.Count == 0) return -1;

            int id = int.Parse(DGVElements.SelectedRows[0].Cells["colEId"].Value.ToString());
            return id;
        }

        private void bEditEl_Click(object sender, EventArgs e)
        {
            if (DGVElements.SelectedRows.Count == 0) return;
            int id = int.Parse(DGVElements.SelectedRows[0].Cells["colEId"].Value.ToString());

            Pathway pw = null;
            Element element = null;
            for (int p = 0; p < task.Pathways.Count; p++)
            {
                for (int el = 0; el < task.Pathways[p].elements.Count && pw == null; el++)
                {
                    if (task.Pathways[p].elements[el].Id == id)
                    {
                        pw = task.Pathways[p];
                        element = task.Pathways[p].elements[el];
                        break;
                    }
                }
            }
            ShowPanel("pElement", new object[] { element, currentPanel.Name, currentPanel.Tag, pw });
        }

        private void bDelEl_Click(object sender, EventArgs e)
        {
            int id = GetElementFromGrid();
            DeleteElementById(id);
        }

        private void bAddEl_Click(object sender, EventArgs e)
        {
            int pid = (int)pPath.Tag;
            Pathway pw = task.GetPathwayById(pid);
            pw.elements.Add(new Element());

            ShowPanel("pElement", new object[] { pw.elements[pw.elements.Count - 1], currentPanel.Name, currentPanel.Tag, pw });
        }

        private void cbFromCont_CheckedChanged(object sender, EventArgs e)
        {
            int pid = (int)pPath.Tag;
            Pathway pw = task.GetPathwayById(pid);
            if (pw == null) return;

            pw.isGetDataFromContainer = cbFromCont.Checked;
        }

        private void bShowWays_Click(object sender, EventArgs e)
        {
            ShowPanel("pPathways");

            treeSteps.BeforeSelect -= treeSteps_BeforeSelect;
            treeSteps.SelectedNode = treeSteps.Nodes[1];
            treeSteps.BeforeSelect += treeSteps_BeforeSelect;
        }
        #endregion

        #region ElementEdit
        private void UpdateElement()
        {
            tableData.SuspendLayout();

            int eid = (GetArrayItem(0) as Element).Id;

            //Очистка
            ClearDataTable();

            //Поиск элемента
            Element el = task.GetElementById(eid);
            if (el == null || el.Tag == null)
            {
                el = (Element)GetArrayItem(0);
                el.Id = GetMinElementId();
            }

            //Заполнение основной информации
            tbTag.Text = el.Tag;
            cbWhere.SelectedIndex = (int)el.Where;
            cbWich.SelectedIndex = EDistanceToInt(el.WhichOne);
            cbFrom.SelectedIndex = 0;
            bSaveAddEl.Enabled = GetArrayItem(3) != null;

            //Заполнение данных
            foreach (Data data in el.Data)
            {
                AddDataRow(data, false);
            }

            tableData.ResumeLayout(false);
            tableData.PerformLayout();
        }

        private int GetMinElementId(int start = -1)
        {
            while (task.GetElementById(start) != null)
            {
                start--;
            }
            return start;
        }

        private void ClearDataTable()
        {
            List<Control> ok = new List<Control>();
            for (int i = 0; i < tableData.ColumnCount; i++)
            {
                Control cont = tableData.GetControlFromPosition(i, 0);
                if (cont != null && !ok.Contains(cont))
                {
                    ok.Add(cont);
                }
            }
            int c = 0;
            while (tableData.Controls.Count > ok.Count && c < tableData.Controls.Count)
            {
                if (ok.Contains(tableData.Controls[c]))
                {
                    c++;
                    continue;
                }
                tableData.Controls[c].Dispose();
            }

            while (tableData.RowCount > 1)
            {
                if (tableData.RowStyles.Count >= tableData.RowCount)
                {
                    tableData.RowStyles.RemoveAt(tableData.RowStyles.Count - 1);
                }
                tableData.RowCount--;
            }
            tableData.RowCount++;
            tableData.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        }

        private void AddDataRow(Data data, bool disableLayout = true)
        {
            if (disableLayout)
            {
                tableData.SuspendLayout();
            }

            ComboBox cb = CreateCB("Авто", "Получить");
            cb.SelectedIndex = (int)data.Type;
            tableData.Controls.Add(cb, 0, tableData.RowCount - 1);

            ShadowTextBox stb = CreateSTB();
            stb.Text = data.Prop;
            tableData.Controls.Add(stb, 1, tableData.RowCount - 1);

            cb = CreateCB("Равно", "Содержит", "Не содержит", "Начинается с", "Заканчивается на");
            cb.SelectedIndex = (int)data.howSearch;
            tableData.Controls.Add(cb, 2, tableData.RowCount - 1);

            TextBox tb = CreateTB();
            tb.Text = data.Value;
            tableData.Controls.Add(tb, 3, tableData.RowCount - 1);

            PictureBox pb = new PictureBox();
            pb.Image = Properties.Resources.крест1;
            pb.SizeMode = PictureBoxSizeMode.CenterImage;
            pb.Size = new Size(14, 14);
            pb.Cursor = Cursors.Hand;
            pb.Dock = DockStyle.Top;
            pb.Margin = new Padding(3, 6, 3, 3);
            pb.Tag = data.Id;
            
            pb.MouseEnter += PbDel_MouseEnter;
            pb.MouseLeave += PbDel_MouseLeave;
            pb.MouseClick += PbDel_MouseClick;
            tableData.Controls.Add(pb, 4, tableData.RowCount - 1);

            tableData.RowCount++;
            tableData.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            if (disableLayout)
            {
                tableData.ResumeLayout(false);
                tableData.PerformLayout();
            }
        }

        ComboBox CreateCB(params string[] Items)
        {
            ComboBox cb = new ComboBox();
            cb.DropDownStyle = ComboBoxStyle.DropDownList;
            cb.Dock = DockStyle.Fill;
            cb.Items.AddRange(Items);
            if (cb.Items.Count != 0)
            {
                cb.SelectedIndex = 0;
            }
            return cb;
        }

        TextBox CreateTB()
        {
            TextBox tb = new TextBox();
            tb.Dock = DockStyle.Fill;
            return tb;
        }

        ShadowTextBox CreateSTB()
        {
            ShadowTextBox tb = new ShadowTextBox();
            tb.Dock = DockStyle.Fill;
            tb.ShadowText = "Текст";
            return tb;
        }

        EDistance IntToEDistance(int i)
        {
            if (i < 3)
            {
                i = -i;
            }
            else
            {
                i--;
            }
            return (EDistance)i;
        }

        int EDistanceToInt(EDistance ed)
        {
            int i = (int)ed;
            if (i <= 0)
            {
                i = -i;
            }
            else
            {
                i++;
            }
            return i;
        }

        private Element ShapeElement(int id)
        {
            Element el = new Element();
            el.Id = id;
            el.Tag = tbTag.Text;
            el.WhichOne = IntToEDistance(cbWich.SelectedIndex);
            el.Where = (ELocation)cbWhere.SelectedIndex;

            for (int i = 1; i < tableData.RowCount - 1; i++)
            {
                Data data = new Data();
                data.Id = GetDataIdFromRow(i);
                data.Prop = tableData.GetControlFromPosition(1, i).Text;
                data.howSearch = (HowSearch)(tableData.GetControlFromPosition(2, i) as ComboBox).SelectedIndex;
                data.Value = tableData.GetControlFromPosition(3, i).Text;
                if (data.Value == "") data.Value = null;
                data.Type = (DataType)(tableData.GetControlFromPosition(0, i) as ComboBox).SelectedIndex;

                el.Data.Add(data);
            }

            return el;
        }

        private void AddDataToElement(int id, Data data)
        {
            Element el = task.GetElementById(id);
            el.Data.Add(data);
            AddDataRow(data);
        }

        private int GetDataIdFromRow(int row)
        {
            int id = int.Parse(tableData.GetControlFromPosition(tableData.ColumnCount - 1, row).Tag.ToString());
            return id;
        }

        int GetMinDataId(int start = -1)
        {
            while (task.GetDataById(start) != null) start--;
            return start;
        }

        private bool SaveElementInTask(bool ignoreErrors = false)
        {
            string errors = CheckElement();
            if (errors != "" && !ignoreErrors)
            {
                MessageBox.Show("Сохранение элемента невозможно по следующим причинам:" + Environment.NewLine + errors, "Невозможно применить изменения", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            int id = (GetArrayItem(0) as Element).Id;
            Element el = task.GetElementById(id);

            task.ReplaceElementById(id, ShapeElement(id));
            return true;
        }

        string CheckElement()
        {
            string errors = "";
            if (tbTag.Text == "")
            {
                errors += "• Впишите тег необходимого вам элемента." + Environment.NewLine;
            }
            /*if (tableData.RowCount <= 2)
            {
                errors += "• Укажите данные, которые вы хотите получить или проверить у элемента." + Environment.NewLine;
            }*/
            return errors;
        }

        object GetArrayItem(int i)
        {
            return (pElement.Tag as object[])[i];
        }

        private void PbDel_MouseClick(object sender, MouseEventArgs e)
        {
            int d_id = GetDataIdFromRow(tableData.GetPositionFromControl((PictureBox)sender).Row);

            SaveElementInTask(true);
            DeleteDataById(d_id);
        }

        private void PbDel_MouseEnter(object sender, EventArgs e)
        {
            (sender as PictureBox).Image = Properties.Resources.крест2;
        }

        private void PbDel_MouseLeave(object sender, EventArgs e)
        {
            (sender as PictureBox).Image = Properties.Resources.крест1;
        }

        private void bAddElement_Click(object sender, EventArgs e)
        {
            pElement.Focus();
            Data data = new Data(DataType.Auto);
            data.Id = GetMinDataId();
            AddDataToElement((GetArrayItem(0) as Element).Id, data);
        }

        private void bElementRedy_Click(object sender, EventArgs e)
        {
            if (SaveElementInTask())
            {
                ShowPanel(GetArrayItem(1).ToString(), GetArrayItem(2));
            }
        }

        private void bElementCancel_Click(object sender, EventArgs e)
        {
            DeleteElementById((GetArrayItem(0) as Element).Id);
            ShowPanel(GetArrayItem(1).ToString(), GetArrayItem(2));
        }

        private void bSaveAddEl_Click(object sender, EventArgs e)
        {
            if (GetArrayItem(3) != null && SaveElementInTask())
            {
                Element el = new Element();
                (GetArrayItem(3) as Pathway).elements.Add(el);

                object[] tag = (object[])pElement.Tag;
                tag[0] = el;
                ShowPanel("pElement", tag);
            }
        }
        #endregion

        private void DeleteDataById(int id)
        {
            command.CommandText = "DELETE FROM data WHERE id = " + id;
            MainForm.ExecuteNonQuery(connection, command);
            task.ReplaceDataById(id, null);

            ReloadTask();
        }

        private void DeleteElementById(int id)
        {
            command.CommandText = "DELETE FROM elements WHERE id = " + id;
            MainForm.ExecuteNonQuery(connection, command);
            task.ReplaceElementById(id, null);

            ReloadTask();
        }

        private void DeletePathById(int id)
        {
            command.CommandText = "DELETE FROM pathways WHERE id = " + id;
            MainForm.ExecuteNonQuery(connection, command);
            task.ReplacePathwayById(id, null);

            ReloadTask();
        }
        
        private void bSaveTask_Click(object sender, EventArgs e)
        {
            bSaveTask.Enabled = false;
            SaveTask();
            bSaveTask.Enabled = true;
        }

        private bool SaveTask()
        {
            string errors = CheckTask();
            if (errors == "")
            {
                MainForm.SaveTask(task);

                DialogResult = DialogResult.OK;
                Close();
                return true;
            }
            else
            {
                MessageBox.Show("Сохранение задания невозможно по следующим причинам:" + Environment.NewLine + errors, "Не удалось сохранить задание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
        }
        
        private void bCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Abort;
            Close();
        }

        private void EditTask_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult == DialogResult.Cancel && task.Id > 0)
            {
                DialogResult = DialogResult.Cancel;
                if (MessageBox.Show("Сохранить изменения в задании?", "Сохранение изменений", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                {
                    if (!SaveTask())
                    {
                        e.Cancel = true;
                        return;
                    }
                }
            }
        }

        #region Checking
        private void bTotalCheck_Click(object sender, EventArgs e)
        {
            bTotalCheck.Enabled = false;
            tbTotalResult.Text = CheckAll();
            bTotalCheck.Enabled = true;
        }
        private void bCheckTask_Click(object sender, EventArgs e)
        {
            bCheckTask.Enabled = false;
            tbCheckPathways.Text = CheckAll();
            bCheckTask.Enabled = true;
        }

        private void bCheckPath_Click(object sender, EventArgs e)
        {
            bCheckPath.Enabled = false;
            tbCheckPath.Text = CheckPath();
            bCheckPath.Enabled = true;
        }
        string CheckTask()
        {
            string errors = "";
            var data = task.Pathways.Where(x => x.elements.Where(e => e.Data.Where(d => d.Type == DataType.Get || d.GetRealType() == DataType.Get).Count() > 0).Count() > 0);
            if (data.Count() == 0)
            {
                errors += "• Данное задание ничего не возвращает, вы уверены, что вам оно действительно нужно?" + Environment.NewLine;
            }
            if (task.Name == null || task.Name == "")
            {
                errors += "• Не указано имя задания" + Environment.NewLine;
            }
            if (task.Link == null || task.Link == "")
            {
                errors += "• Не указана ссылка на сайт" + Environment.NewLine;
            }
            return errors;
        }


        string CheckPath()
        {
            string result = "";

            WSTask path = new WSTask();
            path.Container = task.Container;
            path.Pathways.Add(task.GetPathwayById((int)pPath.Tag));

            if (MainForm.PrepareToDoWork(task.Link, webBrowser1))
            {
                path.DoWork(webBrowser1.Document);
                result = path.GetResults(Environment.NewLine);
            }
            else
            {
                result = "Не удалось открыть сайт, проверьте есть ли к нему доступ и повторите попытку.";
            }
            return result;
        }

        string CheckAll()
        {
            string result = "";
            if (MainForm.PrepareToDoWork(task.Link, webBrowser1))
            {
                task.DoWork(webBrowser1.Document);
                result = task.GetResults(Environment.NewLine);
            }
            else
            {
                result = "Не удалось открыть сайт, проверьте есть ли к нему доступ и повторите попытку.";
            }
            return result;
        }
        #endregion

        private void EditTask_SizeChanged(object sender, EventArgs e)
        {
            if (WindowState != FormWindowState.Minimized)
            {
                //Heigth
                if (pStepMenu.Height > ClientRectangle.Height || (pStepMenu.Height < pStepMenu.MaximumSize.Height && ClientRectangle.Height < pStepMenu.MaximumSize.Height))
                {
                    tableMain.RowStyles[0].Height = ClientRectangle.Height;
                }
                else
                {
                    tableMain.RowStyles[0].Height = pStepMenu.MaximumSize.Height;
                }
                //Width
                if (Width <= MinimumWidth)
                {
                    if (isBigSteps)
                    {
                        ShowStepMenu(false, false);
                    }
                    else
                    {
                        MinimumSize = new Size(MinimumWidth, MinimumSize.Height);
                    }
                }
            }
        }
    }
}
