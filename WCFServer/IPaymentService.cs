using CoreWCF;
using Model;

namespace WCFServer
{
    [ServiceContract]
    public interface IPaymentService
    {
        [OperationContract]
        PaymentMethodList SelectAllPaymentMethods();

        [OperationContract]
        PaymentMethod SelectPaymentMethodByID(int id);

        /*[OperationContract]
		void InsertPaymentMethod(PaymentMethod item);

		[OperationContract]
		void UpdatePaymentMethod(PaymentMethod item);

		[OperationContract]
		void DeletePaymentMethod(PaymentMethod item);*/
    }
}
