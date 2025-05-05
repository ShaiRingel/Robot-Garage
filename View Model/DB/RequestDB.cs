using Model;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace View_Model.DB {
	public class RequestDB : BaseEntityDB {
		public RequestDB() : base() {
			connection = new OleDbConnection(_connectionString);
			cmd = new OleDbCommand();
			cmd.Connection = connection;
		}

		public int Insert(Request request) {
			int records = 0;

			cmd.Parameters.Clear();

			cmd.CommandText = "INSERT INTO RequestTbl ([product_id], [renter_id], [desired_price], [request_status]) " +
								"VALUES (@Product, @Renter, @DesiredPrice, @RequestStatus)";

			cmd.Parameters.AddWithValue("@Product", request.Product);
			cmd.Parameters.AddWithValue("@Renter", request.Renter);
			cmd.Parameters.AddWithValue("@DesiredPrice", request.DesiredPrice);
			cmd.Parameters.AddWithValue("@RequestStatus", request.RequestStatus);

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

		public List<Request> GetAll() {
			cmd.CommandText = "SELECT * FROM RequestTbl";

			List<Request> requestList = SelectRequests();
			return requestList;
		}

		public int Update(Request request) {
			int records = 0;
			string sqlStr = $"UPDATE RequestTbl SET [product_id]={request.Product}, [renter_id]={request.Renter}" +
				$"[desired_price]={request.DesiredPrice}, [request_status]='{request.RequestStatus}' " +
				$"WHERE [request_id]={request.ID}";
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

		public int Delete(Request request) {
			StringBuilder sql_builder = new StringBuilder();
			sql_builder.AppendFormat("DELETE FROM RequestTbl WHERE [request_id]={0}", request.ID);
			return SaveChanges(sql_builder.ToString());
		}

		protected override BaseEntity newEntity() {
			return new Request();
		}

		protected override BaseEntity CreateModel(BaseEntity entity) {
			Request request = (Request)entity;
			request.ID = (int)reader["request_id"];
			request.Product = new Product { ID=(int)reader["product_id"] };
			request.Renter = new Renter { ID=(int)reader["renter_id"] };
			request.DesiredPrice = (decimal)reader["desired_price"];
			request.RequestStatus = reader["request_status"].ToString();
			return request;
		}

		private List<Request> SelectRequests() {
			List<Request> requestList = new List<Request>();
			try {
				cmd.Connection = connection;
				connection.Open();
				reader = cmd.ExecuteReader();
				while (reader.Read()) {
					Request request = (Request)newEntity();
					requestList.Add((Request)CreateModel(request));
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
			return requestList;
		}
	}
}