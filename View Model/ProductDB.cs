using Model;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace View_Model {
	public class ProductDB : BaseEntityDB {
		public ProductDB() : base() {
			this.connection = new OleDbConnection(_connectionString);
			this.cmd = new OleDbCommand();
			this.cmd.Connection = connection;
		}

		public int Insert(Product product) {
			int records = 0;

			cmd.Parameters.Clear();

			cmd.CommandText = "INSERT INTO Product ([vendor_id], [product_name], [description], [condition], [price], [image_url], [availability]) " +
								"VALUES (@Vendor, @ProductName, @Description, @Condition, @Price, @ImageUrl, @Availability)";

			cmd.Parameters.AddWithValue("@Vendor", product.Vendor.ID);
			cmd.Parameters.AddWithValue("@ProductName", product.Name);
			cmd.Parameters.AddWithValue("@Description", product.Description);
			cmd.Parameters.AddWithValue("@Condition", (int)product.Condition);
			cmd.Parameters.AddWithValue("@Price", product.Price);
			cmd.Parameters.AddWithValue("@ImageUrl", product.ImageUrl);
			cmd.Parameters.AddWithValue("@Availability", product.Availability);

			try {
				connection.Open();
				records = cmd.ExecuteNonQuery();
			}
			catch (Exception e) {
				System.Diagnostics.Debug.WriteLine("Error occurred during INSERT operation: " + e.Message);
				System.Diagnostics.Debug.WriteLine("SQL: " + cmd.CommandText);
				foreach (OleDbParameter param in cmd.Parameters) {
					System.Diagnostics.Debug.WriteLine($"{param.ParameterName}: {param.Value}");
				}
			}
			finally {
				if (connection.State == System.Data.ConnectionState.Open) {
					connection.Close();
				}
			}

			return records;
		}

		public List<Product> GetAll() {
			cmd.CommandText = "SELECT * FROM Product";

			List<Product> productList = SelectProducts();
			return productList;
		}

		public int Update(Product product) {
			int records = 0;
			string sqlStr = $"UPDATE Product SET [vendor_id]={product.Vendor.ID}, [product_name]={product.Name}, " +
				$"[description]='{product.Description}', [condition]={(int)product.Condition}, [price]={product.Price}, " +
				$"[image_url]='{product.ImageUrl}', [availability]={product.Availability} WHERE [product_id]={product.ID}";
			try {
				cmd.CommandText = sqlStr;
				connection.Open();
				records = cmd.ExecuteNonQuery();
			}
			catch (Exception e) {
				System.Diagnostics.Debug.WriteLine(e.Message + "\nSQL:" + cmd.CommandText);
			}
			finally {
				if (connection.State == System.Data.ConnectionState.Open)
					connection.Close();
			}
			return records;
		}

		public int Delete(Product product) {
			StringBuilder sql_builder = new StringBuilder();
			sql_builder.AppendFormat($"DELETE FROM Product WHERE [product_id]={product.ID}");
			return SaveChanges(sql_builder.ToString());
		}

		protected override BaseEntity newEntity() {
			return new Product();
		}

		protected override BaseEntity CreateModel(BaseEntity entity) {
			Product product = (Product)entity;
			product.ID = (int)reader["product_id"];
			product.Vendor = new Vendor { ID = (int)reader["vendor_id"] };
			product.Name = reader["product_name"].ToString();
			product.Description = reader["description"].ToString();
			product.Condition = (ItemCondition)reader["condition"];
			product.Price = (decimal)reader["price"];
			product.ImageUrl = reader["image_url"].ToString();
			product.Availability = (bool)reader["availability"];
			return product;
		}

		private List<Product> SelectProducts() {
			List<Product> productList = new List<Product>();
			try {
				cmd.Connection = connection;
				connection.Open();
				reader = cmd.ExecuteReader();
				while (reader.Read()) {
					Product product = (Product)newEntity();
					productList.Add((Product)CreateModel(product));
				}
			}
			catch (Exception e) {
				System.Diagnostics.Debug.WriteLine(e.Message);
			}
			finally {
				if (reader != null)
					reader.Close();

				if (connection.State == System.Data.ConnectionState.Open)
					connection.Close();
			}
			return productList;
		}
	}
}