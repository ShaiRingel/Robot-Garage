using CoreWCF;
using Model;
using View_Model.Services;

namespace WCFServer
{
	public class GarageService : IGarageService {
		TransactionSVC transactionSVC = new TransactionSVC();
		PaymentMethodSVC paymentSVC = new PaymentMethodSVC();
		MessageSVC messageSVC = new MessageSVC();
		ProductSVC productSVC = new ProductSVC();
		UserSVC userSVC = new UserSVC();

        #region User Service
        public UserList SelectAllUsers() {
			return userSVC.SelectAll();
		}

		public User SelectUserByID(int id) {
			return userSVC.SelectByID(id);
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
        #endregion

        #region PaymentMethod Service
        public PaymentMethodList SelectAllPaymentMethods()
        {
            return paymentSVC.SelectAll();
        }

        public PaymentMethod SelectPaymentMethodByID(int id)
        {
            return paymentSVC.SelectByID(id);
        }
        #endregion

        #region Product Service
        public ProductList SelectAllProducts() {
			return productSVC.SelectAll();
		}

		public Product SelectProductByID(int id) {
			return productSVC.SelectByID(id);
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
        #endregion

        #region Message Service
        public MessageList SelectAllMessages() {
			return messageSVC.SelectAll();
		}

		public Message SelectMessageByID(int id) {
			return messageSVC.SelectByID(id);
		}
        #endregion

        #region Transaction Service
        public TransactionList SelectAllPaymentMethod()
        {
            return transactionSVC.SelectAll();
        }

        public Transaction SelectTransactionByID(int id)
        {
            return transactionSVC.SelectByID(id);
        }
        #endregion
    }
}

