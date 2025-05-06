using Model;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using View_Model.DB;
using Xceed.Wpf.Toolkit;

namespace Robot_Garage.Pages
{
    /// <summary>
    /// Interaction logic for PaymentPage.xaml
    /// </summary>
    public partial class PaymentPage : Page
    {
        public OrderItem OrderItem { get; set; }

        private Product _selectedProduct;
        private User _loggedUser;

        public PaymentPage(User loggedUser, Product product)
        {
            InitializeComponent();
            _selectedProduct = product;
            _loggedUser = loggedUser;

            BitmapImage productImage = new BitmapImage();
            if (product.Image != null)
            {
                using (var stream = new MemoryStream(product.Image))
                {
                    productImage.BeginInit();
                    productImage.CacheOption = BitmapCacheOption.OnLoad;
                    productImage.StreamSource = stream;
                    productImage.EndInit();
                }
            }

            OrderItem = new OrderItem
            {
                Name = product.Name,
                Price = (decimal)product.Price,
                Image = productImage
            };

            DataContext = this;

            UpdateTotal();

            OrderItemsControl.ItemsSource = new[] { OrderItem };
        }

        private void UpdateTotal()
        {
            TotalAmountTextBlock.Text = OrderItem.Price.ToString("C");
        }

        private void PayButton_Click(object sender, RoutedEventArgs e)
        {
            var name = CardholderNameTextBox.Text.Trim();
            var number = CardNumberMaskedTextBox.Text.Replace(" ", "").Trim();
            var exp = ExpirationMaskedTextBox.Text.Trim();
            var cvv = CvvPasswordBox.Password.Trim();
            var address = BillingAddressTextBox.Text.Trim();
            var postal = PostalCodeTextBox.Text.Trim();

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(number) ||
                string.IsNullOrEmpty(exp) || string.IsNullOrEmpty(cvv) ||
                string.IsNullOrEmpty(address) || string.IsNullOrEmpty(postal))
            {
                System.Windows.MessageBox.Show("Please fill in all required fields.");
                return;
            }

            if (!CardNumberMaskedTextBox.IsMaskFull)
            {
                System.Windows.MessageBox.Show("Card number must be 16 digits.");
                return;
            }

            if (!ExpirationMaskedTextBox.IsMaskFull)
            {
                System.Windows.MessageBox.Show("Expiration must be filled.");
                return;
            }

            if (!DateTime.TryParseExact(exp, "MM/yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime expDate))
            {
                System.Windows.MessageBox.Show("Expiration date must be in MM/YY format.");
                return;
            }

            var lastValidDay = new DateTime(
                expDate.Year,
                expDate.Month,
                DateTime.DaysInMonth(expDate.Year, expDate.Month),
                23, 59, 59);
            if (lastValidDay < DateTime.Now)
            {
                System.Windows.MessageBox.Show("This card has already expired.");
                return;
            }


            if (cvv.Length < 3)
            {
                System.Windows.MessageBox.Show("CVV must be 3 or 4 digits.");
                return;
            }

            bool success = ProcessPayment(name, number, exp, cvv, address, OrderItem);

            if (success)
            {
                ProductDB productDB = new ProductDB();

                productDB.UpdateAvailabilityByID(_selectedProduct.ID, false);

                TransactionDB transactionDB = new TransactionDB();

                Transaction newTransaction = new Transaction
                {
                    Renter = _loggedUser,
                    Product = _selectedProduct,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now
                };

                transactionDB.Insert(newTransaction);

                System.Windows.MessageBox.Show("Payment successful! Thank you for your purchase.");
            }
            else
            {
                System.Windows.MessageBox.Show("Payment failed. Please check your details or try again later.");
            }
        }

        private bool ProcessPayment(string name, string number, string exp, string cvv, string address, OrderItem items)
        {
            return true;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService != null && NavigationService.CanGoBack)
            {
                NavigationService?.GoBack();
            }
            else
            {
                System.Windows.MessageBox.Show("No previous page to navigate to.");
            }
        }

        private void NumericOnly(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, "^[0-9]+$");
        }

        private void MaskedTextBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var mtb = sender as MaskedTextBox;
            if (mtb == null) return;

            e.Handled = true;

            if (!mtb.IsKeyboardFocused)
            {
                mtb.Focus();
            }

            MoveCaretToFirstPrompt(mtb);
        }

        private void MaskedTextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            var mtb = sender as MaskedTextBox;
            if (mtb == null) return;
            MoveCaretToFirstPrompt(mtb);
        }

        private void MoveCaretToFirstPrompt(MaskedTextBox mtb)
        {
            // Find the first PromptChar (‘_’) position
            char prompt = mtb.PromptChar;
            int idx = mtb.Text.IndexOf(prompt);
            mtb.CaretIndex = idx >= 0 ? idx : mtb.Text.Length;
        }
    }

    public class OrderItem
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public BitmapImage Image { get; set; }
    }
}
