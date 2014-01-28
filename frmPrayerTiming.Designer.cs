namespace PrayerTiming
{
    partial class frmPrayerTiming
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPrayerTiming));
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMnuTrayRightClick = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Timer = new System.Windows.Forms.Timer(this.components);
            this.contextMenuStripRightClick = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.clockTypeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.namazToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dayNightToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bothAboveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clockSizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.smallToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mediumToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.largeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.elementsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showSmallLinesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bigLinesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sunHandToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addEventToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addAlarmToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.thisPieColorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.greenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.blueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.yellowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.grayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.blackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.whiteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.selectCustomToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.sendToTrayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMnuTrayRightClick.SuspendLayout();
            this.contextMenuStripRightClick.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIcon
            // 
            this.notifyIcon.ContextMenuStrip = this.contextMnuTrayRightClick;
            resources.ApplyResources(this.notifyIcon, "notifyIcon");
            this.notifyIcon.MouseMove += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseMove);
            this.notifyIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseClick);
            this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseDoubleClick);
            // 
            // contextMnuTrayRightClick
            // 
            this.contextMnuTrayRightClick.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.contextMnuTrayRightClick.Name = "contextMnuRightClick";
            resources.ApplyResources(this.contextMnuTrayRightClick, "contextMnuTrayRightClick");
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            resources.ApplyResources(this.settingsToolStripMenuItem, "settingsToolStripMenuItem");
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            resources.ApplyResources(this.exitToolStripMenuItem, "exitToolStripMenuItem");
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // Timer
            // 
            this.Timer.Interval = 150000;
            this.Timer.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // contextMenuStripRightClick
            // 
            this.contextMenuStripRightClick.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clockTypeToolStripMenuItem,
            this.clockSizeToolStripMenuItem,
            this.elementsToolStripMenuItem,
            this.addEventToolStripMenuItem,
            this.addAlarmToolStripMenuItem,
            this.thisPieColorToolStripMenuItem,
            this.toolStripMenuItem2,
            this.sendToTrayToolStripMenuItem,
            this.exitToolStripMenuItem1});
            this.contextMenuStripRightClick.Name = "contextMenuStripRightClick";
            resources.ApplyResources(this.contextMenuStripRightClick, "contextMenuStripRightClick");
            // 
            // clockTypeToolStripMenuItem
            // 
            this.clockTypeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.namazToolStripMenuItem,
            this.dayNightToolStripMenuItem,
            this.bothAboveToolStripMenuItem});
            this.clockTypeToolStripMenuItem.Name = "clockTypeToolStripMenuItem";
            resources.ApplyResources(this.clockTypeToolStripMenuItem, "clockTypeToolStripMenuItem");
            // 
            // namazToolStripMenuItem
            // 
            this.namazToolStripMenuItem.Name = "namazToolStripMenuItem";
            resources.ApplyResources(this.namazToolStripMenuItem, "namazToolStripMenuItem");
            this.namazToolStripMenuItem.Click += new System.EventHandler(this.ToolStripMenuItem_Click);
            // 
            // dayNightToolStripMenuItem
            // 
            this.dayNightToolStripMenuItem.Name = "dayNightToolStripMenuItem";
            resources.ApplyResources(this.dayNightToolStripMenuItem, "dayNightToolStripMenuItem");
            this.dayNightToolStripMenuItem.Click += new System.EventHandler(this.ToolStripMenuItem_Click);
            // 
            // bothAboveToolStripMenuItem
            // 
            this.bothAboveToolStripMenuItem.Name = "bothAboveToolStripMenuItem";
            resources.ApplyResources(this.bothAboveToolStripMenuItem, "bothAboveToolStripMenuItem");
            this.bothAboveToolStripMenuItem.Click += new System.EventHandler(this.ToolStripMenuItem_Click);
            // 
            // clockSizeToolStripMenuItem
            // 
            this.clockSizeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.smallToolStripMenuItem,
            this.mediumToolStripMenuItem,
            this.largeToolStripMenuItem});
            this.clockSizeToolStripMenuItem.Name = "clockSizeToolStripMenuItem";
            resources.ApplyResources(this.clockSizeToolStripMenuItem, "clockSizeToolStripMenuItem");
            // 
            // smallToolStripMenuItem
            // 
            this.smallToolStripMenuItem.Name = "smallToolStripMenuItem";
            resources.ApplyResources(this.smallToolStripMenuItem, "smallToolStripMenuItem");
            this.smallToolStripMenuItem.Click += new System.EventHandler(this.ToolStripMenuItem_Click);
            // 
            // mediumToolStripMenuItem
            // 
            this.mediumToolStripMenuItem.Name = "mediumToolStripMenuItem";
            resources.ApplyResources(this.mediumToolStripMenuItem, "mediumToolStripMenuItem");
            this.mediumToolStripMenuItem.Click += new System.EventHandler(this.ToolStripMenuItem_Click);
            // 
            // largeToolStripMenuItem
            // 
            this.largeToolStripMenuItem.Name = "largeToolStripMenuItem";
            resources.ApplyResources(this.largeToolStripMenuItem, "largeToolStripMenuItem");
            this.largeToolStripMenuItem.Click += new System.EventHandler(this.ToolStripMenuItem_Click);
            // 
            // elementsToolStripMenuItem
            // 
            this.elementsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showSmallLinesToolStripMenuItem,
            this.bigLinesToolStripMenuItem,
            this.sunHandToolStripMenuItem});
            this.elementsToolStripMenuItem.Name = "elementsToolStripMenuItem";
            resources.ApplyResources(this.elementsToolStripMenuItem, "elementsToolStripMenuItem");
            // 
            // showSmallLinesToolStripMenuItem
            // 
            this.showSmallLinesToolStripMenuItem.Checked = true;
            this.showSmallLinesToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showSmallLinesToolStripMenuItem.Name = "showSmallLinesToolStripMenuItem";
            resources.ApplyResources(this.showSmallLinesToolStripMenuItem, "showSmallLinesToolStripMenuItem");
            this.showSmallLinesToolStripMenuItem.Click += new System.EventHandler(this.ToolStripMenuItem_Click);
            // 
            // bigLinesToolStripMenuItem
            // 
            this.bigLinesToolStripMenuItem.Checked = true;
            this.bigLinesToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.bigLinesToolStripMenuItem.Name = "bigLinesToolStripMenuItem";
            resources.ApplyResources(this.bigLinesToolStripMenuItem, "bigLinesToolStripMenuItem");
            this.bigLinesToolStripMenuItem.Click += new System.EventHandler(this.ToolStripMenuItem_Click);
            // 
            // sunHandToolStripMenuItem
            // 
            this.sunHandToolStripMenuItem.Checked = true;
            this.sunHandToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.sunHandToolStripMenuItem.Name = "sunHandToolStripMenuItem";
            resources.ApplyResources(this.sunHandToolStripMenuItem, "sunHandToolStripMenuItem");
            this.sunHandToolStripMenuItem.Click += new System.EventHandler(this.ToolStripMenuItem_Click);
            // 
            // addEventToolStripMenuItem
            // 
            this.addEventToolStripMenuItem.Name = "addEventToolStripMenuItem";
            resources.ApplyResources(this.addEventToolStripMenuItem, "addEventToolStripMenuItem");
            this.addEventToolStripMenuItem.Click += new System.EventHandler(this.ToolStripMenuItem_Click);
            // 
            // addAlarmToolStripMenuItem
            // 
            this.addAlarmToolStripMenuItem.Name = "addAlarmToolStripMenuItem";
            resources.ApplyResources(this.addAlarmToolStripMenuItem, "addAlarmToolStripMenuItem");
            this.addAlarmToolStripMenuItem.Click += new System.EventHandler(this.ToolStripMenuItem_Click);
            // 
            // thisPieColorToolStripMenuItem
            // 
            this.thisPieColorToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.redToolStripMenuItem,
            this.greenToolStripMenuItem,
            this.blueToolStripMenuItem,
            this.yellowToolStripMenuItem,
            this.grayToolStripMenuItem,
            this.blackToolStripMenuItem,
            this.whiteToolStripMenuItem,
            this.toolStripMenuItem1,
            this.selectCustomToolStripMenuItem});
            this.thisPieColorToolStripMenuItem.Name = "thisPieColorToolStripMenuItem";
            resources.ApplyResources(this.thisPieColorToolStripMenuItem, "thisPieColorToolStripMenuItem");
            // 
            // redToolStripMenuItem
            // 
            this.redToolStripMenuItem.Name = "redToolStripMenuItem";
            resources.ApplyResources(this.redToolStripMenuItem, "redToolStripMenuItem");
            this.redToolStripMenuItem.Click += new System.EventHandler(this.ToolStripMenuItem_Click);
            // 
            // greenToolStripMenuItem
            // 
            this.greenToolStripMenuItem.Name = "greenToolStripMenuItem";
            resources.ApplyResources(this.greenToolStripMenuItem, "greenToolStripMenuItem");
            this.greenToolStripMenuItem.Click += new System.EventHandler(this.ToolStripMenuItem_Click);
            // 
            // blueToolStripMenuItem
            // 
            this.blueToolStripMenuItem.Name = "blueToolStripMenuItem";
            resources.ApplyResources(this.blueToolStripMenuItem, "blueToolStripMenuItem");
            this.blueToolStripMenuItem.Click += new System.EventHandler(this.ToolStripMenuItem_Click);
            // 
            // yellowToolStripMenuItem
            // 
            this.yellowToolStripMenuItem.Name = "yellowToolStripMenuItem";
            resources.ApplyResources(this.yellowToolStripMenuItem, "yellowToolStripMenuItem");
            this.yellowToolStripMenuItem.Click += new System.EventHandler(this.ToolStripMenuItem_Click);
            // 
            // grayToolStripMenuItem
            // 
            this.grayToolStripMenuItem.Name = "grayToolStripMenuItem";
            resources.ApplyResources(this.grayToolStripMenuItem, "grayToolStripMenuItem");
            this.grayToolStripMenuItem.Click += new System.EventHandler(this.ToolStripMenuItem_Click);
            // 
            // blackToolStripMenuItem
            // 
            this.blackToolStripMenuItem.Name = "blackToolStripMenuItem";
            resources.ApplyResources(this.blackToolStripMenuItem, "blackToolStripMenuItem");
            this.blackToolStripMenuItem.Click += new System.EventHandler(this.ToolStripMenuItem_Click);
            // 
            // whiteToolStripMenuItem
            // 
            this.whiteToolStripMenuItem.Name = "whiteToolStripMenuItem";
            resources.ApplyResources(this.whiteToolStripMenuItem, "whiteToolStripMenuItem");
            this.whiteToolStripMenuItem.Click += new System.EventHandler(this.ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            resources.ApplyResources(this.toolStripMenuItem1, "toolStripMenuItem1");
            // 
            // selectCustomToolStripMenuItem
            // 
            this.selectCustomToolStripMenuItem.Name = "selectCustomToolStripMenuItem";
            resources.ApplyResources(this.selectCustomToolStripMenuItem, "selectCustomToolStripMenuItem");
            this.selectCustomToolStripMenuItem.Click += new System.EventHandler(this.ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            resources.ApplyResources(this.toolStripMenuItem2, "toolStripMenuItem2");
            // 
            // sendToTrayToolStripMenuItem
            // 
            this.sendToTrayToolStripMenuItem.Name = "sendToTrayToolStripMenuItem";
            resources.ApplyResources(this.sendToTrayToolStripMenuItem, "sendToTrayToolStripMenuItem");
            this.sendToTrayToolStripMenuItem.Click += new System.EventHandler(this.ToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem1
            // 
            this.exitToolStripMenuItem1.Name = "exitToolStripMenuItem1";
            resources.ApplyResources(this.exitToolStripMenuItem1, "exitToolStripMenuItem1");
            this.exitToolStripMenuItem1.Click += new System.EventHandler(this.ToolStripMenuItem_Click);
            // 
            // frmPrayerTiming
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmPrayerTiming";
            this.ShowIcon = false;
            this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.PrayerTiming_MouseDoubleClick);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.frmPrayerTiming_MouseClick);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseUp);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseMove);
            this.MouseHover += new System.EventHandler(this.Form1_MouseHover);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseDown);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.contextMnuTrayRightClick.ResumeLayout(false);
            this.contextMenuStripRightClick.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.Timer Timer;
        private System.Windows.Forms.ContextMenuStrip contextMnuTrayRightClick;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripRightClick;
        private System.Windows.Forms.ToolStripMenuItem clockTypeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem namazToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dayNightToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bothAboveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem thisPieColorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clockSizeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem smallToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mediumToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem largeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem elementsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showSmallLinesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bigLinesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sunHandToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addEventToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addAlarmToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem redToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem greenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem blueToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem yellowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem grayToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem blackToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem whiteToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem selectCustomToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem sendToTrayToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem1;
    }
}

