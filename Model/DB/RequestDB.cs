using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DB
{
    public class RequestDB
    {
        private readonly string _connectionString;

        public RequestDB(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Insert(Request request)
        {
            using (OleDbConnection conn = new OleDbConnection(_connectionString))
            {
                conn.Open();
                string sql = "INSERT INTO Request ([Product], [DesiredPrice], [RequestStatus]) VALUES (@Product, @DesiredPrice, @RequestStatus)";
                using (OleDbCommand cmd = new OleDbCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Product", request.Product);
                    cmd.Parameters.AddWithValue("@DesiredPrice", request.DesiredPrice);
                    cmd.Parameters.AddWithValue("@RequestStatus", request.RequestStatus);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Request> GetAll()
        {
            List<Request> requests = new List<Request>();
            using (OleDbConnection conn = new OleDbConnection(_connectionString))
            {
                conn.Open();
                string sql = "SELECT [ID], [Product], [DesiredPrice], [RequestStatus] FROM Request";
                using (OleDbCommand cmd = new OleDbCommand(sql, conn))
                {
                    using (OleDbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            requests.Add(new Request
                            {
                                ID = reader.GetInt32(0),
                                Product = reader.GetInt32(1),
                                DesiredPrice = reader.GetDecimal(2),
                                RequestStatus = reader.GetString(3)
                            });
                        }
                    }
                }
            }
            return requests;
        }
    }
}
