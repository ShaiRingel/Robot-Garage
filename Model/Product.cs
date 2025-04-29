using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using static System.Net.Mime.MediaTypeNames;

namespace Model {

	public enum ItemCondition {
		New,        // Unopened, unused, in original packaging.
		LikeNew,    // Appears new, minor signs of use.
		VeryGood,   // Excellent condition, minimal wear.
		Good,       // Functional, moderate signs of use.
		Acceptable, // Worn but still works as intended.
		Refurbished // Professionally restored to working order.
	}

	public enum ItemCategory {
		Mechanics,
		Electronics,
		Programming,
		Engines,
		Manufacturing,
	}

	public class Product : BaseEntity {
		private User owner;
		private string name;
		private string description;
		private DateTime datePosted;
		private ItemCondition condition;
		private ItemCategory category;
		private double price;
		private byte[] image;
		private bool availability;

		public User Owner {
			get => owner;
			set => owner = value;
		}

		public string Name {
			get => name;
			set => name = value;
		}

		public string Description {
			get => description;
			set => description = value;
		}

		public DateTime DatePosted {
			get => datePosted;
			set => datePosted = value;
		}

		public ItemCondition Condition {
			get => condition;
			set => condition = value;
		}

		public ItemCategory Category {
			get => category;
			set => category = value;
		}

		public double Price {
			get => price;
			set => price = value;
		}

		public byte[] Image {
			get => image;
			set => image = value;
		}

		public bool Availability {
			get => availability;
			set => availability = value;
		}
	}
}