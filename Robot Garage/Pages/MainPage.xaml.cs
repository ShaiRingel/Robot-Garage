using Model;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using View_Model.DB;

namespace Robot_Garage.Pages
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public ObservableCollection<CardViewModel> RecentlyAddedCards { get; set; }
        public ObservableCollection<CardViewModel> MechanicsCards { get; set; }
        public ObservableCollection<CardViewModel> ElectronicsCards { get; set; }
        public ObservableCollection<CardViewModel> ProgrammingCards { get; set; }
        public ObservableCollection<CardViewModel> EnginesCards { get; set; }
        public ObservableCollection<CardViewModel> ManufacturingCards { get; set; }
        public ObservableCollection<CardViewModel> SelectedCategoryCards { get; set; }
        public ObservableCollection<CardViewModel> MyProducts { get; set; }
        ProductDB productDB;
        private User loggedUser;
        private string _currentPage;

        public MainPage(User loggedUser)
        {
            InitializeComponent();

            _currentPage = "Sales";
            this.loggedUser = loggedUser;
            Loaded += SalesPage_Loaded;

            if (this.loggedUser is Viewer) {
				btnMessages.Visibility = Visibility.Collapsed;
				btnAddProduct.Visibility = Visibility.Collapsed;
                btnMyItems.Visibility = Visibility.Collapsed;
			}

            productDB = new ProductDB();

			// Initialize collections
			RecentlyAddedCards = new ObservableCollection<CardViewModel>();
            MechanicsCards = new ObservableCollection<CardViewModel>();
            ElectronicsCards = new ObservableCollection<CardViewModel>();
            ProgrammingCards = new ObservableCollection<CardViewModel>();
            EnginesCards = new ObservableCollection<CardViewModel>();
            ManufacturingCards = new ObservableCollection<CardViewModel>();
            SelectedCategoryCards = new ObservableCollection<CardViewModel>();
            MyProducts = new ObservableCollection<CardViewModel>();

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
                foreach (var p in productDB.SelectLatestAvailable(15))
                    RecentlyAddedCards.Add(new CardViewModel { Product = p, LoggedUser = loggedUser });

                LoadNCardsByCat(ItemCategory.Mechanics, MechanicsCards, 15);
                LoadNCardsByCat(ItemCategory.Electronics, ElectronicsCards, 15);
                LoadNCardsByCat(ItemCategory.Programming, ProgrammingCards, 15);
                LoadNCardsByCat(ItemCategory.Engines, EnginesCards, 15);
                LoadNCardsByCat(ItemCategory.Manufacturing, ManufacturingCards, 15);
            }
            else
            {
                foreach (var p in productDB.SelectLatestRequested(15))
                    RecentlyAddedCards.Add(new CardViewModel { Product = p, LoggedUser = loggedUser });

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
            var list = productDB.SelectAllAvailableByCategory(cat);
            foreach (var p in list)
                col.Add(new CardViewModel { Product = p, LoggedUser = loggedUser });
        }

        void LoadNCardsByCat(ItemCategory cat, ObservableCollection<CardViewModel> col, int n)
        {
            var list = productDB.SelectLatestAvailableByCategory(n, cat);
            foreach (var p in list)
                col.Add(new CardViewModel { Product = p, LoggedUser = loggedUser });
        }

        void LoadRequestingCardsByCat(ItemCategory cat, ObservableCollection<CardViewModel> col)
        {
            var list = productDB.SelectAllRequestedByCategory(cat);
            foreach (var p in list)
                col.Add(new CardViewModel { Product = p, LoggedUser = loggedUser });
        }

        void LoadNRequestingCardsByCat(ItemCategory cat, ObservableCollection<CardViewModel> col, int n)
        {
            var list = productDB.SelectLatestRequestedByCategory(n, cat);
            foreach (var p in list)
                col.Add(new CardViewModel { Product = p, LoggedUser = loggedUser });
        }

		void LoadMyProducts(ObservableCollection<CardViewModel> col) {
			var list = productDB.SelectByOwnerID(loggedUser.ID);
			foreach (var p in list)
				col.Add(new CardViewModel { Product = p, LoggedUser = loggedUser });
		}

		private Task RefreshCards() {
			RecentlyAddedCards.Clear();
            MechanicsCards.Clear();
            ElectronicsCards.Clear();
            ProgrammingCards.Clear();
            EnginesCards.Clear();
            ManufacturingCards.Clear();
            SelectedCategoryCards.Clear();
            MyProducts.Clear();

			if (_currentPage == "MyItems") {
				LoadMyProducts(MyProducts);
			}
			else {
				if (CategoryPanel.Visibility == Visibility.Visible) {
					LoadCatrgoryCardsProducts(SelectedCategoryTitle.Content.ToString());
				}
				else {
					LoadCardsProducts();
				}
			}

			return Task.CompletedTask;
		}


        private async void btnSales_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage == "Sales")
            {
                return;
            }

            _currentPage = "Sales";

            SortByComboBox.SelectedIndex = 0;

            await RefreshCards();

			gridSales.Visibility = Visibility.Visible;
			gridMyItems.Visibility = Visibility.Collapsed;
		}

        private async void btnRequests_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage == "Requests")
            {
                return;
            }

            _currentPage = "Requests";

            SortByComboBox.SelectedIndex = 0;

            await RefreshCards();

			gridSales.Visibility = Visibility.Visible;
			gridMyItems.Visibility = Visibility.Collapsed;
		}

        private async void btnMyItems_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage == "My Items")
            {
                return;
            }

            _currentPage = "My Items";

            await RefreshCards();

			gridMyItems.Visibility = Visibility.Visible;
            gridSales.Visibility = Visibility.Collapsed;
		}

        private async void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            await RefreshCards();
        }

        private void btnMessages_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new ChatsListPage(loggedUser));
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
            NavigationService?.Navigate(new ProductUploadPage(loggedUser));
        }

        private async void CatagoryButton_Click(object sender, RoutedEventArgs e)
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
				await RefreshCards();
                return;
            }

            // Switch to single-category view
            SortByComboBox.SelectedIndex = 0;
            GeneralPanel.Visibility = Visibility.Collapsed;
            CategoryPanel.Visibility = Visibility.Visible;
            SelectedCategoryTitle.Content = category;

            await RefreshCards();
        }
    }

    public class CardViewModel
    {
        public Product Product { get; set; }
        public User LoggedUser { get; set; }
    }
}
