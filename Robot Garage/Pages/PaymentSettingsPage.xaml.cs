using Model;
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
using View_Model.DB;

namespace Robot_Garage.Pages {
	/// <summary>
	/// Interaction logic for SettingsPage.xaml
	/// </summary>
	public partial class PaymentSettingsPage : Page {
		public PaymentSettingsPage() {
			InitializeComponent();
			LoadExistingCard();
		}

		private void LoadExistingCard() {
			var user = App.CurrentUser;
			if (user.PaymentMethod != null) {
				CardholderNameTextBox.Text = user.PaymentMethod.CardholderName;
				CardNumberMaskedTextBox.Text = user.PaymentMethod.CardNumber;
				ExpiryMaskedTextBox.Text = user.PaymentMethod.Expiry.ToString("MM/yy");
			}
		}

		private void SaveButton_Click(object sender, RoutedEventArgs e) {
			string expText = ExpiryMaskedTextBox.Text.Trim();
			DateTime expiry;

			if (!DateTime.TryParseExact(
					expText,
					"MM/yy",
					System.Globalization.CultureInfo.InvariantCulture,
					System.Globalization.DateTimeStyles.None,
					out DateTime expDate)) {
				MessageBox.Show("Expiration date must be in MM/yy format.");
				return;
			}
			expiry = new DateTime(
				expDate.Year,
				expDate.Month,
				DateTime.DaysInMonth(expDate.Year, expDate.Month),
				23, 59, 59
			);

			PaymentMethod method = new PaymentMethod {
				UserID = App.CurrentUser.ID,
				CardholderName = CardholderNameTextBox.Text.Trim(),
				CardNumber = CardNumberMaskedTextBox.Text.Replace(" ", "").Trim(),
				Expiry = expiry,
				Cvc = int.Parse(CvcPasswordBox.Password.Trim())
			};

			App.CurrentUser.PaymentMethod = method;
			PaymentMethodDB paymentMethodDB = new PaymentMethodDB();
			
			if (paymentMethodDB.SelectByUser(App.CurrentUser) == null) {
				paymentMethodDB.Insert(method);
				paymentMethodDB.SaveChanges();
				MessageBox.Show("Payment info saved.");
			}
			else {
				method.ID = paymentMethodDB.SelectByUser(App.CurrentUser).ID;
				paymentMethodDB.Update(method);
				paymentMethodDB.SaveChanges();
				MessageBox.Show("Payment info updated.");
			}

			NavigationService.GoBack();
		}

		private void BackButton_Click(object sender, RoutedEventArgs e) {
			if (NavigationService != null && NavigationService.CanGoBack) {
				NavigationService?.GoBack();
			}
			else {
				MessageBox.Show("No previous page to navigate to.");
			}
		}
	}
}
