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

            if (TypeLB.SelectedIndex == 0)
            {
                try
                {
                    current = Gestor.NextOcurrence(RecurrenceTypes.Once, 0, StartDateTB.DateTime, EndDateTB.DateTime,DateTimeTB.DateTime);
                    NextExecutionTB.Text = current.ToString("dd/MM/yyyy HH:mm");
                    DescriptionTB.Text = "Occurs Once.Schedule will be use on " + current.ToString("dd/MM/yyyy") + " at " + current.ToString("HH:mm") + " starting on " + this.StartDateTB.DateTime.ToString("dd/MM/yyyy");
                }
                catch (SchedulerException ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
            }
            else
            {
                try
                {
                    current = Gestor.NextOcurrence(RecurrenceTypes.Daily, (int)EveryTB.Value, StartDateTB.DateTime, EndDateTB.DateTime, CurrentDateTB.DateTime);
                    NextExecutionTB.Text = current.ToString("dd/MM/yyyy HH:mm");
                    DescriptionTB.Text = "Occurs every " + EveryTB.Value.ToString() + " day(s). Schedule will be used on " + current.ToString("dd/MM/yyyy") + " starting on " + this.StartDateTB.DateTime.ToString("dd/MM/yyyy");
                    CurrentDateTB.DateTime = current;
                }
                catch (SchedulerException ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CurrentDateTB.DateTime = DateTime.Today;
        }
    }
}
