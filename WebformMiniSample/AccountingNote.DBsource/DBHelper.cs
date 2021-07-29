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
    class DBHelper
    {
        // 連線字串，抽到共用方法
        public static string GetConnectionString()
        {
            string val = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            return val;
        }

        // 讀取清單的Method，抽到共用方法中
        public static DataTable ReadDataTable(string connStr, string dbCommand, List<SqlParameter> list)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand comm = new SqlCommand(dbCommand, conn))
                {
                    //comm.Parameters.AddWithValue("@userID", userID);
                    comm.Parameters.AddRange(list.ToArray());


                    conn.Open();
                    var reader = comm.ExecuteReader();

                    DataTable dt = new DataTable();
                    dt.Load(reader);

                    return dt;
                }
            }
        }


        public static DataRow ReadDataRow(string connStr, string dbCommand, List<SqlParameter> list) // AccountingManager的
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand comm = new SqlCommand(dbCommand, conn))
                {
                    comm.Parameters.AddRange(list.ToArray());


                    conn.Open();
                    var reader = comm.ExecuteReader();

                    DataTable dt = new DataTable();
                    dt.Load(reader);

                    if (dt.Rows.Count == 0)
                        return null;

                    DataRow dr = dt.Rows[0]; // 舊的搬過來
                    return dt.Rows[0];

                }
            }
        }

        //public static DataRow ReadDataRowU(string connectionString, string dbCommandString, List<SqlParameter> list) // UserInfoManager的
        //{
        //    using (SqlConnection connection = new SqlConnection(connectionString)) // 開啟連線
        //    {
        //        using (SqlCommand command = new SqlCommand(dbCommandString, connection)) // 建立物件
        //        {

        //            command.Parameters.AddRange(list.ToArray()); // 參數化查詢


        //            connection.Open();
        //            SqlDataReader reader = command.ExecuteReader();

        //            DataTable dt = new DataTable();
        //            dt.Load(reader);
        //            reader.Close();

        //            if (dt.Rows.Count == 0) // 如果查不到id資料的話回傳null
        //                return null;

        //            DataRow dr = dt.Rows[0]; // id為主鍵且不允許重複，因此查第一筆即可
        //            return dr;

        //        }
        //    }
        //}
    }
}
