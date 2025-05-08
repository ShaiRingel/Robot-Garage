using Model;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace View_Model.DB {
	public class ViewerDB : UserDB {

		protected override BaseEntity NewEntity() => new Viewer();

		#region Selectors
		public ViewerList SelectAll() {
			command.Parameters.Clear();
			command.CommandText =
				"SELECT U.user_id, U.username, U.password, U.group_number, U.unique_code " +
				"FROM UserTbl AS U " +
				"INNER JOIN ViewerTbl AS V ON U.user_id = V.viewer_id";

			var list = base.Select();
			return new ViewerList(list.Cast<Viewer>().ToList());
		}

		public Viewer SelectByID(int id) {
			command.Parameters.Clear();
			command.CommandText =
				"SELECT U.user_id, U.username, U.password, U.group_number, U.unique_code " +
				"FROM UserTbl AS U " +
				"INNER JOIN ViewerTbl AS V ON U.user_id = V.viewer_id " +
				"WHERE V.viewer_id = ?";

			command.Parameters.Add(new OleDbParameter {
				OleDbType = OleDbType.Integer,
				Value = id
			});

			return base.Select().Cast<Viewer>().FirstOrDefault();
		}

		public Viewer SelectByCode(string uniqueCode) {
			command.Parameters.Clear();
			command.CommandText =
				"SELECT U.user_id, U.username, U.password, U.group_number, U.unique_code " +
				"FROM UserTbl AS U " +
				"INNER JOIN ViewerTbl AS V ON U.user_id = V.viewer_id " +
				"WHERE U.unique_code = ?";

			command.Parameters.Add(new OleDbParameter {
				OleDbType = OleDbType.VarWChar,
				Value = uniqueCode
			});

			return base.Select().Cast<Viewer>().FirstOrDefault();
		}

		#endregion

		#region CRUD
		public override void Insert(BaseEntity entity) {
			var v = (Viewer)entity;

			inserted.Add(new ChangeEntity(
				base.CreateInsertSQL,
				base.AddInsertParameters,
				entity
			));

			inserted.Add(new ChangeEntity(
				this.CreateInsertSQL,
				this.AddInsertParameters,
				entity
			));
		}

		public override void Update(BaseEntity entity) {
			base.Update(entity);
		}

		/*public override void Delete(BaseEntity entity) {
			var viewer = (Viewer)entity;
			if (viewer == null) return;

			deleted.Add(new ChangeEntity(
				base.CreateDeleteSQL,
				base.AddDeleteParameters,
				entity
			));

			deleted.Add(new ChangeEntity(
				this.CreateDeleteSQL,
				this.AddDeleteParameters,
				entity
			));
		}*/
		#endregion

		#region Create SQL
		public override string CreateInsertSQL(BaseEntity entity)
			=> "INSERT INTO ViewerTbl ([viewer_id]) VALUES (?)";

		public override string CreateUpdateSQL(BaseEntity entity)
			=> throw new NotSupportedException("Viewer table has no updatable columns.");

		public override string CreateDeleteSQL(BaseEntity entity)
			=> "DELETE FROM ViewerTbl WHERE [viewer_id] = ?";

		#endregion

		#region Parameter Binders
		protected override void AddInsertParameters(OleDbCommand cmd, BaseEntity entity) {
			var viewer = (Viewer)entity;
			cmd.Parameters.Add("?", OleDbType.Integer).Value = viewer.ID;
		}

		protected override void AddUpdateParameters(OleDbCommand cmd, BaseEntity entity)
			=> throw new NotSupportedException("Viewer table has no updatable columns.");

		protected override void AddDeleteParameters(OleDbCommand cmd, BaseEntity entity) {
			var viewer = (Viewer)entity;
			cmd.Parameters.Add("?", OleDbType.Integer).Value = viewer.ID;
		}
		#endregion

		public override User Login(string username, int groupnumber, string password) {
			command.Parameters.Clear();

			command.CommandText = "SELECT * FROM UserTbl " +
				"WHERE username=@Username AND group_number=@GroupNumber AND password=@Password";

			command.Parameters.AddWithValue("@Username", username);
			command.Parameters.AddWithValue("@GroupNumber", groupnumber);
			command.Parameters.AddWithValue("@Password", password);

			return (User)Select().FirstOrDefault();
		}
	}
}
