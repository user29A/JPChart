using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Threading.Tasks;

namespace JPChart
{
    public partial class ChartProperties : Form
    {
		public JPChartControl CHART;
		public ChartArea CHARTAREA;
		private bool FIRSTLOAD;

		public ChartProperties(JPChartControl chart)
        {
            InitializeComponent();

            CHART = chart;
            CHARTAREA = CHART.ChartAreas[0];
			FIRSTLOAD = true;
		}

        private void ChartProperties_Load(object sender, EventArgs e)
        {
			try
			{
				ChartPropertiesTab.SelectedIndex = Convert.ToInt32(JPChartControl.GetReg("JPChart", "ChartPropTabIndex"));
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
			XAxisFontSizeUpD.Value = (decimal)CHARTAREA.AxisX.LabelStyle.Font.Size;
			YAxisFontSizeUpD.Value = (decimal)CHARTAREA.AxisY.LabelStyle.Font.Size;
			XAxisTickSpacingTextBox.Text = CHARTAREA.AxisX.MajorTickMark.Interval.ToString();
			YAxisTickSpacingTextBox.Text = CHARTAREA.AxisY.MajorTickMark.Interval.ToString();

			CHART.ResumeLayout();

			FIRSTLOAD = false;
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
			JPChartControl.SetReg("JPChart", "TitleFontName", FontDialogTitle.Font.Name);
			JPChartControl.SetReg("JPChart", "TitleFontSize", FontDialogTitle.Font.Size);
			JPChartControl.SetReg("JPChart", "TitleFontStyle", FontDialogTitle.Font.Style);
			JPChartControl.SetReg("JPChart", "TitleFontColour", FontDialogTitle.Color.ToArgb());

			if (ApplyTitleFontAllChck.Checked)
			{
				CHART.ChartAreas[0].AxisX.TitleFont = CHART.Titles[0].Font;
				CHART.ChartAreas[0].AxisY.TitleFont = CHART.Titles[0].Font;
			}
		}

		private void LegendFontBtn_Click(object sender, EventArgs e)
		{
			FontDialogLegend.Font = CHART.Legends[0].Font;
			if (FontDialogLegend.ShowDialog() == DialogResult.OK)
				FontDialogLegend_Apply(sender, e);
		}

		private void FontDialogLegend_Apply(object sender, EventArgs e)
		{
			CHART.Legends[0].Font = FontDialogLegend.Font;
			CHART.Legends[0].ForeColor = FontDialogLegend.Color;
			JPChartControl.SetReg("JPChart", "LegendFontName", FontDialogLegend.Font.Name);
			JPChartControl.SetReg("JPChart", "LegendFontSize", FontDialogLegend.Font.Size);
			JPChartControl.SetReg("JPChart", "LegendFontStyle", FontDialogLegend.Font.Style);
			JPChartControl.SetReg("JPChart", "LegendFontColour", FontDialogLegend.Color.ToArgb());
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
			JPChartControl.SetReg("JPChart", "ApplyTitleFont", ApplyTitleFontAllChck.Checked);
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
			JPChartControl.SetReg("JPChart", "XAxisFontName", FontDialogXAxis.Font.Name);
			JPChartControl.SetReg("JPChart", "XAxisFontSize", FontDialogXAxis.Font.Size);
			JPChartControl.SetReg("JPChart", "XAxisFontStyle", FontDialogXAxis.Font.Style);
			JPChartControl.SetReg("JPChart", "XAxisFontColour", FontDialogXAxis.Color.ToArgb());
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
			JPChartControl.SetReg("JPChart", "YAxisFontName", FontDialogYAxis.Font.Name);
			JPChartControl.SetReg("JPChart", "YAxisFontSize", FontDialogYAxis.Font.Size);
			JPChartControl.SetReg("JPChart", "YAxisFontStyle", FontDialogYAxis.Font.Style);
			JPChartControl.SetReg("JPChart", "YAxisFontColour", FontDialogYAxis.Color.ToArgb());
		}

        private void ChartSeriesStyleDrop_SelectedIndexChanged(object sender, EventArgs e)
        {
			if (FIRSTLOAD)
				return;

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
			JPChartControl.SetReg("JPChart", "SeriesColour" + seriesname, SeriesColourDlg.Color.ToArgb());
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
				ChartSeriesLegendChck.ContextMenuStrip = LegendChckContext;
				toolTip1.SetToolTip(ChartSeriesLegendChck, "Right Click for Options");
				
				if (CHART.Legends.Count == 0)
				{
					CHART.Legends.Add(new Legend());
					for (int i = 0; i < CHART.Series.Count; i++)
						CHART.Legends.Add(CHART.Series[i].Name);
				}
				else
                {
					LegendChckContext.Enabled = true;
					CHART.Legends[0].Enabled = true;
				}
			}
			else
            {
				ChartSeriesLegendChck.ContextMenuStrip = null;
				CHART.Legends[0].Enabled = false;
				toolTip1.SetToolTip(ChartSeriesLegendChck, "");
			}
        }

        private void LegendTopBtn_Click(object sender, EventArgs e)
        {
			LegendChckContext.Show();
			CHART.Legends[0].Alignment = StringAlignment.Near;

			if (!LegendTopBtn.Checked)
				LegendTopBtn.Checked = true;
			LegendCenterBtn.Checked = false;
			LegendBottomBtn.Checked = false;
		}

        private void LegendCenterBtn_Click(object sender, EventArgs e)
        {
			LegendChckContext.Show();
			CHART.Legends[0].Alignment = StringAlignment.Center;

			if (!LegendCenterBtn.Checked)
				LegendCenterBtn.Checked = true;
			LegendTopBtn.Checked = false;
			LegendBottomBtn.Checked = false;
		}

        private void LegendBottomBtn_Click(object sender, EventArgs e)
        {
			LegendChckContext.Show();
			CHART.Legends[0].Alignment = StringAlignment.Far;

			if (!LegendBottomBtn.Checked)
				LegendBottomBtn.Checked = true;
			LegendTopBtn.Checked = false;
			LegendCenterBtn.Checked = false;
		}

        private void LegendInsideChck_Click(object sender, EventArgs e)
        {
			LegendChckContext.Show();
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
			LegendChckContext.Show();
			CHART.Legends[0].Docking = Docking.Top;

			if (!LegendDockTopBtn.Checked)
				LegendDockTopBtn.Checked = true;

			LegendDockRightBtn.Checked = false;
			LegendDockBottomBtn.Checked = false;
			LegendDockLeftBtn.Checked = false;
		}

        private void LegendDockRightBtn_Click(object sender, EventArgs e)
        {
			LegendChckContext.Show();
			CHART.Legends[0].Docking = Docking.Right;

			if (!LegendDockRightBtn.Checked)
				LegendDockRightBtn.Checked = true;

			LegendDockTopBtn.Checked = false;
			LegendDockBottomBtn.Checked = false;
			LegendDockLeftBtn.Checked = false;
		}

        private void LegendDockBottomBtn_Click(object sender, EventArgs e)
        {
			LegendChckContext.Show();
			CHART.Legends[0].Docking = Docking.Bottom;

			if (!LegendDockBottomBtn.Checked)
				LegendDockBottomBtn.Checked = true;

			LegendDockRightBtn.Checked = false;
			LegendDockTopBtn.Checked = false;
			LegendDockLeftBtn.Checked = false;
		}

        private void LegendDockLeftBtn_Click(object sender, EventArgs e)
        {
			LegendChckContext.Show();
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
				ToolStripButton SeriesDeleteBtn = new ToolStripButton
				{
					Text = CHART.Series[i].Name,
					Tag = i
				};
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

			int index = (int)((ToolStripButton)sender).Tag;
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

				if (((ToolStripTextBox)sender).Text == "")
				{
					((ToolStripTextBox)sender).Text = ChartSeriesNumberNameDrop.Items[(int)((ToolStripTextBox)sender).Tag].ToString();
					return;
				}

				for (int i = 0; i < ChartSeriesNumberNameDrop.Items.Count; i++)
					if (i == (int)((ToolStripTextBox)sender).Tag)
						continue;
					else if (((ToolStripTextBox)sender).Text == ChartSeriesNumberNameDrop.Items[i].ToString())
					{
						((ToolStripTextBox)sender).Text = ChartSeriesNumberNameDrop.Items[(int)((ToolStripTextBox)sender).Tag].ToString();
						return;
					}

				ChartSeriesNumberNameDrop.Items[(int)((ToolStripTextBox)sender).Tag] = ((ToolStripTextBox)sender).Text;
				CHART.Series[(int)((ToolStripTextBox)sender).Tag].Name = ((ToolStripTextBox)sender).Text;
				ChartSeriesNumberNameDrop.SelectedIndex = (int)((ToolStripTextBox)sender).Tag;
				ChartSeriesNumberNameDrop.DroppedDown = true;
			}
		}

		private void ChartSeriesFitDrop_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (ChartSeriesFitDrop.SelectedItem.ToString().Equals("Polynomial"))
			{
				SeriesFitNumericUpD.Visible = true;
				SeriesSplineTypeDrop.Visible = false;
				SeriesFitNormalizedChck.Visible = false;
				SeriesFitRobustChck.Visible = true;

				SeriesFitNumericUpD.Minimum = 1;
				SeriesFitNumericUpD.Maximum = 8;
				SeriesFitNumericUpD.Increment = 1;
				SeriesFitNumericUpD.DecimalPlaces = 0;
				SeriesFitNumericUpD.Value = 1;
			}
			else if (ChartSeriesFitDrop.SelectedItem.ToString().Equals("Spline"))//spline
			{
				SeriesFitNumericUpD.Visible = false;
				SeriesSplineTypeDrop.Visible = true;
				SeriesFitNormalizedChck.Visible = false;
				SeriesFitRobustChck.Visible = false;

				SeriesSplineTypeDrop.Items.Clear();
				SeriesSplineTypeDrop.Items.AddRange(new string[] { "Linear", "Cubic" , "Monotone Cubic", "Catmull-Rom", "Akima" });
				SeriesSplineTypeDrop.Show();


			}
			else if (ChartSeriesFitDrop.SelectedItem.ToString().Equals("Smoothing"))//smoothing
			{
				SeriesFitNumericUpD.Visible = true;
				SeriesSplineTypeDrop.Visible = true;
				SeriesFitNormalizedChck.Visible = false;
				SeriesFitRobustChck.Visible = false;

				SeriesSplineTypeDrop.Items.Clear();
				SeriesSplineTypeDrop.Items.AddRange(new string[] { "Simple Moving Average", "Centered Moving Average", "Exponential", "Linear Regression"});
				SeriesSplineTypeDrop.Show();

				
			}
			else if (ChartSeriesFitDrop.SelectedItem.ToString().Equals("Fourier Polynomial"))//Fourier Polynomial
			{
				SeriesFitNumericUpD.Visible = true;
				SeriesSplineTypeDrop.Visible = false;
				SeriesFitNormalizedChck.Visible = false;
				SeriesFitRobustChck.Visible = false;

				SeriesFitNumericUpD.Minimum = 1;
				SeriesFitNumericUpD.Maximum = 20;
				SeriesFitNumericUpD.Increment = 1;
				SeriesFitNumericUpD.DecimalPlaces = 0;
				SeriesFitNumericUpD.Value = 3;


			}


		}

		private void SeriesFitNumericUpD_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode != Keys.Enter)
				return;

			if (e.KeyCode == Keys.Enter)
				e.Handled = e.SuppressKeyPress = true;

			double[] weights = new double[CHART.Series[ChartSeriesNumberNameDrop.SelectedIndex].Points.Count];
			double[] xdata = new double[CHART.Series[ChartSeriesNumberNameDrop.SelectedIndex].Points.Count];
			double[] ydata = new double[CHART.Series[ChartSeriesNumberNameDrop.SelectedIndex].Points.Count];
			double x0 = CHART.Series[ChartSeriesNumberNameDrop.SelectedIndex].Points[0].XValue;
			for (int i = 0; i < xdata.Length; i++)
			{
				weights[i] = 1.0;
				xdata[i] = CHART.Series[ChartSeriesNumberNameDrop.SelectedIndex].Points[i].XValue;
				ydata[i] = CHART.Series[ChartSeriesNumberNameDrop.SelectedIndex].Points[i].YValues[0];
				if (i > 0)
					if (xdata[i] > x0)
						x0 = xdata[i];
					else
					{
						MessageBox.Show("Series abscissa indicates non-monoticity; please sort the series by increasing x-value.", "Error");
						return;
					}
			}

			if (ChartSeriesFitDrop.SelectedItem.ToString().Equals("Polynomial"))//polynomial
			{
				double[] xc = new double[0];
				double[] yc = new double[0];
				int[] dc = new int[0];
				double[] poly_coeffs;

				int m = (int)(SeriesFitNumericUpD.Value + 1);
				int info;
				alglib.barycentricinterpolant p;
				alglib.polynomialfitreport rep;
				alglib.polynomialfitwc(xdata, ydata, weights, xc, yc, dc, m, out info, out p, out rep);
				alglib.polynomialbar2pow(p, out poly_coeffs);

				if (SeriesFitRobustChck.Checked)//if robust then determine some weights via the residuals and recalculate solution a few times until some convergence criteria is found
				{
					int iteration_count = 0;
					double sigma = rep.rmserror, yfit, rmsrat = Double.MaxValue;

					while (Math.Abs(rmsrat - 1) > 0.0000001 && iteration_count < 50)
					{
						rmsrat = rep.rmserror;//get the previous rms

						for (int i = 0; i < xdata.Length; i++)
						{
							yfit = alglib.barycentriccalc(p, ydata[i]);
							weights[i] = Math.Exp(-(ydata[i] - yfit) * (ydata[i] - yfit) / (2 * sigma * sigma));
							//weights[i] *= weights[i];
						}

						alglib.polynomialfitwc(xdata, ydata, weights, xc, yc, dc, m, out info, out p, out rep);

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

				double[] resids = new double[CHART.Series[ChartSeriesNumberNameDrop.SelectedIndex].Points.Count];
				Parallel.For(0, xdata.Length, i =>
				{
					resids[i] = ydata[i] - alglib.barycentriccalc(p, xdata[i]);
				});

				double deltax = (xdata[xdata.Length - 1] - xdata[0]) / 300;
				double[] interpy = new double[300];
				double[] interpx = new double[300];
				for (int i = 0; i < interpx.Length; i++)
				{
					interpx[i] = xdata[0] + (double)i * deltax;
					interpy[i] = alglib.barycentriccalc(p, interpx[i]);
				}
				string name = CHART.Series[ChartSeriesNumberNameDrop.SelectedIndex].Name.ToString() + "PolyFit";
				CHART.AddXYData(interpx, interpy, SeriesType.Line, name, Color.Red);
				CHART.Series[name].Tag = resids;
				ChartSeriesNumberNameDrop.Items.Remove(name);
				ChartSeriesNumberNameDrop.Items.Add(name);
			}

			else if (ChartSeriesFitDrop.SelectedItem.ToString().Equals("Smoothing"))//smoothing
			{
				if (SeriesSplineTypeDrop.SelectedItem.ToString().Contains("Simple"))
					alglib.filtersma(ref ydata, (int)SeriesFitNumericUpD.Value);
				else if (SeriesSplineTypeDrop.SelectedItem.ToString().Contains("Linear"))
					alglib.filterlrma(ref ydata, (int)SeriesFitNumericUpD.Value);
				else if (SeriesSplineTypeDrop.SelectedItem.ToString().Contains("Exponential"))
					alglib.filterema(ref ydata, (double)SeriesFitNumericUpD.Value);
				else if (SeriesSplineTypeDrop.SelectedItem.ToString().Contains("Centered"))
				{
					int rem = 0;
					Math.DivRem((int)SeriesFitNumericUpD.Value, 2, out rem);
					if (rem == 0)
					{
						MessageBox.Show("Centered smoothing kernel '" + SeriesFitNumericUpD.Value + "' not an odd integer.");
						return;
					}

					int kernelHW = ((int)SeriesFitNumericUpD.Value - 1) / 2;
					double val = 0;
					int kern;
					int L = ydata.Length;

					for (int i = 0; i < L; i++)
					{
						val = 0;

						kern = i - kernelHW;
						if (kern < 0)
						{
							kern = kern + kernelHW;
							for (int j = i - kern; j <= i + kern; j++)
								val += CHART.Series[ChartSeriesNumberNameDrop.SelectedIndex].Points[j].YValues[0];
							ydata[i] = val / (2 * kern + 1);
							continue;
						}

						kern = i + kernelHW;
						if (kern > L - 1)
						{
							kern = -(kern - kernelHW - L + 1);
							for (int j = i - kern; j <= i + kern; j++)
								val += CHART.Series[ChartSeriesNumberNameDrop.SelectedIndex].Points[j].YValues[0];
							ydata[i] = val / (2 * kern + 1);
							continue;
						}

						for (int j = i - kernelHW; j <= i + kernelHW; j++)
							val += CHART.Series[ChartSeriesNumberNameDrop.SelectedIndex].Points[j].YValues[0];
						ydata[i] = val / (double)SeriesFitNumericUpD.Value;
					}
				}
				else
				{
					MessageBox.Show("Smoothing type '" + SeriesSplineTypeDrop.SelectedItem.ToString() + "' not valid!");
					return;
				}

				double[] resids = new double[CHART.Series[ChartSeriesNumberNameDrop.SelectedIndex].Points.Count];
				Parallel.For(0, xdata.Length, i =>
				{
					resids[i] = CHART.Series[ChartSeriesNumberNameDrop.SelectedIndex].Points[i].YValues[0] - ydata[i];
				});

				string name = CHART.Series[ChartSeriesNumberNameDrop.SelectedIndex].Name.ToString() + "Smooth";
				CHART.AddXYData(xdata, ydata, SeriesType.Line, name, Color.Red);
				CHART.Series[name].Tag = resids;
				ChartSeriesNumberNameDrop.Items.Remove(name);
				ChartSeriesNumberNameDrop.Items.Add(name);
			}

			else if (ChartSeriesFitDrop.SelectedItem.ToString().Equals("Fourier Polynomial"))//Fourier Polynomial
			{
				double[] p = Fit_FourierPolynomial(xdata, ydata, (int)SeriesFitNumericUpD.Value);

				double deltax = (xdata[xdata.Length - 1] - xdata[0]) / 300;
				double[] interpy = new double[300];
				double[] interpx = new double[300];
				for (int i = 0; i < interpx.Length; i++)
					interpx[i] = xdata[0] + (double)i * deltax;
				interpy = FourierPolynomial(interpx, p);
				string name = CHART.Series[ChartSeriesNumberNameDrop.SelectedIndex].Name.ToString() + "FourierPolyFit";
				CHART.AddXYData(interpx, interpy, SeriesType.Line, name, Color.Red);
				//CHART.Series[name].Tag = resids;
				ChartSeriesNumberNameDrop.Items.Remove(name);
				ChartSeriesNumberNameDrop.Items.Add(name);
			}
		}

		private double[] Fit_FourierPolynomial(double[] xdata, double[] Fdata, int order)
		{
			double[] p = new double[order * 2 + 2];
			double[] p_lbnd = new double[p.Length];
			double[] p_ubnd = new double[p.Length];
			double[] scale = new double[p.Length];

			double mean = 0, min = Double.MaxValue, max = Double.MinValue, amp;
			double[,] x = new double[Fdata.Length, 1];
			for (int i = 0; i < Fdata.Length; i++)
			{
				x[i, 0] = xdata[i];
				mean += Fdata[i];
				if (Fdata[i] > max)
					max = Fdata[i];
				if (Fdata[i] < min)
					min = Fdata[i];
			}
			mean /= (double)Fdata.Length;
			amp = max - min;

			p[p.Length - 2] = mean;
			p_lbnd[p.Length - 2] = mean;
			p_ubnd[p.Length - 2] = mean;
			scale[p.Length - 2] = Math.Abs(mean) + 1;

			for (int i = 0; i < p.Length - 2; i++)
			{
				p[i] = amp;
				p_lbnd[i] = -amp*100;
				p_ubnd[i] = amp * 100;
				scale[i] = amp + 1;

				//MessageBox.Show(p_lbnd[i] + " " + p_ubnd[i] + " " + scale[i] + " " + p[i]);
			}
			double nyq = 0.5 / (xdata[1] - xdata[0]);
			p_ubnd[p.Length - 1] = nyq;
			p_lbnd[p.Length - 1] = nyq / (double)(xdata.Length);
			p[p.Length - 1] = nyq / 2;
			scale[p.Length - 1] = p[p.Length - 1];

			//MessageBox.Show(p[p.Length - 1] + " " + p_lbnd[p.Length - 1] + " " + p_ubnd[p.Length - 1] + " " + scale[p.Length - 1]);

			alglib.ndimensional_pfunc pfunc = new alglib.ndimensional_pfunc(alglib_Fourier_Polynomial);
			alglib.lsfitstate state;
			double epsx = 0.00001;
			double diffstep = 0.0001;
			int maxits = 0;
			alglib.lsfitcreatef(x, Fdata, p, diffstep, out state);
			alglib.lsfitsetcond(state, epsx, maxits);
			alglib.lsfitsetscale(state, scale);
			alglib.lsfitsetbc(state, p_lbnd, p_ubnd);
			alglib.lsfitfit(state, pfunc, null, order);
			int info;
			alglib.lsfitreport report;
			alglib.lsfitresults(state, out info, out p, out report);

			//MessageBox::Show(info.ToString());
			//MessageBox.Show(p[p.Length - 1] + " ");
			MessageBox.Show(p[0] + " " + p[1] + " " + p[2] + " " + p[3]);

			return p;
		}

		private void alglib_Fourier_Polynomial(double[] p, double[] x, ref double val, object order)
		{
			double xsigma2pi = x[0] * p[p.Length - 1] * 2 * Math.PI;

			//MessageBox.Show(xsigma2pi + "");
			//if (x[0] == 0)
			//	MessageBox.Show(p[0] + " " + p[1] + " " + p[2] + " " + p[3]);

			val = p[p.Length - 2];//mean added in once

			for (int i = 0; i < (int)order; i++)
				val += (p[i * 2] * Math.Cos(((double)(i + 1)) * xsigma2pi) + p[i * 2 + 1] * Math.Sin(((double)(i + 1)) * xsigma2pi));

			//MessageBox.Show(val + "");
		}
		private delegate void function_alglib_Fourier_Polynomial_delegate(double[] p, double[] x, ref double val, object order);

		private double[] FourierPolynomial(double[] xdata, double[] poly_coeffs)
		{
			double[] result = new double[xdata.Length];
			double val = 0;
			int order = (poly_coeffs.Length - 2) / 2;

			for (int i = 0; i < xdata.Length; i++)
			{
				alglib_Fourier_Polynomial(poly_coeffs, new double[1] { xdata[i] }, ref val, order);
				result[i] = val;
			}
			return result;
		}

		private void SeriesFitRobustChck_CheckedChanged(object sender, EventArgs e)
		{
			SeriesFitNumericUpD_KeyDown(sender, new KeyEventArgs(Keys.Enter));
		}

		private void SeriesSplineTypeDrop_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (ChartSeriesFitDrop.SelectedIndex == 1)//spline
			{
				double[] xdata = new double[CHART.Series[ChartSeriesNumberNameDrop.SelectedIndex].Points.Count];
				double[] ydata = new double[CHART.Series[ChartSeriesNumberNameDrop.SelectedIndex].Points.Count];
				double x0 = CHART.Series[ChartSeriesNumberNameDrop.SelectedIndex].Points[0].XValue;
				for (int i = 0; i < xdata.Length; i++)
				{
					xdata[i] = CHART.Series[ChartSeriesNumberNameDrop.SelectedIndex].Points[i].XValue;
					ydata[i] = CHART.Series[ChartSeriesNumberNameDrop.SelectedIndex].Points[i].YValues[0];
					if (i > 0)
						if (xdata[i] > x0)
							x0 = xdata[i];
						else
						{
							MessageBox.Show("Series abscissa indicates non-monoticity; please sort the series by increasing x-value.", "Error" + x0 + " " + xdata[i]);
							return;
						}
				}

				alglib.spline1dinterpolant si;
				unsafe
				{
					void* voidInt = (void*)0;
					si = new alglib.spline1dinterpolant(/*voidInt*/);
				}
				if (SeriesSplineTypeDrop.SelectedItem.ToString() == "Linear")
					alglib.spline1dbuildlinear(xdata, ydata, out si);
				else if (SeriesSplineTypeDrop.SelectedItem.ToString() == "Cubic")
					alglib.spline1dbuildcubic(xdata, ydata, out si);
				else if (SeriesSplineTypeDrop.SelectedItem.ToString() == "Monotone Cubic")
					alglib.spline1dbuildmonotone(xdata, ydata, out si);
				else if (SeriesSplineTypeDrop.SelectedItem.ToString() == "Catmull-Rom")
					alglib.spline1dbuildcatmullrom(xdata, ydata, out si);
				else if (SeriesSplineTypeDrop.SelectedItem.ToString() == "Akima")
					alglib.spline1dbuildakima(xdata, ydata, out si);
				else
				{
					MessageBox.Show("Spline type '" + SeriesSplineTypeDrop.SelectedItem.ToString() + "' not valid!");
					return;
				}

				double[] resids = new double[CHART.Series[ChartSeriesNumberNameDrop.SelectedIndex].Points.Count];
				for (int i = 0; i < resids.Length; i++)
					resids[i] = ydata[i] - alglib.spline1dcalc(si, xdata[i]);

				double deltax = (xdata[xdata.Length - 1] - xdata[0]) / 300;
				double[] interpy = new double[300];
				double[] interpx = new double[300];
				for (int i = 0; i < interpx.Length; i++)
				{
					interpx[i] = xdata[0] + (double)i * deltax;
					interpy[i] = alglib.spline1dcalc(si, interpx[i]);
				}
				string name = CHART.Series[ChartSeriesNumberNameDrop.SelectedIndex].Name.ToString() + "Spline";
				CHART.AddXYData(interpx, interpy, SeriesType.Line, name, Color.Red);
				CHART.Series[name].Tag = resids;
				ChartSeriesNumberNameDrop.Items.Remove(name);
				ChartSeriesNumberNameDrop.Items.Add(name);
			}

			else if (ChartSeriesFitDrop.SelectedIndex == 2)//smoothing
			{
				if (SeriesSplineTypeDrop.SelectedItem.ToString().Contains("Simple") || SeriesSplineTypeDrop.SelectedItem.ToString().Contains("Centered") || SeriesSplineTypeDrop.SelectedItem.ToString().Contains("Linear"))
				{
					SeriesFitNumericUpD.Minimum = 1;
					SeriesFitNumericUpD.Maximum = 1000;
					SeriesFitNumericUpD.Increment = 3;
					SeriesFitNumericUpD.DecimalPlaces = 0;
					SeriesFitNumericUpD.Value = 3;
				}
				else if (SeriesSplineTypeDrop.SelectedItem.ToString().Contains("Exponential"))
				{
					SeriesFitNumericUpD.Minimum = 1e-3M;
					SeriesFitNumericUpD.Maximum = 1;
					SeriesFitNumericUpD.Increment = 0.001M;
					SeriesFitNumericUpD.DecimalPlaces = 3;
					SeriesFitNumericUpD.Value = 0.5M;
				}
			}
		}

		private void SeriesFitNormalizedChck_CheckedChanged(object sender, EventArgs e)
		{
			ChartSeriesFitDrop_SelectedIndexChanged(sender, e);
		}

		private void SeriesFitContextCopySolution_Click(object sender, EventArgs e)
		{

		}

		private void SeriesFitContextCopyFitData_Click(object sender, EventArgs e)
		{

		}

		private void XAxisMinorTickChck_CheckedChanged(object sender, EventArgs e)
		{
			if (XAxisMinorTickChck.Checked)
			{
				CHARTAREA.AxisX.MajorTickMark.Size = CHARTAREA.AxisY.MajorTickMark.Size * 2;
				CHARTAREA.AxisX.MinorTickMark.Enabled = true;
				CHARTAREA.AxisX.MinorTickMark.Size = CHARTAREA.AxisX.MajorTickMark.Size / 2;
				CHARTAREA.AxisX.MinorTickMark.IntervalType = DateTimeIntervalType.Number;
				XAxisMinorTickSpacingTextBox.Enabled = true;
				XAxisMinorTickSpacingTextBox.Text = CHARTAREA.AxisX.MinorTickMark.Interval.ToString();
			}
			else
			{
				CHARTAREA.AxisX.MinorTickMark.Enabled = false;
				XAxisMinorTickSpacingTextBox.Enabled = false;
			}
		}

		private void XAxisTickSpacingText_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode != Keys.Enter)
				return;

			if (e.KeyCode == Keys.Enter)
				e.Handled = e.SuppressKeyPress = true;

			try
			{
				CHARTAREA.AxisX.LabelStyle.IntervalType = DateTimeIntervalType.Number;
				CHARTAREA.AxisX.LabelStyle.Interval = Convert.ToDouble(XAxisTickSpacingTextBox.Text);
				CHARTAREA.AxisX.IntervalType = DateTimeIntervalType.Number;
				CHARTAREA.AxisX.MajorTickMark.Interval = Convert.ToDouble(XAxisTickSpacingTextBox.Text);
				CHARTAREA.RecalculateAxesScale();
			}
			catch 
			{
				XAxisTickSpacingTextBox.Text = CHARTAREA.AxisX.MajorTickMark.Interval.ToString();
			}
		}

		private void YAxisMinorTickChck_CheckedChanged(object sender, EventArgs e)
		{
			if (YAxisMinorTickChck.Checked)
			{
				CHARTAREA.AxisY.MinorTickMark.Enabled = true;
				CHARTAREA.AxisY.MinorTickMark.Size = CHARTAREA.AxisY.MajorTickMark.Size / 2;
				CHARTAREA.AxisY.MinorTickMark.IntervalType = DateTimeIntervalType.Number;
				YAxisMinorTickSpacingTextBox.Enabled = true;
				YAxisMinorTickSpacingTextBox.Text = CHARTAREA.AxisY.MinorTickMark.Interval.ToString();
			}
			else
			{
				CHARTAREA.AxisY.MinorTickMark.Enabled = false;
				YAxisMinorTickSpacingTextBox.Enabled = false;
			}
		}

		private void YAxisTickSpacingTextBox_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode != Keys.Enter)
				return;

			if (e.KeyCode == Keys.Enter)
				e.Handled = e.SuppressKeyPress = true;

			try
			{
				CHARTAREA.AxisY.LabelStyle.IntervalType = DateTimeIntervalType.Number;
				CHARTAREA.AxisY.LabelStyle.Interval = Convert.ToDouble(YAxisTickSpacingTextBox.Text);
				CHARTAREA.AxisY.IntervalType = DateTimeIntervalType.Number;
				CHARTAREA.AxisY.MajorTickMark.Interval = Convert.ToDouble(YAxisTickSpacingTextBox.Text);
				CHARTAREA.RecalculateAxesScale();
			}
			catch 
			{
				YAxisTickSpacingTextBox.Text = CHARTAREA.AxisY.MajorTickMark.Interval.ToString();
			}
		}

		private void XAxisMinorTickSpacingTextBox_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode != Keys.Enter)
				return;

			if (e.KeyCode == Keys.Enter)
				e.Handled = e.SuppressKeyPress = true;

			try
			{
				CHARTAREA.AxisX.MinorTickMark.Interval = Convert.ToDouble(XAxisMinorTickSpacingTextBox.Text);
			}
			catch 
			{
				XAxisMinorTickSpacingTextBox.Text = CHARTAREA.AxisX.MinorTickMark.Interval.ToString();
			}
		}

		private void YAxisMinorTickSpacingTextBox_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode != Keys.Enter)
				return;

			if (e.KeyCode == Keys.Enter)
				e.Handled = e.SuppressKeyPress = true;

			try
			{
				CHARTAREA.AxisY.MinorTickMark.Interval = Convert.ToDouble(YAxisMinorTickSpacingTextBox.Text);
			}
			catch 
			{
				YAxisMinorTickSpacingTextBox.Text = CHARTAREA.AxisY.MinorTickMark.Interval.ToString();
			}
		}

		private void XAxisFontSizeUpD_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode != Keys.Enter)
				return;

			if (e.KeyCode == Keys.Enter)
				e.Handled = e.SuppressKeyPress = true;

			CHARTAREA.AxisX.LabelStyle.Font = new System.Drawing.Font(CHARTAREA.AxisX.LabelStyle.Font.FontFamily, (float)XAxisFontSizeUpD.Value, System.Drawing.FontStyle.Regular);
		}

		private void YAxisFontSizeUpD_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode != Keys.Enter)
				return;

			if (e.KeyCode == Keys.Enter)
				e.Handled = e.SuppressKeyPress = true;

			CHARTAREA.AxisY.LabelStyle.Font = new System.Drawing.Font(CHARTAREA.AxisY.LabelStyle.Font.FontFamily, (float)YAxisFontSizeUpD.Value, System.Drawing.FontStyle.Regular);
		}


	}
}

