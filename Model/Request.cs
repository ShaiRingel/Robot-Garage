using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Request
    {
        private int id;
        private Product product;
        private double desiredPrice;
        private string requestStatus;

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

        public decimal DesiredPrice
        {
            get => desiredPrice;
            set => desiredPrice = value;
        }

        public string RequestStatus
        {
            get => requestStatus;
            set => requestStatus = value;
        }
    }
}
