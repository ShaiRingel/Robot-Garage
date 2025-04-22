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
using System.Windows.Shapes;

namespace Robot_Garage {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {

		private bool _isUpdatingSize = false;
		private const double TargetAspectRatio = 16.0 / 9.0;

		public MainWindow() {
			InitializeComponent();

			SetResourceReference(StyleProperty, typeof(Window));

			MainFrame.Navigate(new LoginPage());
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
    }
}
