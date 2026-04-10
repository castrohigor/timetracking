namespace TimeTracking.Forms
{
    public partial class SplashForm : Form
    {
        private System.Windows.Forms.Timer _timer;

        public SplashForm()
        {
            InitializeComponent();
        }

        private void SplashForm_Load(object sender, EventArgs e)
        {
            // Carregar a imagem
            try
            {
                // Tentar vários caminhos possíveis
                var possiblePaths = new[]
                {
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "img", "splash.png"),
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "img", "splash.png"),
                    Path.Combine(Directory.GetCurrentDirectory(), "img", "splash.png"),
                    @"C:\Users\higor\source\timetracking\TimeTracking\img\splash.png"
                };

                string imagePath = null;
                foreach (var path in possiblePaths)
                {
                    if (File.Exists(path))
                    {
                        imagePath = path;
                        break;
                    }
                }

                if (!string.IsNullOrEmpty(imagePath))
                {
                    var originalImage = Image.FromFile(imagePath);

                    // Redimensionar para 100 pixels de largura mantendo aspect ratio
                    int newWidth = 400;
                    int newHeight = (int)(originalImage.Height * (400.0 / originalImage.Width));

                    var resizedImage = new Bitmap(newWidth, newHeight);
                    using (var graphics = Graphics.FromImage(resizedImage))
                    {
                        graphics.DrawImage(originalImage, 0, 0, newWidth, newHeight);
                    }

                    pictureBoxSplash.Image = resizedImage;
                    pictureBoxSplash.Size = new System.Drawing.Size(newWidth, newHeight);
                    this.ClientSize = new System.Drawing.Size(newWidth, newHeight);
                    originalImage.Dispose();
                }
                else
                {
                    this.BackColor = System.Drawing.Color.DarkBlue;
                }
            }
            catch (Exception ex)
            {
                this.BackColor = System.Drawing.Color.DarkBlue;
                MessageBox.Show($"Erro ao carregar splash: {ex.Message}", "Erro");
            }

            _timer = new System.Windows.Forms.Timer();
            _timer.Interval = 3000; // 3 segundos
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            _timer.Stop();
            _timer.Dispose();
            this.Close();
        }

        private void InitializeComponent()
        {
            this.pictureBoxSplash = new System.Windows.Forms.PictureBox();
            this.panelCenter = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSplash)).BeginInit();
            this.panelCenter.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBoxSplash
            // 
            this.pictureBoxSplash.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxSplash.Name = "pictureBoxSplash";
            this.pictureBoxSplash.Size = new System.Drawing.Size(400, 200);
            this.pictureBoxSplash.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBoxSplash.TabIndex = 0;
            this.pictureBoxSplash.TabStop = false;

            // 
            // panelCenter
            // 
            this.panelCenter.AutoSize = true;
            this.panelCenter.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelCenter.Controls.Add(this.pictureBoxSplash);
            this.panelCenter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCenter.Location = new System.Drawing.Point(0, 0);
            this.panelCenter.Name = "panelCenter";
            this.panelCenter.Size = new System.Drawing.Size(400, 200);

            this.BackColor = Color.Magenta;
            this.TransparencyKey = Color.Magenta;
            // 
            // SplashForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(800, 400);
            this.ControlBox = false;
            this.Controls.Add(this.panelCenter);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SplashForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Loading...";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.SplashForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSplash)).EndInit();
            this.panelCenter.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        public System.Windows.Forms.PictureBox pictureBoxSplash;
        private System.Windows.Forms.Panel panelCenter;
    }
}
