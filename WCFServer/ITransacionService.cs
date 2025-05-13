using CoreWCF;
using Model;

namespace WCFServer
{
    [ServiceContract]
    public interface ITransacionService
    {
        [OperationContract]
        TransactionList SelectAllTransactions();

        [OperationContract]
        Transaction SelectTransactionByID(int id);

        /*[OperationContract]
		void InsertTransactions(Transactions item);

		[OperationContract]
		void UpdateTransactions(Transactions item);

		[OperationContract]
		void DeleteTransactions(Transactions item);*/
    }
}
