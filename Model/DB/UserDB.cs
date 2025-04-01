using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DB
{
    public class UserDB
    {
        private readonly string _connectionString;

        public UserDB(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Insert(User user)
        {
            using (OleDbConnection conn = new OleDbConnection(_connectionString))
            {
                conn.Open();
                // If "User" is a reserved word, enclose it in square brackets.
                string sql = "INSERT INTO [User] ([Username], [Password], [GroupNumber], [UniqueCode]) VALUES (@Username, @Password, @GroupNumber, @UniqueCode)";
                using (OleDbCommand cmd = new OleDbCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", user.Username);
                    cmd.Parameters.AddWithValue("@Password", user.Password);
                    cmd.Parameters.AddWithValue("@GroupNumber", user.GroupNumber);
                    cmd.Parameters.AddWithValue("@UniqueCode", user.UniqueCode);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<User> GetAll()
        {
            List<User> users = new List<User>();
            using (OleDbConnection conn = new OleDbConnection(_connectionString))
            {
                conn.Open();
                string sql = "SELECT [ID], [Username], [Password], [GroupNumber], [UniqueCode] FROM [User]";
                using (OleDbCommand cmd = new OleDbCommand(sql, conn))
                {
                    using (OleDbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            users.Add(new User
                            {
                                ID = reader.GetInt32(0),
                                Username = reader.GetString(1),
                                Password = reader.GetString(2),
                                GroupNumber = reader.GetInt32(3),
                                UniqueCode = reader.GetString(4)
                            });
                        }
                    }
                }
            }
            return users;
        }
    }
}
