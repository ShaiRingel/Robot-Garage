using Model;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace View_Model.DB {
	public abstract class UserDB : BaseEntityDB {
		protected override void CreateModel(BaseEntity entity) {
			User user = (User)entity;

			user.ID = (int)reader["user_id"];
			user.Username = reader["username"].ToString();
			user.Password = reader["password"].ToString();
			user.GroupNumber = (int)reader["group_number"];
			user.UniqueCode = reader["unique_code"].ToString();
		}

		public override string CreateInsertSQL(BaseEntity entity)
			=> "INSERT INTO UserTbl ([username], [password], [group_number], [unique_code]) " +
					"VALUES (?, ?, ?, ?)";

		protected override void AddInsertParameters(OleDbCommand cmd, BaseEntity entity) {
			User user = (User)entity;

			cmd.Parameters.Add(new OleDbParameter { OleDbType = OleDbType.VarWChar, Value = user.Username });
			cmd.Parameters.Add(new OleDbParameter { OleDbType = OleDbType.VarWChar, Value = user.Password });
			cmd.Parameters.Add(new OleDbParameter { OleDbType = OleDbType.Integer, Value = user.GroupNumber });
			cmd.Parameters.Add(new OleDbParameter { OleDbType = OleDbType.VarWChar, Value = user.UniqueCode });
		}

		public abstract User Login(string username, int groupnumber, string password);
	}
}
