using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace Scheduler
{
    public partial class Form1 : Form
    {
        private DateTime current;
        public Form1()
        {
            InitializeComponent();
        }

        private void CalculateBT_Click(object sender, EventArgs e)
        {
            var Gestor = new SchedulerManager();
            var current = new OutData();

            if (TypeLB.SelectedIndex == 0)     // Only one ocurrence
            {
                try
                {
                    Gestor.Type = RecurrenceTypesEnum.Once;
                    Gestor.Periodicity = 0;
                    Gestor.StartDate = StartDateTB.DateTime;
                    Gestor.EndDate = EndDateTB.DateTime;
                    Gestor.CurrentDate = CurrentDateTB.DateTime;

                    current = Gestor.NextOcurrence();

                    NextExecutionTB.Text = current.NextExecutionTime;
                    DescriptionTB.Text = current.Description;
                }
                catch (SchedulerException ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
            } else
            {
                switch (OccursTB.SelectedIndex)
                {
                    case 0:  // Daily Ocurrence
                        try
                        {
                            Gestor.Type = RecurrenceTypesEnum.Daily;
                            Gestor.Periodicity = (int)EveryDayTB.Value;
                            Gestor.StartDate = StartDateTB.DateTime;
                            Gestor.EndDate = EndDateTB.DateTime;
                            Gestor.CurrentDate = CurrentDateTB.DateTime;
                            // Datos Frecuencia horaria
                            if (DailyOccursAtCB.Checked == true)
                            {
                                Gestor.HourlyFrecuency = HourlyFrecuencysEnum.OccursOne;
                                Gestor.HourlyOccursAt = DailyOccursAtTB.Time;
                            }
                            if (DailyOccursEveryCB.Checked == true)
                            {
                                Gestor.HourlyFrecuency = HourlyFrecuencysEnum.OccursEvery;
                                Gestor.HourlyOccursEvery = (int)DailyOccursEveryTB.Value;
                                Gestor.HourlyStartAt = DailyStartingTB.Time;
                                Gestor.HourlyEndAt = this.DailyEndTB.Time;
                            }

                            current = Gestor.NextOcurrence();

                            NextExecutionTB.Text = current.NextExecutionTime;
                            DescriptionTB.Text = current.Description;
                            CurrentDateTB.DateTime = current.OcurrenceDate;
                        }
                        catch (SchedulerException ex)
                        {
                            MessageBox.Show(ex.Message);
                            return;
                        }
                        break;
                    case 1:  // Weekly Ocurrence
                        try
                        {
                            Gestor.Type = RecurrenceTypesEnum.Weekly;
                            Gestor.Periodicity = (int)EveryDayTB.Value;
                            Gestor.StartDate = StartDateTB.DateTime;
                            Gestor.EndDate = EndDateTB.DateTime;
                            Gestor.CurrentDate = CurrentDateTB.DateTime;
                            // Datos Frecuencia horaria
                            if (DailyOccursAtCB.Checked == true)
                            {
                                Gestor.HourlyFrecuency = HourlyFrecuencysEnum.OccursOne;
                                Gestor.HourlyOccursAt = DailyOccursAtTB.Time;
                            }
                            if (DailyOccursEveryCB.Checked == true)
                            {
                                Gestor.HourlyFrecuency = HourlyFrecuencysEnum.OccursEvery;
                                Gestor.HourlyOccursEvery = (int)DailyOccursEveryTB.Value;
                                Gestor.HourlyStartAt = DailyStartingTB.Time;
                                Gestor.HourlyEndAt = this.DailyEndTB.Time;
                            }

                            Gestor.Periodicity = (int)WeekEveryTB.Value;
                            if (MondayCB.Checked)
                                Gestor.WeekDays = (int)WeekDaysEnum.Monday;
                            if (TuesdayCB.Checked)
                                Gestor.WeekDays += (int)WeekDaysEnum.Tuesday;
                            if (WeednesdayCB.Checked)
                                Gestor.WeekDays += (int)WeekDaysEnum.Wednesday;
                            if (ThrusdayCB.Checked)
                                Gestor.WeekDays += (int)WeekDaysEnum.Thursday;
                            if (FridayCB.Checked)
                                Gestor.WeekDays += (int)WeekDaysEnum.Friday;
                            if (SaturdayCB.Checked)
                                Gestor.WeekDays += (int)WeekDaysEnum.Saturday;
                            if (SundayCB.Checked)
                                Gestor.WeekDays += (int)WeekDaysEnum.Sunday;

                            current = Gestor.NextOcurrence();

                            NextExecutionTB.Text = current.NextExecutionTime;
                            DescriptionTB.Text = current.Description;
                            CurrentDateTB.DateTime = current.OcurrenceDate;
                        }
                        catch (SchedulerException ex)
                        {
                            MessageBox.Show(ex.Message);
                            return;
                        }
                        break;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CurrentDateTB.DateTime = DateTime.Today;
        }

        private void DailyEndTB_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void DailyOccursAtTB_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void DailyOccursAtCB_CheckedChanged(object sender, EventArgs e)
        {
            if (DailyOccursAtCB.Checked == true)
            {
                DailyOccursAtTB.Enabled = true;

                DailyOccursEveryCB.Checked = false;
            } else
            {
                DailyOccursAtTB.Enabled = false;
            }
        }

        private void DailyOccursEveryCB_CheckedChanged(object sender, EventArgs e)
        {
            if (DailyOccursEveryCB.Checked == true)
            {

                DailyOccursAtCB.Checked = false;

                DailyOccursEveryTB.Enabled = true;
                DailyOccursEveryCM.Enabled = true;
                DailyStartingTB.Enabled = true;
                DailyEndTB.Enabled = true;

            }
            else
            {
                DailyOccursEveryTB.Enabled = false;

                DailyOccursEveryTB.Enabled = false;
                DailyOccursEveryCM.Enabled = false;
                DailyStartingTB.Enabled = false;
                DailyEndTB.Enabled = false;
            }
        }

        private void DailyOccursEveryCM_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void DailyStartingTB_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {


        }
    }
}
