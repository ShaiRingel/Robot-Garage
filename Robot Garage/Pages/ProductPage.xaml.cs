using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Robot_Garage {
	/// <summary>
	/// Interaction logic for ProductWindow.xaml
	/// </summary>
	public partial class ProductPage : Page {
		private readonly Product _product;
		private readonly User _loggedUser;

		public ProductPage(Product product, User loggedUser) {
			InitializeComponent();
			_product = product;
			_loggedUser = loggedUser;
			if (_product.Owner.ID == _loggedUser.ID)
			{
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

		protected void OnPropertyChanged([CallerMemberName] string name = null) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}

		private void BackButton_Click(object sender, RoutedEventArgs e) {
			if (NavigationService != null && NavigationService.CanGoBack) {
				NavigationService?.Navigate(new SalesPage(_loggedUser));
			}
			else {
				MessageBox.Show("No previous page to navigate to.");
			}
		}
	}
}
