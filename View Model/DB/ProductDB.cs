using Model;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

			return new ProductList(base.Select());
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
		#endregion

		#region CRUD
		public override void Insert(BaseEntity entity) {
			Product product = (Product)entity;

			base.Insert(product);
		}

		public override void Update(BaseEntity entity) {
			Product product = (Product)entity;

			base.Update(product);
		}

		public override void Delete(BaseEntity entity) {
			Product product = (Product)entity;

			base.Delete(product);
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