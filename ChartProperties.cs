using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization;
using System.Windows.Forms.DataVisualization.Charting;

namespace JPChart
{
    public partial class ChartProperties : Form
    {
        public ChartProperties(JPChart chart)
        {
            InitializeComponent();

            CHART = chart;
            CHARTAREA = CHART.ChartAreas[0];
		}

        private void ChartProperties_Load(object sender, EventArgs e)
        {
			try
			{
				ChartPropertiesTab.SelectedIndex = Convert.ToInt32(JPChart.GetReg("JPChart", "ChartPropTabIndex"));
			}
			catch { }

			CHART.SuspendLayout();

			if (CHART.Parent.Text != "")
				this.Text = CHART.Parent.Text;
			ChartTitleTxt.Text = CHART.Titles[0].Text;
			XTitleTxt.Text = CHARTAREA.AxisX.Title;
			YTitleTxt.Text = CHARTAREA.AxisY.Title;
			YAxisMinTxt.Text = CHARTAREA.AxisY.Minimum.ToString();
			XAxisMinTxt.Text = CHARTAREA.AxisX.Minimum.ToString();
			YAxisMaxTxt.Text = CHARTAREA.AxisY.Maximum.ToString();
			XAxisMaxTxt.Text = CHARTAREA.AxisX.Maximum.ToString();

			FontDialogXAxis.Font = CHARTAREA.AxisX.TitleFont;
			FontDialogYAxis.Font = CHARTAREA.AxisY.TitleFont;
			FontDialogTitle.Font = CHART.Titles[0].Font;

			if (CHARTAREA.AxisY.MajorGrid.Enabled)
				YAxisGridChck.Checked = true;
			if (CHARTAREA.AxisX.MajorGrid.Enabled)
				XAxisGridChck.Checked = true;
			if (CHART.Legends.Count > 0 && CHART.Legends[0].Enabled)
				ChartSeriesLegendChck.Checked = true;
			if (CHARTAREA.AxisX.Minimum <= 0)
				XAxisLogarithmicChck.Enabled = false;
			if (CHARTAREA.AxisY.Minimum <= 0)
				YAxisLogarithmicChck.Enabled = false;

			ChartSeriesNumberNameDrop.Items.Clear();
			for (int i = 0; i < CHART.Series.Count; i++)
				ChartSeriesNumberNameDrop.Items.Add(CHART.Series[i].Name);
			ChartSeriesNumberNameDrop.SelectedIndex = 0;

			if (CHART.AXESAUTO || CHART.AXESTIGHT)
			{
				if (CHART.AXESAUTO)
				{
					XAxisMinAutoChck.Checked = true;
					XAxisMinTightChck.Checked = false;
					XAxisMaxAutoChck.Checked = true;
					XAxisMaxTightChck.Checked = false;
					YAxisMinAutoChck.Checked = true;
					YAxisMinTightChck.Checked = false;
					YAxisMaxAutoChck.Checked = true;
					YAxisMaxTightChck.Checked = false;
				}
				else
				{
					XAxisMinAutoChck.Checked = false;
					XAxisMinTightChck.Checked = true;
					XAxisMaxAutoChck.Checked = false;
					XAxisMaxTightChck.Checked = true;
					YAxisMinAutoChck.Checked = false;
					YAxisMinTightChck.Checked = true;
					YAxisMaxAutoChck.Checked = false;
					YAxisMaxTightChck.Checked = true;
				}
				XAxisMinTxt.Enabled = false;
				XAxisMaxTxt.Enabled = false;
				YAxisMinTxt.Enabled = false;
				YAxisMaxTxt.Enabled = false;
			}
			else
			{
				XAxisMinAutoChck.Checked = false;
				XAxisMinTightChck.Checked = false;
				XAxisMinTxt.Enabled = true;
				XAxisMaxAutoChck.Checked = false;
				XAxisMaxTightChck.Checked = false;
				XAxisMaxTxt.Enabled = true;
				YAxisMinAutoChck.Checked = false;
				YAxisMinTightChck.Checked = false;
				YAxisMinTxt.Enabled = true;
				YAxisMaxAutoChck.Checked = false;
				YAxisMaxTightChck.Checked = false;
				YAxisMaxTxt.Enabled = true;
			}
			CHART.ResumeLayout();
		}		    

        private void ChartSeriesNumberNameDrop_SelectedIndexChanged(object sender, EventArgs e)
        {
			int seriesindex = ChartSeriesNumberNameDrop.SelectedIndex;

			//marker size
			ChartSeriesMarkerSizeUpD.Value = CHART.Series[seriesindex].MarkerSize;

			//line width
			ChartSeriesLineWidthUpD.Value = CHART.Series[seriesindex].BorderWidth;

			//color
			ChartSeriesColourBtn.ForeColor = CHART.Series[seriesindex].Color;

			//marker type
			if (CHART.Series[seriesindex].MarkerStyle == MarkerStyle.None)
				ChartSeriesMarkerStyleDrop.SelectedIndex = 0;
			if (CHART.Series[seriesindex].MarkerStyle == MarkerStyle.Circle)
				ChartSeriesMarkerStyleDrop.SelectedIndex = 1;
			if (CHART.Series[seriesindex].MarkerStyle == MarkerStyle.Cross)
				ChartSeriesMarkerStyleDrop.SelectedIndex = 2;
			if (CHART.Series[seriesindex].MarkerStyle == MarkerStyle.Diamond)
				ChartSeriesMarkerStyleDrop.SelectedIndex = 3;
			if (CHART.Series[seriesindex].MarkerStyle == MarkerStyle.Square)
				ChartSeriesMarkerStyleDrop.SelectedIndex = 4;
			if (CHART.Series[seriesindex].MarkerStyle == MarkerStyle.Triangle)
				ChartSeriesMarkerStyleDrop.SelectedIndex = 5;

			//plot type
			if (CHART.Series[seriesindex].ChartType == SeriesChartType.Line || CHART.Series[seriesindex].ChartType == SeriesChartType.Point || CHART.Series[seriesindex].ChartType == SeriesChartType.FastLine || CHART.Series[seriesindex].ChartType == SeriesChartType.FastPoint)
				ChartSeriesStyleDrop.SelectedIndex = 0;
			else if (CHART.Series[seriesindex].ChartType == SeriesChartType.StepLine)
				ChartSeriesStyleDrop.SelectedIndex = 1;
			else if (CHART.Series[seriesindex].ChartType == SeriesChartType.Column)
				ChartSeriesStyleDrop.SelectedIndex = 2;
		}

		private void ChartProperties_FormClosing(object sender, FormClosingEventArgs e)
		{

		}

		private void CloseBtn_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void CharPropertiesTab_SelectedIndexChanged(object sender, EventArgs e)
        {
			
		}

		public void YAxisMinTxt_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == System.Windows.Forms.Keys.Enter)
			{
				e.SuppressKeyPress = true;

				try
				{
					double min = Convert.ToDouble(YAxisMinTxt.Text);
					double max = Convert.ToDouble(YAxisMaxTxt.Text);

					if (min >= max)
						return;

					CHARTAREA.AxisY.Minimum = min;
					CHARTAREA.AxisY.Maximum = max;
					CHARTAREA.RecalculateAxesScale();
				}
				catch { }
			}
		}

        public void XAxisMinTxt_KeyDown(object sender, KeyEventArgs e)
        {
			if (e.KeyCode == System.Windows.Forms.Keys.Enter)
			{
				e.SuppressKeyPress = true;

				try
				{
					double min = Convert.ToDouble(XAxisMinTxt.Text);
					double max = Convert.ToDouble(XAxisMaxTxt.Text);

					if (min >= max)
						return;

					CHARTAREA.AxisX.Minimum = min;
					CHARTAREA.AxisX.Maximum = max;
					CHARTAREA.RecalculateAxesScale();
				}
				catch { }
			}
		}

        private void YAxisMinTxt_TextChanged(object sender, EventArgs e)
        {
			double min;
			try
			{
				min = Convert.ToDouble(YAxisMinTxt.Text);
				if (min > 0)
					YAxisLogarithmicChck.Enabled = true;
				else
					YAxisLogarithmicChck.Enabled = false;
			}
			catch { }
		}

        private void XAxisMinTxt_TextChanged(object sender, EventArgs e)
        {
			double min;
			try
			{
				min = Convert.ToDouble(XAxisMinTxt.Text);
				if (min > 0)
					XAxisLogarithmicChck.Enabled = true;
				else
					XAxisLogarithmicChck.Enabled = false;
			}
			catch { }
		}

        private void XAxisMinAutoChck_CheckedChanged(object sender, EventArgs e)
        {
			if (XAxisMinAutoChck.Checked)
			{
				CHART.SuspendLayout();
				CHARTAREA.AxisX.Minimum = Double.NaN;
				CHARTAREA.RecalculateAxesScale();
				CHART.ResumeLayout();
				CHART.AXESAUTO = true;
				CHART.AXESTIGHT = false;
				CHART.AXESMAN = false;

				XAxisMinTightChck.Checked = false;
				XAxisMinTxt.Enabled = false;
			}
			else if (!XAxisMinAutoChck.Checked && !XAxisMinTightChck.Checked)
			{
				XAxisMinTxt.Enabled = true;
				CHART.AXESAUTO = false;
				CHART.AXESTIGHT = false;
				CHART.AXESMAN = true;
			}
			XAxisMinTxt.Text = CHARTAREA.AxisX.Minimum.ToString();
		}
		private void XAxisMaxAutoChck_CheckedChanged(object sender, EventArgs e)
		{
			if (XAxisMaxAutoChck.Checked)
			{
				CHART.SuspendLayout();
				CHARTAREA.AxisX.Maximum = Double.NaN;
				CHARTAREA.RecalculateAxesScale();
				CHART.ResumeLayout();
				CHART.AXESAUTO = true;
				CHART.AXESTIGHT = false;
				CHART.AXESMAN = false;

				XAxisMaxTightChck.Checked = false;
				XAxisMaxTxt.Enabled = false;
			}
			else if (!XAxisMaxAutoChck.Checked && !XAxisMaxTightChck.Checked)
			{
				XAxisMaxTxt.Enabled = true;
				CHART.AXESAUTO = false;
				CHART.AXESTIGHT = false;
				CHART.AXESMAN = true;
			}
			XAxisMaxTxt.Text = CHARTAREA.AxisX.Maximum.ToString();
		}

		private void YAxisMinAutoChck_CheckedChanged(object sender, EventArgs e)
        {
			if (YAxisMinAutoChck.Checked)
			{
				CHART.SuspendLayout();
				CHARTAREA.AxisY.Minimum = Double.NaN;
				CHARTAREA.RecalculateAxesScale();
				CHART.ResumeLayout();
				CHART.AXESAUTO = true;
				CHART.AXESTIGHT = false;
				CHART.AXESMAN = false;

				YAxisMinTightChck.Checked = false;
				YAxisMinTxt.Enabled = false;
			}
			else if (!YAxisMinAutoChck.Checked && !YAxisMinTightChck.Checked)
			{
				YAxisMinTxt.Enabled = true;
				CHART.AXESAUTO = false;
				CHART.AXESTIGHT = false;
				CHART.AXESMAN = true;
			}
			YAxisMinTxt.Text = CHARTAREA.AxisY.Minimum.ToString();
		}

        private void YAxisMaxAutoChck_CheckedChanged(object sender, EventArgs e)
        {
			if (YAxisMaxAutoChck.Checked)
			{
				CHART.SuspendLayout();
				CHARTAREA.AxisY.Maximum = Double.NaN;
				CHARTAREA.RecalculateAxesScale();
				CHART.ResumeLayout();
				CHART.AXESAUTO = true;
				CHART.AXESTIGHT = false;
				CHART.AXESMAN = false;

				YAxisMaxTightChck.Checked = false;
				YAxisMaxTxt.Enabled = false;
			}
			else if (!YAxisMaxAutoChck.Checked && !YAxisMaxTightChck.Checked)
			{
				YAxisMaxTxt.Enabled = true;
				CHART.AXESAUTO = false;
				CHART.AXESTIGHT = false;
				CHART.AXESMAN = true;
			}
			YAxisMaxTxt.Text = CHARTAREA.AxisY.Maximum.ToString();
		}

		private void XAxisMaxTightChck_CheckedChanged(object sender, EventArgs e)
		{
			if (XAxisMaxTightChck.Checked)
			{
				CHART.SuspendLayout();
				CHARTAREA.AxisX.Maximum = CHART.DATAMAXX;
				CHARTAREA.RecalculateAxesScale();
				CHART.ResumeLayout();
				CHART.AXESAUTO = false;
				CHART.AXESTIGHT = true;
				CHART.AXESMAN = false;

				XAxisMaxAutoChck.Checked = false;
				XAxisMaxTxt.Enabled = false;				
			}
			else if (!XAxisMaxAutoChck.Checked && !XAxisMaxTightChck.Checked)
			{
				XAxisMaxTxt.Enabled = true;
				CHART.AXESAUTO = false;
				CHART.AXESTIGHT = false;
				CHART.AXESMAN = true;
			}
			XAxisMaxTxt.Text = CHART.ChartAreas[0].AxisX.Maximum.ToString();
		}		

        private void XAxisMinTightChck_CheckedChanged(object sender, EventArgs e)
        {
			if (XAxisMinTightChck.Checked)
			{
				CHART.SuspendLayout();
				CHARTAREA.AxisX.Minimum = CHART.DATAMINX;
				CHARTAREA.RecalculateAxesScale();
				CHART.ResumeLayout();
				CHART.AXESAUTO = false;
				CHART.AXESTIGHT = true;
				CHART.AXESMAN = false;

				XAxisMinAutoChck.Checked = false;
				XAxisMinTxt.Enabled = false;
			}
			else if (!XAxisMinAutoChck.Checked && !XAxisMinTightChck.Checked)
			{
				XAxisMinTxt.Enabled = true;
				CHART.AXESAUTO = false;
				CHART.AXESTIGHT = false;
				CHART.AXESMAN = true;
			}
			XAxisMinTxt.Text = CHART.ChartAreas[0].AxisX.Minimum.ToString();
		}
		
		private void YAxisMinTightChck_CheckedChanged(object sender, EventArgs e)
        {
			if (YAxisMinTightChck.Checked)
			{
				CHART.SuspendLayout();
				CHARTAREA.AxisY.Minimum = CHART.DATAMINY;
				CHARTAREA.RecalculateAxesScale();
				CHART.ResumeLayout();
				CHART.AXESAUTO = false;
				CHART.AXESTIGHT = true;
				CHART.AXESMAN = false;

				YAxisMinAutoChck.Checked = false;
				YAxisMinTxt.Enabled = false;
			}
			else if (!YAxisMinAutoChck.Checked && !YAxisMinTightChck.Checked)
			{
				YAxisMinTxt.Enabled = true;
				CHART.AXESAUTO = false;
				CHART.AXESTIGHT = false;
				CHART.AXESMAN = true;
			}
			YAxisMinTxt.Text = CHART.ChartAreas[0].AxisY.Minimum.ToString();
		}
		private void YAxisMaxTightChck_CheckedChanged(object sender, EventArgs e)
		{
			if (YAxisMaxTightChck.Checked)
			{
				CHART.SuspendLayout();
				CHARTAREA.AxisY.Maximum = CHART.DATAMAXY;
				CHARTAREA.RecalculateAxesScale();
				CHART.ResumeLayout();
				CHART.AXESAUTO = false;
				CHART.AXESTIGHT = true;
				CHART.AXESMAN = false;

				YAxisMaxAutoChck.Checked = false;
				YAxisMaxTxt.Enabled = false;
			}
			else if (!YAxisMaxAutoChck.Checked && !YAxisMaxTightChck.Checked)
			{
				YAxisMaxTxt.Enabled = true;
				CHART.AXESAUTO = false;
				CHART.AXESTIGHT = false;
				CHART.AXESMAN = true;
			}
			YAxisMaxTxt.Text = CHART.ChartAreas[0].AxisY.Maximum.ToString();
		}

        private void ChartTitleTxt_KeyDown(object sender, KeyEventArgs e)
        {
			if (e.KeyCode == System.Windows.Forms.Keys.Enter)
			{
				e.SuppressKeyPress = true;
				CHART.Titles[0].Text = ChartTitleTxt.Text;
			}
		}

        private void XTitleTxt_KeyDown(object sender, KeyEventArgs e)
        {
			if (e.KeyCode == System.Windows.Forms.Keys.Enter)
			{
				e.SuppressKeyPress = true;
				CHART.ChartAreas[0].AxisX.Title = XTitleTxt.Text;
			}
		}

        private void YTitleTxt_KeyDown(object sender, KeyEventArgs e)
        {
			if (e.KeyCode == System.Windows.Forms.Keys.Enter)
			{
				e.SuppressKeyPress = true;
				CHART.ChartAreas[0].AxisY.Title = YTitleTxt.Text;
			}
		}

        private void ChartTitleFontBtn_Click(object sender, EventArgs e)
        {
			FontDialogTitle.Font = CHART.Titles[0].Font;
			if (FontDialogTitle.ShowDialog() == DialogResult.OK)
				FontDialogTitle_Apply(sender, e);
		}

        private void FontDialogTitle_Apply(object sender, EventArgs e)
        {
			CHART.Titles[0].Font = FontDialogTitle.Font;
			CHART.Titles[0].ForeColor = FontDialogTitle.Color;
			JPChart.SetReg("JPChart", "TitleFontName", FontDialogTitle.Font.Name);
			JPChart.SetReg("JPChart", "TitleFontSize", FontDialogTitle.Font.Size);
			JPChart.SetReg("JPChart", "TitleFontStyle", FontDialogTitle.Font.Style);
			JPChart.SetReg("JPChart", "TitleFontColour", FontDialogTitle.Color.ToArgb());

			if (ApplyTitleFontAllChck.Checked)
			{
				CHART.ChartAreas[0].AxisX.TitleFont = CHART.Titles[0].Font;
				CHART.ChartAreas[0].AxisY.TitleFont = CHART.Titles[0].Font;
			}
		}

        private void ApplyTitleFontAllChck_CheckedChanged(object sender, EventArgs e)
        {
			if (ApplyTitleFontAllChck.Checked)
			{
				CHART.ChartAreas[0].AxisX.TitleFont = CHART.Titles[0].Font;
				CHART.ChartAreas[0].AxisY.TitleFont = CHART.Titles[0].Font;
			}
			else
			{
				CHART.ChartAreas[0].AxisX.TitleFont = FontDialogXAxis.Font;
				CHART.ChartAreas[0].AxisY.TitleFont = FontDialogYAxis.Font;
			}
			JPChart.SetReg("JPChart", "ApplyTitleFont", ApplyTitleFontAllChck.Checked);
		}

        private void FontBtnXAxis_Click(object sender, EventArgs e)
        {
			FontDialogXAxis.Font = CHART.ChartAreas[0].AxisX.TitleFont;
			if (FontDialogXAxis.ShowDialog() == DialogResult.OK)
				FontDialogXAxis_Apply(sender, e);
		}

        private void FontDialogXAxis_Apply(object sender, EventArgs e)
        {
			CHART.ChartAreas[0].AxisX.TitleFont = FontDialogXAxis.Font;
			CHART.ChartAreas[0].AxisX.TitleForeColor = FontDialogXAxis.Color;
			ApplyTitleFontAllChck.Checked = false;
			JPChart.SetReg("JPChart", "XAxisFontName", FontDialogXAxis.Font.Name);
			JPChart.SetReg("JPChart", "XAxisFontSize", FontDialogXAxis.Font.Size);
			JPChart.SetReg("JPChart", "XAxisFontStyle", FontDialogXAxis.Font.Style);
			JPChart.SetReg("JPChart", "XAxisFontColour", FontDialogXAxis.Color.ToArgb());
		}

        private void FontBtnYAxis_Click(object sender, EventArgs e)
        {
			FontDialogYAxis.Font = CHART.ChartAreas[0].AxisY.TitleFont;
			if (FontDialogYAxis.ShowDialog() == DialogResult.OK)
				FontDialogYAxis_Apply(sender, e);
		}

        private void FontDialogYAxis_Apply(object sender, EventArgs e)
        {
			CHART.ChartAreas[0].AxisY.TitleFont = FontDialogYAxis.Font;
			CHART.ChartAreas[0].AxisY.TitleForeColor = FontDialogYAxis.Color;
			ApplyTitleFontAllChck.Checked = false;
			JPChart.SetReg("JPChart", "YAxisFontName", FontDialogYAxis.Font.Name);
			JPChart.SetReg("JPChart", "YAxisFontSize", FontDialogYAxis.Font.Size);
			JPChart.SetReg("JPChart", "YAxisFontStyle", FontDialogYAxis.Font.Style);
			JPChart.SetReg("JPChart", "YAxisFontColour", FontDialogYAxis.Color.ToArgb());
		}

        private void ChartSeriesStyleDrop_SelectedIndexChanged(object sender, EventArgs e)
        {
			int seriesindex = ChartSeriesNumberNameDrop.SelectedIndex;
			if (seriesindex < 0)
				seriesindex = 0;			

			if (ChartSeriesStyleDrop.SelectedIndex <= 0)//scatter = line &| point
			{
				ChartSeriesMarkerStyleDrop.Enabled = true;
				ChartSeriesMarkerSizeUpD.Enabled = true;
				ChartSeriesLineWidthUpD.Enabled = true;
				CHART.Series[seriesindex].ChartType = SeriesChartType.Point;
				ChartSeriesLineWidthUpD.Value = 0;

				if (ChartSeriesMarkerStyleDrop.SelectedIndex == 0)
					CHART.Series[seriesindex].MarkerStyle = MarkerStyle.None;
				if (ChartSeriesMarkerStyleDrop.SelectedIndex == 1)
					CHART.Series[seriesindex].MarkerStyle = MarkerStyle.Circle;
				if (ChartSeriesMarkerStyleDrop.SelectedIndex == 2)
					CHART.Series[seriesindex].MarkerStyle = MarkerStyle.Cross;
				if (ChartSeriesMarkerStyleDrop.SelectedIndex == 3)
					CHART.Series[seriesindex].MarkerStyle = MarkerStyle.Diamond;
				if (ChartSeriesMarkerStyleDrop.SelectedIndex == 4)
					CHART.Series[seriesindex].MarkerStyle = MarkerStyle.Square;
				if (ChartSeriesMarkerStyleDrop.SelectedIndex == 5)
					CHART.Series[seriesindex].MarkerStyle = MarkerStyle.Triangle;

				CHART.Series[seriesindex].MarkerSize = (int)ChartSeriesMarkerSizeUpD.Value;
				CHART.Series[seriesindex].BorderWidth = (int)ChartSeriesLineWidthUpD.Value;
			}
			else if (ChartSeriesStyleDrop.SelectedIndex == 1)//step line
			{
				ChartSeriesMarkerStyleDrop.Enabled = false;
				ChartSeriesMarkerSizeUpD.Enabled = false;
				ChartSeriesLineWidthUpD.Enabled = true;
				if (ChartSeriesLineWidthUpD.Value == 0)
					ChartSeriesLineWidthUpD.Value = 1;
				CHART.Series[seriesindex].ChartType = SeriesChartType.StepLine;
				CHART.Series[seriesindex].MarkerStyle = MarkerStyle.None;
			}
			else if (ChartSeriesStyleDrop.SelectedIndex == 2)//column
			{
				ChartSeriesMarkerStyleDrop.Enabled = false;
				ChartSeriesMarkerSizeUpD.Enabled = false;
				ChartSeriesLineWidthUpD.Enabled = false;
				ChartSeriesLineWidthUpD.Value = 0;
				CHART.Series[seriesindex].ChartType = SeriesChartType.Column;
				CHART.Series[seriesindex].MarkerStyle = MarkerStyle.None;
				CHART.Series[seriesindex]["PointWidth"] = "1";
			}
		}

		private void ChartSeriesMarkerStyleDrop_SelectedIndexChanged(object sender, EventArgs e)
		{
			int seriesindex = ChartSeriesNumberNameDrop.SelectedIndex;
			if (seriesindex < 0)
				seriesindex = 0;

			if (ChartSeriesMarkerStyleDrop.SelectedIndex == 0)
			{
				CHART.Series[seriesindex].MarkerStyle = MarkerStyle.None;
				ChartSeriesMarkerSizeUpD.Enabled = false;
				if (ChartSeriesStyleDrop.SelectedIndex == 0)
				{
					CHART.Series[seriesindex].ChartType = SeriesChartType.Line;
					if (ChartSeriesLineWidthUpD.Value == 0)
						ChartSeriesLineWidthUpD.Value = 1;
				}
			}
			else
			{
				ChartSeriesMarkerSizeUpD.Enabled = true;
				if (ChartSeriesMarkerStyleDrop.SelectedIndex == 1)
					CHART.Series[seriesindex].MarkerStyle = MarkerStyle.Circle;
				else if (ChartSeriesMarkerStyleDrop.SelectedIndex == 2)
					CHART.Series[seriesindex].MarkerStyle = MarkerStyle.Cross;
				else if (ChartSeriesMarkerStyleDrop.SelectedIndex == 3)
					CHART.Series[seriesindex].MarkerStyle = MarkerStyle.Diamond;
				else if (ChartSeriesMarkerStyleDrop.SelectedIndex == 4)
					CHART.Series[seriesindex].MarkerStyle = MarkerStyle.Square;
				else if (ChartSeriesMarkerStyleDrop.SelectedIndex == 5)
					CHART.Series[seriesindex].MarkerStyle = MarkerStyle.Triangle;
			}
		}

        private void ChartSeriesMarkerSizeUpD_ValueChanged(object sender, EventArgs e)
        {
			int seriesindex = ChartSeriesNumberNameDrop.SelectedIndex;
			if (seriesindex < 0)
				seriesindex = 0;

			CHART.Series[seriesindex].MarkerSize = (int)ChartSeriesMarkerSizeUpD.Value;
		}

        private void ChartSeriesLineWidthUpD_ValueChanged(object sender, EventArgs e)
        {
			int seriesindex = ChartSeriesNumberNameDrop.SelectedIndex;
			if (seriesindex < 0)
				seriesindex = 0;

			if (ChartSeriesStyleDrop.SelectedIndex == 0)
			{
				if (ChartSeriesLineWidthUpD.Value != 0)
					CHART.Series[seriesindex].ChartType = SeriesChartType.Line;
				else
				{
					CHART.Series[seriesindex].ChartType = SeriesChartType.Point;
					if (ChartSeriesMarkerStyleDrop.SelectedIndex == 0)
						ChartSeriesMarkerStyleDrop.SelectedIndex = 1;
				}
			}

			CHART.Series[seriesindex].BorderWidth = (int)ChartSeriesLineWidthUpD.Value;
		}

		private void ChartSeriesColourBtn_Click(object sender, EventArgs e)
		{
			int seriesindex = ChartSeriesNumberNameDrop.SelectedIndex;
			if (seriesindex < 0)
				seriesindex = 0;
			string seriesname = CHART.Series[seriesindex].Name;
			SeriesColourDlg.Color = CHART.Series[seriesindex].MarkerColor;
			if (SeriesColourDlg.ShowDialog() != DialogResult.OK)
				return;

			CHART.Series[seriesindex].MarkerColor = SeriesColourDlg.Color;
			CHART.Series[seriesindex].Color = SeriesColourDlg.Color;
			ChartSeriesColourBtn.ForeColor = CHART.Series[seriesindex].Color;
			JPChart.SetReg("JPChart", "SeriesColour" + seriesname, SeriesColourDlg.Color.ToArgb());
		}

		private void XAxisLogarithmicChck_CheckedChanged(object sender, EventArgs e)
        {
			if (XAxisLogarithmicChck.Checked)
				CHARTAREA.AxisX.IsLogarithmic = true;
			else
				CHARTAREA.AxisX.IsLogarithmic = false;
			CHARTAREA.RecalculateAxesScale();
		}

        private void YAxisLogarithmicChck_CheckedChanged(object sender, EventArgs e)
        {
			if (YAxisLogarithmicChck.Checked)
				CHARTAREA.AxisY.IsLogarithmic = true;
			else
				CHARTAREA.AxisY.IsLogarithmic = false;
			CHARTAREA.RecalculateAxesScale();
		}

        void XAxisGridChck_CheckedChanged(object sender, EventArgs e)
        {
			if (XAxisGridChck.Checked)
				CHARTAREA.AxisX.MajorGrid.Enabled = true;
			else
				CHARTAREA.AxisX.MajorGrid.Enabled = false;
		}

        private void YAxisGridChck_CheckedChanged(object sender, EventArgs e)
        {
			if (YAxisGridChck.Checked)
				CHARTAREA.AxisY.MajorGrid.Enabled = true;
			else
				CHARTAREA.AxisY.MajorGrid.Enabled = false;
		}

        private void ChartSeriesLegendChck_CheckedChanged(object sender, EventArgs e)
        {
			if (ChartSeriesLegendChck.Checked)
			{
				LegendPlacementContext.Enabled = true;
				if (CHART.Legends.Count == 0)
				{
					CHART.Legends.Add(new Legend());
					for (int i = 0; i < CHART.Series.Count; i++)
						CHART.Legends.Add(CHART.Series[i].Name);
				}
				else
                {
					LegendPlacementContext.Enabled = true;
					CHART.Legends[0].Enabled = true;
				}
			}
			else
            {
				LegendPlacementContext.Enabled = false;
				CHART.Legends[0].Enabled = false;
			}
        }

        private void LegendTopBtn_Click(object sender, EventArgs e)
        {
			LegendPlacementContext.Show();
			CHART.Legends[0].Alignment = StringAlignment.Near;

			if (!LegendTopBtn.Checked)
				LegendTopBtn.Checked = true;
			LegendCenterBtn.Checked = false;
			LegendBottomBtn.Checked = false;
		}

        private void LegendCenterBtn_Click(object sender, EventArgs e)
        {
			LegendPlacementContext.Show();
			CHART.Legends[0].Alignment = StringAlignment.Center;

			if (!LegendCenterBtn.Checked)
				LegendCenterBtn.Checked = true;
			LegendTopBtn.Checked = false;
			LegendBottomBtn.Checked = false;
		}

        private void LegendBottomBtn_Click(object sender, EventArgs e)
        {
			LegendPlacementContext.Show();
			CHART.Legends[0].Alignment = StringAlignment.Far;

			if (!LegendBottomBtn.Checked)
				LegendBottomBtn.Checked = true;
			LegendTopBtn.Checked = false;
			LegendCenterBtn.Checked = false;
		}

        private void LegendInsideChck_Click(object sender, EventArgs e)
        {
			LegendPlacementContext.Show();
			if (LegendInsideChck.Checked)
			{
				CHART.Legends[0].DockedToChartArea = CHARTAREA.Name;
				CHART.Legends[0].IsDockedInsideChartArea = true;
			}
			else
				CHART.Legends[0].IsDockedInsideChartArea = false;
		}

        private void LegendDockTopBtn_Click(object sender, EventArgs e)
        {
			LegendPlacementContext.Show();
			CHART.Legends[0].Docking = Docking.Top;

			if (!LegendDockTopBtn.Checked)
				LegendDockTopBtn.Checked = true;

			LegendDockRightBtn.Checked = false;
			LegendDockBottomBtn.Checked = false;
			LegendDockLeftBtn.Checked = false;
		}

        private void LegendDockRightBtn_Click(object sender, EventArgs e)
        {
			LegendPlacementContext.Show();
			CHART.Legends[0].Docking = Docking.Right;

			if (!LegendDockRightBtn.Checked)
				LegendDockRightBtn.Checked = true;

			LegendDockTopBtn.Checked = false;
			LegendDockBottomBtn.Checked = false;
			LegendDockLeftBtn.Checked = false;
		}

        private void LegendDockBottomBtn_Click(object sender, EventArgs e)
        {
			LegendPlacementContext.Show();
			CHART.Legends[0].Docking = Docking.Bottom;

			if (!LegendDockBottomBtn.Checked)
				LegendDockBottomBtn.Checked = true;

			LegendDockRightBtn.Checked = false;
			LegendDockTopBtn.Checked = false;
			LegendDockLeftBtn.Checked = false;
		}

        private void LegendDockLeftBtn_Click(object sender, EventArgs e)
        {
			LegendPlacementContext.Show();
			CHART.Legends[0].Docking = Docking.Left;

			if (!LegendDockLeftBtn.Checked)
				LegendDockLeftBtn.Checked = true;

			LegendDockRightBtn.Checked = false;
			LegendDockBottomBtn.Checked = false;
			LegendDockTopBtn.Checked = false;
		}

		private void SeriesContext_Opening(object sender, CancelEventArgs e)
		{
			SeriesRenameDeleteContext.Items.Clear();

			ToolStripMenuItem deletemenu = new ToolStripMenuItem("Remove:");
			SeriesRenameDeleteContext.Items.Add(deletemenu);

			ToolStripMenuItem renamemenu = new ToolStripMenuItem("Rename:");
			SeriesRenameDeleteContext.Items.Add(renamemenu);

			for (int i = 0; i < CHART.Series.Count; i++)
			{
				ToolStripButton SeriesDeleteBtn = new ToolStripButton();
				SeriesDeleteBtn.Text = CHART.Series[i].Name;
				SeriesDeleteBtn.Tag = i;
				SeriesDeleteBtn.Click += SeriesDeleteBtn_Click;
				deletemenu.DropDownItems.Add(SeriesDeleteBtn);

				ToolStripTextBox SeriesRenameTextBox = new ToolStripTextBox();
				SeriesRenameTextBox.Text = CHART.Series[i].Name;
				SeriesRenameTextBox.Tag = i;
				SeriesRenameTextBox.BackColor = Color.LightGray;
				SeriesRenameTextBox.KeyDown += SeriesRenameTextBox_KeyDown;
				renamemenu.DropDownItems.Add(SeriesRenameTextBox);
			}
		}

		private void SeriesDeleteBtn_Click(object sender, EventArgs e)
		{
			if (CHART.Series.Count == 1)
				return;

			int index = (int)(sender as ToolStripButton).Tag;
			CHART.RemoveSeries(index);
			ChartSeriesNumberNameDrop.Items.Clear();
			for (int i = 0; i < CHART.Series.Count; i++)
				ChartSeriesNumberNameDrop.Items.Add(CHART.Series[i].Name);
			ChartSeriesNumberNameDrop.SelectedIndex = 0;
			ChartSeriesNumberNameDrop.DroppedDown = true;
		}

		private void SeriesRenameTextBox_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				e.SuppressKeyPress = true;

				if ((sender as ToolStripTextBox).Text == "")
				{
					(sender as ToolStripTextBox).Text = ChartSeriesNumberNameDrop.Items[(int)(sender as ToolStripTextBox).Tag].ToString();
					return;
				}

				ChartSeriesNumberNameDrop.Items[(int)(sender as ToolStripTextBox).Tag] = (sender as ToolStripTextBox).Text;
				CHART.Series[(int)(sender as ToolStripTextBox).Tag].Name = (sender as ToolStripTextBox).Text;
				ChartSeriesNumberNameDrop.SelectedIndex = (int)(sender as ToolStripTextBox).Tag;
				ChartSeriesNumberNameDrop.DroppedDown = true;
			}
		}

		private void ChartSeriesFitDrop_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (ChartSeriesFitDrop.SelectedIndex == 0)//polynomial
			{
				SeriesFitNumericUpD.Visible = true;
				SeriesSplineTypeDrop.Visible = false;
				SeriesFitNormalizedChck.Visible = false;
				SeriesFitRobustChck.Visible = true;

				SeriesFitNumericUpD.Minimum = 1;
				SeriesFitNumericUpD.Maximum = 8;
				SeriesFitNumericUpD.Increment = 1;
				SeriesFitNumericUpD.DecimalPlaces = 0;
				SeriesFitNumericUpD.Value = 1;//fit done with value change
			}
			else if (ChartSeriesFitDrop.SelectedIndex == 1)//spline
			{
				SeriesFitNumericUpD.Visible = false;
				SeriesSplineTypeDrop.Visible = true;
				SeriesFitNormalizedChck.Visible = false;
				SeriesFitRobustChck.Visible = false;

				SeriesSplineTypeDrop.Show();


			}
			else if (ChartSeriesFitDrop.SelectedIndex == 2)//smoothing spline
			{
				SeriesFitNumericUpD.Visible = true;
				SeriesSplineTypeDrop.Visible = false;
				SeriesFitNormalizedChck.Visible = false;
				SeriesFitRobustChck.Visible = false;

				SeriesFitNumericUpD.Minimum = 0;
				SeriesFitNumericUpD.Maximum = 1;
				SeriesFitNumericUpD.Increment = 0.01M;
				SeriesFitNumericUpD.DecimalPlaces = 3;
				SeriesFitNumericUpD.Value = 0.1M;


			}
			else if (ChartSeriesFitDrop.SelectedIndex == 3)//radial Gaussian
			{
				SeriesFitNumericUpD.Visible = false;
				SeriesSplineTypeDrop.Visible = false;
				SeriesFitNormalizedChck.Visible = true;
				SeriesFitRobustChck.Visible = false;


			}
			else if (ChartSeriesFitDrop.SelectedIndex == 4)//radial Moffat
			{
				SeriesFitNumericUpD.Visible = false;
				SeriesSplineTypeDrop.Visible = false;
				SeriesFitNormalizedChck.Visible = true;
				SeriesFitRobustChck.Visible = false;


			}
			else if (ChartSeriesFitDrop.SelectedIndex == 5)//Gaussian
			{
				SeriesFitNumericUpD.Visible = false;
				SeriesSplineTypeDrop.Visible = false;
				SeriesFitNormalizedChck.Visible = false;
				SeriesFitRobustChck.Visible = false;


			}
			else if (ChartSeriesFitDrop.SelectedIndex == 6)//Moffat
			{
				SeriesFitNumericUpD.Visible = false;
				SeriesSplineTypeDrop.Visible = false;
				SeriesFitNormalizedChck.Visible = false;
				SeriesFitRobustChck.Visible = false;


			}
			else if (ChartSeriesFitDrop.SelectedIndex == 7)//Power Law
			{
				SeriesFitNumericUpD.Visible = false;
				SeriesSplineTypeDrop.Visible = false;
				SeriesFitNormalizedChck.Visible = false;
				SeriesFitRobustChck.Visible = false;


			}
		}

		private void SeriesFitNumericUpD_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode != Keys.Enter)
				return;

			if (e.KeyCode == Keys.Enter)
				e.Handled = e.SuppressKeyPress = true;

			if (ChartSeriesFitDrop.SelectedIndex == 0)//polynomial
			{
				double[] weights = new double[CHART.XDATA.Length];
				for (int i = 0; i < weights.Length; i++)
					weights[i] = 1.0;

				double[] xc = new double[0];
				double[] yc = new double[0];
				int[] dc = new int[0];
				double[] poly_coeffs;

				int m = (int)(SeriesFitNumericUpD.Value + 1);
				int info;
				alglib.barycentricinterpolant p;
				alglib.polynomialfitreport rep;
				alglib.polynomialfitwc(CHART.XDATA, CHART.YDATA, weights, xc, yc, dc, m, out info, out p, out rep);
				alglib.polynomialbar2pow(p, out poly_coeffs);

				if (SeriesFitRobustChck.Checked)//if robust then determine some weights via the residuals and recalculate solution a few times until some convergence criteria is found
				{
					int iteration_count = 0;
					double sigma = rep.rmserror, yfit, rmsrat = Double.MaxValue;

					while (Math.Abs(rmsrat - 1) > 0.0000001 && iteration_count < 50)
					{
						rmsrat = rep.rmserror;//get the previous rms

						for (int i = 0; i < CHART.XDATA.Length; i++)
						{
							yfit = alglib.barycentriccalc(p, CHART.XDATA[i]);
							weights[i] = Math.Exp(-(CHART.YDATA[i] - yfit) * (CHART.YDATA[i] - yfit) / (2 * sigma * sigma));
							weights[i] *= weights[i];
						}

						alglib.polynomialfitwc(CHART.XDATA, CHART.YDATA, weights, xc, yc, dc, m, out info, out p, out rep);

						sigma = rep.rmserror;
						rmsrat /= rep.rmserror;
						iteration_count++;
					}
					alglib.polynomialbar2pow(p, out poly_coeffs);
				}

				string poly = poly_coeffs[0].ToString();
				for (int i = 1; i < poly_coeffs.Length; i++)
				{
					if (poly_coeffs[i] > 0)
						poly += " + ";
					else
						poly += " - ";

					poly += Math.Abs(poly_coeffs[i]);
					if (i == 1)
						poly += "*x";
					else
						poly += "*x^" + i.ToString();
				}
				//MessageBox.Show(poly);

				double[] xdata = new double[CHART.XDATA.Length];
				for (int i = 0; i < CHART.XDATA.Length; i++)
					xdata[i] = CHART.XDATA[i];
				Array.Sort(xdata);
				double deltax = ((xdata[xdata.Length - 1] - xdata[0]) / 300);
				double[] interpy = new double[300];
				double[] interpx = new double[300];
				for (int i = 0; i < interpx.Length; i++)
				{
					interpx[i] = xdata[0] + (double)i * deltax;
					interpy[i] = alglib.barycentriccalc(p, interpx[i]);
				}					
				string name = "PolyFit" + SeriesFitNumericUpD.Value.ToString();
				CHART.AddXYData(interpx, interpy, SeriesChartType.Line, name, Color.Red);
				ChartSeriesNumberNameDrop.Items.Remove(name);
				ChartSeriesNumberNameDrop.Items.Add(name);
			}
		}

		private void SeriesSplineTypeDrop_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (ChartSeriesFitDrop.SelectedIndex == 1)//spline
			{

			}
		}

		private void SeriesFitNormalizedChck_CheckedChanged(object sender, EventArgs e)
		{
			ChartSeriesFitDrop_SelectedIndexChanged(sender, e);
		}

		
	}
}

