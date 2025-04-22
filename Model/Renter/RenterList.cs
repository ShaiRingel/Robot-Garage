using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model {
	public class RenterList : List<Renter> {
		public RenterList() { }
		public RenterList(IEnumerable<Renter> list) : base(list) { }
		public RenterList(IEnumerable<BaseEntity> list) : base(list.Cast<Renter>().ToList()) { }

		public override string ToString() {
			string output = "";

			for (int i = 0; i < this.Count; i++) {
				output += this[i] + "\n";
			}
			return output;
		}

		public void PrintRenter() {
			for (int i = 0; i < this.Count; i++) {
				Debug.WriteLine("---------------------------");
				Debug.WriteLine($"ID: {this[i].ID}" +
					$"Rentername: {this[i].Username}" +
					$"Group Number: {this[i].GroupNumber}" +
					$"Unique Code: {this[i].UniqueCode}");
			}
			Debug.WriteLine("---------------------------");
		}
	}
}
