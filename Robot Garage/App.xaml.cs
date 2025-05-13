using Model;
using System.Windows;

namespace Robot_Garage
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
		public static User CurrentUser { get; set; }

		private void App_Startup(object sender, StartupEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();

            mainWindow.Top = 100;

            mainWindow.Left = 400;

            mainWindow.Show();
        }
    }

}
