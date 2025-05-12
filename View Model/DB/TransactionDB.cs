using Model;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using View_Model.DB;

namespace View_Model.DB {
	public class TransactionDB : BaseEntityDB {

		#region Model Mapping
		protected override BaseEntity NewEntity() {
			return new Transaction();
		}

		protected override void CreateModel(BaseEntity entity) {
			Transaction transaction = (Transaction)entity;
			UserDB userDB = new UserDB();
			ProductDB productDB = new ProductDB();

			transaction.ID = (int)reader["transaction_id"];
			transaction.Product = productDB.SelectByID((int)reader["product_id"]);
			transaction.Seller = userDB.SelectByID((int)reader["seller_id"]);
			transaction.Buyer = userDB.SelectByID((int)reader["buyer_id"]);
			transaction.Status = (OrderStatus)reader["status"];
		}
		#endregion

		#region Selectors
		public TransactionList SelectAll() {
			this.command.CommandText = "SELECT * FROM TransactionTbl";

			return new TransactionList(base.Select());
		}

		public Transaction SelectByID(int id) {
			this.command.Parameters.Clear();

			this.command.CommandText =
				"SELECT * FROM TransactionTbl WHERE [transaction_id] = ?";

			this.command.Parameters.Add(new OleDbParameter {
				OleDbType = OleDbType.Integer,
				Value = id
			});

			var list = base.Select();

			return list.Cast<Transaction>().FirstOrDefault();
		}

		public Transaction SelectBySeller(Captain seller) {
			this.command.Parameters.Clear();

			this.command.CommandText =
				"SELECT * FROM TransactionTbl WHERE [seller_id] = ?";

			this.command.Parameters.Add(new OleDbParameter {
				OleDbType = OleDbType.Integer,
				Value = seller.ID
			});

			var list = base.Select();

			return list.Cast<Transaction>().FirstOrDefault();
		}

		public Transaction SelectByBuyer(Captain buyer) {
			this.command.Parameters.Clear();

			this.command.CommandText =
				"SELECT * FROM TransactionTbl WHERE [buyer_id] = ?";

			this.command.Parameters.Add(new OleDbParameter {
				OleDbType = OleDbType.Integer,
				Value = buyer.ID
			});

			var list = base.Select();

			return list.Cast<Transaction>().FirstOrDefault();
		}

		#endregion

		#region CRUD
		public override void Insert(BaseEntity entity) {
			Transaction transaction = (Transaction)entity;

			base.Insert(transaction);
		}

		public override void Update(BaseEntity entity) {
			Transaction transaction = (Transaction)entity;

			base.Update(transaction);
		}

		public override void Delete(BaseEntity entity) {
			Transaction transaction = (Transaction)entity;

			base.Delete(transaction);
		}
		#endregion

		#region Create SQL
		public override string CreateInsertSQL(BaseEntity entity)
			=> "INSERT INTO TransactionTbl " +
			   "([product_id], [seller_id], [buyer_id], [status]) " +
			   "VALUES (?, ?, ?, ?)";

		public override string CreateUpdateSQL(BaseEntity entity)
			=> "UPDATE TransactionTbl SET " +
			   "[product_id]=?, [seller_id]=?, [buyer_id]=?, [status]=? " +
			   "WHERE [transaction_id]=?";

		public override string CreateDeleteSQL(BaseEntity entity)
			=> "DELETE FROM TransactionTbl WHERE [transaction_id]=?";


		#endregion

		#region Parameter Binders
		protected override void AddInsertParameters(OleDbCommand cmd, BaseEntity entity) {
			Transaction transaction = (Transaction)entity;
			cmd.Parameters.Add("?", OleDbType.Integer).Value = transaction.Product.ID;
			cmd.Parameters.Add("?", OleDbType.Integer).Value = transaction.Seller.ID;
			cmd.Parameters.Add("?", OleDbType.Integer).Value = transaction.Buyer.ID;
			cmd.Parameters.Add("?", OleDbType.Integer).Value = (int)transaction.Status;
		}

		protected override void AddUpdateParameters(OleDbCommand cmd, BaseEntity entity) {
			AddInsertParameters(cmd, entity);
			Transaction transaction = (Transaction)entity;
			cmd.Parameters.Add("?", OleDbType.Integer).Value = transaction.ID;
		}

		protected override void AddDeleteParameters(OleDbCommand cmd, BaseEntity entity) {
			Transaction transaction = (Transaction)entity;
			cmd.Parameters.Add("?", OleDbType.Integer).Value = transaction.ID;
		}

		#endregion
	}
}
