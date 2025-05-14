using Model;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;

namespace View_Model.DB
{
    public abstract class BaseEntityDB
    {
        protected readonly string connectionString;
        protected OleDbConnection connection;
        protected OleDbCommand command;
        protected OleDbDataReader reader;

		protected List<ChangeEntity> inserted;
		protected List<ChangeEntity> deleted;
		protected List<ChangeEntity> updated;

		public BaseEntityDB()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            DirectoryInfo di = new DirectoryInfo(baseDir);
            string projectDir = di.Parent.Parent.Parent.Parent.FullName;
			string absolutePath = Path.Combine(projectDir, "View Model", "DB", "RobotGarageDB.accdb");
            
            connectionString = $@"Provider=Microsoft.ACE.OLEDB.12.0;
								Data Source={absolutePath};
								Persist Security Info=True";

			connection = new OleDbConnection(connectionString);
			command = new OleDbCommand();

			inserted = new List<ChangeEntity>();
			deleted = new List<ChangeEntity>();
			updated = new List<ChangeEntity>();
		}


		public List<BaseEntity> Select() {
			List<BaseEntity> list = new List<BaseEntity>();

			try {
				this.command.Connection = connection;
				this.connection.Open();
				this.reader = command.ExecuteReader();

				BaseEntity entity;

				while (this.reader.Read()) {
					entity = NewEntity();

					this.CreateModel(entity);

					list.Add(entity);
				}
			}
			catch (Exception ex) {
				System.Diagnostics.Debug.WriteLine(ex.Message + "\nSQL: " + command.CommandText);
			}
			finally {
				if (this.reader != null)
					this.reader.Close();

				if (this.connection.State == ConnectionState.Open)
					this.connection.Close();
			}

			return list;

		}

		public virtual void Insert(BaseEntity entity) {
			if (entity?.GetType() == NewEntity().GetType()) {
				inserted.Add(new ChangeEntity(CreateInsertSQL, AddInsertParameters, entity));
			}
		}
		public virtual void Update(BaseEntity entity) {
			if (entity?.GetType() == NewEntity().GetType())
				updated.Add(new ChangeEntity(CreateUpdateSQL, AddUpdateParameters, entity));
		}
		public virtual void Delete(BaseEntity entity) {
			if (entity?.GetType() == NewEntity().GetType())
				deleted.Add(new ChangeEntity(CreateDeleteSQL, AddDeleteParameters, entity));
		}


		public int SaveChanges() {
			int records = 0;
			var cmd = new OleDbCommand { Connection = this.connection };
			this.connection.Open();

			try {

				foreach (ChangeEntity change in this.inserted) {
					cmd.Parameters.Clear();
					cmd.CommandText = change.CreateSQL(change.Entity);
					change.Binder(cmd, change.Entity);

					records += cmd.ExecuteNonQuery();

					cmd.CommandText = "Select @@Identity";
					cmd.Parameters.Clear();
					change.Entity.ID = (int)cmd.ExecuteScalar();
				}

				foreach (ChangeEntity change in this.updated) {
					cmd.Parameters.Clear();
					cmd.CommandText = change.CreateSQL(change.Entity);
					change.Binder(cmd, change.Entity);

					records += cmd.ExecuteNonQuery();
				}

				foreach (ChangeEntity change in this.deleted) {
					cmd.Parameters.Clear();
					cmd.CommandText = change.CreateSQL(change.Entity);
					change.Binder(cmd, change.Entity);

					records += cmd.ExecuteNonQuery();
				}
			}
			catch (Exception ex) {
				System.Diagnostics.Debug.Write(ex.Message + "\nSQL: " + cmd.CommandText);
			}
			finally {
				inserted.Clear();
				updated.Clear();
				deleted.Clear();

				if (this.connection.State == ConnectionState.Open)
					this.connection.Close();
			}

			return records;
		}

		protected abstract BaseEntity NewEntity();
		protected abstract void CreateModel(BaseEntity entity);
		public abstract string CreateInsertSQL(BaseEntity entity);
		public abstract string CreateUpdateSQL(BaseEntity entity);
		public abstract string CreateDeleteSQL(BaseEntity entity);
		protected abstract void AddInsertParameters(OleDbCommand cmd, BaseEntity entity);
		protected abstract void AddUpdateParameters(OleDbCommand cmd, BaseEntity entity);
		protected abstract void AddDeleteParameters(OleDbCommand cmd, BaseEntity entity);
	}
}
