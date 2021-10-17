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
                    case 2:  // Monthly Ocurrence
                        try
                        {
                            Gestor.Type = RecurrenceTypesEnum.Monthly;
                            if (this.MonthlyDayCB.Checked == true)
                            {
                                Gestor.MonthlyFrecuency = MonthlyFrecuencyEnum.FixedDay;
                                Gestor.MonthlyDay = (int)this.MonthlyDayEveryTB.Value;
                                Gestor.Periodicity = (int)this.MonthlyDayEveryMonthsTB.Value;
                            }
                            else
                            {
                                Gestor.MonthlyFrecuency = MonthlyFrecuencyEnum.DayPosition;
                                Gestor.MonthlyDayPosition = (DayPositionEnum)this.MonthlyThePositionCMB.SelectedIndex;
                                switch((int)this.MonthlyTheDayCMB.SelectedIndex)
                                {
                                    case 0:
                                        Gestor.WeekDays = (int)WeekDaysEnum.Monday;
                                        break;
                                    case 1:
                                        Gestor.WeekDays = (int)WeekDaysEnum.Tuesday;
                                        break;
                                    case 2:
                                        Gestor.WeekDays = (int)WeekDaysEnum.Wednesday ;
                                        break;
                                    case 3:
                                        Gestor.WeekDays = (int)WeekDaysEnum.Thursday;
                                        break;
                                    case 4:
                                        Gestor.WeekDays = (int)WeekDaysEnum.Friday ;
                                        break;
                                    case 5:
                                        Gestor.WeekDays = (int)WeekDaysEnum.Saturday;
                                        break;
                                    case 6:
                                        Gestor.WeekDays = (int)WeekDaysEnum.Sunday ;
                                        break;
                                    case 7:
                                        Gestor.WeekDays = (int)WeekDaysEnum.WeekDays;
                                        break;
                                    case 8:
                                        Gestor.WeekDays = (int)WeekDaysEnum.WeekendDays ;
                                        break;
                                    case 9:
                                        Gestor.WeekDays = (int)WeekDaysEnum.Day;
                                        break;
                                }


                                Gestor.Periodicity = (int)this.MonthlyTheEveryMonthsTB.Value;
                            }
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
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CurrentDateTB.DateTime = DateTime.Today;
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

        private void MonthlyDayCB_CheckedChanged(object sender, EventArgs e)
        {
            if (MonthlyDayCB.Checked == true)
            {
                MonthlyDayEveryTB.Enabled = true;
                MonthlyDayEveryMonthsTB.Enabled = true;

                MonthlyTheCB.Checked = false;
            }
            else
            {
                MonthlyDayEveryTB.Enabled = false;
                MonthlyDayEveryMonthsTB.Enabled = false;
            }
        }

        private void MonthlyTheCB_CheckedChanged(object sender, EventArgs e)
        {
            if (MonthlyTheCB.Checked == true)
            {

                MonthlyTheDayCMB.Enabled = true;
                MonthlyThePositionCMB.Enabled = true;
                MonthlyTheEveryMonthsTB.Enabled = true;

                MonthlyDayCB.Checked = false;
            }
            else
            {
                MonthlyTheDayCMB.Enabled = false;
                MonthlyThePositionCMB.Enabled = false;
                MonthlyTheEveryMonthsTB.Enabled = false;
            }
        }
    }
}
