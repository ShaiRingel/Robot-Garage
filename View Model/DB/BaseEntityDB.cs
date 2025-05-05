using Model;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Net.NetworkInformation;

namespace View_Model.DB {
	public abstract class BaseEntityDB {
		protected readonly string _connectionString;
		protected OleDbConnection connection;
		protected OleDbCommand cmd;
		protected OleDbDataReader reader;
		protected abstract BaseEntity newEntity();
		protected abstract BaseEntity CreateModel(BaseEntity entity);

		public BaseEntityDB() {
			string baseDir = AppDomain.CurrentDomain.BaseDirectory;
			DirectoryInfo di = new DirectoryInfo(baseDir);
			string projectDir = di.Parent.Parent.Parent.Parent.FullName;
			string absolutePath = Path.Combine(projectDir, "View Model", "DB", "RobotGarageDB.accdb");
			Debug.WriteLine("Absolute Path Found: " + absolutePath);
			_connectionString = $@"Provider=Microsoft.ACE.OLEDB.12.0;
								Data Source={absolutePath};
								Persist Security Info=True";
			connection = new OleDbConnection(_connectionString);
			cmd = new OleDbCommand();
		}

		protected int SaveChanges(string cmd_text) {
			OleDbCommand cmd = new OleDbCommand();
			int records = 0;
			try {
				cmd.Connection = connection;
				cmd.CommandText = cmd_text;
				connection.Open();
				records = cmd.ExecuteNonQuery();
			}
			catch (Exception e) {
				Debug.WriteLine(e.Message + "\nSQL:" + cmd.CommandText);
			}
			finally {
				if (connection.State == ConnectionState.Open)
					connection.Close();

			}
			return records;
		}


		protected List<BaseEntity> Select() {
			List<BaseEntity> list = new List<BaseEntity>();
			try {
				cmd.Connection = connection;
				connection.Open();
				reader = cmd.ExecuteReader();

				while (reader.Read()) {
					BaseEntity entity = newEntity();
					list.Add(CreateModel(entity));
				}
			}
			catch (Exception e) {
				Debug.WriteLine(e.Message + "\nSQL" + cmd.CommandText);
			}
			finally {
				if (reader != null) {
					reader.Close();
				}
				if (connection.State == ConnectionState.Open) {
					connection.Close();
				}
			}
			return list;
		}
	}
}
