using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization;
using System.Windows.Forms.DataVisualization.Charting;

namespace JPChart
{
    public partial class JPChart: System.Windows.Forms.DataVisualization.Charting.Chart
    {
        public JPChart()
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
        }

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
            ChartContextZoomResetBtn.Visible = false;
            ChartContextAutoAxesChck.Checked = true;
            ChartContextAutoAxesChck.PerformClick();
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
            CP = new ChartProperties(this);
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

            for (int i = 0; i < XDATA.Length; i++)
                text += XDATA[i].ToString() + "	" + YDATA[i].ToString() + "\r\n";

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
        
        public void PlotXYData(double[] xData, double[] yData, string title, string xLabel, string yLabel, SeriesChartType seriesStyle, string seriesName)
        {
            XDATA = xData;
            YDATA = yData;

            this.SuspendLayout();
            this.Series.Clear();
            this.Series.Add(seriesName);
            this.Series[seriesName].ChartType = seriesStyle;

            for (int i = 0; i < XDATA.Length; i++)
            {
                this.Series[seriesName].Points.AddXY(XDATA[i], YDATA[i]);
                if (XDATA[i] < DATAMINX)
                    DATAMINX = XDATA[i];
                if (XDATA[i] > DATAMAXX)
                    DATAMAXX = XDATA[i];
                if (YDATA[i] < DATAMINY)
                    DATAMINY = YDATA[i];
                if (YDATA[i] > DATAMAXY)
                    DATAMAXY = YDATA[i];
            }

            this.Titles.Clear();
            this.Titles.Add(title);
            this.Titles[0].Text = title;
            this.ChartAreas[0].AxisY.Title = yLabel;
            this.ChartAreas[0].AxisX.Title = xLabel;

            try
            {
                System.Drawing.Font f = new System.Drawing.Font((string)JPChart.GetReg("JPChart", "TitleFontName"), Convert.ToSingle(JPChart.GetReg("JPChart", "TitleFontSize")));//, FontStyle::Bold | FontStyle::Italic);
                this.Titles[0].Font = f;                
                this.Series[seriesName].MarkerColor = System.Drawing.Color.FromArgb((int)JPChart.GetReg("JPChart", "SeriesColour" + seriesName));
                this.Series[seriesName].Color = System.Drawing.Color.FromArgb((int)JPChart.GetReg("JPChart", "SeriesColour" + seriesName));

                if (Convert.ToBoolean(JPChart.GetReg("JPChart", "ApplyTitleFont")))
                {
                    this.ChartAreas[0].AxisX.TitleFont = this.Titles[0].Font;
                    this.ChartAreas[0].AxisY.TitleFont = this.Titles[0].Font;
                }
				else
				{
                    f = new System.Drawing.Font((string)JPChart.GetReg("JPChart", "XAxisFontName"), Convert.ToSingle(JPChart.GetReg("JPChart", "XAxisFontSize")));
                    this.ChartAreas[0].AxisX.TitleFont = f;
                    f = new System.Drawing.Font((string)JPChart.GetReg("JPChart", "YAxisFontName"), Convert.ToSingle(JPChart.GetReg("JPChart", "YAxisFontSize")));
                    this.ChartAreas[0].AxisY.TitleFont = f;
                }
            }
            catch { }            

            if (seriesStyle == SeriesChartType.Point || seriesStyle == SeriesChartType.FastPoint)
            {
                this.Series[seriesName].MarkerStyle = MarkerStyle.Circle;
                this.Series[seriesName].MarkerSize = 5;
                this.Series[seriesName].BorderWidth = 0;
            }
            else if (seriesStyle == SeriesChartType.Line || seriesStyle == SeriesChartType.FastLine || seriesStyle == SeriesChartType.StepLine)
			{
                this.Series[seriesName].MarkerStyle = MarkerStyle.None;
                this.Series[seriesName].BorderWidth = 1;
            }
            else if (seriesStyle == SeriesChartType.Column)
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

        public void AddXYData(double[] x, double[] y, SeriesChartType seriesStyle, string seriesName, Color seriesColor)
        {
            this.SuspendLayout();
            this.RemoveSeries(seriesName);
            this.Series.Add(seriesName);
            this.Series[seriesName].ChartType = seriesStyle;
            for (int i = 0; i < x.Length; i++)
            {
                this.Series[seriesName].Points.AddXY(x[i], y[i]);
                if (x[i] < DATAMINX)
                    DATAMINX = x[i];
                if (x[i] > DATAMAXX)
                    DATAMAXX = x[i];
                if (y[i] < DATAMINY)
                    DATAMINY = y[i];
                if (y[i] > DATAMAXY)
                    DATAMAXY = y[i];
            }

            if (seriesStyle == SeriesChartType.Point || seriesStyle == SeriesChartType.FastPoint)
            {
                this.Series[seriesName].MarkerStyle = MarkerStyle.Circle;
                this.Series[seriesName].MarkerSize = 5;
                this.Series[seriesName].BorderWidth = 0;
            }
            else if (seriesStyle == SeriesChartType.Line || seriesStyle == SeriesChartType.FastLine || seriesStyle == SeriesChartType.StepLine)
            {
                this.Series[seriesName].MarkerStyle = MarkerStyle.None;
                this.Series[seriesName].BorderWidth = 1;
            }
            else if (seriesStyle == SeriesChartType.Column)
            {
                this.Series[seriesName].MarkerStyle = MarkerStyle.None;
                this.Series[seriesName].BorderWidth = 0;
            }
            this.Series[seriesName].Color = seriesColor;
            this.ChartAreas[0].RecalculateAxesScale();
            this.ResumeLayout();
        }        

        void RemoveSeries(int index)
        {
            this.SuspendLayout();
            this.Series.RemoveAt(index);
            this.ResumeLayout();
        }

        void RemoveSeries(string seriesName)
        {
            this.SuspendLayout();
            this.Series.Remove(this.Series.FindByName(seriesName));
            this.ResumeLayout();
        }

        void SetChartTitle(string title)
        {
            this.Titles[0].Text = title;
        }

        void SetAxesLimits(double XMin, double XMax, double YMin, double YMax)
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
                if (expos <= xaxispos && eypos < yaxispos && eypos > 0.66)//-ve y axis, set auto....
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
                    if (ChartContextAutoAxesChck.Checked)
                    {
                        ChartContextTightAxesChck.PerformClick();
                        ChartContextTightAxesChck.Checked = true;
                        ChartContextMenu.Hide();
                        ChartContextPlotAreaMenu.HideDropDown();
                        AXESAUTO = false;
                        AXESTIGHT = true;
                        AXESMAN = false;
                    }
                    else if (ChartContextTightAxesChck.Checked)
                    {
                        ChartContextAutoAxesChck.PerformClick();
                        ChartContextAutoAxesChck.Checked = true;
                        ChartContextMenu.Hide();
                        ChartContextPlotAreaMenu.HideDropDown();
                        AXESAUTO = true;
                        AXESTIGHT = false;
                        AXESMAN = false;
                    }
                    else
                    {
                        ChartContextTightAxesChck.PerformClick();
                        ChartContextTightAxesChck.Checked = true;
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
                HitTestResult result = this.HitTest(e.X, e.Y);

                if (result.ChartElementType == ChartElementType.DataPoint)
                {
                    DataPoint point = this.Series[result.Series.Name].Points[result.PointIndex];

                    ChartToolTip.SetToolTip(this, "X = " + point.XValue.ToString() + "; Y = " + point.YValues[0].ToString());
                    ChartToolTip.Tag = point.XValue.ToString() + "	" + point.YValues[0].ToString();
                    TOOLTIPISSHOWN = true;
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
