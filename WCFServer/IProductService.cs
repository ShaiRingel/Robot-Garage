using CoreWCF;
using Model;

namespace WCFServer {
	[ServiceContract]
	public interface IProductService {
		[OperationContract]
		List<Product> GetAllProducts();

		[OperationContract]
		Product GetProductByID(int id);

		[OperationContract]
		void InsertProduct(Product item);

		[OperationContract]
		void UpdateProduct(Product item);

		[OperationContract]
		void DeleteProduct(Product item);
	}
}

/*
using CoreWCF;

namespace VWS {
	[ServiceContract]
	public interface IVisionService {
		[OperationContract]
		string Status();

		[OperationContract]
		bool Login(string Uname, string Upass, int T);
	}
}
*/