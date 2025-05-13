using Model;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace View_Model.DB {
	public class UserDB : BaseEntityDB {

		#region Model Mapping
		protected override BaseEntity NewEntity() {
			return new User();
		}

		protected override void CreateModel(BaseEntity entity) {
			User user = (User)entity;
			PaymentMethodDB paymentMethodDB = new PaymentMethodDB();

			user.ID = (int)reader["user_id"];
			user.Username = reader["username"].ToString();
			user.Password = reader["password"].ToString();
			user.GroupNumber = (int)reader["group_number"];
			user.UniqueCode = reader["unique_code"].ToString();
			user.PaymentMethod = paymentMethodDB.SelectByUser(user);
		}
		#endregion

		#region Selectors
		public UserList SelectAll() {
			this.command.CommandText = "SELECT * FROM UserTbl";
			return new UserList(base.Select().Cast<User>().ToList());
		}

		public User SelectByID(int id) {
			this.command.Parameters.Clear();
			this.command.CommandText = "SELECT * FROM UserTbl WHERE user_id = ?";
			this.command.Parameters.Add(new OleDbParameter { OleDbType = OleDbType.Integer, Value = id });

			return base.Select().Cast<User>().FirstOrDefault();
		}

		public User SelectByCode(string code) {
			this.command.Parameters.Clear();
			this.command.CommandText = "SELECT * FROM UserTbl WHERE unique_code = ?";
			this.command.Parameters.Add(new OleDbParameter { OleDbType = OleDbType.VarWChar, Value = code });

			return base.Select().Cast<User>().FirstOrDefault();
		}
		#endregion

		#region CreateSQL
		public override string CreateInsertSQL(BaseEntity e)
		=> "INSERT INTO UserTbl ([username],[password],[group_number],[unique_code]) " +
		   "VALUES (?, ?, ?, ?)";

		public override string CreateUpdateSQL(BaseEntity e)
			=> "UPDATE UserTbl SET [password]=? WHERE user_id = ?";

		public override string CreateDeleteSQL(BaseEntity e)
			=> "DELETE FROM UserTbl WHERE user_id = ?";
		#endregion

		#region Parameter Binders
		protected override void AddInsertParameters(OleDbCommand cmd, BaseEntity e) {
			var u = (User)e;
			cmd.Parameters.Add("?", OleDbType.VarWChar).Value = u.Username;
			cmd.Parameters.Add("?", OleDbType.VarWChar).Value = u.Password;
			cmd.Parameters.Add("?", OleDbType.Integer).Value = u.GroupNumber;
			cmd.Parameters.Add("?", OleDbType.VarWChar).Value = u.UniqueCode;
		}

		protected override void AddUpdateParameters(OleDbCommand cmd, BaseEntity e) {
			var u = (User)e;
			cmd.Parameters.Add("?", OleDbType.VarWChar).Value = u.Password;
			cmd.Parameters.Add("?", OleDbType.Integer).Value = u.ID;
		}

		protected override void AddDeleteParameters(OleDbCommand cmd, BaseEntity e) {
			var u = (User)e;
			cmd.Parameters.Add("?", OleDbType.Integer).Value = u.ID;
		}
		#endregion

		public User Login(string username, int groupnumber, string password) {
			command.Parameters.Clear();

			command.CommandText = "SELECT * FROM UserTbl " +
				"WHERE username=@Username AND group_number=@GroupNumber AND password=@Password";

			command.Parameters.AddWithValue("@Username", username);
			command.Parameters.AddWithValue("@GroupNumber", groupnumber);
			command.Parameters.AddWithValue("@Password", password);

			return (User)base.Select().FirstOrDefault();
		}
	}
}
