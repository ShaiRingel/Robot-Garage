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
        private readonly Product product;

        public ProductPage(Product product)
        {
            InitializeComponent();

            productDB = new ProductDB();
            this.product = productDB.SelectByID(product.ID);

			if (App.CurrentUser is Viewer) {
				btnBuy.Visibility = Visibility.Hidden;
			}

			if (this.product.Request) {
				btnBuy.Visibility = Visibility.Hidden;
				btnRequest.Visibility = Visibility.Visible;
			}

            if (this.product.Owner.ID == App.CurrentUser.ID)
            {
				btnContant.Visibility = Visibility.Hidden;
				btnBuy.IsEnabled = false;
            }

			DataContext = this.product;
        }

        public string Name => product.Name;
        public string Description => product.Description;
        public ItemCondition Condition => product.Condition;
        public double Price => product.Price;
        public byte[] Image => product.Image;
        public bool Availability => product.Availability;

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
            }
            else
            {
                MessageBox.Show("No previous page to navigate to.");
            }
        }

        private void btnBuy_Click(object sender, RoutedEventArgs e)
        {
			if (App.CurrentUser?.PaymentMethod == null) {
				MessageBox.Show("You need to set up your paymentMethod first in your settings.", "paymentMethod", MessageBoxButton.OK, MessageBoxImage.Error);
				NavigationService.GoBack();
			}
			else {
				if (!productDB.SelectByID(product.ID).Availability) {
					MessageBox.Show("Product was probably bought in the last couple of minutes, therefore is not available for purchase.");
					return;
				}

				MessageBox.Show("The purchase was successful!", "Purchase", MessageBoxButton.OK, MessageBoxImage.Information);

                TransactionDB transactionDB = new TransactionDB();
                Transaction transaction = new Transaction
                {
                    Seller = product.Owner,
                    Buyer = App.CurrentUser,
                    Product = product,
                    Status = OrderStatus.Confirmed
                };
                transactionDB.Insert(transaction);
                transactionDB.SaveChanges();

                product.Availability = false;
                productDB.Update(product);
                productDB.SaveChanges();

				NavigationService.GoBack();
			}

        }

        private void btnRequest_Click(object sender, RoutedEventArgs e)
        {
			if (App.CurrentUser?.PaymentMethod == null) {
				MessageBox.Show("You need to set up your paymentMethod first in your settings.", "paymentMethod", MessageBoxButton.OK, MessageBoxImage.Error);
                NavigationService.GoBack();
            }
			else {
				if (!productDB.SelectByID(product.ID).Availability) {
					MessageBox.Show("Product was probably bought in the last couple of minutes, therefore is not available for purchase.");
					return;
				}

				MessageBox.Show("Request accepted successfully!", "Request", MessageBoxButton.OK, MessageBoxImage.Information);
                
                product.Availability = false;

                TransactionDB transactionDB = new TransactionDB();
                Transaction transaction = new Transaction
                {
                    Seller = product.Owner,
                    Buyer = App.CurrentUser,
                    Product = product,
                    Status = OrderStatus.Confirmed
                };
                transactionDB.Insert(transaction);
                transactionDB.SaveChanges();

                productDB.Update(product);
                productDB.SaveChanges();

                NavigationService.GoBack();
            }
		}

        private void ChatButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new ChatPage(product.Owner));
        }
    }
}
