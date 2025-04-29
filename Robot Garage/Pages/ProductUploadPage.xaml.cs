using Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using View_Model;
using Xceed.Wpf.Toolkit;

namespace Robot_Garage
{
	/// <summary>
	/// Interaction logic for ProductUploadPage.xaml
	/// </summary>
	public partial class ProductUploadPage : Page {
		ProductDB productDB = new ProductDB();
		private readonly User _loggedUser;

		public ProductUploadPage(User loggedUser) {
			InitializeComponent();
			PopulateList(typeof(ItemCondition), ConditionListBox);
			PopulateList(typeof(ItemCategory), CategoryListBox);
			_loggedUser = loggedUser;
		}

		private void PopulateList(Type enumType, ListBox listBox) {
			var items = Enum.GetValues(enumType).Cast<Enum>();

			foreach (var item in items) {
				listBox.Items.Add(new ListBoxItem {
					Content = item.ToString(),
					Tag = item
				});
			}
		}

		private void txtCondition_Click(object sender, RoutedEventArgs e) {
			ToggleListBoxVisibility(ConditionListBox);
		}

		private void txtCategory_Click(object sender, RoutedEventArgs e) {
			ToggleListBoxVisibility(CategoryListBox);
		}

		private void ToggleListBoxVisibility(ListBox listBox) {
			listBox.Visibility = listBox.Visibility == Visibility.Visible
				? Visibility.Collapsed
				: Visibility.Visible;

			listBox.UpdateLayout();
		}

		private void ConditionListBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			UpdateSelectedItem(ConditionListBox, txtCondition);
		}

		private void CategoryListBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			UpdateSelectedItem(CategoryListBox, txtCategory);
		}

		private void UpdateSelectedItem(ListBox listBox, ContentControl contentControl) {
			if (listBox.SelectedItem is ListBoxItem selectedItem) {
				contentControl.Content = selectedItem.Content.ToString();
				listBox.Visibility = Visibility.Collapsed;
			}
		}

		private void SelectImage_Click(object sender, RoutedEventArgs e) {
			// Open file dialog to select an image
			Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog {
				Filter = "Image Files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png"
			};
			if (openFileDialog.ShowDialog() == true) {
				// Set the selected image path to the button content
				ImgPreview.Source = new BitmapImage(new Uri(openFileDialog.FileName));
				txtImagePath.Text = openFileDialog.FileName;
				Debug.WriteLine(File.ReadAllBytes(txtImagePath.Text));
			}
		}

		private void BackButton_Click(object sender, RoutedEventArgs e) {
			if (NavigationService != null && NavigationService.CanGoBack) {
				NavigationService.GoBack();
			}
			else {
				System.Windows.MessageBox.Show("No previous page to navigate to.");
			}
		}

		private async void Upload_Click(object sender, RoutedEventArgs e) {
			Debug.WriteLine($"Username: {_loggedUser.Username}" +
				$"Group Name: {_loggedUser.GroupNumber}" +
				$"Password: {_loggedUser.Password}" +
				$"Unique Code: {_loggedUser.UniqueCode}");

			if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtPrice.Text)) {
				System.Windows.MessageBox.Show("Please fill in all required fields.");
				return;
			}
			
			if (!File.Exists(txtImagePath.Text)) {
				System.Windows.MessageBox.Show("Image doesn't exist on computer, Try selecting another file!");
				return;
			}

			byte[] imageBytes = File.ReadAllBytes(txtImagePath.Text);
			
			if (ConditionListBox.SelectedItem is ListBoxItem selectedCondition &&
				CategoryListBox.SelectedItem is ListBoxItem selectedCategory) {
				Product newProduct = new Product {
					Owner = _loggedUser,
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

				txtSuccess.Text = "Successfully Registered Product!, Going back to the main menu!";

				await Task.Delay(500);

				NavigationService?.Navigate(new SalesPage(_loggedUser));
			}
			else {
				System.Windows.MessageBox.Show("Please select a valid condition and category.");
				return;
			}
		}

		private void txtPrice_PreviewTextInput(object sender, TextCompositionEventArgs e) {
			if (!(sender is IntegerUpDown control))
				return;

			if (!(control.Template.FindName("PART_TextBox", control) is TextBox textBox))
				return;

			int selectionStart = textBox.SelectionStart;
			int selectionLength = textBox.SelectionLength;
			string currentText = control.Text ?? string.Empty;

			string newText = currentText.Remove(selectionStart, selectionLength)
										.Insert(selectionStart, e.Text);

			if (newText.Length > 1 && newText.StartsWith("0")) {
				e.Handled = true;
				return;
			}

			if (int.TryParse(newText, out int prospectiveValue)) {
				if (prospectiveValue > control.Maximum) {
					e.Handled = true;
				}
			}
			else {
				e.Handled = true;
			}
		}

        private void txtDescription_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtDescriptionCounter.Text = $"{txtDescription.Text.Length}/250";
        }
    }
}
