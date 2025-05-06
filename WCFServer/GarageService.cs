using Model;
using View_Model.Services;

namespace WCFServer
{
    public class GarageService : IGarageService
    {
        ProductSVC productSVC = new ProductSVC();
        UserSVC userSVC = new UserSVC();
        MessageSVC messageSVC = new MessageSVC();
        public List<Product> GetAllProducts()
        {
            return productSVC.GetAll();
        }

        public Product GetProductByID(int id)
        {
            return productSVC.GetByID(id);
        }

        public List<User> GetAllUsers()
        {
            return userSVC.GetAll();
        }

        public User GetUserByID(int id)
        {
            return userSVC.GetByID(id);
        }

        public bool Login(string username, int groupnumber, string password)
        {
            if (string.IsNullOrWhiteSpace(username) ||
                groupnumber <= 0 ||
                string.IsNullOrWhiteSpace(password))
                return false;

            if (userSVC.Login(username, groupnumber, password) != null)
            {
                return true;
            }

            return false;
        }

        public List<Message> GetAllMessages()
        {
            return messageSVC.GetAll();
        }

        public Message GetMessageByID(int id)
        {
            return messageSVC.GetByID(id);
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

