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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Robot_Garage {
	/// <summary>
	/// Interaction logic for Window1.xaml
	/// </summary>
	public partial class SalesPage : Page {
		private User _loggedUser;
		private bool _isUpdatingSize = false;
		private const double TargetAspectRatio = 16.0 / 9.0;

		public SalesPage(User loggedUser) {
			InitializeComponent();

			this._loggedUser = loggedUser;
			
			Loaded += SalesPage_Loaded;

			if (this.RenderTransform == null || !(this.RenderTransform is TranslateTransform))
                this.RenderTransform = new TranslateTransform();
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
			NavigationService?.Navigate(new ProductUploadPage());
		}
    }
}
