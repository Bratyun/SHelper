namespace TrackWebsite
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.timerBreak = new System.Windows.Forms.Timer(this.components);
            this.bReloadSet = new System.Windows.Forms.Button();
            this.bSaveSet = new System.Windows.Forms.Button();
            this.cbLoadWithWin = new System.Windows.Forms.CheckBox();
            this.cbAutoGo = new System.Windows.Forms.CheckBox();
            this.numTaskChange = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.bAddNewTask = new System.Windows.Forms.Button();
            this.numRest = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.bSet = new System.Windows.Forms.Button();
            this.pSet = new System.Windows.Forms.Panel();
            this.bCancelSet = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.panel6 = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cmTask = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miRunTask = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuItemChangeTask = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemDeactivateTask = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemDeleteTask = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuItemAddNewTask = new System.Windows.Forms.ToolStripMenuItem();
            this.miAddTaskByBase = new System.Windows.Forms.ToolStripMenuItem();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.cmTray = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.открытьОкноToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuItemStartStop = new System.Windows.Forms.ToolStripMenuItem();
            this.закрытьПрограммуToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pStartAndSettings = new System.Windows.Forms.TableLayoutPanel();
            this.cbStartStop = new System.Windows.Forms.CheckBox();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.tbAdress = new System.Windows.Forms.TextBox();
            this.pTasks = new System.Windows.Forms.TableLayoutPanel();
            this.panel8 = new System.Windows.Forms.Panel();
            this.lLoadTasks = new System.Windows.Forms.Label();
            this.bAddTask = new System.Windows.Forms.Button();
            this.DGVTasks = new System.Windows.Forms.DataGridView();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.job = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.progress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pTaskLabel = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.lProgress = new System.Windows.Forms.Label();
            this.pResActions = new System.Windows.Forms.TableLayoutPanel();
            this.bIsNew = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.bFirst = new System.Windows.Forms.Button();
            this.bPrevious = new System.Windows.Forms.Button();
            this.bNext = new System.Windows.Forms.Button();
            this.bLast = new System.Windows.Forms.Button();
            this.lResultNum = new System.Windows.Forms.Label();
            this.pResult = new System.Windows.Forms.Panel();
            this.tbResult = new System.Windows.Forms.TextBox();
            this.panel7 = new System.Windows.Forms.Panel();
            this.lTaskName = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lDate = new System.Windows.Forms.Label();
            this.pMain = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.numTaskChange)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRest)).BeginInit();
            this.pSet.SuspendLayout();
            this.panel2.SuspendLayout();
            this.cmTask.SuspendLayout();
            this.cmTray.SuspendLayout();
            this.pStartAndSettings.SuspendLayout();
            this.pTasks.SuspendLayout();
            this.panel8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGVTasks)).BeginInit();
            this.pTaskLabel.SuspendLayout();
            this.pResActions.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.pResult.SuspendLayout();
            this.panel7.SuspendLayout();
            this.pMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // timerBreak
            // 
            this.timerBreak.Interval = 1000;
            this.timerBreak.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // bReloadSet
            // 
            this.bReloadSet.Location = new System.Drawing.Point(3, 189);
            this.bReloadSet.Name = "bReloadSet";
            this.bReloadSet.Size = new System.Drawing.Size(312, 23);
            this.bReloadSet.TabIndex = 8;
            this.bReloadSet.Text = "Сбросить изменения";
            this.bReloadSet.UseVisualStyleBackColor = true;
            this.bReloadSet.Click += new System.EventHandler(this.bReloadSet_Click);
            // 
            // bSaveSet
            // 
            this.bSaveSet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bSaveSet.Location = new System.Drawing.Point(117, 364);
            this.bSaveSet.Name = "bSaveSet";
            this.bSaveSet.Size = new System.Drawing.Size(101, 23);
            this.bSaveSet.TabIndex = 7;
            this.bSaveSet.Text = "Сохранить";
            this.bSaveSet.UseVisualStyleBackColor = true;
            this.bSaveSet.Click += new System.EventHandler(this.bSaveSet_Click);
            // 
            // cbLoadWithWin
            // 
            this.cbLoadWithWin.AutoSize = true;
            this.cbLoadWithWin.Location = new System.Drawing.Point(12, 150);
            this.cbLoadWithWin.Name = "cbLoadWithWin";
            this.cbLoadWithWin.Size = new System.Drawing.Size(287, 17);
            this.cbLoadWithWin.TabIndex = 6;
            this.cbLoadWithWin.Text = "Запускать программу при старте Windows (в фоне)";
            this.cbLoadWithWin.UseVisualStyleBackColor = true;
            // 
            // cbAutoGo
            // 
            this.cbAutoGo.AutoSize = true;
            this.cbAutoGo.Location = new System.Drawing.Point(12, 114);
            this.cbAutoGo.Name = "cbAutoGo";
            this.cbAutoGo.Size = new System.Drawing.Size(266, 30);
            this.cbAutoGo.TabIndex = 5;
            this.cbAutoGo.Text = "Автоматически начинать выполнение заданий \r\nпри старте программы";
            this.cbAutoGo.UseVisualStyleBackColor = true;
            // 
            // numTaskChange
            // 
            this.numTaskChange.Location = new System.Drawing.Point(246, 62);
            this.numTaskChange.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.numTaskChange.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.numTaskChange.Name = "numTaskChange";
            this.numTaskChange.Size = new System.Drawing.Size(61, 20);
            this.numTaskChange.TabIndex = 4;
            this.numTaskChange.ThousandsSeparator = true;
            this.numTaskChange.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 64);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(227, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Пауза между сменой задания (в секундах):";
            // 
            // bAddNewTask
            // 
            this.bAddNewTask.Location = new System.Drawing.Point(3, 241);
            this.bAddNewTask.Name = "bAddNewTask";
            this.bAddNewTask.Size = new System.Drawing.Size(312, 23);
            this.bAddNewTask.TabIndex = 2;
            this.bAddNewTask.Text = "Добавить новое задание";
            this.bAddNewTask.UseVisualStyleBackColor = true;
            this.bAddNewTask.Click += new System.EventHandler(this.bAddNewTask_Click);
            // 
            // numRest
            // 
            this.numRest.Location = new System.Drawing.Point(246, 36);
            this.numRest.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.numRest.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numRest.Name = "numRest";
            this.numRest.Size = new System.Drawing.Size(61, 20);
            this.numRest.TabIndex = 1;
            this.numRest.ThousandsSeparator = true;
            this.numRest.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 38);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(233, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Пауза между повтором заданий (в минутах):";
            // 
            // bSet
            // 
            this.bSet.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("bSet.BackgroundImage")));
            this.bSet.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.bSet.Dock = System.Windows.Forms.DockStyle.Right;
            this.bSet.FlatAppearance.BorderSize = 0;
            this.bSet.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bSet.Location = new System.Drawing.Point(835, 0);
            this.bSet.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.bSet.Name = "bSet";
            this.bSet.Size = new System.Drawing.Size(20, 26);
            this.bSet.TabIndex = 1;
            this.toolTip1.SetToolTip(this.bSet, "Настройки");
            this.bSet.UseVisualStyleBackColor = true;
            this.bSet.Click += new System.EventHandler(this.bSet_Click);
            // 
            // pSet
            // 
            this.pSet.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pSet.Controls.Add(this.bCancelSet);
            this.pSet.Controls.Add(this.panel3);
            this.pSet.Controls.Add(this.bSaveSet);
            this.pSet.Controls.Add(this.panel2);
            this.pSet.Location = new System.Drawing.Point(858, 0);
            this.pSet.Name = "pSet";
            this.pSet.Size = new System.Drawing.Size(336, 399);
            this.pSet.TabIndex = 11;
            this.pSet.Tag = "336";
            this.pSet.Visible = false;
            // 
            // bCancelSet
            // 
            this.bCancelSet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bCancelSet.Location = new System.Drawing.Point(224, 364);
            this.bCancelSet.Name = "bCancelSet";
            this.bCancelSet.Size = new System.Drawing.Size(101, 23);
            this.bCancelSet.TabIndex = 12;
            this.bCancelSet.Text = "Отмена";
            this.bCancelSet.UseVisualStyleBackColor = true;
            this.bCancelSet.Click += new System.EventHandler(this.bCancelSet_Click);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1, 399);
            this.panel3.TabIndex = 10;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.panel2.AutoScroll = true;
            this.panel2.Controls.Add(this.button2);
            this.panel2.Controls.Add(this.panel6);
            this.panel2.Controls.Add(this.cbAutoGo);
            this.panel2.Controls.Add(this.cbLoadWithWin);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.panel5);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.numTaskChange);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Controls.Add(this.numRest);
            this.panel2.Controls.Add(this.panel1);
            this.panel2.Controls.Add(this.bAddNewTask);
            this.panel2.Controls.Add(this.bReloadSet);
            this.panel2.Location = new System.Drawing.Point(7, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(337, 348);
            this.panel2.TabIndex = 13;
            // 
            // button2
            // 
            this.button2.Enabled = false;
            this.button2.Location = new System.Drawing.Point(3, 269);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(312, 23);
            this.button2.TabIndex = 25;
            this.button2.Text = "Импортировать задание";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.panel6.Location = new System.Drawing.Point(24, 177);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(268, 1);
            this.panel6.TabIndex = 12;
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label7.ForeColor = System.Drawing.Color.Silver;
            this.label7.Location = new System.Drawing.Point(116, 90);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(80, 17);
            this.label7.TabIndex = 22;
            this.label7.Text = "Автозапуск";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.Silver;
            this.panel5.Location = new System.Drawing.Point(24, 99);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(268, 1);
            this.panel5.TabIndex = 23;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.ForeColor = System.Drawing.Color.Silver;
            this.label6.Location = new System.Drawing.Point(124, 10);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(56, 17);
            this.label6.TabIndex = 20;
            this.label6.Text = "Паузы";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.Silver;
            this.panel4.Location = new System.Drawing.Point(24, 19);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(268, 1);
            this.panel4.TabIndex = 21;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.panel1.Location = new System.Drawing.Point(24, 225);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(268, 1);
            this.panel1.TabIndex = 11;
            // 
            // cmTask
            // 
            this.cmTask.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miRunTask,
            this.toolStripSeparator3,
            this.MenuItemChangeTask,
            this.MenuItemDeactivateTask,
            this.MenuItemDeleteTask,
            this.toolStripSeparator2,
            this.MenuItemAddNewTask,
            this.miAddTaskByBase});
            this.cmTask.Name = "contextMenuTask";
            this.cmTask.Size = new System.Drawing.Size(264, 148);
            this.cmTask.Opening += new System.ComponentModel.CancelEventHandler(this.cmTask_Opening);
            // 
            // miRunTask
            // 
            this.miRunTask.Name = "miRunTask";
            this.miRunTask.Size = new System.Drawing.Size(263, 22);
            this.miRunTask.Text = "Выполнить";
            this.miRunTask.Click += new System.EventHandler(this.выполнитьToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(260, 6);
            // 
            // MenuItemChangeTask
            // 
            this.MenuItemChangeTask.Name = "MenuItemChangeTask";
            this.MenuItemChangeTask.Size = new System.Drawing.Size(263, 22);
            this.MenuItemChangeTask.Text = "Редактировать";
            this.MenuItemChangeTask.Click += new System.EventHandler(this.MenuItemChangeTask_Click);
            // 
            // MenuItemDeactivateTask
            // 
            this.MenuItemDeactivateTask.Enabled = false;
            this.MenuItemDeactivateTask.Name = "MenuItemDeactivateTask";
            this.MenuItemDeactivateTask.Size = new System.Drawing.Size(263, 22);
            this.MenuItemDeactivateTask.Text = "Деактивировать";
            // 
            // MenuItemDeleteTask
            // 
            this.MenuItemDeleteTask.Name = "MenuItemDeleteTask";
            this.MenuItemDeleteTask.Size = new System.Drawing.Size(263, 22);
            this.MenuItemDeleteTask.Text = "Удалить";
            this.MenuItemDeleteTask.Click += new System.EventHandler(this.MenuItemDeleteTask_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(260, 6);
            // 
            // MenuItemAddNewTask
            // 
            this.MenuItemAddNewTask.Name = "MenuItemAddNewTask";
            this.MenuItemAddNewTask.Size = new System.Drawing.Size(263, 22);
            this.MenuItemAddNewTask.Text = "Добавить новое задание";
            this.MenuItemAddNewTask.Click += new System.EventHandler(this.MenuItemAddNewTask_Click);
            // 
            // miAddTaskByBase
            // 
            this.miAddTaskByBase.Name = "miAddTaskByBase";
            this.miAddTaskByBase.Size = new System.Drawing.Size(263, 22);
            this.miAddTaskByBase.Text = "Добавить задание на основе этого";
            this.miAddTaskByBase.Click += new System.EventHandler(this.miAddTaskByBase_Click);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.notifyIcon1.ContextMenuStrip = this.cmTray;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "Енот";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.BalloonTipClicked += new System.EventHandler(this.notifyIcon1_BalloonTipClicked);
            this.notifyIcon1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseUp);
            // 
            // cmTray
            // 
            this.cmTray.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.открытьОкноToolStripMenuItem,
            this.toolStripSeparator1,
            this.MenuItemStartStop,
            this.закрытьПрограммуToolStripMenuItem});
            this.cmTray.Name = "contextMenuTray";
            this.cmTray.ShowImageMargin = false;
            this.cmTray.Size = new System.Drawing.Size(162, 76);
            this.cmTray.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuTray_Opening);
            // 
            // открытьОкноToolStripMenuItem
            // 
            this.открытьОкноToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.открытьОкноToolStripMenuItem.Name = "открытьОкноToolStripMenuItem";
            this.открытьОкноToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.открытьОкноToolStripMenuItem.Text = "Открыть окно";
            this.открытьОкноToolStripMenuItem.Click += new System.EventHandler(this.открытьОкноToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(158, 6);
            // 
            // MenuItemStartStop
            // 
            this.MenuItemStartStop.Name = "MenuItemStartStop";
            this.MenuItemStartStop.Size = new System.Drawing.Size(161, 22);
            this.MenuItemStartStop.Text = "Start";
            this.MenuItemStartStop.Click += new System.EventHandler(this.MenuItemStartStop_Click);
            // 
            // закрытьПрограммуToolStripMenuItem
            // 
            this.закрытьПрограммуToolStripMenuItem.Name = "закрытьПрограммуToolStripMenuItem";
            this.закрытьПрограммуToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.закрытьПрограммуToolStripMenuItem.Text = "Закрыть программу";
            this.закрытьПрограммуToolStripMenuItem.Click += new System.EventHandler(this.закрытьПрограммуToolStripMenuItem_Click);
            // 
            // pStartAndSettings
            // 
            this.pStartAndSettings.ColumnCount = 2;
            this.pStartAndSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pStartAndSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.pStartAndSettings.Controls.Add(this.bSet, 1, 0);
            this.pStartAndSettings.Controls.Add(this.cbStartStop, 0, 0);
            this.pStartAndSettings.Dock = System.Windows.Forms.DockStyle.Top;
            this.pStartAndSettings.Location = new System.Drawing.Point(3, 234);
            this.pStartAndSettings.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.pStartAndSettings.Name = "pStartAndSettings";
            this.pStartAndSettings.RowCount = 1;
            this.pStartAndSettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.pStartAndSettings.Size = new System.Drawing.Size(858, 29);
            this.pStartAndSettings.TabIndex = 2;
            // 
            // cbStartStop
            // 
            this.cbStartStop.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbStartStop.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbStartStop.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.cbStartStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbStartStop.Location = new System.Drawing.Point(3, 3);
            this.cbStartStop.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.cbStartStop.Name = "cbStartStop";
            this.cbStartStop.Size = new System.Drawing.Size(826, 23);
            this.cbStartStop.TabIndex = 0;
            this.cbStartStop.Text = "Start";
            this.cbStartStop.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cbStartStop.UseVisualStyleBackColor = false;
            this.cbStartStop.CheckedChanged += new System.EventHandler(this.cbStartStop_CheckedChanged);
            // 
            // webBrowser1
            // 
            this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser1.Location = new System.Drawing.Point(0, 286);
            this.webBrowser1.Margin = new System.Windows.Forms.Padding(0);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.ScriptErrorsSuppressed = true;
            this.webBrowser1.Size = new System.Drawing.Size(864, 113);
            this.webBrowser1.TabIndex = 2;
            this.webBrowser1.Navigated += new System.Windows.Forms.WebBrowserNavigatedEventHandler(this.webBrowser1_Navigated);
            this.webBrowser1.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler(this.webBrowser1_Navigating);
            // 
            // tbAdress
            // 
            this.tbAdress.Dock = System.Windows.Forms.DockStyle.Top;
            this.tbAdress.Location = new System.Drawing.Point(0, 266);
            this.tbAdress.Margin = new System.Windows.Forms.Padding(0);
            this.tbAdress.Name = "tbAdress";
            this.tbAdress.ReadOnly = true;
            this.tbAdress.Size = new System.Drawing.Size(864, 20);
            this.tbAdress.TabIndex = 1;
            this.tbAdress.TabStop = false;
            // 
            // pTasks
            // 
            this.pTasks.ColumnCount = 2;
            this.pTasks.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 326F));
            this.pTasks.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pTasks.Controls.Add(this.panel8, 0, 1);
            this.pTasks.Controls.Add(this.pTaskLabel, 0, 0);
            this.pTasks.Controls.Add(this.pResActions, 1, 2);
            this.pTasks.Controls.Add(this.pResult, 1, 1);
            this.pTasks.Controls.Add(this.panel7, 1, 0);
            this.pTasks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pTasks.Location = new System.Drawing.Point(3, 3);
            this.pTasks.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.pTasks.Name = "pTasks";
            this.pTasks.RowCount = 3;
            this.pTasks.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.pTasks.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pTasks.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.pTasks.Size = new System.Drawing.Size(858, 231);
            this.pTasks.TabIndex = 15;
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.lLoadTasks);
            this.panel8.Controls.Add(this.bAddTask);
            this.panel8.Controls.Add(this.DGVTasks);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel8.Location = new System.Drawing.Point(3, 23);
            this.panel8.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.panel8.Name = "panel8";
            this.pTasks.SetRowSpan(this.panel8, 2);
            this.panel8.Size = new System.Drawing.Size(320, 208);
            this.panel8.TabIndex = 18;
            // 
            // lLoadTasks
            // 
            this.lLoadTasks.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lLoadTasks.AutoSize = true;
            this.lLoadTasks.BackColor = System.Drawing.Color.Transparent;
            this.lLoadTasks.Font = new System.Drawing.Font("Segoe UI Light", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lLoadTasks.ForeColor = System.Drawing.Color.SkyBlue;
            this.lLoadTasks.Location = new System.Drawing.Point(85, 80);
            this.lLoadTasks.Name = "lLoadTasks";
            this.lLoadTasks.Size = new System.Drawing.Size(144, 40);
            this.lLoadTasks.TabIndex = 18;
            this.lLoadTasks.Text = "Загрузка...";
            // 
            // bAddTask
            // 
            this.bAddTask.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.bAddTask.BackColor = System.Drawing.SystemColors.Control;
            this.bAddTask.FlatAppearance.BorderSize = 0;
            this.bAddTask.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bAddTask.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bAddTask.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.bAddTask.Location = new System.Drawing.Point(72, 64);
            this.bAddTask.Name = "bAddTask";
            this.bAddTask.Size = new System.Drawing.Size(170, 73);
            this.bAddTask.TabIndex = 17;
            this.bAddTask.Text = "Создать задание";
            this.bAddTask.UseVisualStyleBackColor = false;
            this.bAddTask.Visible = false;
            this.bAddTask.Click += new System.EventHandler(this.bAddTask_Click);
            // 
            // DGVTasks
            // 
            this.DGVTasks.AllowUserToAddRows = false;
            this.DGVTasks.AllowUserToResizeColumns = false;
            this.DGVTasks.AllowUserToResizeRows = false;
            this.DGVTasks.BackgroundColor = System.Drawing.SystemColors.Control;
            this.DGVTasks.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.DGVTasks.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.DGVTasks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGVTasks.ColumnHeadersVisible = false;
            this.DGVTasks.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.job,
            this.progress});
            this.DGVTasks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DGVTasks.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.DGVTasks.Location = new System.Drawing.Point(0, 0);
            this.DGVTasks.MultiSelect = false;
            this.DGVTasks.Name = "DGVTasks";
            this.DGVTasks.RowHeadersVisible = false;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
            this.DGVTasks.RowsDefaultCellStyle = dataGridViewCellStyle1;
            this.DGVTasks.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DGVTasks.Size = new System.Drawing.Size(320, 208);
            this.DGVTasks.StandardTab = true;
            this.DGVTasks.TabIndex = 0;
            this.DGVTasks.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DGVTasks_CellMouseClick);
            this.DGVTasks.SelectionChanged += new System.EventHandler(this.DGVTasks_SelectionChanged);
            // 
            // id
            // 
            this.id.HeaderText = "id";
            this.id.Name = "id";
            this.id.ReadOnly = true;
            this.id.Visible = false;
            // 
            // job
            // 
            this.job.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.job.HeaderText = "Задание";
            this.job.Name = "job";
            // 
            // progress
            // 
            this.progress.HeaderText = "Прогресс";
            this.progress.Name = "progress";
            this.progress.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.progress.Width = 25;
            // 
            // pTaskLabel
            // 
            this.pTaskLabel.AutoSize = true;
            this.pTaskLabel.Controls.Add(this.label1);
            this.pTaskLabel.Controls.Add(this.lProgress);
            this.pTaskLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pTaskLabel.Location = new System.Drawing.Point(6, 3);
            this.pTaskLabel.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.pTaskLabel.Name = "pTaskLabel";
            this.pTaskLabel.Size = new System.Drawing.Size(317, 14);
            this.pTaskLabel.TabIndex = 17;
            this.pTaskLabel.WrapContents = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Задания";
            // 
            // lProgress
            // 
            this.lProgress.AutoSize = true;
            this.lProgress.Location = new System.Drawing.Point(59, 0);
            this.lProgress.Name = "lProgress";
            this.lProgress.Size = new System.Drawing.Size(59, 13);
            this.lProgress.TabIndex = 7;
            this.lProgress.Tag = "Перерыв: ";
            this.lProgress.Text = "Перерыв: ";
            this.lProgress.Visible = false;
            // 
            // pResActions
            // 
            this.pResActions.ColumnCount = 3;
            this.pResActions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pResActions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.pResActions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.pResActions.Controls.Add(this.bIsNew, 0, 0);
            this.pResActions.Controls.Add(this.flowLayoutPanel1, 2, 0);
            this.pResActions.Controls.Add(this.lResultNum, 1, 0);
            this.pResActions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pResActions.Location = new System.Drawing.Point(329, 204);
            this.pResActions.Margin = new System.Windows.Forms.Padding(3, 1, 3, 0);
            this.pResActions.Name = "pResActions";
            this.pResActions.RowCount = 1;
            this.pResActions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pResActions.Size = new System.Drawing.Size(526, 27);
            this.pResActions.TabIndex = 14;
            // 
            // bIsNew
            // 
            this.bIsNew.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bIsNew.Enabled = false;
            this.bIsNew.FlatAppearance.BorderSize = 0;
            this.bIsNew.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bIsNew.Location = new System.Drawing.Point(3, 3);
            this.bIsNew.Name = "bIsNew";
            this.bIsNew.Size = new System.Drawing.Size(334, 21);
            this.bIsNew.TabIndex = 0;
            this.bIsNew.UseVisualStyleBackColor = true;
            this.bIsNew.Click += new System.EventHandler(this.bIsNew_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.Controls.Add(this.bFirst);
            this.flowLayoutPanel1.Controls.Add(this.bPrevious);
            this.flowLayoutPanel1.Controls.Add(this.bNext);
            this.flowLayoutPanel1.Controls.Add(this.bLast);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(385, 2);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(2);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(139, 23);
            this.flowLayoutPanel1.TabIndex = 15;
            this.flowLayoutPanel1.WrapContents = false;
            // 
            // bFirst
            // 
            this.bFirst.Location = new System.Drawing.Point(0, 0);
            this.bFirst.Margin = new System.Windows.Forms.Padding(0);
            this.bFirst.Name = "bFirst";
            this.bFirst.Size = new System.Drawing.Size(34, 23);
            this.bFirst.TabIndex = 3;
            this.bFirst.Text = "<<";
            this.bFirst.UseVisualStyleBackColor = true;
            this.bFirst.Click += new System.EventHandler(this.MoveResult_Click);
            // 
            // bPrevious
            // 
            this.bPrevious.Location = new System.Drawing.Point(34, 0);
            this.bPrevious.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.bPrevious.Name = "bPrevious";
            this.bPrevious.Size = new System.Drawing.Size(34, 23);
            this.bPrevious.TabIndex = 1;
            this.bPrevious.Text = "<";
            this.bPrevious.UseVisualStyleBackColor = true;
            this.bPrevious.Click += new System.EventHandler(this.MoveResult_Click);
            // 
            // bNext
            // 
            this.bNext.Location = new System.Drawing.Point(71, 0);
            this.bNext.Margin = new System.Windows.Forms.Padding(0);
            this.bNext.Name = "bNext";
            this.bNext.Size = new System.Drawing.Size(34, 23);
            this.bNext.TabIndex = 2;
            this.bNext.Text = ">";
            this.bNext.UseVisualStyleBackColor = true;
            this.bNext.Click += new System.EventHandler(this.MoveResult_Click);
            // 
            // bLast
            // 
            this.bLast.Location = new System.Drawing.Point(105, 0);
            this.bLast.Margin = new System.Windows.Forms.Padding(0);
            this.bLast.Name = "bLast";
            this.bLast.Size = new System.Drawing.Size(34, 23);
            this.bLast.TabIndex = 4;
            this.bLast.Text = ">>";
            this.bLast.UseVisualStyleBackColor = true;
            this.bLast.Click += new System.EventHandler(this.MoveResult_Click);
            // 
            // lResultNum
            // 
            this.lResultNum.AutoSize = true;
            this.lResultNum.Dock = System.Windows.Forms.DockStyle.Left;
            this.lResultNum.Location = new System.Drawing.Point(343, 0);
            this.lResultNum.Name = "lResultNum";
            this.lResultNum.Size = new System.Drawing.Size(37, 27);
            this.lResultNum.TabIndex = 16;
            this.lResultNum.Tag = "{0} из {1}";
            this.lResultNum.Text = "0 из 0";
            this.lResultNum.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pResult
            // 
            this.pResult.Controls.Add(this.tbResult);
            this.pResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pResult.Location = new System.Drawing.Point(329, 23);
            this.pResult.Margin = new System.Windows.Forms.Padding(3, 3, 3, 1);
            this.pResult.Name = "pResult";
            this.pResult.Padding = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.pResult.Size = new System.Drawing.Size(526, 179);
            this.pResult.TabIndex = 18;
            // 
            // tbResult
            // 
            this.tbResult.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.tbResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbResult.Location = new System.Drawing.Point(3, 3);
            this.tbResult.Multiline = true;
            this.tbResult.Name = "tbResult";
            this.tbResult.ReadOnly = true;
            this.tbResult.Size = new System.Drawing.Size(520, 176);
            this.tbResult.TabIndex = 0;
            this.tbResult.WordWrap = false;
            // 
            // panel7
            // 
            this.panel7.AutoSize = true;
            this.panel7.Controls.Add(this.lTaskName);
            this.panel7.Controls.Add(this.label2);
            this.panel7.Controls.Add(this.lDate);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel7.Location = new System.Drawing.Point(329, 3);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(526, 14);
            this.panel7.TabIndex = 19;
            // 
            // lTaskName
            // 
            this.lTaskName.AutoSize = true;
            this.lTaskName.Location = new System.Drawing.Point(61, 1);
            this.lTaskName.Name = "lTaskName";
            this.lTaskName.Size = new System.Drawing.Size(0, 13);
            this.lTaskName.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Left;
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Результат:";
            // 
            // lDate
            // 
            this.lDate.AutoSize = true;
            this.lDate.Dock = System.Windows.Forms.DockStyle.Right;
            this.lDate.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.lDate.Location = new System.Drawing.Point(526, 0);
            this.lDate.Name = "lDate";
            this.lDate.Size = new System.Drawing.Size(0, 13);
            this.lDate.TabIndex = 2;
            // 
            // pMain
            // 
            this.pMain.ColumnCount = 1;
            this.pMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pMain.Controls.Add(this.pTasks, 0, 0);
            this.pMain.Controls.Add(this.pStartAndSettings, 0, 1);
            this.pMain.Controls.Add(this.tbAdress, 0, 2);
            this.pMain.Controls.Add(this.webBrowser1, 0, 3);
            this.pMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pMain.Location = new System.Drawing.Point(0, 0);
            this.pMain.Name = "pMain";
            this.pMain.RowCount = 4;
            this.pMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 234F));
            this.pMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.pMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.pMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.pMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.pMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.pMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.pMain.Size = new System.Drawing.Size(864, 399);
            this.pMain.TabIndex = 16;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(864, 399);
            this.Controls.Add(this.pSet);
            this.Controls.Add(this.pMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(350, 303);
            this.Name = "MainForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Енот";
            this.Activated += new System.EventHandler(this.MainForm_Activated);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.numTaskChange)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRest)).EndInit();
            this.pSet.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.cmTask.ResumeLayout(false);
            this.cmTray.ResumeLayout(false);
            this.pStartAndSettings.ResumeLayout(false);
            this.pTasks.ResumeLayout(false);
            this.pTasks.PerformLayout();
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGVTasks)).EndInit();
            this.pTaskLabel.ResumeLayout(false);
            this.pTaskLabel.PerformLayout();
            this.pResActions.ResumeLayout(false);
            this.pResActions.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.pResult.ResumeLayout(false);
            this.pResult.PerformLayout();
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.pMain.ResumeLayout(false);
            this.pMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer timerBreak;
        private System.Windows.Forms.NumericUpDown numRest;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button bAddNewTask;
        private System.Windows.Forms.NumericUpDown numTaskChange;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox cbLoadWithWin;
        private System.Windows.Forms.CheckBox cbAutoGo;
        private System.Windows.Forms.Button bReloadSet;
        private System.Windows.Forms.Button bSaveSet;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Panel pSet;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button bCancelSet;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.ContextMenuStrip cmTask;
        private System.Windows.Forms.ToolStripMenuItem MenuItemChangeTask;
        private System.Windows.Forms.ToolStripMenuItem MenuItemDeactivateTask;
        private System.Windows.Forms.ToolStripMenuItem MenuItemDeleteTask;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem MenuItemAddNewTask;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip cmTray;
        private System.Windows.Forms.ToolStripMenuItem открытьОкноToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem MenuItemStartStop;
        private System.Windows.Forms.ToolStripMenuItem закрытьПрограммуToolStripMenuItem;
        private System.Windows.Forms.TableLayoutPanel pStartAndSettings;
        private System.Windows.Forms.Button bSet;
        private System.Windows.Forms.CheckBox cbStartStop;
        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.TextBox tbAdress;
        private System.Windows.Forms.TableLayoutPanel pTasks;
        private System.Windows.Forms.FlowLayoutPanel pTaskLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lProgress;
        private System.Windows.Forms.TextBox tbResult;
        private System.Windows.Forms.TableLayoutPanel pResActions;
        private System.Windows.Forms.Button bNext;
        private System.Windows.Forms.Button bPrevious;
        private System.Windows.Forms.DataGridView DGVTasks;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn job;
        private System.Windows.Forms.DataGridViewTextBoxColumn progress;
        private System.Windows.Forms.Panel pResult;
        private System.Windows.Forms.TableLayoutPanel pMain;
        private System.Windows.Forms.Button bIsNew;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lDate;
        private System.Windows.Forms.Label lTaskName;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button bFirst;
        private System.Windows.Forms.Button bLast;
        private System.Windows.Forms.Label lResultNum;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Button bAddTask;
        private System.Windows.Forms.Label lLoadTasks;
        private System.Windows.Forms.ToolStripMenuItem miRunTask;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem miAddTaskByBase;
    }
}

