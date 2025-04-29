using Model;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using Microsoft.Office.Interop.Access.Dao;
using System.Diagnostics;
using System.IO;
using System.Windows.Media.Imaging;
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

			cmd.CommandText = "INSERT INTO ProductTbl ([owner_id], [product_name], [description], [date_posted], [condition], [category], [price], [image], [availability]) " +
								"VALUES (@Owner, @ProductName, @Description, @DatePosted, @Condition, @Category, @Price, @Image, @Availability)";

			cmd.Parameters.AddWithValue("@Owner", product.Owner.ID);
			cmd.Parameters.AddWithValue("@ProductName", product.Name);
			cmd.Parameters.AddWithValue("@Description", product.Description);
			cmd.Parameters.AddWithValue("@DatePosted", product.DatePosted);
			cmd.Parameters.AddWithValue("@Condition", (int)product.Condition);
			cmd.Parameters.AddWithValue("@Category", (int)product.Category);
			cmd.Parameters.AddWithValue("@Price", product.Price);
			cmd.Parameters.AddWithValue("@Image", product.Image); // image can be null
			cmd.Parameters.AddWithValue("@Availability", product.Availability);

			try {
				connection.Open();
				records = cmd.ExecuteNonQuery();
			}
			catch (Exception e) {
				Debug.WriteLine("Error occurred during INSERT operation: " + e.Message);
				Debug.WriteLine("SQL: " + cmd.CommandText);
				foreach (OleDbParameter param in cmd.Parameters) {
					Debug.WriteLine($"{param.ParameterName}: {param.Value}");
				}
			}
			finally {
				if (connection.State == System.Data.ConnectionState.Open) {
					connection.Close();
				}
			}

			return records;
		}

		public List<Product> GetAllProducts() {
			cmd.CommandText = "SELECT * FROM ProductTbl";

			List<Product> productList = SelectProducts();
			return productList;
		}

		public List<Product> GetAllAvailableProducts() {
			cmd.CommandText = $"SELECT * FROM ProductTbl WHERE [availability]={true}";

			List<Product> productList = SelectProducts();
			return productList;
		}

		public List<Product> GetAllAvailableProductsByCategory(ItemCategory category) {
			cmd.CommandText = $"SELECT * FROM ProductTbl " +
				$"WHERE [availability]={true} AND [category]={(int)category}";

			List<Product> productList = SelectProducts();
			return productList;
		}

		public List<Product> GetAllAvailableProductsByCondition(ItemCondition condition) {
			cmd.CommandText = $"SELECT * FROM ProductTbl " +
				$"WHERE [availability]={true} AND [condition]={(int)condition}";

			List<Product> productList = SelectProducts();
			return productList;
		}

		public List<Product> GetNLatestAvailableProducts(int n) {
			cmd.CommandText = $"SELECT TOP {n} * FROM ProductTbl " +
							  $"WHERE [availability]={true} " +
							  $"ORDER BY [date_posted] DESC";

			List<Product> productList = SelectProducts();
			return productList;
		}

		public List<Product> GetNLatestAvailableProductsByCategory(int n, ItemCategory category) {
			cmd.CommandText = $"SELECT TOP {n} * FROM ProductTbl " +
				$"WHERE [availability]={true} AND [category]={(int) category}";

			List<Product> productList = SelectProducts();
			return productList;
		}

		public List<Product> GetNLatestAvailableProductsByCondition(int n, ItemCondition condition) {
			cmd.CommandText = $"SELECT TOP {n} * FROM ProductTbl " +
				$"WHERE [availability]={true} AND [condition]={(int)condition}";

			List<Product> productList = SelectProducts();
			return productList;
		}

		public int Update(Product product) {
			int records = 0;

			cmd.Parameters.Clear();

			string sqlStr = $"UPDATE ProductTbl SET [owner_id]=@Owner, [product_name]=@ProductName, " +
				$"[description]=@Description, [date_posted]=@DatePosted, [condition]=@Condition," +
				$"[category]=@Category, [price]=@Price, [image]=@Image, " +
				$"[availability]=@Availability WHERE [product_id]=@ID";

			cmd.Parameters.AddWithValue("@Owner", product.Owner.ID);
			cmd.Parameters.AddWithValue("@ProductName", product.Name);
			cmd.Parameters.AddWithValue("@Description", product.Description);
			cmd.Parameters.AddWithValue("@DatePosted", product.DatePosted);
			cmd.Parameters.AddWithValue("@Condition", (int)product.Condition);
			cmd.Parameters.AddWithValue("@Category", (int)product.Category);
			cmd.Parameters.AddWithValue("@Price", product.Price);
			cmd.Parameters.AddWithValue("@Image", product.Image);
			cmd.Parameters.AddWithValue("@Availability", product.Availability);
			cmd.Parameters.AddWithValue("@ID", product.ID);

			try {
				cmd.CommandText = sqlStr;
				connection.Open();
				records = cmd.ExecuteNonQuery();
			}
			catch (Exception e) {
				Debug.WriteLine(e.Message + "\nSQL:" + cmd.CommandText);
			}
			finally {
				if (connection.State == System.Data.ConnectionState.Open)
					connection.Close();
			}
			return records;
		}

		public int Delete(Product product) {
			StringBuilder sql_builder = new StringBuilder();
			sql_builder.AppendFormat($"DELETE FROM ProductTbl WHERE [product_id]={product.ID}");
			return SaveChanges(sql_builder.ToString());
		}

		protected override BaseEntity newEntity() {
			return new Product();
		}

		protected override BaseEntity CreateModel(BaseEntity entity) {
			Product product = (Product)entity;
			product.ID = (int)reader["product_id"];
			product.Owner = new UserDB().GetByID((int)reader["owner_id"]);
			product.Name = reader["product_name"].ToString();
			product.Description = reader["description"].ToString();
			product.DatePosted = (DateTime)reader["date_posted"];
			product.Condition = (ItemCondition)reader["condition"];
			product.Category = (ItemCategory)reader["category"];
			product.Price = double.Parse(reader["price"].ToString());
			product.Image = GetAttachmentImage(product.ID, "image");
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
				Debug.WriteLine(e.Message);
			}
			finally {
				if (reader != null)
					reader.Close();

				if (connection.State == System.Data.ConnectionState.Open)
					connection.Close();
			}
			return productList;
		}

        private BitmapImage GetAttachmentImage(int productId, string attachmentField)
        {
            string dbPath = _connectionString.Replace("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=", "").TrimEnd(';');
            DBEngine dbEngine = new DBEngine();
            Database db = dbEngine.OpenDatabase(dbPath);

            // Open the recordset containing the product data
            Recordset rs = db.OpenRecordset($"SELECT * FROM ProductTbl WHERE product_id = {productId}",
                RecordsetTypeEnum.dbOpenSnapshot);

            BitmapImage bitmap = null;

            if (!rs.EOF)
            {
                // Fetch the binary image data directly from the attachment field
                object attachmentData = rs.Fields[attachmentField].Value;

                // If attachment data exists and is of type byte[], we proceed
                if (attachmentData != null && attachmentData is byte[])
                {
                    byte[] fileData = (byte[])attachmentData;

                    // Debug log: Check the size of the file data
                    Debug.WriteLine($"File data size: {fileData.Length} bytes");

                    // Check if the data size is valid
                    if (fileData.Length > 0)
                    {
                        // Use MemoryStream to load the byte array into a BitmapImage
                        using (MemoryStream stream = new MemoryStream(fileData))
                        {
                            bitmap = new BitmapImage();
                            bitmap.BeginInit();
                            bitmap.CacheOption = BitmapCacheOption.OnLoad;
                            bitmap.StreamSource = stream;
                            bitmap.EndInit();
                            bitmap.Freeze(); // Optional: makes the image cross-thread accessible
                        }
                    }
                    else
                    {
                        Debug.WriteLine("No valid image data found.");
                    }
                }
                else
                {
                    Debug.WriteLine("Attachment data is not of type byte[].");
                }
            }
            else
            {
                Debug.WriteLine("No records found for product ID.");
            }

            rs.Close();
            db.Close();

            return bitmap;
        }
    }
}
