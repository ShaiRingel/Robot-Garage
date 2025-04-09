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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using View_Model;
using Xceed.Wpf.Toolkit;

namespace Robot_Garage {
	/// <summary>
	/// Interaction logic for LoginWindow.xaml
	/// </summary>
	public partial class LoginWindow : Window {

		private bool _isUpdatingSize = false;
		private const double TargetAspectRatio = 16.0 / 9.0;

		public LoginWindow() {
			InitializeComponent();
			SetResourceReference(StyleProperty, typeof(Window));
		}

		private void btnLogin_Click(object sender, RoutedEventArgs e) {
			User user = new UserDB().GetByName(txtUsername.Text);
			UserList users = new UserDB().GetAll();
			Debug.WriteLine(users.Count);
			if (user == null) {
				Debug.WriteLine("No user found with that name!");
				return;
			}

			if (user.Password == txtPassword.Password && user.GroupNumber.ToString() == txtNumber.Text) {
				SalesWindow mainWindow = new SalesWindow();
				mainWindow.Show();
				this.Close();
			}
			else {
				System.Windows.MessageBox.Show("Invalid account, at least one of the fields are incorrect!");	
			}
		}

		private void Window_SizeChanged(object sender, SizeChangedEventArgs e) {
			if (_isUpdatingSize)
				return;

			_isUpdatingSize = true;

			double currentWidth = this.ActualWidth;
			double currentHeight = this.ActualHeight;

			double currentAspect = currentWidth / currentHeight;

			if (currentAspect > TargetAspectRatio) {
				this.Width = currentHeight * TargetAspectRatio;
			}
			else if (currentAspect < TargetAspectRatio) {
				this.Height = currentWidth / TargetAspectRatio;
			}

			_isUpdatingSize = false;
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

		private void Button_Click(object sender, RoutedEventArgs e) {

		}
	}
}
