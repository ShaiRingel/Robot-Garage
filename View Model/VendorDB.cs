using Model;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace View_Model {
	public class VendorDB : BaseEntityDB {
		public VendorDB() : base() {
			this.connection = new OleDbConnection(_connectionString);
			this.cmd = new OleDbCommand();
			this.cmd.Connection = connection;
		}

		public int Insert(Vendor vendor) {
			int records = 0;

			cmd.Parameters.Clear();

			cmd.CommandText = "INSERT INTO VendorTbl ([ID]) VALUES (@ID)";

			cmd.Parameters.AddWithValue("@ID", vendor.ID);

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

		public List<Vendor> GetAll() {
			cmd.CommandText = "SELECT * FROM VendorTbl";

			List<Vendor> vendorList = SelectVendors();
			return vendorList;
		}

		public List<Vendor> GetByID(int id) {
			cmd.CommandText = "SELECT * FROM UserTbl WHERE [user_id]={id}";

			List<Vendor> vendorList = SelectVendors();
			return vendorList;
		}

		public int Update(Vendor vendor) {
			int records = 0;
			string sqlStr = $"UPDATE VendorTbl SET ID={vendor.ID} WHERE ID={vendor.ID}";
			try {
				cmd.CommandText = sqlStr;
				connection.Open();
				records = cmd.ExecuteNonQuery();
			}
			catch (Exception e) {
				System.Diagnostics.Debug.WriteLine(e.Message + "\nSQL:" + cmd.CommandText);
			}
			finally {
				if (connection.State == System.Data.ConnectionState.Open)
					connection.Close();
			}
			return records;
		}

		public int Delete(Vendor vendor) {
			StringBuilder sql_builder = new StringBuilder();
			sql_builder.AppendFormat("DELETE FROM VendorTbl WHERE ID={0}", vendor.ID);
			return SaveChanges(sql_builder.ToString());
		}

		protected override BaseEntity newEntity() {
			return new Vendor();
		}

		protected override BaseEntity CreateModel(BaseEntity entity) {
			Vendor vendor = (Vendor)entity;
			vendor.ID = (int)reader["vendor_id"];
			return vendor;
		}

		private List<Vendor> SelectVendors() {
			List<Vendor> vendorList = new List<Vendor>();
			try {
				cmd.Connection = connection;
				connection.Open();
				reader = cmd.ExecuteReader();
				while (reader.Read()) {
					Vendor vendor = (Vendor)newEntity();
					vendorList.Add((Vendor)CreateModel(vendor));
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
			return vendorList;
		}
	}
}