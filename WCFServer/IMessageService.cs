using CoreWCF;
using Model;

namespace WCFServer
{
    [ServiceContract]
    public interface IMessageService
    {
        [OperationContract]
        MessageList SelectAllMessages();

        [OperationContract]
        Message SelectMessageByID(int id);

        /*[OperationContract]
		void InsertProduct(Product item);

		[OperationContract]
		void UpdateProduct(Product item);

		[OperationContract]
		void DeleteProduct(Product item);*/
    }
}