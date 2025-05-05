using Model;
using View_Model.Services;

namespace WCFServer {
	public class GarageService : IProductService {
		ProductSVC productSVC = new ProductSVC();
		public List<Product> GetAllProducts() {
			return productSVC.GetAll();
		}

		public Product GetProductByID(int id) {
			return productSVC.GetByID(id);
		}

		public void InsertProduct(Product item) {
			productSVC.InsertProduct(item);
		}

		public void UpdateProduct(Product item) {
			productSVC.UpdateProduct(item);
		}

		public void DeleteProduct(Product item) {
			productSVC.DeleteProduct(item);
		}
	}
}

