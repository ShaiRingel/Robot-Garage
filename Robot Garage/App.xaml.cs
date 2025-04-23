using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;

namespace Robot_Garage {
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application {
		private void App_Startup(object sender, StartupEventArgs e) {
			MainWindow mainWindow = new MainWindow();

			mainWindow.Top = 100;

			mainWindow.Left = 400;

			mainWindow.Show();

		}
	}

}
