using Model;
using View_Model.DB;

namespace View_Model.Services
{
    public class ProductSVC
    {
        ProductDB productDB = new ProductDB();

        public ProductList SelectAll()
        {
            return productDB.SelectAll();
        }

        public Product SelectByID(int id)
        {
            return productDB.SelectByID(id);
        }

        public void Insert(Product product)
        {
            productDB.Insert(product);
			productDB.SaveChanges();
		}

		public void Update(Product product)
        {
            productDB.Update(product);
			productDB.SaveChanges();
		}

		public void Delete(Product product)
        {
            productDB.Delete(product);
			productDB.SaveChanges();
		}
	}
}
