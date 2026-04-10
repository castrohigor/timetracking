using TimeTracking.Data;
using TimeTracking.Models;
using System.Windows.Forms;

namespace TimeTracking.Forms
{
    public partial class ProjectDetailsForm : Form
    {
        private DatabaseService _db;
        private Project _project;
        private System.Windows.Forms.Timer _timer;
        private DateTime _startTime;
        private bool _isRunning;

        public ProjectDetailsForm(DatabaseService db, Project project)
        {
            InitializeComponent();
            _db = db;
            _project = project;
            _isRunning = false;
        }

        private void ProjectDetailsForm_Load(object sender, EventArgs e)
        {
            this.Text = $"Detalhes - {_project.Name}";
            _timer = new System.Windows.Forms.Timer();
            _timer.Interval = 1000;
            _timer.Tick += Timer_Tick;

            LoadTimeEntries();
        }

        private void LoadTimeEntries()
        {
            dataGridViewTimeEntries.DataSource = null;
            var entries = _db.GetTimeEntriesByProject(_project.Id);

            var data = entries.Select(e => new
            {
                e.Id,
                Início = e.StartTime.ToString("dd/MM/yyyy HH:mm"),
                Fim = e.EndTime.ToString("dd/MM/yyyy HH:mm"),
                Horas = Math.Round(e.GetHours(), 2),
                e.Description
            }).ToList();

            dataGridViewTimeEntries.DataSource = data;
            if (dataGridViewTimeEntries.Columns.Count > 0)
            {
                dataGridViewTimeEntries.Columns["Horas"].DefaultCellStyle.Format = "F2";
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (_isRunning)
            {
                var elapsed = DateTime.Now - _startTime;
                labelTimer.Text = $"{(int)elapsed.TotalHours:D2}:{elapsed.Minutes:D2}:{elapsed.Seconds:D2}";
            }
        }

        private void buttonStartStop_Click(object sender, EventArgs e)
        {
            if (!_isRunning)
            {
                _startTime = DateTime.Now;
                _isRunning = true;
                _timer.Start();
                buttonStartStop.Text = "Parar";
                buttonStartStop.BackColor = Color.LightCoral;
            }
            else
            {
                _timer.Stop();
                _isRunning = false;
                buttonStartStop.Text = "Iniciar";
                buttonStartStop.BackColor = Color.LightGreen;

                var endTime = DateTime.Now;
                var entry = new TimeEntry
                {
                    ProjectId = _project.Id,
                    StartTime = _startTime,
                    EndTime = endTime,
                    Description = textBoxDescription.Text
                };

                _db.AddTimeEntry(entry);
                textBoxDescription.Clear();
                labelTimer.Text = "00:00:00";
                LoadTimeEntries();
            }
        }

        private void buttonAddManual_Click(object sender, EventArgs e)
        {
            var form = new TimeEntryForm(_db, _project);
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadTimeEntries();
            }
        }

        private void buttonEditEntry_Click(object sender, EventArgs e)
        {
            if (dataGridViewTimeEntries.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecione um apontamento para editar.", "Aviso");
                return;
            }

            var entryId = (int)dataGridViewTimeEntries.SelectedRows[0].Cells["Id"].Value;
            var entry = _db.GetTimeEntriesByProject(_project.Id).FirstOrDefault(x => x.Id == entryId);

            if (entry != null)
            {
                var form = new TimeEntryForm(_db, _project, entry);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadTimeEntries();
                }
            }
        }

        private void buttonDeleteEntry_Click(object sender, EventArgs e)
        {
            if (dataGridViewTimeEntries.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecione um apontamento para deletar.", "Aviso");
                return;
            }

            var entryId = (int)dataGridViewTimeEntries.SelectedRows[0].Cells["Id"].Value;
            var result = MessageBox.Show("Tem certeza que deseja deletar este apontamento?", "Confirmação", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                _db.DeleteTimeEntry(entryId);
                LoadTimeEntries();
            }
        }

        private void ProjectDetailsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_timer != null)
            {
                _timer.Stop();
                _timer.Dispose();
            }
        }

        private void InitializeComponent()
        {
            this.labelTimerTitle = new System.Windows.Forms.Label();
            this.labelTimer = new System.Windows.Forms.Label();
            this.buttonStartStop = new System.Windows.Forms.Button();
            this.textBoxDescription = new System.Windows.Forms.TextBox();
            this.labelDescription = new System.Windows.Forms.Label();
            this.buttonAddManual = new System.Windows.Forms.Button();
            this.dataGridViewTimeEntries = new System.Windows.Forms.DataGridView();
            this.buttonEditEntry = new System.Windows.Forms.Button();
            this.buttonDeleteEntry = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTimeEntries)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelTimerTitle
            // 
            this.labelTimerTitle.AutoSize = true;
            this.labelTimerTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.labelTimerTitle.Location = new System.Drawing.Point(12, 12);
            this.labelTimerTitle.Name = "labelTimerTitle";
            this.labelTimerTitle.Size = new System.Drawing.Size(62, 19);
            this.labelTimerTitle.TabIndex = 0;
            this.labelTimerTitle.Text = "Tempo:";
            // 
            // labelTimer
            // 
            this.labelTimer.AutoSize = true;
            this.labelTimer.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.labelTimer.Location = new System.Drawing.Point(80, 7);
            this.labelTimer.Name = "labelTimer";
            this.labelTimer.Size = new System.Drawing.Size(120, 45);
            this.labelTimer.TabIndex = 1;
            this.labelTimer.Text = "00:00:00";
            // 
            // buttonStartStop
            // 
            this.buttonStartStop.BackColor = System.Drawing.Color.LightGreen;
            this.buttonStartStop.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.buttonStartStop.Location = new System.Drawing.Point(250, 12);
            this.buttonStartStop.Name = "buttonStartStop";
            this.buttonStartStop.Size = new System.Drawing.Size(100, 40);
            this.buttonStartStop.TabIndex = 2;
            this.buttonStartStop.Text = "Iniciar";
            this.buttonStartStop.UseVisualStyleBackColor = false;
            this.buttonStartStop.Click += new System.EventHandler(this.buttonStartStop_Click);
            // 
            // textBoxDescription
            // 
            this.textBoxDescription.Location = new System.Drawing.Point(12, 75);
            this.textBoxDescription.Name = "textBoxDescription";
            this.textBoxDescription.Size = new System.Drawing.Size(338, 23);
            this.textBoxDescription.TabIndex = 3;
            // 
            // labelDescription
            // 
            this.labelDescription.AutoSize = true;
            this.labelDescription.Location = new System.Drawing.Point(12, 57);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(66, 15);
            this.labelDescription.TabIndex = 4;
            this.labelDescription.Text = "Descrição:";
            // 
            // buttonAddManual
            // 
            this.buttonAddManual.Location = new System.Drawing.Point(12, 12);
            this.buttonAddManual.Name = "buttonAddManual";
            this.buttonAddManual.Size = new System.Drawing.Size(120, 30);
            this.buttonAddManual.TabIndex = 5;
            this.buttonAddManual.Text = "+ Manual";
            this.buttonAddManual.UseVisualStyleBackColor = true;
            this.buttonAddManual.Click += new System.EventHandler(this.buttonAddManual_Click);
            // 
            // dataGridViewTimeEntries
            // 
            this.dataGridViewTimeEntries.AllowUserToAddRows = false;
            this.dataGridViewTimeEntries.AllowUserToDeleteRows = false;
            this.dataGridViewTimeEntries.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewTimeEntries.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewTimeEntries.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewTimeEntries.MultiSelect = false;
            this.dataGridViewTimeEntries.Name = "dataGridViewTimeEntries";
            this.dataGridViewTimeEntries.ReadOnly = true;
            this.dataGridViewTimeEntries.RowHeadersWidth = 51;
            this.dataGridViewTimeEntries.RowTemplate.Height = 25;
            this.dataGridViewTimeEntries.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewTimeEntries.Size = new System.Drawing.Size(784, 250);
            this.dataGridViewTimeEntries.TabIndex = 6;
            // 
            // buttonEditEntry
            // 
            this.buttonEditEntry.Location = new System.Drawing.Point(138, 12);
            this.buttonEditEntry.Name = "buttonEditEntry";
            this.buttonEditEntry.Size = new System.Drawing.Size(100, 30);
            this.buttonEditEntry.TabIndex = 7;
            this.buttonEditEntry.Text = "Editar";
            this.buttonEditEntry.UseVisualStyleBackColor = true;
            this.buttonEditEntry.Click += new System.EventHandler(this.buttonEditEntry_Click);
            // 
            // buttonDeleteEntry
            // 
            this.buttonDeleteEntry.Location = new System.Drawing.Point(244, 12);
            this.buttonDeleteEntry.Name = "buttonDeleteEntry";
            this.buttonDeleteEntry.Size = new System.Drawing.Size(100, 30);
            this.buttonDeleteEntry.TabIndex = 8;
            this.buttonDeleteEntry.Text = "Deletar";
            this.buttonDeleteEntry.UseVisualStyleBackColor = true;
            this.buttonDeleteEntry.Click += new System.EventHandler(this.buttonDeleteEntry_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.labelTimerTitle);
            this.panel1.Controls.Add(this.labelTimer);
            this.panel1.Controls.Add(this.buttonStartStop);
            this.panel1.Controls.Add(this.labelDescription);
            this.panel1.Controls.Add(this.textBoxDescription);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(784, 110);
            this.panel1.TabIndex = 9;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.buttonAddManual);
            this.panel2.Controls.Add(this.buttonEditEntry);
            this.panel2.Controls.Add(this.buttonDeleteEntry);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 360);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(784, 50);
            this.panel2.TabIndex = 10;
            // 
            // ProjectDetailsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 410);
            this.Controls.Add(this.dataGridViewTimeEntries);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "ProjectDetailsForm";
            this.Text = "Detalhes do Projeto";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ProjectDetailsForm_FormClosing);
            this.Load += new System.EventHandler(this.ProjectDetailsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTimeEntries)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Label labelTimerTitle;
        private System.Windows.Forms.Label labelTimer;
        private System.Windows.Forms.Button buttonStartStop;
        private System.Windows.Forms.TextBox textBoxDescription;
        private System.Windows.Forms.Label labelDescription;
        private System.Windows.Forms.Button buttonAddManual;
        private System.Windows.Forms.DataGridView dataGridViewTimeEntries;
        private System.Windows.Forms.Button buttonEditEntry;
        private System.Windows.Forms.Button buttonDeleteEntry;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
    }
}
