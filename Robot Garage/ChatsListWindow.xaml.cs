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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Robot_Garage {
	/// <summary>
	/// Interaction logic for ChatsListWindow.xaml
	/// </summary>
	public partial class ChatsListWindow : Window {
		public ChatsListWindow() {
			InitializeComponent();
			SetResourceReference(StyleProperty, typeof(Window));
		}

		private void ChatsList_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			if (ChatsList.SelectedItem != null) {
				ChatWindow chatPage = new ChatWindow();
			}
		}
	}
}
