using AccountingNote.DBsource;
using Ray0728am.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Ray0728am.Handlers
{
    /// <summary>
    /// AccountingNoteList 的摘要描述
    /// </summary>
    public class AccountingNoteList : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            // localhost:65087/Handlers/AccountingNoteList.ashx?Account=Ray

            // 取得傳進來是哪個使用者 透過Get來取
            string account = context.Request.QueryString["Account"];

            if (string.IsNullOrWhiteSpace(account))
            {
                context.Response.StatusCode = 404;
                context.Response.End();
                return;
            }

            var dr = UserInfoManager.GetUserInfoByAccount(account);

            if (dr == null)
            {
                context.Response.StatusCode = 404;
                context.Response.End();
                return;
            }

            // 查詢這個使用者所有的流水帳
            string userID = dr["ID"].ToString();
            DataTable dataTable = AccountingManager.GetAccountingList(userID); // 用使用者ID取得此使用者的所有流水帳

            List<AccountingNoteViewModel> list = new List<AccountingNoteViewModel>();
            foreach(DataRow drAccounting in dataTable.Rows)
            {
                AccountingNoteViewModel model = new AccountingNoteViewModel()
                {
                    ID = drAccounting["ID"].ToString(),
                    Caption = drAccounting["Caption"].ToString(),
                    Amount = drAccounting.Field<int>("Amount"),
                    ActType = (drAccounting.Field<int>("ActType") == 0) ? "支出" : "收入",
                    CreateDate = drAccounting.Field<DateTime>("CreateDate").ToString("yyyy-MM-dd")
                };
                list.Add(model);
            }
            string jsonText = Newtonsoft.Json.JsonConvert.SerializeObject(dataTable); // 序列化

            context.Response.ContentType = "application/json";
            context.Response.Write(jsonText);

            //context.Response.Write("Hello World");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}