using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DB
{
    public class RenterDB
    {
        private readonly string _connectionString;

        public RenterDB(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Insert(Renter renter)
        {
            using (OleDbConnection conn = new OleDbConnection(_connectionString))
            {
                conn.Open();
                string sql = "INSERT INTO Renter ([ID]) VALUES (@ID)";
                using (OleDbCommand cmd = new OleDbCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ID", renter.ID);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Renter> GetAll()
        {
            List<Renter> renters = new List<Renter>();
            using (OleDbConnection conn = new OleDbConnection(_connectionString))
            {
                conn.Open();
                string sql = "SELECT [ID] FROM Renter";
                using (OleDbCommand cmd = new OleDbCommand(sql, conn))
                {
                    using (OleDbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            renters.Add(new Renter
                            {
                                ID = reader.GetInt32(0)
                            });
                        }
                    }
                }
            }
            return renters;
        }
    }
}
