using Model;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace View_Model {
	public class RenterDB : BaseEntityDB {
		public RenterDB() : base() {
			this.connection = new OleDbConnection(_connectionString);
			this.cmd = new OleDbCommand();
			this.cmd.Connection = connection;
		}

		public int Insert(Renter renter) {
			int records = 0;

			cmd.Parameters.Clear();

			cmd.CommandText = "INSERT INTO RenterTbl ([username], [password], [group_number], [unique_code]) " +
								"VALUES (@Username, @Password, @GroupNumber, @UniqueCode)";

			cmd.Parameters.AddWithValue("@Username", renter.Username);
			cmd.Parameters.AddWithValue("@Password", renter.Password);
			cmd.Parameters.AddWithValue("@GroupNumber", renter.GroupNumber);
			cmd.Parameters.AddWithValue("@UniqueCode", renter.UniqueCode);

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

		public RenterList GetAll() {
			cmd.CommandText = "SELECT * FROM RenterTbl";
			return SelectRenters();
		}

		public int Update(Renter renter) {
			int records = 0;

			cmd.Parameters.Clear();

			cmd.CommandText = "UPDATE RenterTbl SET username=@Username, password=@Password, group_number=@GroupNumber, unique_code=@UniqueCode WHERE ID=@ID";

			cmd.Parameters.AddWithValue("@Username", renter.Username);
			cmd.Parameters.AddWithValue("@Password", renter.Password);
			cmd.Parameters.AddWithValue("@GroupNumber", renter.GroupNumber);
			cmd.Parameters.AddWithValue("@UniqueCode", renter.UniqueCode);
			cmd.Parameters.AddWithValue("@ID", renter.ID);

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

		public int Delete(Renter renter) {
			int records = 0;

			cmd.Parameters.Clear();

			cmd.CommandText = "DELETE FROM RenterTbl WHERE ID=@ID";
			cmd.Parameters.AddWithValue("@ID", renter.ID);

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
			return new Renter();
		}

		protected override BaseEntity CreateModel(BaseEntity entity) {
			Renter renter = (Renter) entity;
			renter.ID = (int)reader["ID"];
			renter.Username = reader["username"].ToString();
			renter.Password = reader["password"].ToString();
			renter.GroupNumber = (int)reader["group_number"];
			renter.UniqueCode = reader["unique_code"].ToString();
			return renter;
		}

		private RenterList SelectRenters() {
			RenterList renterList = new RenterList();
			try {
				cmd.Connection = connection;
				connection.Open();
				reader = cmd.ExecuteReader();
				while (reader.Read()) {
					Renter renter = (Renter)newEntity();
					renterList.Add((Renter)CreateModel(renter));
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
			return renterList;
		}
	}
}
