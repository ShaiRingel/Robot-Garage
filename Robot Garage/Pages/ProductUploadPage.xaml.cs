using Model;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using View_Model;

namespace Robot_Garage
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
			PopulateConditionList();
		}

		private void PopulateConditionList()
		{
			var conditions = Enum.GetValues(typeof(ItemCondition)).Cast<ItemCondition>();
			
			foreach (var condition in conditions)
			{
				ConditionListBox.Items.Add(new ListBoxItem
				{
					Content = condition.ToString(),
					Tag = condition
				});
			}
		}

		private void txtCondition_Click(object sender, RoutedEventArgs e)
		{
			ConditionListBox.Visibility = ConditionListBox.Visibility == Visibility.Visible
				? Visibility.Collapsed
				: Visibility.Visible;

			ConditionListBox.UpdateLayout();
		}

		private void ConditionListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (ConditionListBox.SelectedItem != null)
			{
				txtCondition.Content = ConditionListBox.SelectedItem.ToString();
				ConditionListBox.Visibility = Visibility.Collapsed; // Collapse the ListBox after selection
			}
		}

		private void SelectImage_Click(object sender, RoutedEventArgs e)
		{
			ImgPreview.Visibility = Visibility.Visible;
			ImgButton.Visibility = Visibility.Collapsed;
		}

		private async void Upload_Click(object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtPrice.Text))
			{
				MessageBox.Show("Please fill in all required fields.");
				return;
			}

			if (ConditionListBox.SelectedItem is ListBoxItem selectedCondition)
			{
				Product newProduct = new Product
				{
					Vendor = new Vendor { ID = 1 },
					Name = txtName.Text,
					Price = decimal.Parse(txtPrice.Text),
					Description = txtDescription.Text,
					ImageUrl = ImgPreview.Source.ToString(),
					Condition = (ItemCondition)selectedCondition.Tag, // Use the Tag property of the selected ListBoxItem
					Availability = true
				};

				productDB.Insert(newProduct);

				txtSuccess.Text = "Successfully Registered Product!, Going back to the main menu!";

				await Task.Delay(500);

				NavigationService?.GoBack();
			}
			else
			{
				MessageBox.Show("Please select a valid condition.");
				return;
			}
		}
	}
}
