using System.Diagnostics;

namespace Model {
	public class CaptainList : List<Captain> {
		public CaptainList() { }
		public CaptainList(IEnumerable<Captain> list) : base(list) { }
		public CaptainList(IEnumerable<BaseEntity> list) : base(list.Cast<Captain>().ToList()) { }

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
