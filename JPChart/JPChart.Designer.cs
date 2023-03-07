namespace JPChart
{
    partial class JPChartControl
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

		#region Component Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
        {
			this.components = new System.ComponentModel.Container();
			this.ChartContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.ChartContextZoomBtn = new System.Windows.Forms.ToolStripMenuItem();
			this.ChartContextZoomResetBtn = new System.Windows.Forms.ToolStripMenuItem();
			this.ChartContextPlotAreaMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.ChartContextAutoAxesChck = new System.Windows.Forms.ToolStripMenuItem();
			this.ChartContextTightAxesChck = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.ChartContextGridChck = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.ChartContextPropertiesBtn = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.ChartContextCopyImageBtn = new System.Windows.Forms.ToolStripMenuItem();
			this.ChartContextCopyDataBtn = new System.Windows.Forms.ToolStripMenuItem();
			this.ChartContextSaveAsBtn = new System.Windows.Forms.ToolStripMenuItem();
			this.ChartToolTip = new System.Windows.Forms.ToolTip(this.components);
			this.ChartContextMenu.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
			this.SuspendLayout();
			// 
			// ChartContextMenu
			// 
			this.ChartContextMenu.ImageScalingSize = new System.Drawing.Size(24, 24);
			this.ChartContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ChartContextZoomBtn,
            this.ChartContextPlotAreaMenu,
            this.toolStripSeparator2,
            this.ChartContextPropertiesBtn,
            this.toolStripSeparator3,
            this.ChartContextCopyImageBtn,
            this.ChartContextCopyDataBtn,
            this.ChartContextSaveAsBtn});
			this.ChartContextMenu.Name = "ChartContext";
			this.ChartContextMenu.Size = new System.Drawing.Size(229, 208);
			this.ChartContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.ChartContextMenu_Opening);
			// 
			// ChartContextZoomBtn
			// 
			this.ChartContextZoomBtn.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ChartContextZoomResetBtn});
			this.ChartContextZoomBtn.Name = "ChartContextZoomBtn";
			this.ChartContextZoomBtn.Size = new System.Drawing.Size(228, 32);
			this.ChartContextZoomBtn.Text = "Zoom";
			this.ChartContextZoomBtn.Click += new System.EventHandler(this.ChartContextZoomBtn_Click);
			// 
			// ChartContextZoomResetBtn
			// 
			this.ChartContextZoomResetBtn.Name = "ChartContextZoomResetBtn";
			this.ChartContextZoomResetBtn.Size = new System.Drawing.Size(198, 34);
			this.ChartContextZoomResetBtn.Text = "Reset Axes";
			this.ChartContextZoomResetBtn.Visible = false;
			this.ChartContextZoomResetBtn.Click += new System.EventHandler(this.ChartContextZoomResetBtn_Click);
			// 
			// ChartContextPlotAreaMenu
			// 
			this.ChartContextPlotAreaMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ChartContextAutoAxesChck,
            this.ChartContextTightAxesChck,
            this.toolStripSeparator1,
            this.ChartContextGridChck});
			this.ChartContextPlotAreaMenu.Name = "ChartContextPlotAreaMenu";
			this.ChartContextPlotAreaMenu.Size = new System.Drawing.Size(228, 32);
			this.ChartContextPlotAreaMenu.Text = "Plot Area";
			// 
			// ChartContextAutoAxesChck
			// 
			this.ChartContextAutoAxesChck.Checked = true;
			this.ChartContextAutoAxesChck.CheckOnClick = true;
			this.ChartContextAutoAxesChck.CheckState = System.Windows.Forms.CheckState.Checked;
			this.ChartContextAutoAxesChck.Name = "ChartContextAutoAxesChck";
			this.ChartContextAutoAxesChck.Size = new System.Drawing.Size(196, 34);
			this.ChartContextAutoAxesChck.Text = "Auto Axes";
			this.ChartContextAutoAxesChck.Click += new System.EventHandler(this.ChartContextAutoAxesChck_Click);
			// 
			// ChartContextTightAxesChck
			// 
			this.ChartContextTightAxesChck.CheckOnClick = true;
			this.ChartContextTightAxesChck.Name = "ChartContextTightAxesChck";
			this.ChartContextTightAxesChck.Size = new System.Drawing.Size(196, 34);
			this.ChartContextTightAxesChck.Text = "Tight Axes";
			this.ChartContextTightAxesChck.Click += new System.EventHandler(this.ChartContextTightAxesChck_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(193, 6);
			// 
			// ChartContextGridChck
			// 
			this.ChartContextGridChck.CheckOnClick = true;
			this.ChartContextGridChck.Name = "ChartContextGridChck";
			this.ChartContextGridChck.Size = new System.Drawing.Size(196, 34);
			this.ChartContextGridChck.Text = "Grid";
			this.ChartContextGridChck.Click += new System.EventHandler(this.ChartContextGridChck_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(225, 6);
			// 
			// ChartContextPropertiesBtn
			// 
			this.ChartContextPropertiesBtn.Name = "ChartContextPropertiesBtn";
			this.ChartContextPropertiesBtn.Size = new System.Drawing.Size(228, 32);
			this.ChartContextPropertiesBtn.Text = "Properties";
			this.ChartContextPropertiesBtn.Click += new System.EventHandler(this.ChartContextPropertiesBtn_Click);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(225, 6);
			// 
			// ChartContextCopyImageBtn
			// 
			this.ChartContextCopyImageBtn.Name = "ChartContextCopyImageBtn";
			this.ChartContextCopyImageBtn.Size = new System.Drawing.Size(228, 32);
			this.ChartContextCopyImageBtn.Text = "Copy Chart Image";
			this.ChartContextCopyImageBtn.Click += new System.EventHandler(this.ChartContextCopyImageBtn_Click);
			// 
			// ChartContextCopyDataBtn
			// 
			this.ChartContextCopyDataBtn.Name = "ChartContextCopyDataBtn";
			this.ChartContextCopyDataBtn.Size = new System.Drawing.Size(228, 32);
			this.ChartContextCopyDataBtn.Text = "Copy Chart Data";
			this.ChartContextCopyDataBtn.Click += new System.EventHandler(this.ChartContextCopyDataBtn_Click);
			// 
			// ChartContextSaveAsBtn
			// 
			this.ChartContextSaveAsBtn.Name = "ChartContextSaveAsBtn";
			this.ChartContextSaveAsBtn.Size = new System.Drawing.Size(228, 32);
			this.ChartContextSaveAsBtn.Text = "Save As...";
			this.ChartContextSaveAsBtn.Click += new System.EventHandler(this.ChartContextSaveAsBtn_Click);
			// 
			// ChartToolTip
			// 
			this.ChartToolTip.AutomaticDelay = 125;
			this.ChartToolTip.AutoPopDelay = 5000;
			this.ChartToolTip.InitialDelay = 125;
			this.ChartToolTip.ReshowDelay = 25;
			this.ChartToolTip.UseAnimation = false;
			this.ChartToolTip.UseFading = false;
			// 
			// JPChartControl
			// 
			this.BorderlineColor = System.Drawing.Color.Black;
			this.ContextMenuStrip = this.ChartContextMenu;
			this.Name = "JPChartControl";
			this.Size = new System.Drawing.Size(800, 450);
			this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.JPChart_MouseDoubleClick);
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.JPChart_MouseDown);
			this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.JPChart_MouseMove);
			this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.JPChart_MouseUp);
			this.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.JPChart_MouseWheel);
			this.ChartContextMenu.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this)).EndInit();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip ChartContextMenu;
        private System.Windows.Forms.ToolTip ChartToolTip;
        private System.Windows.Forms.ToolStripMenuItem ChartContextZoomBtn;
        private System.Windows.Forms.ToolStripMenuItem ChartContextPlotAreaMenu;
        public System.Windows.Forms.ToolStripMenuItem ChartContextAutoAxesChck;
        public System.Windows.Forms.ToolStripMenuItem ChartContextTightAxesChck;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        public System.Windows.Forms.ToolStripMenuItem ChartContextGridChck;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        public System.Windows.Forms.ToolStripMenuItem ChartContextPropertiesBtn;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem ChartContextCopyImageBtn;
        private System.Windows.Forms.ToolStripMenuItem ChartContextCopyDataBtn;
        private System.Windows.Forms.ToolStripMenuItem ChartContextSaveAsBtn;
        private System.Windows.Forms.ToolStripMenuItem ChartContextZoomResetBtn;        
	}
}
