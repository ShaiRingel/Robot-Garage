using Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using View_Model;

namespace Robot_Garage {
	/// <summary>
	/// Interaction logic for ForgotPasswordWindow.xaml
	/// </summary>
	public partial class ForgotPasswordPage : Page {
		UserDB userDB;
		User currentUser;
		public ForgotPasswordPage() {
			InitializeComponent();

			userDB = new UserDB();

			NewPasswordGrid.Visibility = Visibility.Collapsed;
		}

		private async void ChangePassword_Click(object sender, RoutedEventArgs e) {
			if (UniqueCodeGrid.Visibility == Visibility.Visible)
			{
                currentUser = userDB.GetByCode(txtCode.Text);
				if (currentUser != null)
				{
					UniqueCodeGrid.Visibility = Visibility.Collapsed;
					NewPasswordGrid.Visibility = Visibility.Visible;
				}
				else
				{
                    System.Windows.MessageBox.Show("Invalid account, at least one of the fields are incorrect!");
                }
            }
            else
            {
				if (!string.IsNullOrEmpty(txtPassword.Text))
				{
					currentUser.Password = txtPassword.Text;
					userDB.Update(currentUser);
					txtPasswordChnage.Visibility = Visibility.Visible;

					await Task.Delay(750);

					NavigationService?.Navigate(new LoginPage());
                }
				else
				{
                    System.Windows.MessageBox.Show("Invalid password, the password is blank!");
                }
            }
        }

		private void BackButton_Click(object sender, RoutedEventArgs e) {
			if (NavigationService != null && NavigationService.CanGoBack) {
				NavigationService.GoBack();
			}
			else {
				System.Windows.MessageBox.Show("No previous page to navigate to.");
			}
		}
	}
}
