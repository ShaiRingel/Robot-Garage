using Model;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace View_Model {
	public class UserDB : BaseEntityDB {
		public UserDB() : base() {
			this.connection = new OleDbConnection(_connectionString);
			this.cmd = new OleDbCommand();
			this.cmd.Connection = connection;
		}

		public int Insert(User user) {
			int records = 0;

			cmd.Parameters.Clear();

			cmd.CommandText = "INSERT INTO UserTbl ([username], [password], [group_number], [unique_code]) " +
								"VALUES (@Username, @Password, @GroupNumber, @UniqueCode)";

			cmd.Parameters.AddWithValue("@Username", user.Username);
			cmd.Parameters.AddWithValue("@Password", user.Password);
			cmd.Parameters.AddWithValue("@GroupNumber", user.GroupNumber);
			cmd.Parameters.AddWithValue("@UniqueCode", user.UniqueCode);

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

		public UserList GetAll() {
			cmd.CommandText = "SELECT * FROM UserTbl";

			return SelectUsers();
		}

		public User GetByID(int id) {
			cmd.Parameters.Clear();

			cmd.CommandText = "SELECT * FROM UserTbl WHERE user_id=@ID";

			cmd.Parameters.AddWithValue("@ID", id);

			return SelectUsers().FirstOrDefault();
		}

		public User GetByName(string username) {
			cmd.Parameters.Clear();

			cmd.CommandText = "SELECT * FROM UserTbl WHERE username=@Username";

			cmd.Parameters.AddWithValue("@Username", username);

			return SelectUsers().FirstOrDefault();
		}

        public User GetByCode(string uniqueCode)
        {
            cmd.Parameters.Clear();

            cmd.CommandText = "SELECT * FROM UserTbl WHERE unique_code=@UniqueCode";

            cmd.Parameters.AddWithValue("@UniqueCode", uniqueCode);

            return SelectUsers().FirstOrDefault();
        }

        public int Update(User user) {
			int records = 0;

			cmd.Parameters.Clear();

			cmd.CommandText = "UPDATE UserTbl SET [username]=@Username, [password]=@Password, [group_number]=@GroupNumber, " +
				"[unique_code]=@UniqueCode WHERE [user_id]=@ID";

			cmd.Parameters.AddWithValue("@Username", user.Username);
			cmd.Parameters.AddWithValue("@Password", user.Password);
			cmd.Parameters.AddWithValue("@GroupNumber", user.GroupNumber);
			cmd.Parameters.AddWithValue("@UniqueCode", user.UniqueCode);
			cmd.Parameters.AddWithValue("@ID", user.ID);

			try {
				connection.Open();
				records = cmd.ExecuteNonQuery();
			}
			catch (Exception e) {
				System.Diagnostics.Debug.WriteLine("Error occurred during UPDATE operation: " + e.Message);
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

		public int Delete(User user) {
			int records = 0;

			cmd.Parameters.Clear();

			cmd.CommandText = "DELETE FROM UserTbl WHERE ID=@ID";
			cmd.Parameters.AddWithValue("@ID", user.ID);

			try {
				connection.Open();
				records = cmd.ExecuteNonQuery();
			}
			catch (Exception e) {
				System.Diagnostics.Debug.WriteLine("Error occurred during DELETE operation: " + e.Message);
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

		protected override BaseEntity newEntity() {
			return new User();
		}

		protected override BaseEntity CreateModel(BaseEntity entity) {
			User user = (User)entity;
			user.ID = (int)reader["user_id"];
			user.Username = reader["username"].ToString();
			user.Password = reader["password"].ToString();
			user.GroupNumber = (int)reader["group_number"];
			user.UniqueCode = reader["unique_code"].ToString();
			return user;
		}

		private UserList SelectUsers() {
			UserList userList = new UserList();
			try {
				cmd.Connection = connection;
				connection.Open();
				reader = cmd.ExecuteReader();
				while (reader.Read()) {
					User user = (User)newEntity();
					userList.Add((User)CreateModel(user));
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
			return userList;
		}
	}
}
