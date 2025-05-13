using Model;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using View_Model.DB;
using Xceed.Wpf.Toolkit;

namespace Robot_Garage.Pages
{
    /// <summary>
    /// Interaction logic for ProductUploadPage.xaml
    /// </summary>
    public partial class ProductUploadPage : Page
    {
        ProductDB productDB = new ProductDB();

        public ProductUploadPage()
        {
            InitializeComponent();
            PopulateList(typeof(ItemCondition), txtCondition);
            PopulateList(typeof(ItemCategory), txtCategory);
        }

        private void PopulateList(Type enumType, ComboBox comboBox)
        {
            var items = Enum.GetValues(enumType).Cast<Enum>();

            foreach (var item in items)
            {
                comboBox.Items.Add(new ComboBoxItem
                {
                    Content = item.ToString(),
                    Tag = item
                });
            }
        }

        private void ConditionListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateSelectedItem(txtCondition);
        }

        private void CategoryListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateSelectedItem(txtCategory);
        }

        private void UpdateSelectedItem(ComboBox comboBox)
        {
            if (comboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                comboBox.SelectedItem = selectedItem;
            }
            else
            {
                comboBox.SelectedItem = null;
            }
        }


        private void SelectImage_Click(object sender, RoutedEventArgs e)
        {
            // Open file dialog to select an image
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Image Files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                // Set the selected image path to the button content
                ImgPreview.Source = new BitmapImage(new Uri(openFileDialog.FileName));
                txtImagePath.Text = openFileDialog.FileName;
                Debug.WriteLine(File.ReadAllBytes(txtImagePath.Text));
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService != null && NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
            else
            {
                System.Windows.MessageBox.Show("No previous page to navigate to.");
            }
        }

        private async void Upload_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine($"Username: {App.CurrentUser.Username}" +
                $"\nGroup Name: {App.CurrentUser.GroupNumber}" +
                $"\nPassword: {App.CurrentUser.Password}" +
                $"\nUnique Code: {App.CurrentUser.UniqueCode}");

            if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtPrice.Text))
            {
                System.Windows.MessageBox.Show("Please fill in all required fields.");
                return;
            }

            if (!File.Exists(txtImagePath.Text))
            {
                System.Windows.MessageBox.Show("Image either was not uploaded, or doesn't exist on computer, Try selecting another file!");
                return;
            }

            byte[] imageBytes = File.ReadAllBytes(txtImagePath.Text);

            // Check if a valid condition and category are selected
            if (txtCondition.SelectedItem is ComboBoxItem selectedCondition && selectedCondition.Tag != null &&
                txtCategory.SelectedItem is ComboBoxItem selectedCategory && selectedCategory.Tag != null)
            {
                Product newProduct = new Product
                {
                    Owner = (Captain)App.CurrentUser,
                    Name = txtName.Text,
                    Price = double.Parse(txtPrice.Text),
                    Description = txtDescription.Text,
                    DatePosted = DateTime.Now,
                    Image = imageBytes,
                    Condition = (ItemCondition)selectedCondition.Tag,
                    Category = (ItemCategory)selectedCategory.Tag,
                    Availability = true
                };

                productDB.Insert(newProduct);
                productDB.SaveChanges();

                txtSuccess.Text = "Successfully Registered Product!, Going back to the main menu!";

                await Task.Delay(500);

                NavigationService?.Navigate(new MainPage());
            }
            else
            {
                System.Windows.MessageBox.Show("Please select a valid condition and category.");
                return;
            }
        }

        private void txtPrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!(sender is IntegerUpDown control))
                return;

            if (!(control.Template.FindName("PART_TextBox", control) is TextBox textBox))
                return;

            int selectionStart = textBox.SelectionStart;
            int selectionLength = textBox.SelectionLength;
            string currentText = control.Text ?? string.Empty;

            string newText = currentText.Remove(selectionStart, selectionLength)
                                        .Insert(selectionStart, e.Text);

            if (newText.Length > 1 && newText.StartsWith("0"))
            {
                e.Handled = true;
                return;
            }

            if (int.TryParse(newText, out int prospectiveValue))
            {
                if (prospectiveValue > control.Maximum)
                {
                    e.Handled = true;
                }
            }
            else
            {
                e.Handled = true;
            }
        }

        private void txtName_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtNameCounter.Text = $"{txtName.Text.Length}/20";
        }

        private void txtDescription_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtDescriptionCounter.Text = $"{txtDescription.Text.Length}/350";
        }

        private void txtDescription_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Return)
            {
                e.Handled = true; // Prevents the Enter key from being processed
            }
        }

    }
}
