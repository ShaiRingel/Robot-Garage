using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Robot_Garage.Controls
{
    /// <summary>
    /// Interaction logic for MessageBox.xaml
    /// </summary>
    public partial class MessageBubble : UserControl
    {
        public MessageBubble()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty MessageTextProperty =
            DependencyProperty.Register("MessageText", typeof(string), typeof(MessageBubble), new PropertyMetadata(string.Empty));

        public string MessageText
        {
            get => (string)GetValue(MessageTextProperty);
            set => SetValue(MessageTextProperty, value);
        }

        public static readonly DependencyProperty IsSenderProperty =
            DependencyProperty.Register("IsSender", typeof(bool), typeof(MessageBubble), new PropertyMetadata(true, OnIsSenderChanged));

        public bool IsSender
        {
            get => (bool)GetValue(IsSenderProperty);
            set => SetValue(IsSenderProperty, value);
        }

        private static void OnIsSenderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MessageBubble messageBox)
            {
                messageBox.UpdateBackground();
            }
        }

        private void UpdateBackground()
        {
            Background = IsSender
                ? (Brush)FindResource("LightGreenBrush")
                : (Brush)FindResource("LightBlueBrush");
        }
    }
}
