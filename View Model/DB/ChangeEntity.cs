using Model;
using System.Data.OleDb;

namespace View_Model.DB {
	public delegate string CreateSql(BaseEntity e);
	public delegate void BindParams(OleDbCommand cmd, BaseEntity e);

	public class ChangeEntity {
		private BaseEntity entity;
		private BindParams binder;
		private CreateSql createSQL;

		public ChangeEntity(CreateSql createSql, BindParams binder, BaseEntity entity) {
			createSQL = createSql;
			this.binder = binder;
			this.entity = entity;
		}

		public BaseEntity Entity { get => entity; set => entity = value; }
		public BindParams Binder { get => binder; set => binder = value; }
		public CreateSql CreateSQL { get => createSQL; set => createSQL = value; }
	}
}