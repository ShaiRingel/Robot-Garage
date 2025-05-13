using Model;
using Robot_Garage.Pages;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Robot_Garage.Controls
{
    /// <summary>
    /// Interaction logic for Card.xaml
    /// </summary>
    public partial class Card : UserControl
    {
        public Card()
        {
            InitializeComponent();
		}

        private void ReadMoreButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is CardViewModel cardViewModel)
            {
                var product = cardViewModel.Product;

                // Debug NavigationService
                var navigationService = NavigationService.GetNavigationService(this);
                if (navigationService == null)
                {
                    MessageBox.Show("NavigationService is null. Ensure the Card is inside a Frame or Page.");
                    return;
                }

                // Navigate to ProductPage
                if (App.CurrentUser != null)
                {
                    var productPage = new ProductPage(product);
                    navigationService.Navigate(productPage);
                }
                else
                {
                    MessageBox.Show("Logged user is not set.");
                }
            }
        }
    }
}