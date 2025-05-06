using CoreWCF;
using Model;

namespace WCFServer
{
    [ServiceContract]
    public interface IMessageService
    {
        [OperationContract]
        List<Message> GetAllMessages();

        [OperationContract]
        Message GetMessageByID(int id);

        /*[OperationContract]
		void InsertProduct(Product item);

		[OperationContract]
		void UpdateProduct(Product item);

		[OperationContract]
		void DeleteProduct(Product item);*/
    }
}