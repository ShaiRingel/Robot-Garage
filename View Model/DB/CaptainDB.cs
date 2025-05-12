using Model;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace View_Model.DB {
	public class CaptainDB : UserDB {

		protected override BaseEntity NewEntity() => new Captain();

		#region Selectors
		public CaptainList SelectAll() {
			command.Parameters.Clear();
			command.CommandText =
				"SELECT U.user_id, U.username, U.password, U.group_number, U.unique_code " +
				"FROM UserTbl AS U " +
				"INNER JOIN CaptainTbl AS V ON U.user_id = V.Captain_id";

			var list = base.Select();
			return new CaptainList(list.Cast<Captain>().ToList());
		}

		public Captain SelectByID(int id) {
			command.Parameters.Clear();
			command.CommandText =
				"SELECT U.user_id, U.username, U.password, U.group_number, U.unique_code " +
				"FROM UserTbl AS U " +
				"INNER JOIN CaptainTbl AS V ON U.user_id = V.Captain_id " +
				"WHERE V.Captain_id = ?";

			command.Parameters.Add(new OleDbParameter {
				OleDbType = OleDbType.Integer,
				Value = id
			});

			return base.Select().Cast<Captain>().FirstOrDefault();
		}

		public Captain SelectByCode(string uniqueCode) {
			command.Parameters.Clear();
			command.CommandText =
				"SELECT U.user_id, U.username, U.password, U.group_number, U.unique_code " +
				"FROM UserTbl AS U " +
				"INNER JOIN CaptainTbl AS V ON U.user_id = V.Captain_id " +
				"WHERE U.unique_code = ?";

			command.Parameters.Add(new OleDbParameter {
				OleDbType = OleDbType.VarWChar,
				Value = uniqueCode
			});

			return base.Select().Cast<Captain>().FirstOrDefault();
		}

		#endregion

		#region CRUD
		public override void Insert(BaseEntity entity) {
			var v = (Captain)entity;

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

			this.SaveChanges();
		}

		public override void Update(BaseEntity entity) {
			base.Update(entity);
			this.SaveChanges();
		}

		/*public override void Delete(BaseEntity entity) {
			var Captain = (Captain)entity;
			if (Captain == null) return;

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
			=> "INSERT INTO CaptainTbl ([Captain_id]) VALUES (?)";

		public override string CreateUpdateSQL(BaseEntity entity)
			=> throw new NotSupportedException("Captain table has no updatable columns.");

		public override string CreateDeleteSQL(BaseEntity entity)
			=> "DELETE FROM CaptainTbl WHERE [Captain_id] = ?";

		#endregion

		#region Parameter Binders
		protected override void AddInsertParameters(OleDbCommand cmd, BaseEntity entity) {
			var Captain = (Captain)entity;
			cmd.Parameters.Add("?", OleDbType.Integer).Value = Captain.ID;
		}

		protected override void AddUpdateParameters(OleDbCommand cmd, BaseEntity entity)
			=> throw new NotSupportedException("Captain table has no updatable columns.");

		protected override void AddDeleteParameters(OleDbCommand cmd, BaseEntity entity) {
			var Captain = (Captain)entity;
			cmd.Parameters.Add("?", OleDbType.Integer).Value = Captain.ID;
		}
		#endregion
	}
}
