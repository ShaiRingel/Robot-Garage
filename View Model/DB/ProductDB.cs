using Model;
using System.Data.OleDb;
using System.Diagnostics;

namespace View_Model.DB
{
    public class ProductDB : BaseEntityDB
    {

        public ProductDB() : base()
        {
            connection = new OleDbConnection(_connectionString);
            cmd = new OleDbCommand();
            cmd.Connection = connection;
        }

        public int Insert(Product product)
        {
            int records = 0;
            cmd.Parameters.Clear();

            cmd.CommandText = "INSERT INTO ProductTbl ([owner_id], [product_name], [description], [date_posted], [condition], [category], [price], [image], [availability]) " +
                              "VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?)";

            cmd.Parameters.Add("?", OleDbType.Integer).Value = product.Owner.ID;
            cmd.Parameters.Add("?", OleDbType.VarWChar).Value = product.Name;
            cmd.Parameters.Add("?", OleDbType.LongVarWChar).Value = product.Description;
            cmd.Parameters.Add("?", OleDbType.Date).Value = product.DatePosted;
            cmd.Parameters.Add("?", OleDbType.Integer).Value = (int)product.Condition;
            cmd.Parameters.Add("?", OleDbType.Integer).Value = (int)product.Category;
            cmd.Parameters.Add("?", OleDbType.Double).Value = product.Price;

            if (product.Image != null && product.Image.Length > 0)
            {
                cmd.Parameters.Add("?", OleDbType.LongVarBinary).Value = product.Image;
            }
            else
            {
                cmd.Parameters.Add("?", OleDbType.LongVarBinary).Value = DBNull.Value;
            }

            cmd.Parameters.Add("?", OleDbType.Boolean).Value = product.Availability;

            try
            {
                connection.Open();
                records = cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error occurred during INSERT operation: " + e.Message);
                Debug.WriteLine("SQL: " + cmd.CommandText);
                foreach (OleDbParameter param in cmd.Parameters)
                {
                    Debug.WriteLine($"{param.Value} ({param.OleDbType})");
                }
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                    connection.Close();
            }

            return records;
        }


        public List<Product> GetAllProducts()
        {
            cmd.CommandText = "SELECT * FROM ProductTbl";

            List<Product> productList = SelectProducts();
            return productList;
        }

        public List<Product> GetAllAvailableProducts()
        {
            cmd.CommandText = $"SELECT * FROM ProductTbl WHERE [availability]={true}";

            List<Product> productList = SelectProducts();
            return productList;
        }

        public List<Product> GetAllRequestedAvailableProducts()
        {
            cmd.CommandText = $"SELECT * FROM ProductTbl WHERE [availability]={true} AND [requesting]={true}";

            List<Product> productList = SelectProducts();
            return productList;
        }

        public List<Product> GetNLatestRequestedAvailableProducts(int n)
        {
            cmd.CommandText = $"SELECT TOP {n} * FROM ProductTbl WHERE [availability]={true} AND [requesting]={true}";

            List<Product> productList = SelectProducts();
            return productList;
        }

        public List<Product> GetAllRequestedAvailableProductsByCategory(ItemCategory category)
        {
            cmd.CommandText = $"SELECT * FROM ProductTbl " +
                $"WHERE [availability]={true} AND [category]={(int)category} AND [requesting]={true}";
            List<Product> productList = SelectProducts();
            return productList;
        }


        public List<Product> GetNLatestRequestedAvailableProductsByCategory(int n, ItemCategory category)
        {
            cmd.CommandText = $"SELECT TOP {n} * FROM ProductTbl " +
                $"WHERE [availability]={true} AND [category]={(int)category} AND [requesting]={true}";
            List<Product> productList = SelectProducts();
            return productList;
        }

        public Product GetProductByID(int id)
        {
            cmd.Parameters.Clear();

            cmd.CommandText = "SELECT * FROM ProductTbl WHERE [product_id]=@ID";

            cmd.Parameters.AddWithValue("@ID", id);

            List<Product> productList = SelectProducts();
            return productList.FirstOrDefault();
        }


        public List<Product> GetAllAvailableProductsByCategory(ItemCategory category)
        {
            cmd.CommandText = $"SELECT * FROM ProductTbl " +
                $"WHERE [availability]={true} AND [category]={(int)category} AND [requesting]={false}";

            List<Product> productList = SelectProducts();
            return productList;
        }

        public List<Product> GetAllAvailableProductsByCondition(ItemCondition condition)
        {
            cmd.CommandText = $"SELECT * FROM ProductTbl " +
                $"WHERE [availability]={true} AND [condition]={(int)condition} AND [requesting]={false}";

            List<Product> productList = SelectProducts();
            return productList;
        }

        public List<Product> GetNLatestAvailableProducts(int n)
        {
            cmd.CommandText = $"SELECT TOP {n} * FROM ProductTbl " +
                              $"WHERE [availability]={true} AND [requesting]={false} " +
                              $"ORDER BY [date_posted] DESC";

            List<Product> productList = SelectProducts();
            return productList;
        }

        public List<Product> GetNLatestAvailableProductsByCategory(int n, ItemCategory category)
        {
            cmd.CommandText = $"SELECT TOP {n} * FROM ProductTbl " +
                $"WHERE [availability]={true} AND [category]={(int)category}";

            List<Product> productList = SelectProducts();
            return productList;
        }

        public List<Product> GetNLatestAvailableProductsByCondition(int n, ItemCondition condition)
        {
            cmd.CommandText = $"SELECT TOP {n} * FROM ProductTbl " +
                $"WHERE [availability]={true} AND [condition]={(int)condition}";

            List<Product> productList = SelectProducts();
            return productList;
        }

        public int Update(Product product)
        {
            int records = 0;
            cmd.Parameters.Clear();

            string sqlStr = "UPDATE ProductTbl SET " +
                "[owner_id] = ?, " +
                "[product_name] = ?, " +
                "[description] = ?, " +
                "[date_posted] = ?, " +
                "[condition] = ?, " +
                "[category] = ?, " +
                "[price] = ?, " +
                "[image2] = ?, " +
                "[availability] = ? " +
                "WHERE [product_id] = ?";

            cmd.Parameters.Add("?", OleDbType.Integer).Value = product.Owner.ID;
            cmd.Parameters.Add("?", OleDbType.VarWChar).Value = product.Name;
            cmd.Parameters.Add("?", OleDbType.LongVarWChar).Value = product.Description;
            cmd.Parameters.Add("?", OleDbType.Date).Value = product.DatePosted;
            cmd.Parameters.Add("?", OleDbType.Integer).Value = (int)product.Condition;
            cmd.Parameters.Add("?", OleDbType.Integer).Value = (int)product.Category;
            cmd.Parameters.Add("?", OleDbType.Double).Value = product.Price;

            if (product.Image != null && product.Image.Length > 0)
            {
                cmd.Parameters.Add("?", OleDbType.LongVarBinary).Value = product.Image;
            }
            else
            {
                cmd.Parameters.Add("?", OleDbType.LongVarBinary).Value = DBNull.Value;
            }

            cmd.Parameters.Add("?", OleDbType.Boolean).Value = product.Availability;

            cmd.Parameters.Add("?", OleDbType.Integer).Value = product.ID;

            try
            {
                cmd.CommandText = sqlStr;
                connection.Open();
                records = cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error occurred during UPDATE operation: " + e.Message);
                Debug.WriteLine("SQL: " + cmd.CommandText);
                foreach (OleDbParameter param in cmd.Parameters)
                {
                    Debug.WriteLine($"{param.Value} ({param.OleDbType})");
                }
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                    connection.Close();
            }

            return records;
        }

        public int UpdateAvailabilityByID(int id, bool availability)
        {
            int records = 0;
            cmd.Parameters.Clear();

            string sqlStr = $"UPDATE ProductTbl SET [availability] = {availability} " +
                            $"WHERE [product_id] = {id}";

            try
            {
                cmd.CommandText = sqlStr;
                connection.Open();
                records = cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error occurred during UPDATE operation: " + e.Message);
                Debug.WriteLine("SQL: " + cmd.CommandText);
                foreach (OleDbParameter param in cmd.Parameters)
                {
                    Debug.WriteLine($"{param.Value} ({param.OleDbType})");
                }
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                    connection.Close();
            }

            return records;
        }


        public int Delete(Product product)
        {
            cmd.Parameters.Clear();
            cmd.CommandText = "DELETE FROM ProductTbl WHERE [product_id]=@ID";
            cmd.Parameters.AddWithValue("@ID", product.ID);

            try
            {
                connection.Open();
                int records = cmd.ExecuteNonQuery();
                return records;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error in DELETE operation: " + e.Message);
                return 0;
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                    connection.Close();
            }
        }

        protected override BaseEntity newEntity()
        {
            return new Product();
        }

        protected override BaseEntity CreateModel(BaseEntity entity)
        {
            Product product = (Product)entity;

            product.ID = (int)reader["product_id"];
            product.Owner = new UserDB().GetUserByID((int)reader["owner_id"]);
            product.Name = reader["product_name"].ToString();
            product.Description = reader["description"].ToString();
            product.DatePosted = (DateTime)reader["date_posted"];
            product.Condition = (ItemCondition)reader["condition"];
            product.Category = (ItemCategory)reader["category"];
            product.Price = double.Parse(reader["price"].ToString());
            product.Availability = (bool)reader["availability"];
            product.Image = reader["image"] as byte[];

            return product;
        }

        private List<Product> SelectProducts()
        {
            List<Product> productList = new List<Product>();
            try
            {
                cmd.Connection = connection;
                connection.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Product product = (Product)newEntity();
                    productList.Add((Product)CreateModel(product));
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                if (connection.State == System.Data.ConnectionState.Open)
                    connection.Close();
            }
            return productList;
        }
    }
}
