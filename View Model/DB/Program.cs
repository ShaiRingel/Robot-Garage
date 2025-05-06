using Model;

namespace View_Model.DB
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MessageDB messageDB = new MessageDB();
            MessageList messageList = messageDB.GetAllMessagesInChat(1, 2);

            Console.WriteLine(messageList);
        }
    }
}
