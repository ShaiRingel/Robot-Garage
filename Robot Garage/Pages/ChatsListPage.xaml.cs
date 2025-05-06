using Model;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using View_Model.DB;

namespace Robot_Garage.Pages
{
    /// <summary>
    /// Interaction logic for ChatsListWindow.xaml
    /// </summary>
    public partial class ChatsListPage : Page
    {
        public ObservableCollection<ChatListItemViewModel> ChatItems { get; set; }

        private User _loggedUser;

        public ChatsListPage(User loggedUser)
        {
            InitializeComponent();

            this._loggedUser = loggedUser;

            ChatItems = new ObservableCollection<ChatListItemViewModel>();

            LoadChatUsersWithLastMessages();

            this.Loaded += (s, e) =>
            {
                if (this.NavigationService != null)
                    this.NavigationService.Navigated += Frame_Navigated;
            };

            this.Unloaded += (s, e) =>
            {
                if (this.NavigationService != null)
                    this.NavigationService.Navigated -= Frame_Navigated;
            };

            DataContext = this;
        }
        private void Frame_Navigated(object sender, NavigationEventArgs e)
        {
            // if this page is now the active content, refresh
            if (e.Content == this)
            {
                LoadChatUsersWithLastMessages();
            }
        }
        private void LoadChatUsersWithLastMessages()
        {
            MessageDB messageDB = new MessageDB();

            var chatUsers = messageDB.GetChatUsers(_loggedUser.ID);

            ChatItems.Clear();

            foreach (var user in chatUsers)
            {
                var lastMessage = messageDB.GetAllMessagesInChat(_loggedUser.ID, user.ID)
                                           .OrderByDescending(m => m.Timestamp)
                                           .FirstOrDefault();

                ChatItems.Add(new ChatListItemViewModel
                {
                    User = user,
                    LastMessage = lastMessage?.Content ?? "No messages yet",
                    LastMessageTime = lastMessage?.Timestamp ?? DateTime.MinValue
                });
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService != null && NavigationService.CanGoBack)
            {
                NavigationService?.GoBack();
            }
            else
            {
                MessageBox.Show("No previous page to navigate to.");
            }
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadChatUsersWithLastMessages();
        }

        private void ChatListItem_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement element && element.DataContext is ChatListItemViewModel chatItem)
            {
                NavigationService?.Navigate(new ChatPage(this._loggedUser, chatItem.User));
            }
        }
    }


    public class ChatListItemViewModel
    {
        public User User { get; set; }
        public string LastMessage { get; set; }
        public DateTime LastMessageTime { get; set; }
    }
}
