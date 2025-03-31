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
using System.Windows.Navigation;
using System.Windows.Shapes;
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
			if (txtUsername.Text == "Shai Ringel" && txtNumber.Text == "5554" && txtPassword.Password == "123456") {
				SalesWindow mainWindow = new SalesWindow();
				mainWindow.Show();
				this.Close();
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
			// Cast sender to IntegerUpDown
			if (!(sender is IntegerUpDown control))
				return;

			// Find the underlying TextBox (usually named PART_TextBox)
			if (!(control.Template.FindName("PART_TextBox", control) is TextBox textBox))
				return;

			// Get the current text, selection start and selection length
			int selectionStart = textBox.SelectionStart;
			int selectionLength = textBox.SelectionLength;
			string currentText = control.Text ?? string.Empty;

			// Calculate the prospective text by replacing the selected text with the new input
			string newText = currentText.Remove(selectionStart, selectionLength)
										.Insert(selectionStart, e.Text);

			// Prevent input that leads to multiple-digit numbers with a leading zero (unless the value is "0")
			if (newText.Length > 1 && newText.StartsWith("0")) {
				e.Handled = true;
				return;
			}

			// Ensure the new text is a valid integer
			if (int.TryParse(newText, out int prospectiveValue)) {
				// Check if the prospective value exceeds the Maximum property
				if (prospectiveValue > control.Maximum) {
					e.Handled = true;
				}
			}
			else {
				// If parsing fails, reject the input
				e.Handled = true;
			}
		}

		private void Button_Click(object sender, RoutedEventArgs e) {

		}
	}
}
