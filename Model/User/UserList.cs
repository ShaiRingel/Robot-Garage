using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model {
	public class UserList : List<User> {
		public UserList() { }
		public UserList(IEnumerable<User> list) : base(list) { }
		public UserList(IEnumerable<BaseEntity> list) : base(list.Cast<User>().ToList()) { }

		public override string ToString() {
			string output = "";

			for (int i = 0; i < this.Count; i++) {
				output += this[i] + "\n";
			}
			return output;
		}

		public void PrintUsers() {
			for (int i = 0; i < this.Count; i++) {
				Debug.WriteLine("---------------------------");
				Debug.WriteLine($"ID: {this[i].ID}" +
					$"Username: {this[i].Username}" +
					$"Group Number: {this[i].GroupNumber}" +
					$"Unique Code: {this[i].UniqueCode}");
			}
			Debug.WriteLine("---------------------------");
		}
	}
}
