using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model {
	public class ProductList : List<Product> {
		public ProductList() { }
		public ProductList(IEnumerable<Product> list) : base(list) { }
		public ProductList(IEnumerable<BaseEntity> list) : base(list.Cast<Product>().ToList()) { }

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
					$"\nOwner Name: {this[i].Owner.Username}" +
					$"\nProducts Name: {this[i].Name}");
			}
			Debug.WriteLine("---------------------------");
		}
	}
}
