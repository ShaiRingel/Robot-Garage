using Model;
using System.Data;
using System.Data.OleDb;

namespace View_Model.DB {
	public class PaymentMethodDB : BaseEntityDB {

		#region Model Mapping
		protected override BaseEntity NewEntity() {
			return new PaymentMethod();
		}

		protected override void CreateModel(BaseEntity entity) {
			PaymentMethod paymentMethod = (PaymentMethod)entity;

			paymentMethod.ID = (int)reader["payment_id"];
			paymentMethod.UserID = (int)reader["user_id"];
			paymentMethod.CardholderName = reader["cardholder_name"].ToString();
			paymentMethod.CardNumber = reader["card_number"].ToString();
			paymentMethod.Expiry = (DateTime)reader["expiry"];
			paymentMethod.Cvc = (int)reader["cvc"];
		}
		#endregion

		#region Selectors
		public PaymentMethodList SelectAll() {
			this.command.CommandText = "SELECT * FROM PaymentMethodTbl";
			return new PaymentMethodList(base.Select().Cast<PaymentMethod>().ToList());
		}

		public PaymentMethod SelectByID(int id) {
			this.command.Parameters.Clear();
			this.command.CommandText = "SELECT * FROM PaymentMethodTbl WHERE [payment_id] = ?";
			this.command.Parameters.Add(new OleDbParameter { OleDbType = OleDbType.Integer, Value = id });

			return base.Select().Cast<PaymentMethod>().FirstOrDefault();
		}

		public PaymentMethod SelectByUser(User user) {
			this.command.Parameters.Clear();
			this.command.CommandText = "SELECT * FROM PaymentMethodTbl WHERE [user_id] = ?";
			this.command.Parameters.Add(new OleDbParameter { OleDbType = OleDbType.Integer, Value = user.ID });

			return base.Select().Cast<PaymentMethod>().FirstOrDefault();
		}
		#endregion

		#region CRUD
		public override void Insert(BaseEntity entity) {
			inserted.Add(new ChangeEntity(
				this.CreateInsertSQL,
				this.AddInsertParameters,
				entity
			));
		}

		public override void Update(BaseEntity entity) {
			updated.Add(new ChangeEntity(
				this.CreateUpdateSQL,
				this.AddUpdateParameters,
				entity
			));
		}

		public override void Delete(BaseEntity entity) {
			deleted.Add(new ChangeEntity(
				this.CreateDeleteSQL,
				this.AddDeleteParameters,
				entity
			));
		}
		#endregion

		#region CreateSQL
		public override string CreateInsertSQL(BaseEntity e)
			=> "INSERT INTO PaymentMethodTbl ([user_id],[cardholder_name],[card_number],[expiry],[cvc]) VALUES (?, ?, ?, ?, ?)";

		public override string CreateUpdateSQL(BaseEntity e)
			=> "UPDATE PaymentMethodTbl SET [user_id]=?, [cardholder_name]=?, [card_number]=?, [expiry]=?, [cvc]=? WHERE [payment_id]=?";

		public override string CreateDeleteSQL(BaseEntity e)
			=> "DELETE FROM PaymentMethodTbl WHERE payment_id = ?";
		#endregion

		#region Parameter Binders
		protected override void AddInsertParameters(OleDbCommand cmd, BaseEntity e) {
			PaymentMethod paymentMethod = (PaymentMethod)e;
			cmd.Parameters.Add("?", OleDbType.Integer).Value = paymentMethod.UserID;
			cmd.Parameters.Add("?", OleDbType.VarChar).Value = paymentMethod.CardholderName;
			cmd.Parameters.Add("?", OleDbType.VarChar).Value = paymentMethod.CardNumber;
			cmd.Parameters.Add("?", OleDbType.Date).Value = paymentMethod.Expiry;
			cmd.Parameters.Add("?", OleDbType.Integer).Value = paymentMethod.Cvc;
		}

		protected override void AddUpdateParameters(OleDbCommand cmd, BaseEntity e) {
			AddInsertParameters(cmd, e);
			PaymentMethod paymentMethod = (PaymentMethod)e;
			cmd.Parameters.Add("?", OleDbType.Integer).Value = paymentMethod.ID;
		}

		protected override void AddDeleteParameters(OleDbCommand cmd, BaseEntity e) {
			PaymentMethod paymentMethod = (PaymentMethod)e;
			cmd.Parameters.Add("?", OleDbType.Integer).Value = paymentMethod.ID;
		}
		#endregion

	}
}