using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media.Animation;

namespace Robot_Garage.Controls
{
    /// <summary>
    /// Interaction logic for HorizontalScrollerArrows.xaml
    /// </summary>
    [ContentProperty("Items")]
    public partial class Carousel : UserControl
    {
        public ObservableCollection<UIElement> Items { get; } = new ObservableCollection<UIElement>();

        public Carousel()
        {
            InitializeComponent();
            ItemsHost.ItemsSource = Items;
        }

        // Enhanced Smooth Scroll with longer duration and easing
        private void SmoothScrollTo(double targetOffset)
        {
            double currentOffset = PART_ScrollViewer.HorizontalOffset;

            DoubleAnimation animation = new DoubleAnimation
            {
                From = currentOffset,
                To = targetOffset,
                Duration = TimeSpan.FromMilliseconds(700), // Longer duration for smoother scrolling
                EasingFunction = new QuinticEase { EasingMode = EasingMode.EaseInOut } // Smoother easing
            };

            animation.Completed += (s, e) => PART_ScrollViewer.ScrollToHorizontalOffset(targetOffset);

            AnimationClock clock = animation.CreateClock();
            clock.CurrentTimeInvalidated += (s, e) =>
            {
                if (clock.CurrentProgress.HasValue)
                {
                    double value = currentOffset + (targetOffset - currentOffset) * clock.CurrentProgress.Value;
                    PART_ScrollViewer.ScrollToHorizontalOffset(value);
                }
            };

            clock.Controller.Begin();
        }

        // Left Arrow click logic
        private void OnLeftClick(object sender, RoutedEventArgs e)
        {
            double newOffset = Math.Max(0, PART_ScrollViewer.HorizontalOffset - 200);
            SmoothScrollTo(newOffset);
        }

        // Right Arrow click logic
        private void OnRightClick(object sender, RoutedEventArgs e)
        {
            double maxOffset = PART_ScrollViewer.ScrollableWidth;
            double newOffset = Math.Min(maxOffset, PART_ScrollViewer.HorizontalOffset + 200);
            SmoothScrollTo(newOffset);
        }

        // Show and hide arrows based on scroll position
        private void OnScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            BtnLeft.Visibility = PART_ScrollViewer.HorizontalOffset > 0
                ? Visibility.Visible : Visibility.Collapsed;

            BtnRight.Visibility = PART_ScrollViewer.HorizontalOffset < PART_ScrollViewer.ScrollableWidth
                ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
