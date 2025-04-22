using System;
using Model;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace View_Model {
	internal class Program {
		static void Main(string[] args) { 
			MessageDB messageDB = new MessageDB();
			MessageList messageList = messageDB.GetAllMessagesInChat(1, 2);

			Console.WriteLine(messageList);
		}
	}
}
