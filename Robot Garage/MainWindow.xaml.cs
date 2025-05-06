using Robot_Garage.Pages;
using System.Windows;

namespace Robot_Garage
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private bool _isUpdatingSize = false;
        private const double TargetAspectRatio = 16.0 / 9.0;

        public MainWindow()
        {
            InitializeComponent();

            SetResourceReference(StyleProperty, typeof(Window));

            MainFrame.Navigate(new LoginPage());
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (_isUpdatingSize)
                return;

            _isUpdatingSize = true;

            double currentWidth = this.ActualWidth;
            double currentHeight = this.ActualHeight;

            double currentAspect = currentWidth / currentHeight;

            if (currentAspect > TargetAspectRatio)
            {
                this.Width = currentHeight * TargetAspectRatio;
            }
            else if (currentAspect < TargetAspectRatio)
            {
                this.Height = currentWidth / TargetAspectRatio;
            }

            _isUpdatingSize = false;
        }
    }
}
