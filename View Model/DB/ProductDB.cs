using Model;
using System.Data.OleDb;

namespace View_Model.DB {
	public class ProductDB : BaseEntityDB {
		#region Model Mapping
		protected override BaseEntity NewEntity() {
			return new Product();
		}

		protected override void CreateModel(BaseEntity entity) {
			Product product = (Product)entity;

			product.ID = (int)reader["product_id"];
			product.Owner = new CaptainDB().SelectByID((int)reader["owner_id"]);
			product.Name = reader["product_name"].ToString();
			product.Description = reader["description"].ToString();
			product.DatePosted = (DateTime)reader["date_posted"];
			product.Condition = (ItemCondition)reader["condition"];
			product.Category = (ItemCategory)reader["category"];
			product.Price = double.Parse(reader["price"].ToString());
			product.Image = reader["image"] as byte[];
			product.Availability = (bool)reader["availability"];
			product.Request = (bool)reader["request"];
		}
		#endregion

		#region Selectors
		public ProductList SelectAll() {
			this.command.CommandText = "SELECT * FROM ProductTbl";

			return new ProductList(base.Select().Cast<Product>().ToList());
		}

		public Product SelectByID(int id) {
			this.command.Parameters.Clear();

			this.command.CommandText =
				"SELECT * FROM ProductTbl WHERE product_id = ?";

			this.command.Parameters.Add(new OleDbParameter {
				OleDbType = OleDbType.Integer,
				Value = id
			});

			var list = base.Select();

			return list.Cast<Product>().FirstOrDefault();
		}

		public ProductList SelectByOwnerID(int ownerID) {
			this.command.Parameters.Clear();
			this.command.CommandText =
				"SELECT * FROM ProductTbl WHERE owner_id = ? AND availability = true AND request = false";
			this.command.Parameters.Add(new OleDbParameter {
				OleDbType = OleDbType.Integer,
				Value = ownerID
			});

			return new ProductList(base.Select());
		}

		public ProductList SelectRequestedByOwnerID(int ownerID) {
			this.command.Parameters.Clear();
			this.command.CommandText =
				"SELECT * FROM ProductTbl WHERE owner_id = ? AND availability = true AND request = true";
			this.command.Parameters.Add(new OleDbParameter {
				OleDbType = OleDbType.Integer,
				Value = ownerID
			});

			return new ProductList(base.Select());
		}

		public ProductList SelectAllAvailable() {
			this.command.CommandText =
				"SELECT * FROM ProductTbl WHERE availability = true";

			return new ProductList(base.Select());
		}

		public ProductList SelectAllRequested() {
			this.command.CommandText =
				"SELECT * FROM ProductTbl WHERE availability = true AND request = true";

			return new ProductList(base.Select());
		}

		public ProductList SelectAllRequestedByCategory(ItemCategory category) {
			this.command.Parameters.Clear();
			this.command.CommandText =
				"SELECT * FROM ProductTbl WHERE availability = true AND request = true AND category = ?";
			this.command.Parameters.Add(new OleDbParameter { OleDbType = OleDbType.Integer, Value = (int)category });

			return new ProductList(base.Select());
		}

		public ProductList SelectAllAvailableByCategory(ItemCategory category) {
			this.command.CommandText =
				"SELECT * FROM ProductTbl WHERE availability = true AND request = false AND category = ?";
			this.command.Parameters.Clear();
			this.command.Parameters.Add(new OleDbParameter { OleDbType = OleDbType.Integer, Value = (int)category });

			return new ProductList(base.Select());
		}

		public ProductList SelectAllAvailableByCondition(ItemCondition condition) {
			this.command.CommandText =
				"SELECT * FROM ProductTbl WHERE availability = true AND request = false AND condition = ?";
			this.command.Parameters.Clear();
			this.command.Parameters.Add(new OleDbParameter { OleDbType = OleDbType.Integer, Value = (int)condition });

			return new ProductList(base.Select());
		}

		public ProductList SelectLatestAvailableByCategory(int count, ItemCategory category) {
			this.command.CommandText =
				$"SELECT TOP {count} * FROM ProductTbl WHERE availability = true AND request = false AND category = ?";
			this.command.Parameters.Clear();
			this.command.Parameters.Add(new OleDbParameter { OleDbType = OleDbType.Integer, Value = (int)category });

			return new ProductList(base.Select());
		}

		public ProductList SelectLatestRequestedByCategory(int count, ItemCategory category) {
			this.command.CommandText =
				$"SELECT TOP {count} * FROM ProductTbl WHERE availability = true AND request = true AND category = ?";
			this.command.Parameters.Clear();
			this.command.Parameters.Add(new OleDbParameter { OleDbType = OleDbType.Integer, Value = (int)category });

			return new ProductList(base.Select());
		}

		public ProductList SelectLatestAvailableByCondition(int count, ItemCondition condition) {
			this.command.CommandText =
				$"SELECT TOP {count} * FROM ProductTbl WHERE availability = true AND request = false AND condition = ?";
			this.command.Parameters.Clear();
			this.command.Parameters.Add(new OleDbParameter { OleDbType = OleDbType.Integer, Value = (int)condition });

			return new ProductList(base.Select());
		}

		public ProductList SelectLatestAvailable(int count) {
			this.command.CommandText =
				$"SELECT TOP {count} * FROM ProductTbl WHERE availability = true AND request = false ORDER BY date_posted DESC";

			return new ProductList(base.Select());
		}

		public ProductList SelectLatestRequested(int count) {
			this.command.CommandText =
				$"SELECT TOP {count} * FROM ProductTbl WHERE availability = true AND request = true ORDER BY date_posted DESC";
			return new ProductList(base.Select());
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
		public override string CreateInsertSQL(BaseEntity entity)
			=> "INSERT INTO ProductTbl " +
			   "([owner_id],[product_name],[description],[date_posted],[condition],[category],[price],[image],[availability],[request]) " +
			   "VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";

		public override string CreateUpdateSQL(BaseEntity entity)
			=> "UPDATE ProductTbl SET " +
			   "[owner_id]=?, [product_name]=?, [description]=?, [date_posted]=?, [condition]=?, " +
			   "[category]=?, [price]=?, [image]=?, [availability]=?, [request]=? " +
			   "WHERE [product_id]=?";

		public override string CreateDeleteSQL(BaseEntity entity)
			=> "DELETE FROM ProductTbl WHERE [product_id]=?";

		#endregion

		#region Parameter Binders
		protected override void AddInsertParameters(OleDbCommand cmd, BaseEntity entity) {
			Product product = (Product)entity;
			cmd.Parameters.Add("?", OleDbType.Integer).Value = product.Owner.ID;
			cmd.Parameters.Add("?", OleDbType.VarWChar).Value = product.Name;
			cmd.Parameters.Add("?", OleDbType.LongVarWChar).Value = product.Description;
			cmd.Parameters.Add("?", OleDbType.Date).Value = product.DatePosted;
			cmd.Parameters.Add("?", OleDbType.Integer).Value = (int)product.Condition;
			cmd.Parameters.Add("?", OleDbType.Integer).Value = (int)product.Category;
			cmd.Parameters.Add("?", OleDbType.Double).Value = product.Price;
			cmd.Parameters.Add("?", OleDbType.LongVarBinary).Value = (product.Image?.Length > 0) ? product.Image : DBNull.Value;
			cmd.Parameters.Add("?", OleDbType.Boolean).Value = product.Availability;
			cmd.Parameters.Add("?", OleDbType.Boolean).Value = product.Request;
		}

		protected override void AddUpdateParameters(OleDbCommand cmd, BaseEntity entity) {
			AddInsertParameters(cmd, entity);
			Product product = (Product)entity;
			cmd.Parameters.Add("?", OleDbType.Integer).Value = product.ID;
		}

		protected override void AddDeleteParameters(OleDbCommand cmd, BaseEntity entity) {
			Product product = (Product)entity;
			cmd.Parameters.Add("?", OleDbType.Integer).Value = product.ID;
		}

		#endregion
	}
}