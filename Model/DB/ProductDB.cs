using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DB
{
    public class ProductDB
    {
        private readonly string _connectionString;

        public ProductDB(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Insert(Product product)
        {
            using (OleDbConnection conn = new OleDbConnection(_connectionString))
            {
                conn.Open();
                string sql = "INSERT INTO Product ([Vendor], [Renter], [Name], [Description], [Condition], [Price], [ImageUrl], [Availability]) " +
                             "VALUES (@Vendor, @Renter, @Name, @Description, @Condition, @Price, @ImageUrl, @Availability)";
                using (OleDbCommand cmd = new OleDbCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Vendor", product.Vendor);
                    cmd.Parameters.AddWithValue("@Renter", product.Renter);
                    cmd.Parameters.AddWithValue("@Name", product.Name);
                    cmd.Parameters.AddWithValue("@Description", product.Description);
                    // Save the enum as an integer.
                    cmd.Parameters.AddWithValue("@Condition", (int)product.Condition);
                    cmd.Parameters.AddWithValue("@Price", product.Price);
                    cmd.Parameters.AddWithValue("@ImageUrl", product.ImageUrl);
                    cmd.Parameters.AddWithValue("@Availability", product.Availability);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Product> GetAll()
        {
            List<Product> products = new List<Product>();
            using (OleDbConnection conn = new OleDbConnection(_connectionString))
            {
                conn.Open();
                string sql = "SELECT [ID], [Vendor], [Renter], [Name], [Description], [Condition], [Price], [ImageUrl], [Availability] FROM Product";
                using (OleDbCommand cmd = new OleDbCommand(sql, conn))
                {
                    using (OleDbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            products.Add(new Product
                            {
                                ID = reader.GetInt32(0),
                                Vendor = new Vendor(reader.GetInt32(1)),
                                Renter = new Renter(reader.GetInt32(2)),
                                Name = reader.GetString(3),
                                Description = reader.GetString(4),
                                // Convert stored integer back to the enum.
                                Condition = (ItemCondition)reader.GetInt32(5),
                                Price = reader.GetDecimal(6),
                                ImageUrl = reader.GetString(7),
                                Availability = reader.GetBoolean(8)
                            });
                        }
                    }
                }
            }
            return products;
        }
    }
}
