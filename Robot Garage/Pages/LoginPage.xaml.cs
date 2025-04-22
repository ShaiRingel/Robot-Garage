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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using View_Model;
using Xceed.Wpf.Toolkit;

namespace Robot_Garage {
	/// <summary>
	/// Interaction logic for LoginWindow.xaml
	/// </summary>
	public partial class LoginPage : Page {
		public LoginPage() {
			InitializeComponent();

			if (this.RenderTransform == null || !(this.RenderTransform is TranslateTransform))
				this.RenderTransform = new TranslateTransform();
		}

		private async void btnLogin_ClickAsync(object sender, RoutedEventArgs e) {
			User user = new UserDB().GetByName(txtUsername.Text);
			UserList users = new UserDB().GetAll();
			Debug.WriteLine(users.Count);
			if (user == null) {
				Debug.WriteLine("No user found with that name!");
				return;
			}

			if (user.Password == txtPassword.Password && user.GroupNumber.ToString() == txtNumber.Text) {
				lblStatus.Text = "Login Successful";

				await Task.Delay(500);

				AnimationHelper.PlayAnimation(this, "SlideLeftStoryboard");

				NavigationService?.Navigate(new SalesPage(user));
			}
			else {
				System.Windows.MessageBox.Show("Invalid account, at least one of the fields are incorrect!");	
			}
		}

		private void IntegerUpDown_PreviewTextInput(object sender, TextCompositionEventArgs e) {
			if (!(sender is IntegerUpDown control))
				return;

			if (!(control.Template.FindName("PART_TextBox", control) is TextBox textBox))
				return;

			int selectionStart = textBox.SelectionStart;
			int selectionLength = textBox.SelectionLength;
			string currentText = control.Text ?? string.Empty;

			string newText = currentText.Remove(selectionStart, selectionLength)
										.Insert(selectionStart, e.Text);

			if (newText.Length > 1 && newText.StartsWith("0")) {
				e.Handled = true;
				return;
			}

			if (int.TryParse(newText, out int prospectiveValue)) {
				if (prospectiveValue > control.Maximum) {
					e.Handled = true;
				}
			}
			else {
				e.Handled = true;
			}
		}

		private void btnForgotPassword_Clicked(object sender, RoutedEventArgs e) {

		}
	}
}
