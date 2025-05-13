using CoreWCF;

namespace WCFServer
{
    [ServiceContract]
    public interface IGarageService : IUserService, IProductService, IMessageService, ITransacionService
    {

    }
}
