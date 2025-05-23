namespace Model {
	public class PaymentMethod : BaseEntity {
		private int userID;
		private string cardholderName;
		private string cardNumber;
		private DateTime expiry;
		private int cvc;

		public int UserID {
			get => userID;
			set => userID = value;
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
