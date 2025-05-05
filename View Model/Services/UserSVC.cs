using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using View_Model.DB;

namespace View_Model.Services {
	public class UserSVC {
		UserDB userDB = new UserDB();

		public List<User> GetAll() {
			return userDB.GetAllUsers();
		}

		public User GetByID(int id) {
			return userDB.GetUserByID(id);
		}

		public void InsertUser(User user) {
			userDB.Insert(user);
		}

		public void UpdateProduct(User user) {
			userDB.Update(user);
		}

		public void DeleteProduct(User user) {
			userDB.Delete(user);
		}
	}
}
