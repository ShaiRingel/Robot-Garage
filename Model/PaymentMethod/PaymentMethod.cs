using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model {
	public class PaymentMethod : BaseEntity {
		private User user;
		private string cardholderName;
		private string cardNumber;
		private DateTime expiry;
		private int cvc;

		public User User {
			get => user;
			set => user = value;
		}

		public string CardholderName { 
			get => cardholderName;
			set => cardholderName = value;
		}

		public string CardNumber { 
			get => cardNumber;
			set => cardNumber = value;
		}

		public DateTime Expiry {
			get => expiry;
			set => expiry = value;
		}

		public int Cvc {
			get => cvc;
			set => cvc = value;
		}
	}
}
