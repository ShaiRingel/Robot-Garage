using Model;
using Robot_Garage.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
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
using System.Windows.Threading;
using View_Model;

namespace Robot_Garage.Pages {
	/// <summary>
	/// Interaction logic for ChatPage.xaml
	/// </summary>
	public partial class ChatPage : Page {
		private User _loggedUser;
		private User _otherUser;
		private MessageDB messagesDB;
		private DispatcherTimer messagePollingTimer;
		private MessageList currentMessages;

		public ChatPage(User loggedUser, User otherUser) {
			InitializeComponent();

			_loggedUser = loggedUser;
			_otherUser = otherUser;
			messagesDB = new MessageDB();
			currentMessages = new MessageList();

			txtName.Text = _otherUser.Username;

			messagePollingTimer = new DispatcherTimer();
			messagePollingTimer.Interval = TimeSpan.FromSeconds(1);
			messagePollingTimer.Tick += MessagePollingTimer_Tick;
			messagePollingTimer.Start();

			LoadMessages();


			Chat.ScrollToEnd();
		}

		private void LoadMessages() {
			currentMessages = messagesDB.GetAllMessagesInChat(_loggedUser.ID, _otherUser.ID);
			MessagesPanel.Children.Clear();

			foreach (Message message in currentMessages) {
				AddMessageToUI(message);
			}
		}

		private void MessagePollingTimer_Tick(object sender, EventArgs e) {
			var latestMessages = messagesDB.GetAllMessagesInChat(_loggedUser.ID, _otherUser.ID);

			var newMessages = latestMessages.Except(currentMessages).ToList();

			if (newMessages.Any()) {
				foreach (var message in newMessages) {
					AddMessageToUI(message);
					currentMessages.Add(message);
				}
			}
		}

		private void AddMessageToUI(Message message) {
			MessageBubble newMessage = new MessageBubble();
			newMessage.SetValue(MessageBubble.MessageTextProperty, message.Content);
			newMessage.SetValue(MessageBubble.IsSenderProperty, message.Sender.ID == _loggedUser.ID);
			newMessage.SetValue(MessageBubble.BackgroundProperty, System.Windows.Media.Brushes.Transparent);
			MessagesPanel.Children.Add(newMessage);
		}

		private void SendButton_Click(object sender, RoutedEventArgs e) {
			string message = MessageInput.Text.Trim();
			if (!string.IsNullOrEmpty(message)) {
				var newMessage = new Message() {
					Product = new Product() { ID = 1 },
					Sender = _loggedUser,
					Receiver = _otherUser,
					Content = message,
					Timestamp = DateTime.Now
				};

				messagesDB.Insert(newMessage);
				MessageInput.Text = string.Empty;
			}
		}

		private void BackButton_Click(object sender, RoutedEventArgs e) {
			if (NavigationService != null && NavigationService.CanGoBack) {
				messagePollingTimer.Stop();
				NavigationService?.Navigate(new ChatsListPage(_loggedUser));
			}
			else {
				MessageBox.Show("No previous page to navigate to.");
			}
		}
	}
}
