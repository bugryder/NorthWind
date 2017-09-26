using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using NorthWind.Domain;
using NorthWind.Infrastructure;

namespace NorthWind.Repository.NorthWind
{
    public class NorthWindProductsRepository: NorthWindBaseRepository
    {

        public List<Products> GetProducts(int? ProductID)
        {
            List<Products> pList = new List<Products>();
            DataTable dt = new DataTable();
            List<SqlParameter> paramList = new List<SqlParameter>();
            #region sql
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@"
                             SELECT ProductID, ProductName ,SupplierID ,CategoryID ,QuantityPerUnit ,UnitPrice ,UnitsInStock ,UnitsOnOrder ,ReorderLevel , Discontinued
                             FROM Products WITH (NOLOCK)
                             WHERE (1=1)
                             AND  Discontinued = 0
                            ");


            if (ProductID != null)
            {
                sql.AppendLine(" AND ProductID=@ProductID");
                paramList.Add(SetParameterByIsNull("@ProductID", DbType.Int32, ProductID));
            }
            #endregion
            dt = OpenDataTable(sql.ToString(), paramList);
            pList = dt.ToList<Products>().ToList();

            return pList;
        }

        public int AddProducts(Products Product)
        {

            DataTable dt = new DataTable();
            List<SqlParameter> paramList = new List<SqlParameter>();
            #region sql
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@"
                             insert Products( ProductName ,SupplierID ,CategoryID ,QuantityPerUnit ,UnitPrice ,UnitsInStock ,UnitsOnOrder ,ReorderLevel , Discontinued)
                             values (@ProductName ,@SupplierID ,@CategoryID ,@QuantityPerUnit ,@UnitPrice ,@UnitsInStock ,@UnitsOnOrder ,@ReorderLevel , @Discontinued);
                             SELECT CAST(scope_identity() AS int)
                            ");

            paramList.Add(SetParameterByIsNull("@ProductName", DbType.String, Product.ProductName));
            paramList.Add(SetParameterByIsNull("@SupplierID", DbType.Int32, Product.SupplierID));
            paramList.Add(SetParameterByIsNull("@CategoryID", DbType.Int32, Product.CategoryID));
            paramList.Add(SetParameterByIsNull("@QuantityPerUnit", DbType.String, Product.QuantityPerUnit));
            paramList.Add(SetParameterByIsNull("@UnitPrice", DbType.Decimal, Product.UnitPrice));
            paramList.Add(SetParameterByIsNull("@UnitsInStock", DbType.Int16, Product.UnitsInStock));
            paramList.Add(SetParameterByIsNull("@UnitsOnOrder", DbType.Int16, Product.UnitsOnOrder));
            paramList.Add(SetParameterByIsNull("@ReorderLevel", DbType.Int16, Product.ReorderLevel));
            paramList.Add(SetParameterByIsNull("@Discontinued", DbType.Boolean, Product.Discontinued));

            #endregion
            return ExecuteScalar<int>(sql.ToString(), paramList);

        }

        public int UpdateProducts(Products Product)
        {
            DataTable dt = new DataTable();
            List<SqlParameter> paramList = new List<SqlParameter>();
            #region sql
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@"
                             update Products set 
                             ProductName = ISNULL(@ProductName, ProductName) 
                            ,SupplierID = ISNULL(@SupplierID, SupplierID) 
                            ,CategoryID = ISNULL(@CategoryID, CategoryID) 
                            ,QuantityPerUnit = ISNULL(@QuantityPerUnit, QuantityPerUnit) 
                            ,UnitPrice = ISNULL(@UnitPrice, UnitPrice) 
                            ,UnitsInStock = ISNULL(@UnitsInStock, UnitsInStock) 
                            ,UnitsOnOrder = ISNULL(@UnitsOnOrder, UnitsOnOrder) 
                            ,ReorderLevel = ISNULL(@ReorderLevel, ReorderLevel) 
                            ,Discontinued = ISNULL(@Discontinued, Discontinued) 
                             where ProductID = @ProductID
                            ");
            paramList.Add(SetParameterByIsNull("@ProductID", DbType.Int32, Product.ProductID));
            paramList.Add(SetParameterByIsNull("@ProductName", DbType.String, Product.ProductName));
            paramList.Add(SetParameterByIsNull("@SupplierID", DbType.Int32, Product.SupplierID));
            paramList.Add(SetParameterByIsNull("@CategoryID", DbType.Int32, Product.CategoryID));
            paramList.Add(SetParameterByIsNull("@QuantityPerUnit", DbType.String, Product.QuantityPerUnit));
            paramList.Add(SetParameterByIsNull("@UnitPrice", DbType.Decimal, Product.UnitPrice));
            paramList.Add(SetParameterByIsNull("@UnitsInStock", DbType.Int16, Product.UnitsInStock));
            paramList.Add(SetParameterByIsNull("@UnitsOnOrder", DbType.Int16, Product.UnitsOnOrder));
            paramList.Add(SetParameterByIsNull("@ReorderLevel", DbType.Int16, Product.ReorderLevel));
            paramList.Add(SetParameterByIsNull("@Discontinued", DbType.Boolean, Product.Discontinued));

            #endregion
            return ExecSQL(sql.ToString(), paramList);

        }

        public int DeleteProducts(int? ProductID)
        {
            List<Products> pList = new List<Products>();
            DataTable dt = new DataTable();
            List<SqlParameter> paramList = new List<SqlParameter>();
            #region sql
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@"
                             Update Products set Discontinued = 1 where ProductID=@ProductID
                            ");
            paramList.Add(SetParameterByIsNull("@ProductID", DbType.Int32, ProductID));
            #endregion
            return ExecSQL(sql.ToString(), paramList);

        }



    }
}
