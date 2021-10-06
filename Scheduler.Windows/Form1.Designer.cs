
namespace Scheduler
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.CalculateBT = new System.Windows.Forms.Button();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.CurrentDateTB = new DevExpress.XtraEditors.DateEdit();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.EveryTB = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.OccursTB = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.DateTimeTB = new DevExpress.XtraEditors.DateEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.TypeLB = new DevExpress.XtraEditors.ComboBoxEdit();
            this.groupControl3 = new DevExpress.XtraEditors.GroupControl();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this.EndDateTB = new DevExpress.XtraEditors.DateEdit();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.StartDateTB = new DevExpress.XtraEditors.DateEdit();
            this.groupControl4 = new DevExpress.XtraEditors.GroupControl();
            this.DescriptionTB = new DevExpress.XtraEditors.MemoEdit();
            this.labelControl10 = new DevExpress.XtraEditors.LabelControl();
            this.NextExecutionTB = new DevExpress.XtraEditors.TextEdit();
            this.labelControl9 = new DevExpress.XtraEditors.LabelControl();
            this.behaviorManager1 = new DevExpress.Utils.Behaviors.BehaviorManager(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CurrentDateTB.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CurrentDateTB.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.EveryTB.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OccursTB.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DateTimeTB.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DateTimeTB.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TypeLB.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).BeginInit();
            this.groupControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.EndDateTB.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EndDateTB.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StartDateTB.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StartDateTB.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl4)).BeginInit();
            this.groupControl4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DescriptionTB.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NextExecutionTB.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.behaviorManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.CalculateBT);
            this.groupControl1.Controls.Add(this.labelControl1);
            this.groupControl1.Controls.Add(this.CurrentDateTB);
            this.groupControl1.Location = new System.Drawing.Point(26, 26);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(537, 67);
            this.groupControl1.TabIndex = 0;
            this.groupControl1.Text = "Input";
            // 
            // CalculateBT
            // 
            this.CalculateBT.Location = new System.Drawing.Point(246, 35);
            this.CalculateBT.Name = "CalculateBT";
            this.CalculateBT.Size = new System.Drawing.Size(270, 23);
            this.CalculateBT.TabIndex = 1;
            this.CalculateBT.Text = "Calculate Next Date";
            this.CalculateBT.UseVisualStyleBackColor = true;
            this.CalculateBT.Click += new System.EventHandler(this.CalculateBT_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(16, 45);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(63, 13);
            this.labelControl1.TabIndex = 1;
            this.labelControl1.Text = "Current Date";
            // 
            // CurrentDateTB
            // 
            this.CurrentDateTB.EditValue = null;
            this.CurrentDateTB.Location = new System.Drawing.Point(97, 42);
            this.CurrentDateTB.Name = "CurrentDateTB";
            this.CurrentDateTB.Properties.BeepOnError = false;
            this.CurrentDateTB.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.CurrentDateTB.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.CurrentDateTB.Properties.MaskSettings.Set("mask", "d");
            this.CurrentDateTB.Size = new System.Drawing.Size(107, 20);
            this.CurrentDateTB.TabIndex = 0;
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.labelControl6);
            this.groupControl2.Controls.Add(this.EveryTB);
            this.groupControl2.Controls.Add(this.labelControl5);
            this.groupControl2.Controls.Add(this.labelControl4);
            this.groupControl2.Controls.Add(this.OccursTB);
            this.groupControl2.Controls.Add(this.labelControl3);
            this.groupControl2.Controls.Add(this.DateTimeTB);
            this.groupControl2.Controls.Add(this.labelControl2);
            this.groupControl2.Controls.Add(this.TypeLB);
            this.groupControl2.Location = new System.Drawing.Point(26, 114);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(537, 129);
            this.groupControl2.TabIndex = 3;
            this.groupControl2.Text = "Configuration";
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(485, 101);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(31, 13);
            this.labelControl6.TabIndex = 9;
            this.labelControl6.Text = "day(s)";
            // 
            // EveryTB
            // 
            this.EveryTB.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.EveryTB.Location = new System.Drawing.Point(408, 94);
            this.EveryTB.Name = "EveryTB";
            this.EveryTB.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.EveryTB.Properties.IsFloatValue = false;
            this.EveryTB.Properties.MaskSettings.Set("mask", "N00");
            this.EveryTB.Size = new System.Drawing.Size(56, 20);
            this.EveryTB.TabIndex = 6;
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(374, 97);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(28, 13);
            this.labelControl5.TabIndex = 7;
            this.labelControl5.Text = "Every";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(16, 97);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(33, 13);
            this.labelControl4.TabIndex = 6;
            this.labelControl4.Text = "Occurs";
            // 
            // OccursTB
            // 
            this.OccursTB.EditValue = "Daily";
            this.OccursTB.Location = new System.Drawing.Point(97, 94);
            this.OccursTB.Name = "OccursTB";
            this.OccursTB.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.OccursTB.Properties.Items.AddRange(new object[] {
            "Daily"});
            this.OccursTB.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.OccursTB.Size = new System.Drawing.Size(107, 20);
            this.OccursTB.TabIndex = 5;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(16, 71);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(48, 13);
            this.labelControl3.TabIndex = 4;
            this.labelControl3.Text = "Date Time";
            // 
            // DateTimeTB
            // 
            this.DateTimeTB.EditValue = null;
            this.DateTimeTB.Location = new System.Drawing.Point(97, 68);
            this.DateTimeTB.Name = "DateTimeTB";
            this.DateTimeTB.Properties.BeepOnError = false;
            this.DateTimeTB.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.DateTimeTB.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.DateTimeTB.Properties.MaskSettings.Set("mask", "g");
            this.DateTimeTB.Size = new System.Drawing.Size(419, 20);
            this.DateTimeTB.TabIndex = 4;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(16, 45);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(24, 13);
            this.labelControl2.TabIndex = 2;
            this.labelControl2.Text = "Type";
            // 
            // TypeLB
            // 
            this.TypeLB.EditValue = "Once";
            this.TypeLB.Location = new System.Drawing.Point(97, 42);
            this.TypeLB.Name = "TypeLB";
            this.TypeLB.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.TypeLB.Properties.Items.AddRange(new object[] {
            "Once",
            "Recurring"});
            this.TypeLB.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.TypeLB.Size = new System.Drawing.Size(107, 20);
            this.TypeLB.TabIndex = 3;
            // 
            // groupControl3
            // 
            this.groupControl3.Controls.Add(this.labelControl8);
            this.groupControl3.Controls.Add(this.EndDateTB);
            this.groupControl3.Controls.Add(this.labelControl7);
            this.groupControl3.Controls.Add(this.StartDateTB);
            this.groupControl3.Location = new System.Drawing.Point(26, 266);
            this.groupControl3.Name = "groupControl3";
            this.groupControl3.Size = new System.Drawing.Size(537, 67);
            this.groupControl3.TabIndex = 3;
            this.groupControl3.Text = "Limits";
            // 
            // labelControl8
            // 
            this.labelControl8.Location = new System.Drawing.Point(327, 45);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(44, 13);
            this.labelControl8.TabIndex = 3;
            this.labelControl8.Text = "End Date";
            // 
            // EndDateTB
            // 
            this.EndDateTB.EditValue = null;
            this.EndDateTB.Location = new System.Drawing.Point(408, 42);
            this.EndDateTB.Name = "EndDateTB";
            this.EndDateTB.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.EndDateTB.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.EndDateTB.Size = new System.Drawing.Size(107, 20);
            this.EndDateTB.TabIndex = 8;
            // 
            // labelControl7
            // 
            this.labelControl7.Location = new System.Drawing.Point(16, 45);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(50, 13);
            this.labelControl7.TabIndex = 1;
            this.labelControl7.Text = "Start Date";
            // 
            // StartDateTB
            // 
            this.StartDateTB.EditValue = null;
            this.StartDateTB.Location = new System.Drawing.Point(97, 42);
            this.StartDateTB.Name = "StartDateTB";
            this.StartDateTB.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.StartDateTB.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.StartDateTB.Size = new System.Drawing.Size(107, 20);
            this.StartDateTB.TabIndex = 7;
            // 
            // groupControl4
            // 
            this.groupControl4.Controls.Add(this.DescriptionTB);
            this.groupControl4.Controls.Add(this.labelControl10);
            this.groupControl4.Controls.Add(this.NextExecutionTB);
            this.groupControl4.Controls.Add(this.labelControl9);
            this.groupControl4.Location = new System.Drawing.Point(26, 357);
            this.groupControl4.Name = "groupControl4";
            this.groupControl4.Size = new System.Drawing.Size(537, 167);
            this.groupControl4.TabIndex = 4;
            this.groupControl4.Text = "Ouput";
            // 
            // DescriptionTB
            // 
            this.DescriptionTB.Location = new System.Drawing.Point(16, 87);
            this.DescriptionTB.Name = "DescriptionTB";
            this.DescriptionTB.Properties.ReadOnly = true;
            this.DescriptionTB.Properties.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.DescriptionTB.Size = new System.Drawing.Size(500, 62);
            this.DescriptionTB.TabIndex = 10;
            // 
            // labelControl10
            // 
            this.labelControl10.Location = new System.Drawing.Point(16, 68);
            this.labelControl10.Name = "labelControl10";
            this.labelControl10.Size = new System.Drawing.Size(53, 13);
            this.labelControl10.TabIndex = 4;
            this.labelControl10.Text = "Description";
            // 
            // NextExecutionTB
            // 
            this.NextExecutionTB.Location = new System.Drawing.Point(134, 37);
            this.NextExecutionTB.Name = "NextExecutionTB";
            this.NextExecutionTB.Properties.ReadOnly = true;
            this.NextExecutionTB.Size = new System.Drawing.Size(382, 20);
            this.NextExecutionTB.TabIndex = 9;
            // 
            // labelControl9
            // 
            this.labelControl9.Location = new System.Drawing.Point(16, 40);
            this.labelControl9.Name = "labelControl9";
            this.labelControl9.Size = new System.Drawing.Size(96, 13);
            this.labelControl9.TabIndex = 2;
            this.labelControl9.Text = "Next execution time";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(587, 539);
            this.Controls.Add(this.groupControl4);
            this.Controls.Add(this.groupControl3);
            this.Controls.Add(this.groupControl2);
            this.Controls.Add(this.groupControl1);
            this.Name = "Form1";
            this.Text = "Schedule";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CurrentDateTB.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CurrentDateTB.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            this.groupControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.EveryTB.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OccursTB.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DateTimeTB.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DateTimeTB.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TypeLB.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).EndInit();
            this.groupControl3.ResumeLayout(false);
            this.groupControl3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.EndDateTB.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.EndDateTB.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StartDateTB.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StartDateTB.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl4)).EndInit();
            this.groupControl4.ResumeLayout(false);
            this.groupControl4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DescriptionTB.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NextExecutionTB.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.behaviorManager1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.DateEdit CurrentDateTB;
        private System.Windows.Forms.Button CalculateBT;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraEditors.ComboBoxEdit TypeLB;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.ComboBoxEdit OccursTB;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.DateEdit DateTimeTB;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.SpinEdit EveryTB;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.GroupControl groupControl3;
        private DevExpress.XtraEditors.LabelControl labelControl8;
        private DevExpress.XtraEditors.DateEdit EndDateTB;
        private DevExpress.XtraEditors.LabelControl labelControl7;
        private DevExpress.XtraEditors.DateEdit StartDateTB;
        private DevExpress.XtraEditors.GroupControl groupControl4;
        private DevExpress.XtraEditors.TextEdit NextExecutionTB;
        private DevExpress.XtraEditors.LabelControl labelControl9;
        private DevExpress.XtraEditors.MemoEdit DescriptionTB;
        private DevExpress.XtraEditors.LabelControl labelControl10;
        private DevExpress.Utils.Behaviors.BehaviorManager behaviorManager1;
    }
}

