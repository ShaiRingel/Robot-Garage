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
	/// Interaction logic for ChatWindow.xaml
	/// </summary>
	public partial class ChatWindow : Window {
		public ChatWindow() {
			InitializeComponent();
			SetResourceReference(StyleProperty, typeof(Window));
		}

		private void SendButton_Click(object sender, RoutedEventArgs e) {
			string message = MessageInput.Text.Trim();
			if (!string.IsNullOrEmpty(message)) {
				TextBlock newMessage = new TextBlock {
					Text = $"You: {message}",
					Margin = new Thickness(5)
				};

				MessagesPanel.Children.Add(newMessage);

				MessageInput.Text = string.Empty;
			}
		}
	}
}
