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
		public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register(
			"ImageSource",
			typeof(ImageSource),
			typeof(Card),
			new PropertyMetadata(null));


		public static readonly DependencyProperty PriceProperty =
	DependencyProperty.Register(
		"Price",
		typeof(double),
		typeof(Card),
		new PropertyMetadata(0.0));

		public ImageSource ImageSource {
			get { return (ImageSource)GetValue(ImageSourceProperty); }
			set { SetValue(ImageSourceProperty, value); }
		}

		public double Price {
			get { return (double)GetValue(PriceProperty); }
			set { SetValue(PriceProperty, value); }
		}

		public Card() {
			InitializeComponent();
		}
	}
}
