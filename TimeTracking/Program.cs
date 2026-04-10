using TimeTracking.Forms;

namespace TimeTracking;

static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();

        // Mostrar splash screen
        var splashForm = new SplashForm();
        splashForm.Show();
        Application.DoEvents();

        // Aguardar o splash screen fechar
        while (splashForm.Visible)
        {
            Application.DoEvents();
        }

        splashForm.Dispose();

        // Mostrar formulário principal
        Application.Run(new MainForm());
    }
}