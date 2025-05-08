using Model;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace View_Model.DB {
	public class MessageDB : BaseEntityDB {

		#region Model Mapping
		protected override BaseEntity NewEntity() {
			return new Message();
		}

		protected override void CreateModel(BaseEntity entity) {
			Message message = (Message)entity;
			//UserDB userDB = new UserDB();
			ProductDB productDB = new ProductDB();

			message.ID = (int)reader["message_id"];
			//message.Sender = userDB.GetUserByID((int)reader["sender_id"]);
			Debug.WriteLine(message.Sender.Username);
			//message.Receiver = userDB.GetUserByID((int)reader["receiver_id"]);
			Debug.WriteLine(message.Receiver.Username);
			message.Product = new Product { ID = (int)reader["product_id"] };
			message.Content = reader["message"].ToString();
			message.Timestamp = (DateTime)reader["timestamp"];
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

		#region Selectors
		public MessageList SelectAll() {
			this.command.CommandText = "SELECT * FROM MessageTbl";

			return new MessageList(base.Select());
		}

		public Message SelectByID(int id) {
			this.command.Parameters.Clear();

			this.command.CommandText =
				"SELECT * FROM MessageTbl WHERE message_id = ?";

			this.command.Parameters.Add(new OleDbParameter {
				OleDbType = OleDbType.Integer,
				Value = id
			});

			var list = base.Select();

			return list.Cast<Message>().FirstOrDefault();
		}

		#endregion

		#region CreateSQL
		public override string CreateInsertSQL(BaseEntity entity)
			=> "INSERT INTO MessageTbl ([product_id],[sender_id],[receiver_id],[message],[timestamp]) " +
			   "VALUES (?, ?, ?, ?, ?)";

		public override string CreateUpdateSQL(BaseEntity entity)
			=> "UPDATE MessageTbl " +
			   "SET [product_id]=?, [sender_id]=?, [receiver_id]=?, [message]=?, [timestamp]=? " +
			   "WHERE message_id = ?";

		public override string CreateDeleteSQL(BaseEntity entity)
			=> "DELETE FROM MessageTbl WHERE message_id = ?";
		
		#endregion

		#region Parameter Binders
		protected override void AddInsertParameters(OleDbCommand cmd, BaseEntity entity) {
			Message message = (Message)entity;
			cmd.Parameters.Add(new OleDbParameter { OleDbType = OleDbType.Integer, Value = message.Product.ID });
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
