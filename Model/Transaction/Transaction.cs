namespace Model
{
    public enum OrderStatus
    {
        Confirmed, // Order confirmed, preparing for shipment.
        Shipped,   // Order dispatched, in transit.
        Delivered, // Order received by customer.
    }

    public class Transaction : BaseEntity
    {
        private Product product;
        private User seller;
        private User buyer;
        private OrderStatus status;

        public Product Product
        {
            get => product;
            set => product = value;
        }

        public User Seller
        {
            get => seller;
            set => seller = value;
        }

        public User Buyer
        {
            get => buyer;
            set => buyer = value;
        }

        public OrderStatus Status
        {
            get => status;
            set => status = value;
        }
    }

}
