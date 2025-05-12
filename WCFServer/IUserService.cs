using CoreWCF;
using Model;

namespace WCFServer
{
    [ServiceContract]
    public interface IUserService
    {
        [OperationContract]
        bool Login(string username, int groupnumber, string password);
        [OperationContract]
        UserList SelectAllUsers();

        [OperationContract]
        User SelectUserByID(int id);

        /*[OperationContract]
        void InsertUser(User item);

        [OperationContract]
        void UpdateUser(User item);

        [OperationContract]
        void DeleteUser(User item);*/
    }
}