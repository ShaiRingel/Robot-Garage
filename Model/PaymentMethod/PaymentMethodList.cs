using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model {
	public class PaymentMethodList : List<PaymentMethod> {
		public PaymentMethodList() { }
		public PaymentMethodList(IEnumerable<PaymentMethod> list) : base(list) { }
		public PaymentMethodList(IEnumerable<BaseEntity> list) : base(list.Cast<PaymentMethod>().ToList()) { }

		/*public override string ToString() {
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
					$"\nOwner Name: {this[i].Owner.Username}" +
					$"\nProducts Name: {this[i].Name}");
			}
			Debug.WriteLine("---------------------------");
		}*/
	}
}
