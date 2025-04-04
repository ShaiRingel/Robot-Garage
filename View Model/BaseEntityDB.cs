using Model;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace View_Model {
	public abstract class BaseEntityDB {
		protected readonly string _connectionString;
		protected OleDbConnection connection;
		protected OleDbCommand cmd;
		protected OleDbDataReader reader;
		protected abstract BaseEntity newEntity();
		protected abstract BaseEntity CreateModel(BaseEntity entity);

		public BaseEntityDB() {
			string relativePath = @"..\..\RobotGarageDB.accdb";
			string absolutePath = Path.GetFullPath(relativePath);
			_connectionString = $@"Provider=Microsoft.ACE.OLEDB.12.0;
								Data Source={absolutePath};
								Persist Security Info=True";
			this.connection = new OleDbConnection(_connectionString);
			cmd = new OleDbCommand();
		}

		protected int SaveChanges(string cmd_text) {
			OleDbCommand cmd = new OleDbCommand();
			int records = 0;
			try {
				cmd.Connection = this.connection;
				cmd.CommandText = cmd_text;
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


		protected List<BaseEntity> Select() {
			List<BaseEntity> list = new List<BaseEntity>();
			try {
				cmd.Connection = this.connection;
				//cmd.CommandText = SqlStr;
				connection.Open();
				reader = cmd.ExecuteReader();

				while (reader.Read()) {
					BaseEntity entity = newEntity();
					list.Add(CreateModel(entity));
				}
			}
			catch (Exception e) {
				System.Diagnostics.Debug.WriteLine(e.Message + /*"\nOle:"*/ "\nSQL" + cmd.CommandText);
			}
			finally {
				if (reader != null) {
					reader.Close();
				}
				if (connection.State == /*System.Data.ConnectionState.Open*/ ConnectionState.Open) {
					connection.Close();
				}
			}
			return list;
		}
	}
}
