using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using View_Model.DB;

namespace View_Model.Services {
	public class MessageSVC {
		MessageDB messageDB = new MessageDB();

		public List<Message> GetAll() {
			return messageDB.GetAll();
		}

		public Message GetByID(int id) {
			return messageDB.GetMessageByID(id);
		}

		public List<Message> GetByUsers(User sender, User reciever) {
			return messageDB.GetAllMessagesInChat(sender.ID, reciever.ID);
		}

		public void InsertUser(Message message) {
			messageDB.Insert(message);
		}

		public void UpdateProduct(Message message) {
			messageDB.Update(message);
		}

		public void DeleteProduct(Message message) {
			messageDB.Delete(message);
		}
	}
}
