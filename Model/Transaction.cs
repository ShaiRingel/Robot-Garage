using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
	public enum OrderStatus {
		Pending,   // Order placed, awaiting processing.
		Confirmed, // Order confirmed, preparing for shipment.
		Shipped,   // Order dispatched, in transit.
		Delivered, // Order received by customer.
		Canceled,  // Order canceled before processing.
		Returned   // Order sent back by customer.
	}

	public class Transaction : BaseEntity {
		private int product;
		private Renter renter;
		private DateTime startDate;
		private DateTime endDate;
		private OrderStatus status;

		public int Product {
			get => product;
			set => product = value;
		}

		public Renter Renter {
			get => renter;
			set => renter = value;
		}

		public DateTime StartDate {
			get => startDate;
			set => startDate = value;
		}

		public DateTime EndDate {
			get => endDate;
			set => endDate = value;
		}

		public OrderStatus Status {
			get => status;
			set => status = value;
		}
	}

}
