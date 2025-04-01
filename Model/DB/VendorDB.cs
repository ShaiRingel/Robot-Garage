using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DB
{
    public class VendorDB
    {
        private readonly string _connectionString;

        public VendorDB(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Insert(Vendor vendor)
        {
            using (OleDbConnection conn = new OleDbConnection(_connectionString))
            {
                conn.Open();
                string sql = "INSERT INTO Vendor ([ID]) VALUES (@ID)";
                using (OleDbCommand cmd = new OleDbCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ID", vendor.ID);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Vendor> GetAll()
        {
            List<Vendor> vendors = new List<Vendor>();
            using (OleDbConnection conn = new OleDbConnection(_connectionString))
            {
                conn.Open();
                string sql = "SELECT [ID] FROM Vendor";
                using (OleDbCommand cmd = new OleDbCommand(sql, conn))
                {
                    using (OleDbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            vendors.Add(new Vendor
                            {
                                ID = reader.GetInt32(0)
                            });
                        }
                    }
                }
            }
            return vendors;
        }
    }
}
