using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using View_Model.DB;

namespace Robot_Garage.Pages {
	/// <summary>
	/// Interaction logic for Window1.xaml
	/// </summary>
	public partial class MainPage : Page {
		public ObservableCollection<CardViewModel> RecentlyAddedCards { get; set; }
		public ObservableCollection<CardViewModel> MechanicsCards { get; set; }
		public ObservableCollection<CardViewModel> ElectronicsCards { get; set; }
		public ObservableCollection<CardViewModel> ProgrammingCards { get; set; }
		public ObservableCollection<CardViewModel> EnginesCards { get; set; }
		public ObservableCollection<CardViewModel> ManufacturingCards { get; set; }
		public ObservableCollection<CardViewModel> SelectedCategoryCards { get; set; }
		ProductDB productDB;
		private User _loggedUser;
		private string _currentPage;

		public MainPage(User loggedUser)
		{
			InitializeComponent();

			_currentPage = "Sales";
			_loggedUser = loggedUser;
			Loaded += SalesPage_Loaded;

			productDB = new ProductDB();

			// Initialize collections
			RecentlyAddedCards = new ObservableCollection<CardViewModel>();
			MechanicsCards = new ObservableCollection<CardViewModel>();
			ElectronicsCards = new ObservableCollection<CardViewModel>();
			ProgrammingCards = new ObservableCollection<CardViewModel>();
			EnginesCards = new ObservableCollection<CardViewModel>();
			ManufacturingCards = new ObservableCollection<CardViewModel>();
			SelectedCategoryCards = new ObservableCollection<CardViewModel>();

			// Load data
			LoadCardsProducts();

			// Default view: General panel
			GeneralPanel.Visibility = Visibility.Visible;
			CategoryPanel.Visibility = Visibility.Collapsed;

			SortByComboBox.SelectedIndex = 0;

			// Bind DataContext
			DataContext = this;
		}

		private void LoadCardsProducts()
		{
			if (_currentPage == "Sales")
			{
				foreach (var p in productDB.GetNLatestAvailableProducts(15))
					RecentlyAddedCards.Add(new CardViewModel { Product = p, LoggedUser = _loggedUser });
				
				LoadNCardsByCat(ItemCategory.Mechanics, MechanicsCards, 15);
				LoadNCardsByCat(ItemCategory.Electronics, ElectronicsCards, 15);
				LoadNCardsByCat(ItemCategory.Programming, ProgrammingCards, 15);
				LoadNCardsByCat(ItemCategory.Engines, EnginesCards, 15);
				LoadNCardsByCat(ItemCategory.Manufacturing, ManufacturingCards, 15);
			}
			else
			{
				foreach (var p in productDB.GetNLatestRequestedAvailableProducts(15))
					RecentlyAddedCards.Add(new CardViewModel { Product = p, LoggedUser = _loggedUser });

				LoadNRequestingCardsByCat(ItemCategory.Mechanics, MechanicsCards, 15);
				LoadNRequestingCardsByCat(ItemCategory.Electronics, ElectronicsCards, 15);
				LoadNRequestingCardsByCat(ItemCategory.Programming, ProgrammingCards, 15);
				LoadNRequestingCardsByCat(ItemCategory.Engines, EnginesCards, 15);
				LoadNRequestingCardsByCat(ItemCategory.Manufacturing, ManufacturingCards, 15);
			}
		}

		private void LoadCatrgoryCardsProducts(string category)
		{
			ObservableCollection<CardViewModel> source = new ObservableCollection<CardViewModel>();

			if (_currentPage == "Sales")
			{
				switch (category)
				{
					case "Mechanics":
						LoadCardsByCat(ItemCategory.Mechanics, source);
						break;
					case "Electronics":
						LoadCardsByCat(ItemCategory.Electronics, source);
						break;
					case "Programming":
						LoadCardsByCat(ItemCategory.Programming, source);
						break;
					case "Engines":
						LoadCardsByCat(ItemCategory.Engines, source);
						break;
					case "Manufacturing":
						LoadCardsByCat(ItemCategory.Manufacturing, source);
						break;
					default:
						source = RecentlyAddedCards;
						break;
				}
			}
			else
			{
				switch (category)
				{
					case "Mechanics":
						LoadRequestingCardsByCat(ItemCategory.Mechanics, source);
						break;
					case "Electronics":
						LoadRequestingCardsByCat(ItemCategory.Electronics, source);
						break;
					case "Programming":
						LoadRequestingCardsByCat(ItemCategory.Programming, source);
						break;
					case "Engines":
						LoadRequestingCardsByCat(ItemCategory.Engines, source);
						break;
					case "Manufacturing":
						LoadRequestingCardsByCat(ItemCategory.Manufacturing, source);
						break;
					default:
						source = RecentlyAddedCards;
						break;
				}
			}

			SelectedCategoryCards.Clear();
			foreach (var card in source)
				SelectedCategoryCards.Add(card);
		}

		void LoadCardsByCat(ItemCategory cat, ObservableCollection<CardViewModel> col)
		{
			var list = productDB.GetAllAvailableProductsByCategory(cat);
			foreach (var p in list)
				col.Add(new CardViewModel { Product = p, LoggedUser = _loggedUser });
		}

		void LoadNCardsByCat(ItemCategory cat, ObservableCollection<CardViewModel> col, int n)
		{
			var list = productDB.GetNLatestAvailableProductsByCategory(n, cat);
			foreach (var p in list)
				col.Add(new CardViewModel { Product = p, LoggedUser = _loggedUser });
		}

		void LoadRequestingCardsByCat(ItemCategory cat, ObservableCollection<CardViewModel> col)
		{
			var list = productDB.GetAllRequestedAvailableProductsByCategory(cat);
			foreach (var p in list)
				col.Add(new CardViewModel { Product = p, LoggedUser = _loggedUser });
		}

		void LoadNRequestingCardsByCat(ItemCategory cat, ObservableCollection<CardViewModel> col, int n)
		{
			var list = productDB.GetNLatestRequestedAvailableProductsByCategory(n, cat);
			foreach (var p in list)
				col.Add(new CardViewModel { Product = p, LoggedUser = _loggedUser });
		}

		void RefreshCards()
		{
			RecentlyAddedCards.Clear();
			MechanicsCards.Clear();
			ElectronicsCards.Clear();
			ProgrammingCards.Clear();
			EnginesCards.Clear();
			ManufacturingCards.Clear();
			SelectedCategoryCards.Clear();

			if (CategoryPanel.Visibility == Visibility.Visible)
			{
				LoadCatrgoryCardsProducts(SelectedCategoryTitle.Content.ToString());
			}
			else
			{
				LoadCardsProducts();
			}
		}


		private void btnSales_Click(object sender, RoutedEventArgs e) {
			if (_currentPage == "Sales") {
				return;
			}

			_currentPage = "Sales";

			SortByComboBox.SelectedIndex = 0;

			RefreshCards();
		}

		private void btnRequests_Click(object sender, RoutedEventArgs e) {
			if (_currentPage == "Requests") {
				return;
			}

			_currentPage = "Requests";

			SortByComboBox.SelectedIndex = 0;

			RefreshCards();
		}

		private void btnMyItems_Click(object sender, RoutedEventArgs e) {
			if (_currentPage == "My Items") {
				return;
			}

			_currentPage = "My Items";
			MessageBox.Show("My Items clicked");
		}

		private void btnRefresh_Click(object sender, RoutedEventArgs e) {
			RefreshCards();
		}

		private void btnMessages_Click(object sender, RoutedEventArgs e) {
			NavigationService?.Navigate(new ChatsListPage(_loggedUser));
		}

		private void SalesPage_Loaded(object sender, RoutedEventArgs e)
		{
			if (!(RenderTransform is TranslateTransform trans))
				RenderTransform = trans = new TranslateTransform();

			trans.X = ActualWidth;
			var anim = new DoubleAnimation(ActualWidth, 0, TimeSpan.FromMilliseconds(250));
			trans.BeginAnimation(TranslateTransform.XProperty, anim);
		}

		private void SortByComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			// Determine selected sort criteria
			var selected = SortByComboBox.SelectedItem as ComboBoxItem;
			if (selected == null) return;
			var criteria = selected.Tag.ToString();

			// Assuming SelectedCategoryCards is an ObservableCollection<CardViewModel>
			var items = SelectedCategoryCards.ToList();
			switch (criteria)
			{
				case "ReleaseDate":
					items = items.OrderByDescending(i => i.Product.DatePosted).ToList();
					break;
				case "Condition":
					items = items.OrderBy(i => i.Product.Condition).ToList();
					break;
				case "PriceAsc":
					items = items.OrderBy(i => i.Product.Price).ToList();
					break;
				case "PriceDesc":
					items = items.OrderByDescending(i => i.Product.Price).ToList();
					break;
			}

			// Refresh the bound collection
			SelectedCategoryCards.Clear();
			foreach (var item in items)
				SelectedCategoryCards.Add(item);
		}

		private void btnAddProduct_Click(object sender, RoutedEventArgs e)
		{
			NavigationService?.Navigate(new ProductUploadPage(_loggedUser));
		}

		private void CatagoryButton_Click(object sender, RoutedEventArgs e)
		{
			var btn = (Button)sender;
			var panel = btn.Content as StackPanel;
			var label = panel.Children.OfType<Label>().FirstOrDefault();
			var category = label?.Content.ToString();

			if (category == "General")
			{
				// Show all carousels
				GeneralPanel.Visibility = Visibility.Visible;
				CategoryPanel.Visibility = Visibility.Collapsed;
				RefreshCards();
				return;
			}

			// Switch to single-category view
			SortByComboBox.SelectedIndex = 0;
			GeneralPanel.Visibility = Visibility.Collapsed;
			CategoryPanel.Visibility = Visibility.Visible;
			SelectedCategoryTitle.Content = category;

			RefreshCards();
		}
	}

	public class CardViewModel {
		public Product Product { get; set; }
		public User LoggedUser { get; set; }
	}
}
