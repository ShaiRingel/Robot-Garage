using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model {
	public class ViewerList : List<Viewer> {
		public ViewerList() { }
		public ViewerList(IEnumerable<Viewer> list) : base(list) { }
		public ViewerList(IEnumerable<BaseEntity> list) : base(list.Cast<Viewer>().ToList()) { }

		public override string ToString() {
			string output = "";

			for (int i = 0; i < this.Count; i++) {
				output += this[i] + "\n";
			}
			return output;
		}

		public void Print() {
			for (int i = 0; i < this.Count; i++) {
				Debug.WriteLine("---------------------------");
				Debug.WriteLine($"ID: {this[i].ID}" +
					$"\nViewername: {this[i].Username}" +
					$"\nGroup Number: {this[i].GroupNumber}" +
					$"\nUnique Code: {this[i].UniqueCode}");
			}
			Debug.WriteLine("---------------------------");
		}
	}
}
