using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model {
	public class Message : BaseEntity {
		private Product product;
		private User sender;
		private User receiver;
		private string content;
		private DateTime timestamp;

		public Product Product {
			get => product;
			set => product = value;
		}
		public User Sender {
			get => sender;
			set => sender = value;
		}
		public User Receiver {
			get => receiver;
			set => receiver = value;
		}
		public string Content {
			get => content;
			set => content = value;
		}
		public DateTime Timestamp {
			get => timestamp;
			set => timestamp = value;
		}
	}
}