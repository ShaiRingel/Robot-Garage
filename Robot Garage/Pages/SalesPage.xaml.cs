using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using View_Model;

namespace Robot_Garage {
	/// <summary>
	/// Interaction logic for Window1.xaml
	/// </summary>
	public partial class SalesPage : Page {
		public ObservableCollection<CardViewModel> RecentlyAddedCards { get; set; }
		public ObservableCollection<CardViewModel> MechanicsCards { get; set; }
		public ObservableCollection<CardViewModel> ElectronicsCards { get; set; }
		public ObservableCollection<CardViewModel> ProgrammingCards { get; set; }
		public ObservableCollection<CardViewModel> EnginesCards { get; set; }
		public ObservableCollection<CardViewModel> ManufacturingCards { get; set; }
		private User _loggedUser;

		public SalesPage(User loggedUser) {
			InitializeComponent();

			this._loggedUser = loggedUser;
			
			Loaded += SalesPage_Loaded;

			if (this.RenderTransform == null || !(this.RenderTransform is TranslateTransform))
                this.RenderTransform = new TranslateTransform();

			RecentlyAddedCards = new ObservableCollection<CardViewModel>();
			MechanicsCards = new ObservableCollection<CardViewModel>();
			ElectronicsCards = new ObservableCollection<CardViewModel>();
			ProgrammingCards = new ObservableCollection<CardViewModel>();
			EnginesCards = new ObservableCollection<CardViewModel>();
			ManufacturingCards = new ObservableCollection<CardViewModel>();

			LoadCardsProducts();

			DataContext = this;
		}

		private void LoadCardsProducts() {
			ProductDB productDB = new ProductDB();

			// Load Recently Added Products
			List<Product> recentlyAddedProducts = productDB.GetNLatestAvailableProducts(15);
			foreach (Product product in recentlyAddedProducts) {
				RecentlyAddedCards.Add(new CardViewModel {
					Product = product,
					LoggedUser = _loggedUser
				});
			}

			// Load Mechanics Products
			List<Product> mechanicsProducts = productDB.GetAllAvailableProductsByCategory(ItemCategory.Mechanics);
			foreach (Product product in mechanicsProducts) {
				MechanicsCards.Add(new CardViewModel {
					Product = product,
					LoggedUser = _loggedUser
				});
			}

			// Load Electronics Products
			List<Product> electronicsProducts = productDB.GetAllAvailableProductsByCategory(ItemCategory.Electronics);
			foreach (Product product in electronicsProducts) {
				ElectronicsCards.Add(new CardViewModel {
					Product = product,
					LoggedUser = _loggedUser
				});
			}

			// Load Programming Products
			List<Product> programmingProducts = productDB.GetAllAvailableProductsByCategory(ItemCategory.Programming);
			foreach (Product product in programmingProducts) {
				ProgrammingCards.Add(new CardViewModel {
					Product = product,
					LoggedUser = _loggedUser
				});
			}

			// Load Engines Products
			List<Product> enginesProducts = productDB.GetAllAvailableProductsByCategory(ItemCategory.Engines);
			foreach (Product product in enginesProducts) {
				EnginesCards.Add(new CardViewModel {
					Product = product,
					LoggedUser = _loggedUser
				});
			}

			// Load Manufacturing Products
			List<Product> manufacturingProducts = productDB.GetAllAvailableProductsByCategory(ItemCategory.Manufacturing);
			foreach (Product product in manufacturingProducts) {
				ManufacturingCards.Add(new CardViewModel {
					Product = product,
					LoggedUser = _loggedUser
				});
			}
		}


		private void btnSales_Click(object sender, RoutedEventArgs e) {
			MessageBox.Show("Sales clicked");
		}

		private void btnRequests_Click(object sender, RoutedEventArgs e) {
			MessageBox.Show("Requests clicked");
		}

		private void btnMyItems_Click(object sender, RoutedEventArgs e) {
			MessageBox.Show("MyItems clicked");
		}

		private void btnNotifications_Click(object sender, RoutedEventArgs e) {
			MessageBox.Show("Notifications clicked");
		}

		private void btnMessages_Click(object sender, RoutedEventArgs e) {
			NavigationService?.Navigate(new ChatsListPage(_loggedUser));
		}

		private void SalesPage_Loaded(object sender, RoutedEventArgs e) {
			TranslateTransform trans = (TranslateTransform)this.RenderTransform;

			trans.X = this.ActualWidth;

			DoubleAnimation slideIn = new DoubleAnimation(this.ActualWidth, 0, new Duration(TimeSpan.FromMilliseconds(250)));
			trans.BeginAnimation(TranslateTransform.XProperty, slideIn);
		}

		private void btnAddProduct_Click(object sender, RoutedEventArgs e)
		{
			NavigationService?.Navigate(new ProductUploadPage(_loggedUser));
		}
    }

	public class CardViewModel {
		public Product Product { get; set; }
		public User LoggedUser { get; set; }
	}
}
