using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model {
	public class TransactionList : List<Transaction> {
		public TransactionList() { }
		public TransactionList(IEnumerable<Transaction> list) : base(list) { }
		public TransactionList(IEnumerable<BaseEntity> list) : base(list.Cast<Transaction>().ToList()) { }

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
					$"\nProducts Name: {this[i].Product.Name}" + 
					$"\nBuyer Name: {this[i].Buyer.ID}");
			}
			Debug.WriteLine("---------------------------");
		}
	}
}
