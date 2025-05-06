namespace Model
{
    public class Message : BaseEntity
    {
        private Product product;
        private User sender;
        private User receiver;
        private string content;
        private DateTime timestamp;

        public Product Product
        {
            get => product;
            set => product = value;
        }
        public User Sender
        {
            get => sender;
            set => sender = value;
        }
        public User Receiver
        {
            get => receiver;
            set => receiver = value;
        }
        public string Content
        {
            get => content;
            set => content = value;
        }
        public DateTime Timestamp
        {
            get => timestamp;
            set => timestamp = value;
        }

        public override bool Equals(object obj)
        {
            if (obj is Message otherMessage)
            {
                return this.ID == otherMessage.ID;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }
    }
}