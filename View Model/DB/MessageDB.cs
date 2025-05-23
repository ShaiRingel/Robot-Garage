using Model;
using System.Data.OleDb;
using System.Diagnostics;

namespace View_Model.DB {
	public class MessageDB : BaseEntityDB {

		#region Model Mapping
		protected override BaseEntity NewEntity() {
			return new Message();
		}

		protected override void CreateModel(BaseEntity entity) {
			Message message = (Message)entity;
			CaptainDB captainDB = new CaptainDB();

			message.ID = (int)reader["message_id"];
			message.Sender = captainDB.SelectByID((int)reader["sender_id"]);
			message.Receiver = captainDB.SelectByID((int)reader["receiver_id"]);
			message.Content = reader["message"].ToString();
			message.Timestamp = (DateTime)reader["timestamp"];
		}

		#endregion

		#region Selectors
		public MessageList SelectAll() {
			command.Parameters.Clear();
			command.CommandText = "SELECT * FROM MessageTbl";
			return new MessageList(base.Select().Cast<Message>().ToList());
		}

		public Message SelectByID(int id) {
			command.Parameters.Clear();
			command.CommandText = "SELECT * FROM MessageTbl WHERE message_id = ?";
			command.Parameters.Add(new OleDbParameter { OleDbType = OleDbType.Integer, Value = id });
			return base.Select().Cast<Message>().FirstOrDefault();
		}

		public MessageList SelectConversation(int userAId, int userBId) {
			command.Parameters.Clear();
			command.CommandText =
				"SELECT * FROM MessageTbl WHERE (sender_id = ? AND receiver_id = ?) " +
				"OR (sender_id = ? AND receiver_id = ?) ORDER BY [timestamp] ASC";
			command.Parameters.Add(new OleDbParameter { OleDbType = OleDbType.Integer, Value = userAId });
			command.Parameters.Add(new OleDbParameter { OleDbType = OleDbType.Integer, Value = userBId });
			command.Parameters.Add(new OleDbParameter { OleDbType = OleDbType.Integer, Value = userBId });
			command.Parameters.Add(new OleDbParameter { OleDbType = OleDbType.Integer, Value = userAId });
			return new MessageList(base.Select().Cast<Message>().ToList());
		}

		public CaptainList SelectChatParticipants(int currentUserId) {
			CaptainList users = new CaptainList();
			command.Parameters.Clear();
			command.CommandText =
				"SELECT DISTINCT sender_id AS user_id FROM MessageTbl WHERE receiver_id = ? " +
				"UNION SELECT DISTINCT receiver_id AS user_id FROM MessageTbl WHERE sender_id = ?";
			command.Parameters.Add(new OleDbParameter { OleDbType = OleDbType.Integer, Value = currentUserId });
			command.Parameters.Add(new OleDbParameter { OleDbType = OleDbType.Integer, Value = currentUserId });

			try {
				connection.Open();
				command.Connection = connection;
				reader = command.ExecuteReader();
				while (reader.Read()) {
					users.Add(new CaptainDB().SelectByID((int)reader["user_id"]));
				}
			}
			catch (Exception ex) {
				Debug.WriteLine("Error in GetChatParticipants: " + ex.Message);
			}
			finally {
				reader?.Close();
				if (connection.State == System.Data.ConnectionState.Open)
					connection.Close();
			}

			return users;
		}

		#endregion

		#region CRUD
		public override void Insert(BaseEntity entity) {
			Message message = (Message)entity;

			base.Insert(message);
		}

		public override void Update(BaseEntity entity) {
			Message message = (Message)entity;

			base.Update(message);
		}

		public override void Delete(BaseEntity entity) {
			Message message = (Message)entity;

			base.Delete(message);
		}

		#endregion

		#region CreateSQL
		public override string CreateInsertSQL(BaseEntity entity)
			=> "INSERT INTO MessageTbl ([sender_id],[receiver_id],[message],[timestamp]) " +
			   "VALUES (?, ?, ?, ?)";

		public override string CreateUpdateSQL(BaseEntity entity)
			=> "UPDATE MessageTbl " +
			   "SET [sender_id]=?, [receiver_id]=?, [message]=?, [timestamp]=? " +
			   "WHERE message_id = ?";

		public override string CreateDeleteSQL(BaseEntity entity)
			=> "DELETE FROM MessageTbl WHERE message_id = ?";

		#endregion

		#region Parameter Binders
		protected override void AddInsertParameters(OleDbCommand cmd, BaseEntity entity) {
			Message message = (Message)entity;
			cmd.Parameters.Add(new OleDbParameter { OleDbType = OleDbType.Integer, Value = message.Sender.ID });
			cmd.Parameters.Add(new OleDbParameter { OleDbType = OleDbType.Integer, Value = message.Receiver.ID });
			cmd.Parameters.Add(new OleDbParameter { OleDbType = OleDbType.VarWChar, Value = message.Content });
			cmd.Parameters.Add(new OleDbParameter { OleDbType = OleDbType.Date, Value = message.Timestamp });
		}

		protected override void AddUpdateParameters(OleDbCommand cmd, BaseEntity entity) {
			AddInsertParameters(cmd, entity);
			Message message = (Message)entity;
			cmd.Parameters.Add(new OleDbParameter { OleDbType = OleDbType.Integer, Value = message.ID });
		}

		protected override void AddDeleteParameters(OleDbCommand cmd, BaseEntity entity) {
			Message m = (Message)entity;
			cmd.Parameters.Add(new OleDbParameter { OleDbType = OleDbType.Integer, Value = m.ID });
		}
	}

	#endregion
}
