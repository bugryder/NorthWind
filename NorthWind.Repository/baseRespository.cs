using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NorthWind.Domain;
using System.Data;
using NorthWind.Infrastructure;

namespace NorthWind.Repository
{
    public class baseRespository
    {
        public SqlConnection baseCon;
        public PageSetting _pageSetting = new PageSetting();

        /// <summary>
        /// 開啟連線
        /// </summary>
        public void Open()
        {
            baseCon.Open();
        }

        /// <summary>
        /// 關閉連線
        /// </summary>
        public void Close()
        {
            baseCon.Close();
        }

        /// <summary>
        /// 執行SQL語法
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        public int ExecSQL(String sql, List<SqlParameter> parameters)
        {

            //取得連線
            int record = 0;
            if (baseCon.State == ConnectionState.Closed)
            {
                baseCon.Open();
            }
            try
            {
                SqlCommand cmdObject = new SqlCommand(sql, baseCon);
                if (parameters != null)
                {
                    foreach (SqlParameter sp in parameters)
                    {
                        cmdObject.Parameters.Add(sp);
                    }
                }

                record = cmdObject.ExecuteNonQuery();
                //Log.WriteERPLog(sql);

            }
            catch (Exception ex)
            {

                Log.SqlError(ex, sql, "");
                throw ex;


            }
            finally
            {
                if (baseCon.State == ConnectionState.Open)
                {
                    baseCon.Close();
                }
            }
            return record;
        }

        /// <summary>
        /// 取得DataTable
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public DataTable OpenDataTable(String sql, List<SqlParameter> parameters)
        {
            DataTable dt = new DataTable();
            if (_pageSetting.TurnOnPage)
            {
                this.SetPageSql(ref sql, ref parameters);
            }
            if (baseCon.State == ConnectionState.Closed)
            {
                baseCon.Open();
            }

            SqlCommand cmd = new SqlCommand();
            //設parameter
            cmd.Connection = baseCon;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sql;
            if (parameters != null)
            {
                foreach (SqlParameter sp in parameters)
                {
                    cmd.Parameters.Add(sp);
                }
            }
            try
            {
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                Log.SqlError(ex, sql, "");
                throw ex;
            }
            finally
            {
                if (baseCon.State == ConnectionState.Open)
                {
                    baseCon.Close();
                }

            }
            return dt;
        }


        /// <summary>
        /// 擴充設定sqlParameter的參數，當輸入的參數有可能是空的(null)
        /// </summary>
        /// <param name="inputParameterName">Name of the input parameter.</param>
        /// <param name="inputDbType">Type of the input db.</param>
        /// <param name="inputValue">The input value.</param>
        /// <returns></returns>
        /// <history>
        /// </history>
        protected SqlParameter SetParameterByIsNull<T>(String parameterName, DbType parameterType, T parameterValue)
        {
            SqlParameter parameter = new SqlParameter();


            if (parameterValue == null
                || parameterValue.GetType() == typeof(DBNull))
            {
                parameter = SetParameter(parameterName, parameterType, DBNull.Value);

            }
            else
            {
                parameter = SetParameter(parameterName, parameterType, parameterValue);
            }
            return parameter;
        }


        /// <summary>
        /// 設定sqlParameter的參數
        /// </summary>
        /// <param name="inputParameterName">Name of the input parameter.</param>
        /// <param name="inputDbType">Type of the input db.</param>
        /// <param name="inputValue">The input value.</param>
        /// <returns></returns>
        /// <history>
        /// </history>
        private SqlParameter SetParameter(String inputParameterName, DbType inputDbType, object inputValue)
        {
            SqlParameter singleParameter = new SqlParameter();
            singleParameter.ParameterName = inputParameterName;
            singleParameter.DbType = inputDbType;
            singleParameter.Value = inputValue;

            return singleParameter;
        }


        public Boolean BatchInsert(DataTable dt, String tableName)
        {
            Boolean result = false;
            if (baseCon.State == ConnectionState.Closed)
            {
                baseCon.Open();
            }


            SqlBulkCopy bulkCopy;
            try
            {
                bulkCopy = new SqlBulkCopy(baseCon);
                bulkCopy.DestinationTableName = tableName;

                foreach (DataColumn item in dt.Columns)
                {
                    bulkCopy.ColumnMappings.Add(item.ColumnName, item.ColumnName);
                }
                bulkCopy.WriteToServer(dt);
                result = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (baseCon.State == ConnectionState.Open)
                {
                    baseCon.Close();
                }
            }
            return result;
        }




        public Boolean BatchUpdate(DataTable dt, String sql, List<SqlParameter> parameters)
        {
            Boolean result = false;
            SqlDataAdapter adapter = new SqlDataAdapter();
            if (baseCon.State == ConnectionState.Closed)
            {
                baseCon.Open();
            }

            try
            {

                adapter.InsertCommand = new SqlCommand(sql, baseCon);
                foreach (var param in parameters)
                {
                    adapter.InsertCommand.Parameters.Add(param);
                }
                adapter.InsertCommand.UpdatedRowSource = UpdateRowSource.None;

                adapter.Update(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (baseCon.State == ConnectionState.Open)
                {
                    baseCon.Close();
                }
            }
            return result;
        }


        public Boolean BatchDelete(DataTable dt, String sql, List<SqlParameter> parameters)
        {
            Boolean result = false;
            SqlDataAdapter adapter = new SqlDataAdapter();
            if (baseCon.State == ConnectionState.Closed)
            {
                baseCon.Open();
            }

            try
            {

                adapter.DeleteCommand = new SqlCommand(sql, baseCon);
                foreach (var param in parameters)
                {
                    adapter.DeleteCommand.Parameters.Add(param);
                }
                adapter.DeleteCommand.UpdatedRowSource = UpdateRowSource.None;

                adapter.Update(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (baseCon.State == ConnectionState.Open)
                {
                    baseCon.Close();
                }
            }
            return result;
        }


        #region PageSettting


        protected void SetPageSql(ref String sql, ref List<SqlParameter> paramList)
        {
            if (_pageSetting == null)
            {
                return;
            }
            if (_pageSetting.Key == null)
            {
                return;
            }
            if (_pageSetting.Key.Trim().Length == 0)
            {
                return;
            }

            StringBuilder pageSql = new StringBuilder();
            pageSql.AppendLine(@"  
                                SELECT 
	                                *
                                FROM (
	                                SELECT
		                                ROW_NUMBER() OVER (ORDER BY " + _pageSetting.Key + @") AS RowNumber,
		                                *
	                                FROM (
                                ");

            //加入原始撈資料的sql
            pageSql.AppendLine(sql);


            pageSql.AppendLine(@"	      ) AA
	                                 ) A
                                WHERE RowNumber > @PageSize * (@PageNumber - 1)
                                AND RowNumber <= @PageSize * @PageNumber
                                ");
            sql = pageSql.ToString();
            paramList.Add(SetParameterByIsNull("@PageNumber", DbType.Int32, _pageSetting.PageNumber));
            paramList.Add(SetParameterByIsNull("@PageSize", DbType.Int32, _pageSetting.PageSize));
        }
        #endregion
    }
}
