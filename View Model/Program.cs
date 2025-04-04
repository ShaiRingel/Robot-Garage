using System;
using Model;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace View_Model {
	internal class Program {
		static void Main(string[] args) {
			UserDB users = new UserDB();

			User newUser = new User("Rona Shafer", "Bleit!", 2231, "Amazed");
			users.Insert(newUser);

			UserList usersList = users.GetAll();

			Console.WriteLine(
				"id: " + usersList.First().ID +
				"\nusername: " + usersList.First().Username +
				"\npassword: " + usersList.First().Password +
				"\nGroupNumber: " + usersList.First().GroupNumber +
				"\nUniqueCode: " + usersList.First().UniqueCode
				);

			Console.WriteLine(
				"id: " + usersList[1].ID +
				"\nusername: " + usersList[1].Username +
				"\npassword: " + usersList[1].Password +
				"\nGroupNumber: " + usersList[1].GroupNumber +
				"\nUniqueCode: " + usersList[1].UniqueCode
				);
		}
	}
}
