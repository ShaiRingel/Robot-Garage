﻿using Model;
using Robot_Garage.Controls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows.Threading;
using View_Model.DB;

namespace Robot_Garage.Pages
{
    /// <summary>
    /// Interaction logic for ChatPage.xaml
    /// </summary>
    public partial class ChatPage : Page
    {
        private User _otherUser;
        private MessageDB messagesDB;
        private DispatcherTimer messagePollingTimer;
        private MessageList currentMessages;

        public ChatPage(User otherUser)
        {
            InitializeComponent();

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

        private void LoadMessages()
        {
            currentMessages = messagesDB.SelectConversation(App.CurrentUser.ID, _otherUser.ID);
            MessagesPanel.Children.Clear();

            foreach (Message message in currentMessages)
            {
                AddMessageToUI(message);
            }
        }

        private void MessagePollingTimer_Tick(object sender, EventArgs e)
        {
            var latestMessages = messagesDB.SelectConversation(App.CurrentUser.ID, _otherUser.ID);

            var newMessages = latestMessages.Except(currentMessages).ToList();

            if (newMessages.Any())
            {
                foreach (var message in newMessages)
                {
                    AddMessageToUI(message);
                    currentMessages.Add(message);
                }
            }
        }

        private void AddMessageToUI(Message message)
        {
            MessageBubble newMessage = new MessageBubble();
            newMessage.SetValue(MessageBubble.MessageTextProperty, message.Content);
            newMessage.SetValue(MessageBubble.IsSenderProperty, message.Sender.ID == App.CurrentUser.ID);
            newMessage.SetValue(BackgroundProperty, System.Windows.Media.Brushes.Transparent);
            MessagesPanel.Children.Add(newMessage);
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            string message = MessageInput.Text.Trim();
            if (!string.IsNullOrEmpty(message))
            {
                var newMessage = new Message()
                {
                    Product = new Product() { ID = 1 },
                    Sender = (Captain)App.CurrentUser,
                    Receiver = (Captain)_otherUser,
                    Content = message,
                    Timestamp = DateTime.Now
                };

                messagesDB.Insert(newMessage);
                messagesDB.SaveChanges();
				MessageInput.Text = string.Empty;
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService != null && NavigationService.CanGoBack)
            {
                messagePollingTimer.Stop();
                NavigationService.Refresh();
                NavigationService.GoBack();
            }
            else
            {
                MessageBox.Show("No previous page to navigate to.");
            }
        }
    }
}
