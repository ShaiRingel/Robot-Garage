using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DB
{
    public class MessageDB
    {
        private readonly string _connectionString;

        public MessageDB(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Insert(Message message)
        {
            using (OleDbConnection conn = new OleDbConnection(_connectionString))
            {
                conn.Open();
                string sql = "INSERT INTO Message ([Product], [Message], [Timestamp]) VALUES (@Product, @Message, @Timestamp)";
                using (OleDbCommand cmd = new OleDbCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Product", message.Product);
                    cmd.Parameters.AddWithValue("@Message", message.Content);
                    cmd.Parameters.AddWithValue("@Timestamp", message.Timestamp);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Message> GetAll()
        {
            List<Message> messages = new List<Message>();
            using (OleDbConnection conn = new OleDbConnection(_connectionString))
            {
                conn.Open();
                string sql = "SELECT [ID], [Product], [Message], [Timestamp] FROM Message";
                using (OleDbCommand cmd = new OleDbCommand(sql, conn))
                {
                    using (OleDbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            messages.Add(new Message
                            {
                                ID = reader.GetInt32(0),
                                Product = reader.GetInt32(1),
                                Content = reader.GetString(2),
                                Timestamp = reader.GetDateTime(3)
                            });
                        }
                    }
                }
            }
            return messages;
        }
    }
}
