using CoreWCF;

namespace WCFServer
{
    [ServiceContract]
    public interface IGarageService : IProductService, IUserService, IMessageService 
    {

    }
}
