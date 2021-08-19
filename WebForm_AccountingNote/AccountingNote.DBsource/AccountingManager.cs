using AccountingNote.ORM.DBModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingNote.DBsource
{
    public class AccountingManager
    {

        //private static string GetConnectionString()
        //{
        //    string val = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        //    return val;
        //}

        /// <summary> 查詢流水帳清單 </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static DataTable GetAccountingList(string userID)
        {
            string connStr = DBHelper.GetConnectionString();
            string dbCommand =
                $@" SELECT
                        ID,
                        Caption,
                        Amount,
                        ActType,
                        CreateDate
                    FROM Accounting
                    WHERE UserID = @userID
                ";

            // 用List把Parameter裝起來，再裝到共用參數
            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@userID", userID));
            try // 讓錯誤可以被凸顯，因此 TryCatch 不應該重構進 DBHelper
            {
                return DBHelper.ReadDataTable(connStr, dbCommand, list);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
                return null;
            }
        }

        public static List<Accounting> GetAccountingList(Guid userID)
        {
            try
            {
                //Guid.TryParse(userID, out Guid TempGuid);

                using (ContextModel context = new ContextModel())
                {
                    var query =
                        (from item in context.Accountings
                         where item.UserID == userID
                         select item);
                    var list = query.ToList(); // 不能回傳 query 因為只有這裡使用 ORM
                    // 回到網站後就沒有地方使用 ORM 了，變成"物件"的東西就跟 ORM 沒關係了
                    return list;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
                return null;
            }

        }

        /// <summary> 查詢流水帳 </summary>
        /// <param name="id"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static DataRow GetAccounting(int id, string userID)
        {
            string connStr = DBHelper.GetConnectionString();
            string dbCommand =
                $@" SELECT 
                        ID,
                        Caption,
                        Amount,
                        ActType,
                        CreateDate,
                        Body
                    FROM Accounting
                    WHERE id = @id AND UserID = @userID
                "; // userID = 防止偷看其他使用者的資料

            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@id", id));
            list.Add(new SqlParameter("@userID", userID));

            try
            {
                return DBHelper.ReadDataRow(connStr, dbCommand, list);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
                return null;
            }
        }


        /// <summary> 建立流水帳 </summary>
        /// <param name="userID"></param>
        /// <param name="caption"></param>
        /// <param name="amount"></param>
        /// <param name="actType"></param>
        /// <param name="body"></param>
        public static void CreateAccounting(string userID, string caption, int amount, int actType, string body)
        {
            // <<<<< check input >>>>>
            if (amount < 0 || amount > 1000000)
                throw new ArgumentException("Amount must between 0 and 1,000,000.");

            if (actType < 0 || actType > 1)
                throw new ArgumentException("ActType must be 0 or 1.");
            // <<<<< check input >>>>>

            // 檢查傳進來的body參數
            string bodyColumnSQL = "";
            string bodyValueSQL = "";
            if (!string.IsNullOrWhiteSpace(body))
            {
                bodyColumnSQL = ", Body";
                bodyValueSQL = ", @body";
            }

            string connStr = DBHelper.GetConnectionString();
            string dbCommand =
                $@" INSERT INTO [dbo].[Accounting]
                    (
                        UserID
                       ,Caption
                       ,Amount
                       ,ActType
                       ,CreateDate
                       {bodyColumnSQL}
                    )
                    VALUES
                    (
                        @userID
                       ,@caption
                       ,@amount
                       ,@actType
                       ,@createDate
                       {bodyValueSQL}
                    )
                ";
            // connect db & execute
            List<SqlParameter> paramList = new List<SqlParameter>();
            // comm.Parameters.AddWithValue("@userID", userID);
            paramList.Add(new SqlParameter("@userID", userID));
            paramList.Add(new SqlParameter("@caption", caption));
            paramList.Add(new SqlParameter("@amount", amount));
            paramList.Add(new SqlParameter("@actType", actType));
            paramList.Add(new SqlParameter("@createDate", DateTime.Now));
            //paramList.Add(new SqlParameter("@body", body));
            // 0812pm
            if (!string.IsNullOrWhiteSpace(body))
                paramList.Add(new SqlParameter("@body", body));
            //
            try
            {
                DBHelper.CreatData(connStr, dbCommand, paramList);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }


        /// <summary> 編輯流水帳 </summary>
        /// <param name="userID"></param>
        /// <param name="caption"></param>
        /// <param name="amount"></param>
        /// <param name="actType"></param>
        /// <param name="body"></param>
        public static bool UpdateAccounting(int ID, string userID, string caption, int amount, int actType, string body)
        {
            // <<<<< check input >>>>>
            if (amount < 0 || amount > 1000000)
                throw new ArgumentException("Amount must between 0 and 1,000,000.");

            if (actType < 0 || actType > 1)
                throw new ArgumentException("ActType must be 0 or 1.");
            // <<<<< check input >>>>>

            // 檢查傳進來的body參數
            string bodyColumnSQL = "";
            string bodyValueSQL = "";
            if (!string.IsNullOrWhiteSpace(body))
            {
                bodyColumnSQL = ", Body";
                bodyValueSQL = " @body";
            }

            string connStr = DBHelper.GetConnectionString();
            string dbCommand =
                $@" UPDATE [Accounting]
                    SET
                       UserID      = @userID
                       ,Caption    = @caption
                       ,Amount     = @amount
                       ,ActType    = @actType
                       ,CreateDate = @createDate
                       {bodyColumnSQL} = {bodyValueSQL}
                    WHERE
                        ID = @id ";

            List<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@userID", userID));
            paramList.Add(new SqlParameter("@caption", caption));
            paramList.Add(new SqlParameter("@amount", amount));
            paramList.Add(new SqlParameter("@actType", actType));
            paramList.Add(new SqlParameter("@createDate", DateTime.Now));
            //
            if (!string.IsNullOrWhiteSpace(body))
                paramList.Add(new SqlParameter("@body", body));
            paramList.Add(new SqlParameter("@id", ID));
            //
            try
            {
                // connect db & execute
                //using (SqlConnection conn = new SqlConnection(connStr))
                //{
                //    using (SqlCommand comm = new SqlCommand(dbCommand, conn))
                //    {
                //        //comm.Parameters.AddWithValue("@userID", userID);
                //        //comm.Parameters.AddWithValue("@caption", caption);
                //        //comm.Parameters.AddWithValue("@amount", amount);
                //        //comm.Parameters.AddWithValue("@actType", actType);
                //        //comm.Parameters.AddWithValue("@createDate", DateTime.Now);
                //        //comm.Parameters.AddWithValue("@body", body);
                //        //comm.Parameters.AddWithValue("@id", ID);
                //        comm.Parameters.AddRange(paramList.ToArray());


                //        conn.Open();
                //        int effectRows = comm.ExecuteNonQuery();

                //    }
                //}
                int effectRows = DBHelper.ModifyData(connStr, dbCommand, paramList);

                if (effectRows == 1)
                    return true;
                else
                    return false;

            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
                return false;
            }
        }


        /// <summary> 刪除流水帳 </summary>
        /// <param name="ID"></param>
        public static void DeleteAccounting(int ID)
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString =
                @" DELETE [Accounting]
                    WHERE ID = @id ";

            List<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@id", ID));

            try
            {
                DBHelper.ModifyData(connectionString, dbCommandString, paramList);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }


    }
}
