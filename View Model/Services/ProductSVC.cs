using Model;
using View_Model.DB;

namespace View_Model.Services
{
    public class ProductSVC
    {
        ProductDB productDB = new ProductDB();

        public List<Product> GetAll()
        {
            return productDB.SelectAll();
        }

        public Product GetByID(int id)
        {
            return productDB.SelectByID(id);
        }

        public void InsertProduct(Product product)
        {
            productDB.Insert(product);
        }

        public void UpdateProduct(Product product)
        {
            productDB.Update(product);
        }

        public void DeleteProduct(Product product)
        {
            productDB.Delete(product);
        }
    }
}
