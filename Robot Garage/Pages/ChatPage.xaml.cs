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
using View_Model;

namespace Robot_Garage {
	/// <summary>
	/// Interaction logic for ChatWindow.xaml
	/// </summary>
	public partial class ChatPage : Page {

		private User _loggedUser;
		private User _otherUser;
		MessageDB messagesDB;

		public ChatPage(User loggedUser, User otherUser) {
			InitializeComponent();

			_loggedUser = loggedUser;
			_otherUser = otherUser;
			messagesDB = new MessageDB();

			createAllMessages();
		}

		private void createAllMessages()
		{ 
			MessageList messages = messagesDB.GetAllMessagesInChat(1, 2);

			foreach (Message message in messages)
			{
				CreateMessage(message);
			}
		}

		private void SendButton_Click(object sender, RoutedEventArgs e) {
			string message = MessageInput.Text.Trim();
			if (!string.IsNullOrEmpty(message)) {
				messagesDB.Insert(new Message()
				{
					Product = null,
					Sender = _loggedUser,
					Receiver = _otherUser,
					Content = message,
					Timestamp = DateTime.Now
				});
			}
		}


		private void CreateMessage(Message message)
		{
			MessageBubble newMessage = new MessageBubble();
			newMessage.SetValue(MessageBubble.MessageTextProperty, message.Content);
			newMessage.SetValue(MessageBubble.IsSenderProperty, message.Sender.ID == _loggedUser.ID);
			newMessage.SetValue(MessageBubble.BackgroundProperty, System.Windows.Media.Brushes.Transparent);
			MessagesPanel.Children.Add(newMessage);
		}
	}
}
