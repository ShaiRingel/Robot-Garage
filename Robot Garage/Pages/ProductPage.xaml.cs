using Model;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using View_Model.DB;

namespace Robot_Garage.Pages
{
    /// <summary>
    /// Interaction logic for ProductWindow.xaml
    /// </summary>
    public partial class ProductPage : Page
    {
        private readonly ProductDB productDB;
        private readonly Product _product;
        private readonly User _loggedUser;

        public ProductPage(Product product, User loggedUser)
        {
            InitializeComponent();

            productDB = new ProductDB();
            _product = productDB.GetProductByID(product.ID);
            _loggedUser = loggedUser;

            if (_product.Request) {
                btnBuy.Visibility = Visibility.Hidden;
                btnRequest.Visibility = Visibility.Visible;
			}

            if (_product.Owner.ID == _loggedUser.ID)
            {
                btnContant.Visibility = Visibility.Hidden;
                btnBuy.IsEnabled = false;
            }

            DataContext = _product;
        }

        public string Name => _product.Name;
        public string Description => _product.Description;
        public ItemCondition Condition => _product.Condition;
        public double Price => _product.Price;
        public byte[] Image => _product.Image;
        public bool Availability => _product.Availability;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService != null && NavigationService.CanGoBack)
            {
                NavigationService?.GoBack();
                // NavigationService?.Navigate(new MainPage(_loggedUser));
            }
            else
            {
                MessageBox.Show("No previous page to navigate to.");
            }
        }

        private void btnBuy_Click(object sender, RoutedEventArgs e)
        {
            if (!productDB.GetProductByID(_product.ID).Availability)
            {
                MessageBox.Show("Product was probably bought in the last couple of minutes, therefore is not available for purchase.");
                return;
            }

            productDB.UpdateAvailabilityByID(_product.ID, false);

            NavigationService?.Navigate(new PaymentPage(_loggedUser, _product));
        }

        private void btnRequest_Click(object sender, RoutedEventArgs e)
        {
            if (!productDB.GetProductByID(_product.ID).Availability)
            {
                MessageBox.Show("Product was probably bought in the last couple of minutes, therefore is not available for purchase.");
                return;
            }


        }

        private void ChatButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new ChatPage(_loggedUser, _product.Owner));
        }
    }
}
