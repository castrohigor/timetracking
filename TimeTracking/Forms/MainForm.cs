using TimeTracking.Data;
using TimeTracking.Models;
using TimeTracking.Forms;

namespace TimeTracking.Forms
{
    public partial class MainForm : Form
    {
        private DatabaseService _db;
        private DateTime _currentMonth;

        public MainForm()
        {
            InitializeComponent();
            _db = new DatabaseService();
            _currentMonth = DateTime.Now;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            labelMonth.Text = _currentMonth.ToString("MMMM/yyyy");
            LoadProjects();
        }

        private void LoadProjects()
        {
            dataGridViewProjects.DataSource = null;
            var projects = _db.GetAllProjects();

            var projectsData = projects.Select(p => new
            {
                p.Id,
                p.Name,
                HourlyRate = $"R$ {p.HourlyRate:F2}",
                TotalHours = _db.GetTotalHoursByProjectAndMonth(p.Id, _currentMonth.Year, _currentMonth.Month),
                TotalValue = _db.GetTotalHoursByProjectAndMonth(p.Id, _currentMonth.Year, _currentMonth.Month) * p.HourlyRate
            }).ToList();

            dataGridViewProjects.DataSource = projectsData;
            if (dataGridViewProjects.Columns.Count > 0)
            {
                dataGridViewProjects.Columns["TotalValue"].DefaultCellStyle.Format = "C2";
                dataGridViewProjects.Columns["TotalHours"].DefaultCellStyle.Format = "F2";
            }
        }

        private void buttonAddProject_Click(object sender, EventArgs e)
        {
            var form = new ProjectForm(_db);
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadProjects();
            }
        }

        private void buttonDeleteProject_Click(object sender, EventArgs e)
        {
            if (dataGridViewProjects.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecione um projeto para deletar.", "Aviso");
                return;
            }

            var projectId = (int)dataGridViewProjects.SelectedRows[0].Cells["Id"].Value;
            var result = MessageBox.Show("Tem certeza que deseja deletar este projeto? Todos os apontamentos serão removidos.", "Confirmação", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                _db.DeleteProject(projectId);
                LoadProjects();
            }
        }

        private void buttonViewDetails_Click(object sender, EventArgs e)
        {
            if (dataGridViewProjects.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecione um projeto para visualizar detalhes.", "Aviso");
                return;
            }

            var projectId = (int)dataGridViewProjects.SelectedRows[0].Cells["Id"].Value;
            var project = _db.GetProject(projectId);

            var form = new ProjectDetailsForm(_db, project);
            form.ShowDialog();
            LoadProjects();
        }

        private void buttonPreviousMonth_Click(object sender, EventArgs e)
        {
            _currentMonth = _currentMonth.AddMonths(-1);
            labelMonth.Text = _currentMonth.ToString("MMMM/yyyy");
            LoadProjects();
        }

        private void buttonNextMonth_Click(object sender, EventArgs e)
        {
            _currentMonth = _currentMonth.AddMonths(1);
            labelMonth.Text = _currentMonth.ToString("MMMM/yyyy");
            LoadProjects();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _db?.CloseConnection();
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.dataGridViewProjects = new System.Windows.Forms.DataGridView();
            this.buttonAddProject = new System.Windows.Forms.Button();
            this.buttonDeleteProject = new System.Windows.Forms.Button();
            this.buttonViewDetails = new System.Windows.Forms.Button();
            this.labelMonth = new System.Windows.Forms.Label();
            this.buttonPreviousMonth = new System.Windows.Forms.Button();
            this.buttonNextMonth = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panelButtons = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewProjects)).BeginInit();
            this.panel1.SuspendLayout();
            this.panelButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelButtons
            // 
            this.panelButtons.Controls.Add(this.buttonAddProject);
            this.panelButtons.Controls.Add(this.buttonDeleteProject);
            this.panelButtons.Controls.Add(this.buttonViewDetails);
            this.panelButtons.Controls.Add(this.labelMonth);
            this.panelButtons.Controls.Add(this.buttonPreviousMonth);
            this.panelButtons.Controls.Add(this.buttonNextMonth);
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelButtons.Location = new System.Drawing.Point(0, 0);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(800, 50);
            this.panelButtons.TabIndex = 8;
            // 
            // dataGridViewProjects
            // 
            this.dataGridViewProjects.AllowUserToAddRows = false;
            this.dataGridViewProjects.AllowUserToDeleteRows = false;
            this.dataGridViewProjects.AllowUserToOrderColumns = true;
            this.dataGridViewProjects.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewProjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewProjects.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewProjects.MultiSelect = false;
            this.dataGridViewProjects.Name = "dataGridViewProjects";
            this.dataGridViewProjects.ReadOnly = true;
            this.dataGridViewProjects.RowHeadersWidth = 51;
            this.dataGridViewProjects.RowTemplate.Height = 25;
            this.dataGridViewProjects.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewProjects.Size = new System.Drawing.Size(800, 350);
            this.dataGridViewProjects.TabIndex = 0;
            // 
            // buttonAddProject
            // 
            this.buttonAddProject.Location = new System.Drawing.Point(12, 12);
            this.buttonAddProject.Name = "buttonAddProject";
            this.buttonAddProject.Size = new System.Drawing.Size(100, 30);
            this.buttonAddProject.TabIndex = 1;
            this.buttonAddProject.Text = "+ Projeto";
            this.buttonAddProject.UseVisualStyleBackColor = true;
            this.buttonAddProject.Click += new System.EventHandler(this.buttonAddProject_Click);
            // 
            // buttonDeleteProject
            // 
            this.buttonDeleteProject.Location = new System.Drawing.Point(118, 12);
            this.buttonDeleteProject.Name = "buttonDeleteProject";
            this.buttonDeleteProject.Size = new System.Drawing.Size(100, 30);
            this.buttonDeleteProject.TabIndex = 2;
            this.buttonDeleteProject.Text = "- Projeto";
            this.buttonDeleteProject.UseVisualStyleBackColor = true;
            this.buttonDeleteProject.Click += new System.EventHandler(this.buttonDeleteProject_Click);
            // 
            // buttonViewDetails
            // 
            this.buttonViewDetails.Location = new System.Drawing.Point(224, 12);
            this.buttonViewDetails.Name = "buttonViewDetails";
            this.buttonViewDetails.Size = new System.Drawing.Size(100, 30);
            this.buttonViewDetails.TabIndex = 3;
            this.buttonViewDetails.Text = "Detalhes";
            this.buttonViewDetails.UseVisualStyleBackColor = true;
            this.buttonViewDetails.Click += new System.EventHandler(this.buttonViewDetails_Click);
            // 
            // labelMonth
            // 
            this.labelMonth.AutoSize = true;
            this.labelMonth.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.labelMonth.Location = new System.Drawing.Point(350, 18);
            this.labelMonth.Name = "labelMonth";
            this.labelMonth.Size = new System.Drawing.Size(58, 21);
            this.labelMonth.TabIndex = 4;
            this.labelMonth.Text = "label1";
            // 
            // buttonPreviousMonth
            // 
            this.buttonPreviousMonth.Location = new System.Drawing.Point(550, 12);
            this.buttonPreviousMonth.Name = "buttonPreviousMonth";
            this.buttonPreviousMonth.Size = new System.Drawing.Size(40, 30);
            this.buttonPreviousMonth.TabIndex = 5;
            this.buttonPreviousMonth.Text = "<";
            this.buttonPreviousMonth.UseVisualStyleBackColor = true;
            this.buttonPreviousMonth.Click += new System.EventHandler(this.buttonPreviousMonth_Click);
            // 
            // buttonNextMonth
            // 
            this.buttonNextMonth.Location = new System.Drawing.Point(596, 12);
            this.buttonNextMonth.Name = "buttonNextMonth";
            this.buttonNextMonth.Size = new System.Drawing.Size(40, 30);
            this.buttonNextMonth.TabIndex = 6;
            this.buttonNextMonth.Text = ">";
            this.buttonNextMonth.UseVisualStyleBackColor = true;
            this.buttonNextMonth.Click += new System.EventHandler(this.buttonNextMonth_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dataGridViewProjects);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 50);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(800, 350);
            this.panel1.TabIndex = 7;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 400);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panelButtons);
            this.Name = "MainForm";
            this.Text = "Time Tracking - Apontamento de Horas";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewProjects)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panelButtons.ResumeLayout(false);
            this.panelButtons.PerformLayout();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.DataGridView dataGridViewProjects;
        private System.Windows.Forms.Button buttonAddProject;
        private System.Windows.Forms.Button buttonDeleteProject;
        private System.Windows.Forms.Button buttonViewDetails;
        private System.Windows.Forms.Label labelMonth;
        private System.Windows.Forms.Button buttonPreviousMonth;
        private System.Windows.Forms.Button buttonNextMonth;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panelButtons;
    }
}
