using Model;
using View_Model.DB;

namespace View_Model.Services
{
    public class UserSVC
    {
        UserDB userDB = new UserDB();

        public UserList SelectAll()
        {
            return userDB.SelectAll();
        }

        public User SelectByID(int id)
        {
            return userDB.SelectByID(id);
        }

        public User Login(string name, int groupnumber, string password)
        {
            return userDB.Login(name, groupnumber, password);
        }

        public void Insert(User user)
        {
            userDB.Insert(user);
			userDB.SaveChanges();
		}

		public void Update(User user)
        {
            userDB.Update(user);
			userDB.SaveChanges();
        }

        public void Delete(User user)
        {
            userDB.Delete(user);
			userDB.SaveChanges();
        }
    }
}
