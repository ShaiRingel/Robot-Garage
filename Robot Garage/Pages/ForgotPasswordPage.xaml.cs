using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Robot_Garage {
	/// <summary>
	/// Interaction logic for ForgotPasswordWindow.xaml
	/// </summary>
	public partial class ForgotPasswordPage : Page {
		public ForgotPasswordPage() {
			InitializeComponent();
		}

		private void SendResetLink_Click(object sender, RoutedEventArgs e) {

        }

		private void BackButton_Click(object sender, RoutedEventArgs e) {
			if (NavigationService != null && NavigationService.CanGoBack) {
				NavigationService.GoBack();
			}
			else {
				MessageBox.Show("No previous page to navigate to.");
			}
		}
	}
}
