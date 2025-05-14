using CoreWCF;
using Model;

namespace WCFServer
{
    [ServiceContract]
    public interface ITransacionService
    {
        [OperationContract]
        TransactionList SelectAllPaymentMethod();

        [OperationContract]
        Transaction SelectTransactionByID(int id);

        /*[OperationContract]
		void InsertPaymentMethod(PaymentMethod item);

		[OperationContract]
		void UpdatePaymentMethod(PaymentMethod item);

		[OperationContract]
		void DeletePaymentMethod(PaymentMethod item);*/
    }
}
