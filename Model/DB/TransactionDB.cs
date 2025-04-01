using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DB
{
    public class TransactionDB
    {
        private readonly string _connectionString;

        public TransactionDB(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Insert(Transaction transaction)
        {
            using (OleDbConnection conn = new OleDbConnection(_connectionString))
            {
                conn.Open();
                string sql = "INSERT INTO Transaction ([Product], [StartDate], [EndDate], [Status]) VALUES (@Product, @StartDate, @EndDate, @Status)";
                using (OleDbCommand cmd = new OleDbCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Product", transaction.Product);
                    cmd.Parameters.AddWithValue("@StartDate", transaction.StartDate);
                    cmd.Parameters.AddWithValue("@EndDate", transaction.EndDate);
                    // Save the enum as an integer.
                    cmd.Parameters.AddWithValue("@Status", (int)transaction.Status);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Transaction> GetAll()
        {
            List<Transaction> transactions = new List<Transaction>();
            using (OleDbConnection conn = new OleDbConnection(_connectionString))
            {
                conn.Open();
                string sql = "SELECT [ID], [Product], [StartDate], [EndDate], [Status] FROM Transaction";
                using (OleDbCommand cmd = new OleDbCommand(sql, conn))
                {
                    using (OleDbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            transactions.Add(new Transaction
                            {
                                ID = reader.GetInt32(0),
                                Product = reader.GetInt32(1),
                                StartDate = reader.GetDateTime(2),
                                EndDate = reader.GetDateTime(3),
                                // Convert the integer back to the OrderStatus enum.
                                Status = (OrderStatus)reader.GetInt32(4)
                            });
                        }
                    }
                }
            }
            return transactions;
        }
    }
}
