using System.Data.Common;
using Model;
using View_Model.DB;

namespace View_Model.Services
{
    public class TransactionSVC
    {
        TransactionDB transactionDB = new TransactionDB();

        public TransactionList SelectAll()
        {
            return transactionDB.SelectAll();
        }

        public Transaction SelectByID(int id)
        {
            return transactionDB.SelectByID(id);
        }

        public void Insert(Transaction transaction)
        {
            transactionDB.Insert(transaction);
            transactionDB.SaveChanges();
        }

        public void Update(Transaction transaction)
        {
            transactionDB.Update(transaction);
            transactionDB.SaveChanges();
        }

        public void Delete(Transaction transaction)
        {
            transactionDB.Delete(transaction);
            transactionDB.SaveChanges();
        }
    }
}
