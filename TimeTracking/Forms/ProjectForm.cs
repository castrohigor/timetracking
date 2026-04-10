using TimeTracking.Data;
using TimeTracking.Models;

namespace TimeTracking.Forms
{
    public partial class ProjectForm : Form
    {
        private DatabaseService _db;
        private Project _project;

        public ProjectForm(DatabaseService db, Project project = null)
        {
            InitializeComponent();
            _db = db;
            _project = project;
        }

        private void ProjectForm_Load(object sender, EventArgs e)
        {
            if (_project != null)
            {
                textBoxName.Text = _project.Name;
                textBoxRate.Text = _project.HourlyRate.ToString("F2");
                this.Text = "Editar Projeto";
            }
            else
            {
                this.Text = "Novo Projeto";
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxName.Text))
            {
                MessageBox.Show("Nome do projeto é obrigatório.", "Erro");
                return;
            }

            if (!decimal.TryParse(textBoxRate.Text, out var rate) || rate <= 0)
            {
                MessageBox.Show("Taxa horária inválida.", "Erro");
                return;
            }

            try
            {
                if (_project == null)
                {
                    _project = new Project
                    {
                        Name = textBoxName.Text,
                        HourlyRate = rate
                    };
                    _db.AddProject(_project);
                }
                else
                {
                    _project.Name = textBoxName.Text;
                    _project.HourlyRate = rate;
                    _db.UpdateProject(_project);
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao salvar projeto: {ex.Message}", "Erro");
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void InitializeComponent()
        {
            this.labelName = new System.Windows.Forms.Label();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.labelRate = new System.Windows.Forms.Label();
            this.textBoxRate = new System.Windows.Forms.TextBox();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(12, 15);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(40, 15);
            this.labelName.TabIndex = 0;
            this.labelName.Text = "Nome:";
            // 
            // textBoxName
            // 
            this.textBoxName.Location = new System.Drawing.Point(12, 33);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(260, 23);
            this.textBoxName.TabIndex = 1;
            // 
            // labelRate
            // 
            this.labelRate.AutoSize = true;
            this.labelRate.Location = new System.Drawing.Point(12, 65);
            this.labelRate.Name = "labelRate";
            this.labelRate.Size = new System.Drawing.Size(98, 15);
            this.labelRate.TabIndex = 2;
            this.labelRate.Text = "Valor por Hora:";
            // 
            // textBoxRate
            // 
            this.textBoxRate.Location = new System.Drawing.Point(12, 83);
            this.textBoxRate.Name = "textBoxRate";
            this.textBoxRate.Size = new System.Drawing.Size(120, 23);
            this.textBoxRate.TabIndex = 3;
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(116, 120);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 4;
            this.buttonSave.Text = "Salvar";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(197, 120);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 5;
            this.buttonCancel.Text = "Cancelar";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // ProjectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 155);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.textBoxRate);
            this.Controls.Add(this.labelRate);
            this.Controls.Add(this.textBoxName);
            this.Controls.Add(this.labelName);
            this.Name = "ProjectForm";
            this.Text = "Projeto";
            this.Load += new System.EventHandler(this.ProjectForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.Label labelRate;
        private System.Windows.Forms.TextBox textBoxRate;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonCancel;
    }
}
