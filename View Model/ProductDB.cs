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

namespace View_Model
{
    public class ProductDB : BaseEntityDB
    {
        private const string ColumnList = @"
            product_id,
            owner_id,
            product_name,
            description,
            date_posted,
            [condition],
            [category],
            price,
            availability,
            [image].FileData AS ImageData";

        public ProductDB() : base()
        {
            this.connection = new OleDbConnection(_connectionString);
            this.cmd = new OleDbCommand();
            this.cmd.Connection = connection;
        }

        public int Insert(Product product)
        {
            int records = 0;
            int newId = -1;

            // 1) Insert all the non-image fields
            cmd.Parameters.Clear();
            cmd.CommandText = @"
        INSERT INTO ProductTbl
            ([owner_id], [product_name], [description], [date_posted],
             [condition], [category], [price], [availability])
        VALUES
            (@Owner, @ProductName, @Description, @DatePosted,
             @Condition, @Category, @Price, @Availability)";
            cmd.Parameters.AddWithValue("@Owner", product.Owner.ID);
            cmd.Parameters.AddWithValue("@ProductName", product.Name);
            cmd.Parameters.AddWithValue("@Description", product.Description);
            cmd.Parameters.AddWithValue("@DatePosted", product.DatePosted);
            cmd.Parameters.AddWithValue("@Condition", (int)product.Condition);
            cmd.Parameters.AddWithValue("@Category", (int)product.Category);
            cmd.Parameters.AddWithValue("@Price", product.Price);
            cmd.Parameters.AddWithValue("@Availability", product.Availability);

            try
            {
                connection.Open();
                records = cmd.ExecuteNonQuery();

                // 2) Grab the auto-generated key
                cmd.CommandText = "SELECT @@IDENTITY";
                object idObj = cmd.ExecuteScalar();
                if (idObj != null && int.TryParse(idObj.ToString(), out int id))
                    newId = id;

                // 3) If there's image data, use DAO to append it to the Attachment field
                if (newId > 0 && product.Image != null)
                {
                    // --- get the .accdb path from your connection string ---
                    var builder = new OleDbConnectionStringBuilder(_connectionString);
                    string dbPath = builder.DataSource;

                    // --- open with DAO ---
                    var engine = new DBEngine();
                    Database daoDb = engine.OpenDatabase(dbPath, false, false, null);

                    // --- open the single record we just inserted ---
                    var rs = daoDb.OpenRecordset(
                        $"SELECT * FROM ProductTbl WHERE product_id={newId}",
                        RecordsetTypeEnum.dbOpenDynaset,
                        LockTypeEnum.dbPessimistic);

                    rs.MoveFirst();

                    // --- get the attachment‐field recordset ---
                    var attachFld = (Field2)rs.Fields["image"];
                    var rsAttach = (Recordset2)attachFld.Value;

                    // --- write your byte[] out to a temp file ---
                    string tempFile = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.img");
                    File.WriteAllBytes(tempFile, product.Image);

                    // --- add a new attachment row ----
                    rsAttach.AddNew();
                    rsAttach.Fields["FileData"].LoadFromFile(tempFile);
                    rsAttach.Fields["FileName"].Value = Path.GetFileName(tempFile);
                    rsAttach.Update();

                    // --- commit the parent record & cleanup ---
                    rs.Update();
                    rsAttach.Close();
                    rs.Close();
                    daoDb.Close();
                    File.Delete(tempFile);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error during INSERT+attachment: " + ex.Message);
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
            cmd.CommandText = $"SELECT {ColumnList} FROM ProductTbl";
            return SelectProducts();
        }

        public List<Product> GetAllAvailableProducts()
        {
            cmd.CommandText = $"SELECT {ColumnList} FROM ProductTbl WHERE [availability]=True";
            return SelectProducts();
        }

        public List<Product> GetAllAvailableProductsByCategory(ItemCategory category)
        {
            cmd.CommandText = $"SELECT {ColumnList} FROM ProductTbl " +
                              $"WHERE [availability]=True AND [category]={(int)category}";
            return SelectProducts();
        }

        public List<Product> GetAllAvailableProductsByCondition(ItemCondition condition)
        {
            cmd.CommandText = $"SELECT {ColumnList} FROM ProductTbl " +
                              $"WHERE [availability]=True AND [condition]={(int)condition}";
            return SelectProducts();
        }

        public List<Product> GetNLatestAvailableProducts(int n)
        {
            cmd.CommandText = $"SELECT TOP {n} {ColumnList} FROM ProductTbl " +
                              $"WHERE [availability]=True " +
                              $"ORDER BY [date_posted] DESC";
            return SelectProducts();
        }

        public List<Product> GetNLatestAvailableProductsByCategory(int n, ItemCategory category)
        {
            cmd.CommandText = $"SELECT TOP {n} {ColumnList} FROM ProductTbl " +
                              $"WHERE [availability]=True AND [category]={(int)category} " +
                              $"ORDER BY [date_posted] DESC";
            return SelectProducts();
        }

        public List<Product> GetNLatestAvailableProductsByCondition(int n, ItemCondition condition)
        {
            cmd.CommandText = $"SELECT TOP {n} {ColumnList} FROM ProductTbl " +
                              $"WHERE [availability]=True AND [condition]={(int)condition} " +
                              $"ORDER BY [date_posted] DESC";
            return SelectProducts();
        }

        public int Update(Product product)
        {
            int records = 0;
            cmd.Parameters.Clear();

            cmd.CommandText = @"
                UPDATE ProductTbl SET
                    [owner_id]=@Owner,
                    [product_name]=@ProductName,
                    [description]=@Description,
                    [date_posted]=@DatePosted,
                    [condition]=@Condition,
                    [category]=@Category,
                    [price]=@Price,
                    [image]=@Image,
                    [availability]=@Availability
                WHERE [product_id]=@ID";

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

            try
            {
                connection.Open();
                records = cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error during UPDATE: " + e.Message);
                Debug.WriteLine("SQL: " + cmd.CommandText);
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
            var sql = $"DELETE FROM ProductTbl WHERE [product_id]={product.ID}";
            return SaveChanges(sql);
        }

        protected override BaseEntity newEntity()
        {
            return new Product();
        }

        protected override BaseEntity CreateModel(BaseEntity entity)
        {
            var product = (Product)entity;
            product.ID = (int)reader["product_id"];
            product.Owner = new UserDB().GetByID((int)reader["owner_id"]);
            product.Name = reader["product_name"].ToString();
            product.Description = reader["description"].ToString();
            product.DatePosted = (DateTime)reader["date_posted"];
            product.Condition = (ItemCondition)reader["condition"];
            product.Category = (ItemCategory)reader["category"];
            product.Price = double.Parse(reader["price"].ToString());
            product.Availability = (bool)reader["availability"];

            if (reader["ImageData"] != DBNull.Value)
            {
                var rawBytes = (byte[])reader["ImageData"];
                var imageBytes = StripOleHeader(rawBytes);
                product.Image = imageBytes;
            }
            else
            {
                product.Image = null;
            }

            return product;
        }

        private byte[] StripOleHeader(byte[] oleBytes)
        {
            // full signatures
            var sigs = new List<byte[]>
            {
                new byte[]{ 0xFF,0xD8,0xFF },                           // JPEG start
                new byte[]{ 0x89,0x50,0x4E,0x47,0x0D,0x0A,0x1A,0x0A },   // PNG
                new byte[]{ 0x47,0x49,0x46,0x38,0x37,0x61 },             // GIF87a
                new byte[]{ 0x47,0x49,0x46,0x38,0x39,0x61 },             // GIF89a
                new byte[]{ 0x42,0x4D }                                  // BMP
            };

            foreach (var sig in sigs)
            {
                for (int i = 0; i + sig.Length < oleBytes.Length; i++)
                {
                    bool match = true;
                    for (int j = 0; j < sig.Length; j++)
                    {
                        if (oleBytes[i + j] != sig[j])
                        {
                            match = false;
                            break;
                        }
                    }
                    if (match)
                    {
                        var clean = new byte[oleBytes.Length - i];
                        Array.Copy(oleBytes, i, clean, 0, clean.Length);
                        return clean;
                    }
                }
            }

            return oleBytes;
        }

        private BitmapImage BytesToBitmapImage(byte[] bytes)
        {
            using (var ms = new MemoryStream(bytes))
            {
                var img = new BitmapImage();
                img.BeginInit();
                img.CacheOption = BitmapCacheOption.OnLoad;
                img.StreamSource = ms;
                img.EndInit();
                img.Freeze();
                return img;
            }
        }



        private List<Product> SelectProducts()
        {
            var productList = new List<Product>();
            try
            {
                cmd.Connection = connection;
                connection.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    productList.Add((Product)CreateModel(newEntity()));
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("SelectProducts error: " + e.Message);
            }
            finally
            {
                reader?.Close();
                if (connection.State == System.Data.ConnectionState.Open)
                    connection.Close();
            }
            return productList;
        }
    }
}
