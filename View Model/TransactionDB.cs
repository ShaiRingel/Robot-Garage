using Model;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace View_Model {
	public class TransactionDB : BaseEntityDB {
		public TransactionDB() : base() {
			this.connection = new OleDbConnection(_connectionString);
			this.cmd = new OleDbCommand();
			this.cmd.Connection = connection;
		}

		public int Insert(Transaction transaction) {
			int records = 0;

			cmd.Parameters.Clear();

			cmd.CommandText = "INSERT INTO Transaction ([product_id], [renter_id], [start_date], [end_date], [status]) " +
								"VALUES (@Product, @Renter, @StartDate, @EndDate, @Status)";

			cmd.Parameters.AddWithValue("@Product", transaction.Product);
			cmd.Parameters.AddWithValue("@Renter", transaction.Renter);
			cmd.Parameters.AddWithValue("@StartDate", transaction.StartDate);
			cmd.Parameters.AddWithValue("@EndDate", transaction.EndDate);
			cmd.Parameters.AddWithValue("@Status", (int)transaction.Status);

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

		public List<Transaction> GetAll() {
			cmd.CommandText = "SELECT * FROM Transaction";

			List<Transaction> transactionList = SelectTransactions();
			return transactionList;
		}

		public int Update(Transaction transaction) {
			int records = 0;
			string sqlStr = $"UPDATE Transaction SET [product_id]={transaction.Product}, [renter_id]={transaction.Renter}, " +
				$"[start_date]='{transaction.StartDate}', [end_date]='{transaction.EndDate}', " +
				$"[status]={(int)transaction.Status} WHERE [transaction_id]={transaction.ID}";
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

		public int Delete(Transaction transaction) {
			StringBuilder sql_builder = new StringBuilder();
			sql_builder.AppendFormat($"DELETE FROM Transaction WHERE [transaction_id]={transaction.ID}");
			return SaveChanges(sql_builder.ToString());
		}

		protected override BaseEntity newEntity() {
			return new Transaction();
		}

		protected override BaseEntity CreateModel(BaseEntity entity) {
			Transaction transaction = (Transaction)entity;
			transaction.ID = (int)reader["transaction_id"];
			transaction.Product = (int)reader["product_id"];
			transaction.Renter = new Renter { ID= (int)reader["renter_id"] };
			transaction.StartDate = (DateTime)reader["start_date"];
			transaction.EndDate = (DateTime)reader["end_date"];
			transaction.Status = (OrderStatus)reader["status"];
			return transaction;
		}

		private List<Transaction> SelectTransactions() {
			List<Transaction> transactionList = new List<Transaction>();
			try {
				cmd.Connection = connection;
				connection.Open();
				reader = cmd.ExecuteReader();
				while (reader.Read()) {
					Transaction transaction = (Transaction)newEntity();
					transactionList.Add((Transaction)CreateModel(transaction));
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
			return transactionList;
		}
	}
}