using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

		public ProductPage(Product product) {
			InitializeComponent();
			_product = product;
		}

		public string Name => _product.Name;
		public string Description => _product.Description;
		public ItemCondition Condition => _product.Condition;
		public decimal Price => _product.Price;
		public string ImageUrl => _product.ImageUrl;
		public bool Availability => _product.Availability;

		public event PropertyChangedEventHandler PropertyChanged;
		protected void OnPropertyChanged([CallerMemberName] string name = null) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}
	}
}
