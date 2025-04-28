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

namespace Robot_Garage.Controls {
	/// <summary>
	/// Interaction logic for Card.xaml
	/// </summary>
	public partial class Card : UserControl {
		User _loggedUser;

		public Card() {
			InitializeComponent();
		}

		private void ReadMoreButton_Click(object sender, RoutedEventArgs e) {
			// Get the bound Product object from the DataContext
			if (DataContext is Product product) {
				// Navigate to ProductPage with the specific product
				if (_loggedUser != null) {
					var productPage = new ProductPage(product, _loggedUser);
					NavigationService.GetNavigationService(this)?.Navigate(productPage);
				}
				else {
					MessageBox.Show("Logged user is not set.");
				}
			}
		}
	}
}
