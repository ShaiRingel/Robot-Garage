using CoreWCF;
using Model;
using View_Model.Services;

namespace WCFServer
{
	public class GarageService : IGarageService {
		ProductSVC productSVC = new ProductSVC();
		UserSVC userSVC = new UserSVC();
		MessageSVC messageSVC = new MessageSVC();

		public ProductList SelectAllProducts() {
			return productSVC.SelectAll();
		}

		public Product SelectProductByID(int id) {
			return productSVC.SelectByID(id);
		}

		public UserList SelectAllUsers() {
			return userSVC.SelectAll();
		}

		public User SelectUserByID(int id) {
			return userSVC.SelectByID(id);
		}

		public bool Login(string username, int groupnumber, string password) {
			if (string.IsNullOrWhiteSpace(username) ||
				groupnumber <= 0 ||
				string.IsNullOrWhiteSpace(password))
				return false;

			if (userSVC.Login(username, groupnumber, password) != null) {
				return true;
			}

			return false;
		}

		public MessageList SelectAllMessages() {
			return messageSVC.SelectAll();
		}

		public Message SelectMessageByID(int id) {
			return messageSVC.SelectByID(id);
		}

		/*public void InsertProduct(Product item) {
			productSVC.InsertProduct(item);
		}

		public void UpdateProduct(Product item) {
			productSVC.UpdateProduct(item);
		}

		public void DeleteProduct(Product item) {
			productSVC.DeleteProduct(item);
		}*/
	}
}

