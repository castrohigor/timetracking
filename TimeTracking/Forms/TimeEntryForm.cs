using TimeTracking.Data;
using TimeTracking.Models;

namespace TimeTracking.Forms
{
    public partial class TimeEntryForm : Form
    {
        private DatabaseService _db;
        private Project _project;
        private TimeEntry _entry;

        public TimeEntryForm(DatabaseService db, Project project, TimeEntry entry = null)
        {
            InitializeComponent();
            _db = db;
            _project = project;
            _entry = entry;
        }

        private void TimeEntryForm_Load(object sender, EventArgs e)
        {
            if (_entry != null)
            {
                dateTimePickerStart.Value = _entry.StartTime;
                dateTimePickerEnd.Value = _entry.EndTime;
                textBoxDescription.Text = _entry.Description;
                this.Text = "Editar Apontamento";
            }
            else
            {
                dateTimePickerStart.Value = DateTime.Now;
                dateTimePickerEnd.Value = DateTime.Now.AddHours(1);
                this.Text = "Novo Apontamento";
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            var startTime = dateTimePickerStart.Value;
            var endTime = dateTimePickerEnd.Value;

            if (endTime <= startTime)
            {
                MessageBox.Show("O horário final deve ser maior que o horário inicial.", "Erro");
                return;
            }

            try
            {
                if (_entry == null)
                {
                    _entry = new TimeEntry
                    {
                        ProjectId = _project.Id,
                        StartTime = startTime,
                        EndTime = endTime,
                        Description = textBoxDescription.Text
                    };
                    _db.AddTimeEntry(_entry);
                }
                else
                {
                    _entry.StartTime = startTime;
                    _entry.EndTime = endTime;
                    _entry.Description = textBoxDescription.Text;
                    _db.UpdateTimeEntry(_entry);
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao salvar apontamento: {ex.Message}", "Erro");
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void InitializeComponent()
        {
            this.labelStart = new System.Windows.Forms.Label();
            this.dateTimePickerStart = new System.Windows.Forms.DateTimePicker();
            this.labelEnd = new System.Windows.Forms.Label();
            this.dateTimePickerEnd = new System.Windows.Forms.DateTimePicker();
            this.labelDescription = new System.Windows.Forms.Label();
            this.textBoxDescription = new System.Windows.Forms.TextBox();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelStart
            // 
            this.labelStart.AutoSize = true;
            this.labelStart.Location = new System.Drawing.Point(12, 15);
            this.labelStart.Name = "labelStart";
            this.labelStart.Size = new System.Drawing.Size(56, 15);
            this.labelStart.TabIndex = 0;
            this.labelStart.Text = "Início:";
            // 
            // dateTimePickerStart
            // 
            this.dateTimePickerStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerStart.CustomFormat = "dd/MM/yyyy HH:mm";
            this.dateTimePickerStart.Location = new System.Drawing.Point(12, 33);
            this.dateTimePickerStart.Name = "dateTimePickerStart";
            this.dateTimePickerStart.Size = new System.Drawing.Size(200, 23);
            this.dateTimePickerStart.TabIndex = 1;
            // 
            // labelEnd
            // 
            this.labelEnd.AutoSize = true;
            this.labelEnd.Location = new System.Drawing.Point(12, 65);
            this.labelEnd.Name = "labelEnd";
            this.labelEnd.Size = new System.Drawing.Size(33, 15);
            this.labelEnd.TabIndex = 2;
            this.labelEnd.Text = "Fim:";
            // 
            // dateTimePickerEnd
            // 
            this.dateTimePickerEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerEnd.CustomFormat = "dd/MM/yyyy HH:mm";
            this.dateTimePickerEnd.Location = new System.Drawing.Point(12, 83);
            this.dateTimePickerEnd.Name = "dateTimePickerEnd";
            this.dateTimePickerEnd.Size = new System.Drawing.Size(200, 23);
            this.dateTimePickerEnd.TabIndex = 3;
            // 
            // labelDescription
            // 
            this.labelDescription.AutoSize = true;
            this.labelDescription.Location = new System.Drawing.Point(12, 115);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(66, 15);
            this.labelDescription.TabIndex = 4;
            this.labelDescription.Text = "Descrição:";
            // 
            // textBoxDescription
            // 
            this.textBoxDescription.Location = new System.Drawing.Point(12, 133);
            this.textBoxDescription.Multiline = true;
            this.textBoxDescription.Name = "textBoxDescription";
            this.textBoxDescription.Size = new System.Drawing.Size(260, 80);
            this.textBoxDescription.TabIndex = 5;
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(116, 220);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 6;
            this.buttonSave.Text = "Salvar";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(197, 220);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 7;
            this.buttonCancel.Text = "Cancelar";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // TimeEntryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 255);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.textBoxDescription);
            this.Controls.Add(this.labelDescription);
            this.Controls.Add(this.dateTimePickerEnd);
            this.Controls.Add(this.labelEnd);
            this.Controls.Add(this.dateTimePickerStart);
            this.Controls.Add(this.labelStart);
            this.Name = "TimeEntryForm";
            this.Text = "Apontamento";
            this.Load += new System.EventHandler(this.TimeEntryForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label labelStart;
        private System.Windows.Forms.DateTimePicker dateTimePickerStart;
        private System.Windows.Forms.Label labelEnd;
        private System.Windows.Forms.DateTimePicker dateTimePickerEnd;
        private System.Windows.Forms.Label labelDescription;
        private System.Windows.Forms.TextBox textBoxDescription;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonCancel;
    }
}
