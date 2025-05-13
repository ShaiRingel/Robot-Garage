using Model;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using View_Model.DB;
using Xceed.Wpf.Toolkit;

namespace Robot_Garage.Pages
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();

            if (this.RenderTransform == null || !(this.RenderTransform is TranslateTransform))
                this.RenderTransform = new TranslateTransform();
        }

        private async void btnLogin_ClickAsync(object sender, RoutedEventArgs e)
        {
			Debug.WriteLine(txtUsername.Text);
			Debug.WriteLine(int.Parse(txtNumber.Text));
			Debug.WriteLine(txtPassword.Password);

			UserDB userDB = new UserDB();

			User user = userDB.Login(txtUsername.Text, int.Parse(txtNumber.Text), txtPassword.Password);

            if (user == null)
            {
                System.Windows.MessageBox.Show("Invalid account, at least one of the fields are incorrect!");
                return;
            }

            lblStatus.Visibility = Visibility.Visible;

            await Task.Delay(500);

            Captain captain = new CaptainDB().SelectByID(user.ID);
            App.CurrentUser = captain != null ? captain : new ViewerDB().SelectByID(user.ID);

			NavigationService?.Navigate(new MainPage());
        }

        private void IntegerUpDown_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!(sender is IntegerUpDown control))
                return;

            if (!(control.Template.FindName("PART_TextBox", control) is TextBox textBox))
                return;

            int selectionStart = textBox.SelectionStart;
            int selectionLength = textBox.SelectionLength;
            string currentText = control.Text ?? string.Empty;

            string newText = currentText.Remove(selectionStart, selectionLength)
                                        .Insert(selectionStart, e.Text);

            if (newText.Length > 1 && newText.StartsWith("0"))
            {
                e.Handled = true;
                return;
            }

            if (int.TryParse(newText, out int prospectiveValue))
            {
                if (prospectiveValue > control.Maximum)
                {
                    e.Handled = true;
                }
            }
            else
            {
                e.Handled = true;
            }
        }

        private void btnForgotPassword_Clicked(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new ForgotPasswordPage());
        }
    }
}
