using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection.Emit;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
#nullable enable

namespace JPChart
{
	/// <summary>
	/// Specifies the SeriesChartType from a list of valid options.
	/// </summary>
	public enum SeriesType
	{
		Point = SeriesChartType.Point,

		FastPoint = SeriesChartType.FastPoint,

		Line = SeriesChartType.Line,

		FastLine = SeriesChartType.FastLine,

		StepLine = SeriesChartType.StepLine,

		Column = SeriesChartType.Column
	}

	public partial class JPChartControl: System.Windows.Forms.DataVisualization.Charting.Chart
    {
		#region PRIVATE

		bool DOZOOM;
        bool TOOLTIPISSHOWN;
        int CURRENTMOUSEEX;
        int CURRENTMOUSEEY;
        public bool AXESAUTO, AXESTIGHT, AXESMAN;
        public double DATAMINX, DATAMAXX, DATAMINY, DATAMAXY;
        public double AXESMANMINX, AXESMANMAXX, AXESMANMINY, AXESMANMAXY;
        Color[] PLOTCOLORS;
        int PLOTCOLORINDEX;
        string TOUCHEDSERIES;
        bool TOUCHEDSERIESBOLDED;

		#endregion

		#region CONSTRUCTORS

		public JPChartControl()
        {
            InitializeComponent();

            DOZOOM = false;
            TOOLTIPISSHOWN = false;
            CURRENTMOUSEEX = 0;
            CURRENTMOUSEEY = 0;
            AXESAUTO = true;
            AXESTIGHT = false;
            AXESMAN = false;
            DATAMINX = Double.MaxValue;
            DATAMAXX = Double.MinValue;
            DATAMINY = Double.MaxValue;
            DATAMAXY = Double.MinValue;
			PLOTCOLORS = new Color[] { Color.Black, Color.Blue, Color.Red, Color.Green, Color.DeepPink, Color.Aqua, Color.Crimson, Color.DarkGoldenrod, Color.Chartreuse, Color.HotPink };
            PLOTCOLORINDEX = 0;
            TOUCHEDSERIES = "";
            TOUCHEDSERIESBOLDED = false;

            //this.ChartAreas[0].BorderColor = Color.Black;
			//this.ChartAreas[0].BorderWidth = 2;
		}

		#endregion


		public static void SetReg(string programName, string keyName, object keyValue)
        {
            Microsoft.Win32.RegistryKey user = Microsoft.Win32.Registry.CurrentUser;
            Microsoft.Win32.RegistryKey sw = user.OpenSubKey("Software", true);
            Microsoft.Win32.RegistryKey astrowerks = sw.CreateSubKey("AstroWerks");
            Microsoft.Win32.RegistryKey subkey = astrowerks.CreateSubKey(programName);
            subkey.SetValue(keyName, keyValue);
        }

        public static object GetReg(string programName, string keyName)
        {
            Microsoft.Win32.RegistryKey user = Microsoft.Win32.Registry.CurrentUser;
            Microsoft.Win32.RegistryKey sw = user.OpenSubKey("Software", true);
            Microsoft.Win32.RegistryKey astrowerks = sw.CreateSubKey("AstroWerks");
            Microsoft.Win32.RegistryKey subkey = astrowerks.CreateSubKey(programName);
	        return subkey.GetValue(keyName); ;
        }
        
        private void ChartContextMenu_Opening(object sender, CancelEventArgs e)
        {
            if (AXESAUTO)
            {
                ChartContextAutoAxesChck.Checked = true;
                ChartContextTightAxesChck.Checked = false;
            }
            else if (AXESTIGHT)
            {
                ChartContextAutoAxesChck.Checked = false;
                ChartContextTightAxesChck.Checked = true;
            }
            else if (AXESMAN)
            {
                ChartContextAutoAxesChck.Checked = false;
                ChartContextTightAxesChck.Checked = false;
            }
            if (this.ChartAreas[0].AxisX.MajorGrid.Enabled && this.ChartAreas[0].AxisY.MajorGrid.Enabled)
                ChartContextGridChck.Checked = true;
        }
        
        private void ChartContextZoomBtn_Click(object sender, System.EventArgs e)
        {
            ChartContextZoomResetBtn.Visible = true;
            ChartContextZoomResetBtn.HideDropDown();
            ChartContextMenu.Hide();
            this.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            this.ChartAreas[0].CursorY.IsUserSelectionEnabled = true;
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            DOZOOM = true;
        }
        
        private void ChartContextZoomResetBtn_Click(object sender, System.EventArgs e)
        {
			//ChartContextZoomResetBtn.Visible = false;
			//ChartContextAutoAxesChck.Checked = true;
			//ChartContextAutoAxesChck.PerformClick();

			this.SuspendLayout();
			this.ChartAreas[0].AxisX.ScaleView.ZoomReset();
			this.ChartAreas[0].AxisY.ScaleView.ZoomReset();
			this.ResumeLayout();
		}
        
        private void ChartContextAutoAxesChck_Click(object sender, System.EventArgs e)
        {
            this.SuspendLayout();
            this.ChartAreas[0].AxisY.Minimum = Double.NaN;
            this.ChartAreas[0].AxisX.Minimum = Double.NaN;
            this.ChartAreas[0].AxisY.Maximum = Double.NaN;
            this.ChartAreas[0].AxisX.Maximum = Double.NaN;
            this.ChartAreas[0].RecalculateAxesScale();
            this.ResumeLayout();

            AXESAUTO = true;
            AXESTIGHT = false;
            AXESMAN = false;
            ChartContextAutoAxesChck.Checked = true;
            ChartContextTightAxesChck.Checked = false;
            ChartContextMenu.Show();
            ChartContextPlotAreaMenu.ShowDropDown();
        }
        
        private void ChartContextTightAxesChck_Click(object sender, System.EventArgs e)
        {
            this.SuspendLayout();
            this.ChartAreas[0].AxisY.Minimum = DATAMINY;
            this.ChartAreas[0].AxisX.Minimum = DATAMINX;
            this.ChartAreas[0].AxisY.Maximum = DATAMAXY;
            this.ChartAreas[0].AxisX.Maximum = DATAMAXX;
            this.ChartAreas[0].RecalculateAxesScale();
            this.ResumeLayout();

            AXESAUTO = false;
            AXESTIGHT = true;
            AXESMAN = false;
            ChartContextAutoAxesChck.Checked = false;
            ChartContextTightAxesChck.Checked = true;
            ChartContextMenu.Show();
            ChartContextPlotAreaMenu.ShowDropDown();
        }
        
        private void ChartContextGridChck_Click(object sender, System.EventArgs e)
        {
            this.SuspendLayout();
            if (ChartContextGridChck.Checked)//set grid
            {
                this.ChartAreas[0].AxisX.MajorGrid.Enabled = true;
                this.ChartAreas[0].AxisY.MajorGrid.Enabled = true;
            }
            else
            {
                this.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
                this.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
            }
            this.ResumeLayout();

            ChartContextMenu.Show();
            ChartContextPlotAreaMenu.ShowDropDown();
        }
        
        private void ChartContextPropertiesBtn_Click(object sender, System.EventArgs e)
        {
			ChartProperties CP = new ChartProperties(this);
            CP.ShowDialog();
		}

		private void ChartContextCopyImageBtn_Click(object sender, System.EventArgs e)
        {
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(this.Width, this.Height);
            this.DrawToBitmap(bmp, this.DisplayRectangle);
            System.Windows.Forms.Clipboard.SetImage(bmp);
            MessageBox.Show("Image copied to clipboard.", "Image...");
        }
        
        private void ChartContextCopyDataBtn_Click(object sender, System.EventArgs e)
        {
            string text = "";

            for (int i = 0; i < this.Series[0].Points.Count; i++)
                text += this.Series[0].Points[i].XValue.ToString() + "	" + this.Series[0].Points[i].YValues[0].ToString() + "\r\n";

            System.Windows.Forms.Clipboard.SetText(text);
            MessageBox.Show("Data copied to clipboard.", "Data...");
        }
        
        private void ChartContextSaveAsBtn_Click(object sender, System.EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "JPEG (*.jpg)|*.jpg";
            try
            {
                sfd.InitialDirectory = (string)GetReg("JPChart", "SaveImagePath");
            }
            catch { }

            if (sfd.ShowDialog() == DialogResult.Cancel)
                return;

            string dir = sfd.FileName.Substring(0, sfd.FileName.LastIndexOf("\\") + 1);
            SetReg("JPChart", "SaveImagePath", dir);
            SetReg("JPChart", "SaveImageFilterIndex", sfd.FilterIndex.ToString());

            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(this.Width, this.Height);
            this.DrawToBitmap(bmp, this.DisplayRectangle);
            this.SaveImage(sfd.FileName, ChartImageFormat.Jpeg);
        }

        /// <summary>
        /// Plots a data vector on a chart with several options. Will plot additional vectors by calling with unique seriesName's.
        /// </summary>
        /// <param name="xData">The abscissa. Pass null for an automatic axis with values from 1 to the length of the yData vector.</param>
        /// <param name="yData">The ordinate.</param>
        /// <param name="title">The title of the plot. Pass empty string if not needed.</param>
        /// <param name="xLabel">The label for the x-axis. Pass empty string if not needed.</param>
        /// <param name="yLabel">The label for the y-axis. Pass empty string if not needed.</param>
        /// <param name="seriesStyle">The SeriesChartType.
        /// <param name="seriesName">The name of the series. This should be a unique identifier to keep multiple plots separate. If it already exists, it will be overwritten with the new plot data.</param>
        /// <param name="seriesColor">The color for the data series. Pass null for automatic coloring.</param>
        public void PlotXYData(double[]? xData, double[] yData, string title, string xLabel, string yLabel, SeriesType seriesStyle, string seriesName, Color? seriesColor = null)
        {
            this.SuspendLayout();
			this.RemoveSeries(seriesName);
			this.Series.Add(seriesName);
            this.Series[seriesName].ChartType = (SeriesChartType)seriesStyle;

            if (title != "" || this.Series.Count == 1)
            {
				this.Titles.Clear();
				this.Titles.Add(title);
				this.Titles[0].Text = title;
			}

            if (yLabel != "")
			    this.ChartAreas[0].AxisY.Title = yLabel;

            if (xLabel != "")
			    this.ChartAreas[0].AxisX.Title = xLabel;

			if (seriesColor == null)
            {
				if (PLOTCOLORINDEX == (PLOTCOLORS.Length - 1))
                    PLOTCOLORINDEX = 0;

                seriesColor = PLOTCOLORS[PLOTCOLORINDEX];
                PLOTCOLORINDEX++;
			}
			this.Series[seriesName].Color = (Color)seriesColor;

			if (xData == null)
            {
                xData = new double[yData.Length];
                for (int i = 0; i < xData.Length; i++)
                    xData[i] = (double)(i + 1);
            }

            for (int i = 0; i < yData.Length; i++)
            {
                this.Series[seriesName].Points.AddXY(xData[i], yData[i]);
                if (xData[i] < DATAMINX)
                    DATAMINX = xData[i];
                if (xData[i] > DATAMAXX)
                    DATAMAXX = xData[i];
                if (yData[i] < DATAMINY)
                    DATAMINY = yData[i];
                if (yData[i] > DATAMAXY)
                    DATAMAXY = yData[i];
            }            

            try
            {
                System.Drawing.Font f = new System.Drawing.Font((string)JPChartControl.GetReg("JPChart", "TitleFontName"), Convert.ToSingle(JPChartControl.GetReg("JPChart", "TitleFontSize")));//, FontStyle::Bold | FontStyle::Italic);
                this.Titles[0].Font = f;                

                if (Convert.ToBoolean(JPChartControl.GetReg("JPChart", "ApplyTitleFont")))
                {
                    this.ChartAreas[0].AxisX.TitleFont = this.Titles[0].Font;
                    this.ChartAreas[0].AxisY.TitleFont = this.Titles[0].Font;
                }
				else
				{
                    f = new System.Drawing.Font((string)JPChartControl.GetReg("JPChart", "XAxisFontName"), Convert.ToSingle(JPChartControl.GetReg("JPChart", "XAxisFontSize")));
                    this.ChartAreas[0].AxisX.TitleFont = f;
                    f = new System.Drawing.Font((string)JPChartControl.GetReg("JPChart", "YAxisFontName"), Convert.ToSingle(JPChartControl.GetReg("JPChart", "YAxisFontSize")));
                    this.ChartAreas[0].AxisY.TitleFont = f;
                }
            }
            catch { }

            if ((SeriesChartType)seriesStyle == SeriesChartType.Point || (SeriesChartType)seriesStyle == SeriesChartType.FastPoint)
            {
                this.Series[seriesName].MarkerStyle = MarkerStyle.Circle;
                this.Series[seriesName].MarkerSize = 5;
                this.Series[seriesName].BorderWidth = 0;
            }
            else if ((SeriesChartType)seriesStyle == SeriesChartType.Line || (SeriesChartType)seriesStyle == SeriesChartType.FastLine || (SeriesChartType)seriesStyle == SeriesChartType.StepLine)
			{
                this.Series[seriesName].MarkerStyle = MarkerStyle.None;
                this.Series[seriesName].BorderWidth = 1;
            }
            else if ((SeriesChartType)seriesStyle == SeriesChartType.Column)
            {
                this.Series[seriesName]["PointWidth"] = "1";
                this.Series[seriesName].MarkerStyle = MarkerStyle.None;
                this.Series[seriesName].BorderWidth = 0;
            }

            if (AXESTIGHT)
            {
                this.ChartAreas[0].AxisY.Minimum = DATAMINY;
                this.ChartAreas[0].AxisX.Minimum = DATAMINX;
                this.ChartAreas[0].AxisY.Maximum = DATAMAXY;
                this.ChartAreas[0].AxisX.Maximum = DATAMAXX;
            }
            else if (AXESAUTO)
            {
                this.ChartAreas[0].AxisY.Minimum = Double.NaN;
                this.ChartAreas[0].AxisX.Minimum = Double.NaN;
                this.ChartAreas[0].AxisY.Maximum = Double.NaN;
                this.ChartAreas[0].AxisX.Maximum = Double.NaN;
            }
            else if (AXESMAN)
            {
                this.ChartAreas[0].AxisY.Minimum = AXESMANMINY;
                this.ChartAreas[0].AxisX.Minimum = AXESMANMINX;
                this.ChartAreas[0].AxisY.Maximum = AXESMANMAXY;
                this.ChartAreas[0].AxisX.Maximum = AXESMANMAXX;
            }

            this.ChartAreas[0].RecalculateAxesScale();
            this.ResumeLayout();
        }

		/// <summary>
		/// Add a plot to an existing chart. Keeps existing title and axis labels.
		/// </summary>
		/// <param name="xData">The abscissa. Pass null for an automatic axis with values from 1 to the length of the yData vector.</param>
		/// <param name="yData">The ordinate.</param>
		/// <param name="seriesStyle">The SeriesChartType.
		/// <param name="seriesName">The name of the series. This should be a unique identifier to keep multiple plots separate. If it already exists, it will be overwritten with the new plot data.</param>
		/// <param name="seriesColor">The name of the series. This should be a unique identifier to keep multiple plots separate. If it already exists, it will be overwritten with the new plot data.</param>
		public void AddXYData(double[]? xData, double[] yData, SeriesType seriesStyle, string seriesName, Color? seriesColor = null)
        {
            this.SuspendLayout();
            this.RemoveSeries(seriesName);
            this.Series.Add(seriesName);
            this.Series[seriesName].ChartType = (SeriesChartType)seriesStyle;
			if (seriesColor == null)
			{
				if (PLOTCOLORINDEX == (PLOTCOLORS.Length - 1))
					PLOTCOLORINDEX = 0;

				seriesColor = PLOTCOLORS[PLOTCOLORINDEX];
				PLOTCOLORINDEX++;
			}
			this.Series[seriesName].Color = (Color)seriesColor;

			if (xData == null)
			{
				xData = new double[yData.Length];
				for (int i = 0; i < xData.Length; i++)
					xData[i] = (double)(i + 1);
			}

			for (int i = 0; i < xData.Length; i++)
            {
                this.Series[seriesName].Points.AddXY(xData[i], yData[i]);
                if (xData[i] < DATAMINX)
                    DATAMINX = xData[i];
                if (xData[i] > DATAMAXX)
                    DATAMAXX = xData[i];
                if (yData[i] < DATAMINY)
                    DATAMINY = yData[i];
                if (yData[i] > DATAMAXY)
                    DATAMAXY = yData[i];
            }            

            if ((SeriesChartType)seriesStyle == SeriesChartType.Point || (SeriesChartType)seriesStyle == SeriesChartType.FastPoint)
            {
                this.Series[seriesName].MarkerStyle = MarkerStyle.Circle;
                this.Series[seriesName].MarkerSize = 5;
                this.Series[seriesName].BorderWidth = 0;
            }
            else if ((SeriesChartType)seriesStyle == SeriesChartType.Line || (SeriesChartType)seriesStyle == SeriesChartType.FastLine || (SeriesChartType)seriesStyle == SeriesChartType.StepLine)
            {
                this.Series[seriesName].MarkerStyle = MarkerStyle.None;
                this.Series[seriesName].BorderWidth = 1;
            }
            else if ((SeriesChartType)seriesStyle == SeriesChartType.Column)
            {
				this.Series[seriesName]["PointWidth"] = "1";
				this.Series[seriesName].MarkerStyle = MarkerStyle.None;
                this.Series[seriesName].BorderWidth = 0;
            }           
            this.ChartAreas[0].RecalculateAxesScale();
            this.ResumeLayout();
        }        

        public void RemoveSeries(int index)
        {
            this.SuspendLayout();
            this.Series.RemoveAt(index);
            this.ResumeLayout();
        }

        public void RemoveSeries(string seriesName)
        {
            this.SuspendLayout();
            this.Series.Remove(this.Series.FindByName(seriesName));
            this.ResumeLayout();
        }

        public void RemoveAllSeries()
        {
			this.Series.Clear();
		}

        public void SetTitle(string title)
        {
            this.Titles.Clear();
			this.Titles.Add(title);
			this.Titles[0].Text = title;
        }

        public void SetXLabel(string xlabel)
        {
            this.ChartAreas[0].AxisX.Title = xlabel;
        }

        public void SetYLabel(string ylabel)
		{
			this.ChartAreas[0].AxisY.Title = ylabel;
		}

        public void SetAxis_XMin(double xmin)
        {
            this.ChartAreas[0].AxisX.Minimum = xmin;
            AXESMANMINX = xmin;
        }

		public void SetAxis_XMax(double xmax)
		{
            this.ChartAreas[0].AxisX.Maximum = xmax;
			AXESMANMAXX = xmax;
        }

		public void SetAxis_YMin(double ymin)
		{
			this.ChartAreas[0].AxisY.Minimum = ymin;
			AXESMANMINX = ymin;
		}

		public void SetAxis_YMax(double ymax)
		{
			this.ChartAreas[0].AxisY.Maximum = ymax;
			AXESMANMINX = ymax;
		}

		public void SetAxesLimits(double XMin, double XMax, double YMin, double YMax)
        {
            this.SuspendLayout();
            this.ChartAreas[0].AxisX.Minimum = XMin;
            this.ChartAreas[0].AxisX.Maximum = XMax;
            this.ChartAreas[0].AxisY.Minimum = YMin;
            this.ChartAreas[0].AxisY.Maximum = YMax;
            this.ChartAreas[0].RecalculateAxesScale();
            this.ResumeLayout();

            AXESAUTO = false;
            AXESTIGHT = false;
            AXESMAN = true;
            AXESMANMINX = XMin;
            AXESMANMAXX = XMax;
            AXESMANMINY = YMin;
            AXESMANMAXY = YMax;

            double xrange = XMax - XMin;
            double yrange = YMax - YMin;
            double nmajticks = 7;
            double xinterv = xrange / nmajticks;
            double yinterv = yrange / nmajticks;
            double xpow = Math.Floor(Math.Log10(xinterv));
            double xbase = Math.Round(xinterv / Math.Pow(10, xpow));
			xinterv = ROUNDTOHUMANINTERVAL(xbase) * Math.Pow(10, xpow);
			double ypow = Math.Floor(Math.Log10(yinterv));
			double ybase = Math.Round(yinterv / Math.Pow(10, ypow));
			yinterv = ROUNDTOHUMANINTERVAL(ybase) * Math.Pow(10, ypow);

			this.ChartAreas[0].AxisX.Interval = xinterv;
            this.ChartAreas[0].AxisY.Interval = yinterv;

			this.ChartAreas[0].AxisX.IntervalOffset = (-this.ChartAreas[0].AxisX.Minimum) % this.ChartAreas[0].AxisX.Interval;
			this.ChartAreas[0].AxisY.IntervalOffset = (-this.ChartAreas[0].AxisY.Minimum) % this.ChartAreas[0].AxisY.Interval;
		}

		private double ROUNDTOHUMANINTERVAL(double value)
		{
			if (value <= 1)
				return 1;
			else if (value > 1 && value <= 2)
			{
				if (value < 1.25)
					return 1;
				else
					return 2;
			}
			else if (value > 2 && value <= 5)
			{
				if (value < 3.5)
					return 2;
				else
					return 5;
			}
			else if (value > 5 && value <= 10)
			{
				if (value < 7)
					return 5;
				else
					return 10;
			}
			else
				return 25;
		}

		private void JPChart_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                double expos = (double)e.X / (double)this.Width;
                double eypos = (double)(e.Y) / (double)(this.Height);
                double xaxispos = (double)(this.ChartAreas[0].InnerPlotPosition.X) / 100;
                double yaxispos = (double)(this.ChartAreas[0].InnerPlotPosition.Bottom) / 100;

                if (expos < xaxispos)//yaxis
                    SetReg("JPChart", "ChartPropTabIndex", 2);
                else if (eypos > yaxispos)//xaxis
                    SetReg("JPChart", "ChartPropTabIndex", 1);
                else
                    SetReg("JPChart", "ChartPropTabIndex", 0);
            }
        }

        private void JPChart_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            double expos = (double)(e.X) / (double)(this.Width);
            double eypos = (double)(e.Y) / (double)(this.Height);
            double xaxispos = (double)(this.ChartAreas[0].InnerPlotPosition.X) / 100;
            double yaxispos = (double)(this.ChartAreas[0].InnerPlotPosition.Bottom) / 100;
            double xaxisright = (double)(this.ChartAreas[0].Position.Right) / 100;
            double yaxistop = (double)(this.ChartAreas[0].Position.Y) / 100;

            this.SuspendLayout();
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (expos <= xaxispos && eypos < yaxispos && eypos > 0.66)//-ve y axis,
                {
                    this.ChartAreas[0].AxisY.Minimum = Double.NaN;
                    AXESAUTO = false;
                    AXESTIGHT = false;
                    AXESMAN = true;
                    AXESMANMINX = this.ChartAreas[0].AxisX.Minimum;
                    AXESMANMINY = this.ChartAreas[0].AxisY.Minimum;
                    AXESMANMAXX = this.ChartAreas[0].AxisX.Maximum;
                    AXESMANMAXY = this.ChartAreas[0].AxisY.Maximum;
                }
                else if (expos <= xaxispos && eypos < 0.33 && eypos > yaxistop)//+ve y axis
                {
                    this.ChartAreas[0].AxisY.Maximum = Double.NaN;
                    AXESAUTO = false;
                    AXESTIGHT = false;
                    AXESMAN = true;
                    AXESMANMINX = this.ChartAreas[0].AxisX.Minimum;
                    AXESMANMINY = this.ChartAreas[0].AxisY.Minimum;
                    AXESMANMAXX = this.ChartAreas[0].AxisX.Maximum;
                    AXESMANMAXY = this.ChartAreas[0].AxisY.Maximum;
                }
                else if (expos < 0.33 && expos > xaxispos && eypos > yaxispos)//-ve x axis
                {
                    this.ChartAreas[0].AxisX.Minimum = Double.NaN;
                    AXESAUTO = false;
                    AXESTIGHT = false;
                    AXESMAN = true;
                    AXESMANMINX = this.ChartAreas[0].AxisX.Minimum;
                    AXESMANMINY = this.ChartAreas[0].AxisY.Minimum;
                    AXESMANMAXX = this.ChartAreas[0].AxisX.Maximum;
                    AXESMANMAXY = this.ChartAreas[0].AxisY.Maximum;
                }
                else if (expos > 0.66 && expos < xaxisright && eypos > yaxispos)//+ve x axis
                {
                    this.ChartAreas[0].AxisX.Maximum = Double.NaN;
                    AXESAUTO = false;
                    AXESTIGHT = false;
                    AXESMAN = true;
                    AXESMANMINX = this.ChartAreas[0].AxisX.Minimum;
                    AXESMANMINY = this.ChartAreas[0].AxisY.Minimum;
                    AXESMANMAXX = this.ChartAreas[0].AxisX.Maximum;
                    AXESMANMAXY = this.ChartAreas[0].AxisY.Maximum;
                }
                else if (expos > xaxispos && expos < xaxisright && eypos > yaxistop && eypos < yaxispos)//then in chart area - reset all axes
                {
                    if (ChartContextTightAxesChck.Checked)
                    {
                        ChartContextAutoAxesChck.PerformClick();
                        ChartContextMenu.Hide();
                        ChartContextPlotAreaMenu.HideDropDown();
                        AXESAUTO = true;
                        AXESTIGHT = false;
                        AXESMAN = false;
                    }
					else //if (ChartContextAutoAxesChck.Checked) or neither checked
					{
                        ChartContextTightAxesChck.PerformClick();
                        ChartContextMenu.Hide();
                        ChartContextPlotAreaMenu.HideDropDown();
                        AXESAUTO = false;
                        AXESTIGHT = true;
                        AXESMAN = false;
                    }
                }
            }
            this.ChartAreas[0].RecalculateAxesScale();
            this.ResumeLayout();
        }

        private void JPChart_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            double xrange = this.ChartAreas[0].AxisX.Maximum - this.ChartAreas[0].AxisX.Minimum;
            double yrange = this.ChartAreas[0].AxisY.Maximum - this.ChartAreas[0].AxisY.Minimum;

            double expos = (double)(e.X) / (double)(this.Width);
            double eypos = (double)(e.Y) / (double)(this.Height);
            double xaxispos = (double)(this.ChartAreas[0].InnerPlotPosition.X) / 100;
            double yaxispos = (double)(this.ChartAreas[0].InnerPlotPosition.Bottom) / 100;
            double xaxisright = (double)(this.ChartAreas[0].Position.Right) / 100;
            double yaxistop = (double)(this.ChartAreas[0].Position.Y) / 100;

            double xinterval = Math.Pow(10, (double)((int)(Math.Log10(xrange)))) / 20;
            double yinterval = Math.Pow(10, (double)((int)(Math.Log10(yrange)))) / 20;

            this.SuspendLayout();
            if (eypos < yaxispos && eypos > yaxistop && expos > xaxispos && expos < xaxisright)//inside plot area...zoom
            {
				//if (e.Delta > 0)//zoom in
				//{
				//	double relativescalex = (expos - xaxispos) / (xaxisright - xaxispos);
				//	double relativescaley = (eypos - yaxistop) / (yaxispos - yaxistop);
				//	this.ChartAreas[0].AxisX.Minimum += (xinterval * relativescalex);
				//	this.ChartAreas[0].AxisX.Maximum -= (xinterval * (1 - relativescalex));
				//	this.ChartAreas[0].AxisY.Minimum += (yinterval * relativescaley);
				//	this.ChartAreas[0].AxisY.Maximum -= (yinterval * (1 - relativescaley));
				//}
				//else if (e.Delta < 0)//zoom out
				//{
				//	if (!(this.ChartAreas[0].AxisX.Minimum - xinterval <= 0 && this.ChartAreas[0].AxisX.IsLogarithmic)) //because if logarithmic, can't go below = 0
				//		this.ChartAreas[0].AxisX.Minimum -= xinterval;
				//	if (!(this.ChartAreas[0].AxisY.Minimum - yinterval <= 0 && this.ChartAreas[0].AxisY.IsLogarithmic)) //because if logarithmic, can't go below = 0
				//		this.ChartAreas[0].AxisY.Minimum -= yinterval;
				//}


			}
            else if (eypos > yaxispos)//x axis
            {
                if (expos < 0.33 && expos > xaxispos)//-ve x axis
                {
                    if (e.Delta > 0)
                    {
                        if (this.ChartAreas[0].AxisX.Minimum + xinterval < this.ChartAreas[0].AxisX.Maximum)
                            this.ChartAreas[0].AxisX.Minimum += xinterval;
                    }
                    else if (e.Delta < 0)
                    {
                        if (this.ChartAreas[0].AxisX.Minimum - xinterval <= 0 && this.ChartAreas[0].AxisX.IsLogarithmic) //because if logarithmic, can't go below = 0
                            return;
                        this.ChartAreas[0].AxisX.Minimum -= xinterval;
                    }
                    AXESAUTO = false;
                    AXESTIGHT = false;
                    AXESMAN = true;
                    AXESMANMINX = this.ChartAreas[0].AxisX.Minimum;
                    AXESMANMINY = this.ChartAreas[0].AxisY.Minimum;
                    AXESMANMAXX = this.ChartAreas[0].AxisX.Maximum;
                    AXESMANMAXY = this.ChartAreas[0].AxisY.Maximum;
                }
                else if (expos > 0.66 && expos < xaxisright)//+ve x axis
                {
                    if (e.Delta < 0)
                    {
                        if (this.ChartAreas[0].AxisX.Maximum - xinterval > this.ChartAreas[0].AxisX.Minimum)
                            this.ChartAreas[0].AxisX.Maximum -= xinterval;
                    }
                    else if (e.Delta > 0)
                        this.ChartAreas[0].AxisX.Maximum += xinterval;
                    AXESAUTO = false;
                    AXESTIGHT = false;
                    AXESMAN = true;
                    AXESMANMINX = this.ChartAreas[0].AxisX.Minimum;
                    AXESMANMINY = this.ChartAreas[0].AxisY.Minimum;
                    AXESMANMAXX = this.ChartAreas[0].AxisX.Maximum;
                    AXESMANMAXY = this.ChartAreas[0].AxisY.Maximum;
                }
            }
            else if (expos <= xaxispos)//y axis
            {
                if (eypos < yaxispos && eypos > 0.66)//-ve y axis
                {
                    if (e.Delta > 0)
                    {
                        if (this.ChartAreas[0].AxisY.Minimum + yinterval < this.ChartAreas[0].AxisY.Maximum)
                            this.ChartAreas[0].AxisY.Minimum += yinterval;
                    }
                    else if (e.Delta < 0)
                    {
                        if (this.ChartAreas[0].AxisY.Minimum - yinterval <= 0 && this.ChartAreas[0].AxisY.IsLogarithmic) //because if logarithmic, can't go below = 0
                            return;
                        this.ChartAreas[0].AxisY.Minimum -= yinterval;
                    }
                    AXESAUTO = false;
                    AXESTIGHT = false;
                    AXESMAN = true;
                    AXESMANMINX = this.ChartAreas[0].AxisX.Minimum;
                    AXESMANMINY = this.ChartAreas[0].AxisY.Minimum;
                    AXESMANMAXX = this.ChartAreas[0].AxisX.Maximum;
                    AXESMANMAXY = this.ChartAreas[0].AxisY.Maximum;
                }
                else if (eypos < 0.33 && eypos > yaxistop)//+ve y axis
                {
                    if (e.Delta < 0)
                    {
                        if (this.ChartAreas[0].AxisY.Maximum - yinterval > this.ChartAreas[0].AxisY.Minimum)
                            this.ChartAreas[0].AxisY.Maximum -= yinterval;
                    }
                    else if (e.Delta > 0)
                        this.ChartAreas[0].AxisY.Maximum += yinterval;
                    AXESAUTO = false;
                    AXESTIGHT = false;
                    AXESMAN = true;
                    AXESMANMINX = this.ChartAreas[0].AxisX.Minimum;
                    AXESMANMINY = this.ChartAreas[0].AxisY.Minimum;
                    AXESMANMAXX = this.ChartAreas[0].AxisX.Maximum;
                    AXESMANMAXY = this.ChartAreas[0].AxisY.Maximum;
                }
            }
            this.ChartAreas[0].RecalculateAxesScale();
            this.ResumeLayout();
            this.Focus();
        }

        private void JPChart_MouseUp(object sender, MouseEventArgs e)
        {   
            if (DOZOOM)
            {
                DOZOOM = false;

                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    ChartContextMenu.Close();
                    this.ChartAreas[0].CursorX.IsUserSelectionEnabled = false;
                    this.ChartAreas[0].CursorY.IsUserSelectionEnabled = false;
                    this.Cursor = System.Windows.Forms.Cursors.Default;
                    this.ChartAreas[0].CursorX.SelectionStart = Double.NaN;
                    this.ChartAreas[0].CursorY.SelectionStart = Double.NaN;
                    return;
                }

                double xstart = this.ChartAreas[0].CursorX.SelectionStart;
                double xend = this.ChartAreas[0].CursorX.SelectionEnd;
                double ystart = this.ChartAreas[0].CursorY.SelectionStart;
                double yend = this.ChartAreas[0].CursorY.SelectionEnd;

                if (xstart > xend)
                {
                    double temp = xstart;
                    xstart = xend;
                    xend = temp;
                }
                if (ystart > yend)
                {
                    double temp = ystart;
                    ystart = yend;
                    yend = temp;
                }                

                this.SuspendLayout();
                this.ChartAreas[0].AxisX.Minimum = xstart;
                this.ChartAreas[0].AxisX.Maximum = xend;
                this.ChartAreas[0].AxisY.Minimum = ystart;
                this.ChartAreas[0].AxisY.Maximum = yend;
                this.ChartAreas[0].RecalculateAxesScale();
                this.ResumeLayout();

                this.ChartAreas[0].CursorX.IsUserSelectionEnabled = false;
                this.ChartAreas[0].CursorY.IsUserSelectionEnabled = false;
                this.Cursor = System.Windows.Forms.Cursors.Default;
                this.ChartAreas[0].CursorX.SelectionStart = Double.NaN;
                this.ChartAreas[0].CursorY.SelectionStart = Double.NaN;

                AXESAUTO = false;
                AXESTIGHT = false;
                AXESMAN = true;
                AXESMANMINX = this.ChartAreas[0].AxisX.Minimum;
                AXESMANMINY = this.ChartAreas[0].AxisY.Minimum;
                AXESMANMAXX = this.ChartAreas[0].AxisX.Maximum;
                AXESMANMAXY = this.ChartAreas[0].AxisY.Maximum;
                return;
            }

            if (TOOLTIPISSHOWN)
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    ChartContextMenu.Close();
                    ChartToolTip.SetToolTip(this, "Copied data point to clipboard.");
                    System.Windows.Forms.Clipboard.SetText((string)ChartToolTip.Tag);
                }
                return;
            }
        }

        private void JPChart_MouseMove(object sender, MouseEventArgs e)
        {
            if (Math.Abs(CURRENTMOUSEEX - e.X) < 2 && Math.Abs(CURRENTMOUSEEY - e.Y) < 2)
                return;

            CURRENTMOUSEEX = e.X;
            CURRENTMOUSEEY = e.Y;

            if (!DOZOOM && e.Button == MouseButtons.None)
            {
				if (TOUCHEDSERIES != "")
				{
					this.Series[TOUCHEDSERIES].BorderWidth--;
					this.Series[TOUCHEDSERIES].MarkerSize-=2;
					TOUCHEDSERIES = "";
					TOUCHEDSERIESBOLDED = false;
				}

				HitTestResult result = this.HitTest(e.X, e.Y);

                if (result.ChartElementType == ChartElementType.DataPoint)
                {
                    DataPoint point = this.Series[result.Series.Name].Points[result.PointIndex];

                    ChartToolTip.SetToolTip(this, "X = " + point.XValue.ToString() + "; Y = " + point.YValues[0].ToString());
                    ChartToolTip.Tag = point.XValue.ToString() + "	" + point.YValues[0].ToString();
                    TOOLTIPISSHOWN = true;

                    TOUCHEDSERIES = result.Series.Name;
                    if (!TOUCHEDSERIESBOLDED)
                    {
                        this.Series[TOUCHEDSERIES].BorderWidth++;
                        this.Series[TOUCHEDSERIES].MarkerSize+=2;
					}
                    TOUCHEDSERIESBOLDED = true;
				}
                else
                {
                    ChartToolTip.RemoveAll();
                    TOOLTIPISSHOWN = false;                                       
				}				
			}
        }
    }
}
