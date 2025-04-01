using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Message
    {
        private int id;
        private Product product;
        private string content;
        private DateTime timestamp;

        public int ID
        {
            get => id;
            set => id = value;
        }

        public Product Product
        {
            get => product;
            set => product = value;
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
    }
}