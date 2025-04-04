using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
	public class Request : BaseEntity {
		private Product product;
		private Renter renter;
		private decimal desiredPrice;
		private string requestStatus;

		public Product Product {
			get => product;
			set => product = value;
		}

		public Renter Renter {
			get => renter;
			set => renter = value;
		}

		public decimal DesiredPrice {
			get => desiredPrice;
			set => desiredPrice = value;
		}

		public string RequestStatus {
			get => requestStatus;
			set => requestStatus = value;
		}
	}
}
