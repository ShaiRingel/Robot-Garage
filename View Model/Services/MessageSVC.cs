using Model;
using View_Model.DB;

namespace View_Model.Services
{
    public class MessageSVC
    {
        MessageDB messageDB = new MessageDB();

        public MessageList SelectAll()
        {
            return messageDB.SelectAll();
        }

        public Message SelectByID(int id)
        {
            return messageDB.SelectByID(id);
        }

        public MessageList SelectConversation(User sender, User reciever)
        {
            return messageDB.SelectConversation(sender.ID, reciever.ID);
        }

        public void Insert(Message message)
        {
            messageDB.Insert(message);
            messageDB.SaveChanges();
		}

        public void Update(Message message)
        {
            messageDB.Update(message);
			messageDB.SaveChanges();
		}

		public void Delete(Message message)
        {
            messageDB.Delete(message);
            messageDB.SaveChanges();
		}
	}
}
