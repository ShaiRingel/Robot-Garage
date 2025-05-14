using CoreWCF;

namespace WCFServer
{
    [ServiceContract]
    public interface IGarageService : IUserService, IPaymentService, IProductService, IMessageService, ITransacionService
    {

    }
}
