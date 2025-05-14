using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using View_Model.DB;

namespace View_Model.Services
{
    public class PaymentMethodSVC
    {
        PaymentMethodDB paymentDB = new PaymentMethodDB();

        public PaymentMethodList SelectAll()
        {
            return paymentDB.SelectAll();
        }

        public PaymentMethod SelectByID(int id)
        {
            return paymentDB.SelectByID(id);
        }

        public void Insert(PaymentMethod payment)
        {
            paymentDB.Insert(payment);
            paymentDB.SaveChanges();
		}

		public void Update(PaymentMethod payment)
        {
            paymentDB.Update(payment);
            paymentDB.SaveChanges();
		}

		public void Delete(PaymentMethod payment)
        {
            paymentDB.Delete(payment);
            paymentDB.SaveChanges();
		}
    }
}
