using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public enum ItemCondition
    {
        New, // Unopened, unused, in original packaging.
        LikeNew, // Appears new, minor signs of use.
        VeryGood, // Excellent condition, minimal wear.
        Good, // Functional, moderate signs of use.
        Acceptable, // Worn but still works as intended.
        Refurbished, // Professionally restored to working order.
    }

    public class Product
    {
        private int id;
        private Vendor vendor;
        private Renter renter;
        private string name;
        private string description;
        private ItemCondition condition;
        private decimal price;
        private string imageUrl;
        private bool availability;

        public int ID
        {
            get => id;
            set => id = value;
        }

        public Vendor Vendor
        {
            get => vendor;
            set => vendor = value;
        }

        public Renter Renter
        {
            get => renter;
            set => renter = value;
        }

        public string Name
        {
            get => name;
            set => name = value;
        }

        public string Description
        {
            get => description;
            set => description = value;
        }

        public ItemCondition Condition
        {
            get => condition;
            set => condition = value;
        }

        public decimal Price
        {
            get => price;
            set => price = value;
        }

        public string ImageUrl
        {
            get => imageUrl;
            set => imageUrl = value;
        }

        public bool Availability
        {
            get => availability;
            set => availability = value;
        }
    }
}