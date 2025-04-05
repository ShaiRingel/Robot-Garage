using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model {
	public class RenterList : List<User> {
		public RenterList() { }
		public RenterList(IEnumerable<User> list) : base(list) { }
		public RenterList(IEnumerable<BaseEntity> list) : base(list.Cast<User>().ToList()) { }

		public override string ToString() {
			string output = "";

			for (int i = 0; i < this.Count; i++) {
				output += this[i] + "\n";
			}
			return output;
		}

		public void PrintRenter() {
			for (int i = 0; i < this.Count; i++) {
				Console.WriteLine("---------------------------");
				Console.WriteLine($"ID: {this[i].ID}" +
					$"Username: {this[i].Username}" +
					$"Group Number: {this[i].GroupNumber}" +
					$"Unique Code: {this[i].UniqueCode}");
			}
			Console.WriteLine("---------------------------");
		}
	}
}
