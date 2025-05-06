using Model;
using System.Windows;
using System.Windows.Controls;
using View_Model.DB;

namespace Robot_Garage.Pages
{
    /// <summary>
    /// Interaction logic for ForgotPasswordWindow.xaml
    /// </summary>
    public partial class ForgotPasswordPage : Page
    {
        UserDB userDB;
        User currentUser;
        public ForgotPasswordPage()
        {
            InitializeComponent();

            userDB = new UserDB();

            NewPasswordGrid.Visibility = Visibility.Collapsed;
        }

        private async void ChangePassword_Click(object sender, RoutedEventArgs e)
        {
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
                    System.Windows.MessageBox.Show("unique code not found!");
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

                    NavigationService?.GoBack();
                }
                else
                {
                    System.Windows.MessageBox.Show("Invalid password, the password is blank!");
                }
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService != null && NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
            else
            {
                System.Windows.MessageBox.Show("No previous page to navigate to.");
            }
        }
    }
}
