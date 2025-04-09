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
using System.Windows.Shapes;

namespace Robot_Garage {
	/// <summary>
	/// Interaction logic for Window1.xaml
	/// </summary>
	public partial class SalesWindow : Window {
		private bool _isUpdatingSize = false;
		private const double TargetAspectRatio = 16.0 / 9.0;

		public SalesWindow() {
			InitializeComponent();
			SetResourceReference(StyleProperty, typeof(Window));
			createCard("/assets/images/Hammer.png", 50);
		}

		private void createCard(string source, double price) {
			Card card = new Card();
			card.SetValue(Card.ImageSourceProperty, new BitmapImage(new Uri($"pack://application:,,,{source}")));
			card.SetValue(Card.PriceProperty, price);
			RecentlyAddedCsl.Items.Add(card);
		}

		private void Window_SizeChanged(object sender, SizeChangedEventArgs e) {
			if (_isUpdatingSize)
				return;

			_isUpdatingSize = true;

			double currentWidth = this.ActualWidth;
			double currentHeight = this.ActualHeight;

			double currentAspect = currentWidth / currentHeight;

			if (currentAspect > TargetAspectRatio) {
				this.Width = currentHeight * TargetAspectRatio;
			}
			else if (currentAspect < TargetAspectRatio) {
				this.Height = currentWidth / TargetAspectRatio;
			}

			_isUpdatingSize = false;
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
			new ChatWindow().Show();
			this.Close();
		}
    }
}
