using Model;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace View_Model {
	public class MessageDB : BaseEntityDB {
		public MessageDB() : base() {
			this.connection = new OleDbConnection(_connectionString);
			this.cmd = new OleDbCommand();
			this.cmd.Connection = connection;
		}

		public int Insert(Message message) {
			int records = 0;

			cmd.Parameters.Clear();

			cmd.CommandText = "INSERT INTO MessageTbl ([product_id], [sender_id], [receiver_id], [message], [timestamp]) " +
								"VALUES (@Product, @Sender, @Receiver, @Message, @Timestamp)";

			cmd.Parameters.AddWithValue("@Product", message.Product.ID);
			cmd.Parameters.AddWithValue("@Sender", message.Sender.ID);
			cmd.Parameters.AddWithValue("@Receiver", message.Receiver.ID);
			cmd.Parameters.AddWithValue("@Message", message.Content);
			cmd.Parameters.AddWithValue("@Timestamp", message.Timestamp);

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

		public List<Message> GetAll() {
			cmd.CommandText = "SELECT * FROM MessageTbl";

			List<Message> messageList = SelectMessages();
			return messageList;
		}

		public int Update(Message message) {
			int records = 0;
			string sqlStr = $"UPDATE MessageTbl SET [product_id]={message.Product.ID}, [sender_id]={message.Sender.ID}, " +
				$"[receiver_id]={message.Receiver.ID}, [message]='{message.Content}', [timestamp]='{message.Timestamp}' " +
				$"WHERE [message_id]={message.ID}";
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

		public int Delete(Message message) {
			StringBuilder sql_builder = new StringBuilder();
			sql_builder.AppendFormat($"DELETE FROM MessageTbl WHERE [message_id]={message.ID}");
			return SaveChanges(sql_builder.ToString());
		}

		protected override BaseEntity newEntity() {
			return new Message();
		}

		protected override BaseEntity CreateModel(BaseEntity entity) {
			Message message = (Message)entity;
			message.ID = (int)reader["message_id"];
			message.Product = new Product { ID = (int)reader["product_id"] };
			message.Content = reader["message"].ToString();
			message.Timestamp = (DateTime)reader["timestamp"];
			return message;
		}

		private List<Message> SelectMessages() {
			List<Message> messageList = new List<Message>();
			try {
				cmd.Connection = connection;
				connection.Open();
				reader = cmd.ExecuteReader();
				while (reader.Read()) {
					Message message = (Message)newEntity();
					messageList.Add((Message)CreateModel(message));
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
			return messageList;
		}
	}
}