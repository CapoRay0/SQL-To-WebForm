using AccountingNote.DBsource;
using Ray0728am.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using AccountingNote.ORM.DBModels;
using Ray0728am.Extensions;

namespace Ray0728am.Handlers
{
    /// <summary>
    /// AccountingNoteHandler 的摘要描述
    /// </summary>
    public class AccountingNoteHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string actionName = context.Request.QueryString["ActionName"];

            if (string.IsNullOrEmpty(actionName))
            {
                context.Response.StatusCode = 400;
                context.Response.ContentType = "text/plain";
                context.Response.Write("ActionName is required");
                context.Response.End();
            }

            if (actionName == "create")
            {
                string caption = context.Request.Form["Caption"];
                string amountText = context.Request.Form["Amount"];
                string actTypeText = context.Request.Form["ActType"];
                string body = context.Request.Form["Body"];

                // ID of Ray
                string id = "7E85BB22-C671-4150-8B97-1A6756ACFD72";

                // 必填檢查
                if (string.IsNullOrWhiteSpace(caption) ||
                    string.IsNullOrWhiteSpace(amountText) ||
                    string.IsNullOrWhiteSpace(actTypeText))
                {
                    this.ProcessError(context, "caption, amount, actType is required.");
                    return;
                }

                // 轉型
                int tempAmount, tempActType;
                if (!int.TryParse(amountText, out tempAmount) ||
                    !int.TryParse(actTypeText, out tempActType))
                {
                    this.ProcessError(context, "amount, actType should be a integer.");
                    return;
                }

                try
                {
                    // 0819
                    //--------EF建立流水帳------------------//
                    Accounting accounting = new Accounting()
                    {
                        UserID = id.ToGuid(),
                        Caption = caption,
                        Body = body,
                        Amount = tempAmount,
                        ActType = tempActType
                    };
                    AccountingManager.CreateAccounting(accounting);
                    //--------EF建立流水帳------------------//
                    
                    //AccountingManager.CreateAccounting(id, caption, tempAmount, tempActType, body);
                    context.Response.ContentType = "text/plain";
                    context.Response.Write("Create Succeed");
                }
                catch /*(Exception ex)*/
                {
                    context.Response.StatusCode = 503;
                    context.Response.ContentType = "text/plain";
                    context.Response.Write("Error");
                }

            }
            else if (actionName == "update")
            {
                string caption = context.Request.Form["Caption"];
                string amountText = context.Request.Form["Amount"];
                string actTypeText = context.Request.Form["ActType"];
                string body = context.Request.Form["Body"];
                string idText = context.Request.Form["ID"];

                // ID of Ray
                string UserId = "7E85BB22-C671-4150-8B97-1A6756ACFD72";

                // 必填檢查
                if (string.IsNullOrWhiteSpace(caption) ||
                    string.IsNullOrWhiteSpace(amountText) ||
                    string.IsNullOrWhiteSpace(actTypeText) ||
                    string.IsNullOrWhiteSpace(idText))
                {
                    this.ProcessError(context, "caption, amount, actType and ID is required.");
                    return;
                }

                // 轉型
                int tempAmount, tempActType, tempID; ;
                if (!int.TryParse(amountText, out tempAmount) ||
                    !int.TryParse(actTypeText, out tempActType) ||
                    !int.TryParse(idText, out tempID))
                {
                    this.ProcessError(context, "amount, actType and ID should be a integer.");
                    return;
                }

                try
                {
                    //--------EF建立流水帳------------------//
                    Accounting accounting = new Accounting()
                    {
                        UserID = UserId.ToGuid(),
                        Caption = caption,
                        Body = body,
                        Amount = tempAmount,
                        ActType = tempActType
                    };
                    AccountingManager.UpdateAccounting(accounting);
                    //--------EF建立流水帳------------------//

                    //建立流水帳
                    //AccountingManager.UpdateAccounting(tempID, UserId, caption, tempAmount, tempActType, body);
                    context.Response.ContentType = "text/plain";
                    context.Response.Write("Update Succeed");
                }
                catch/* (Exception ex)*/
                {
                    context.Response.StatusCode = 503;
                    context.Response.ContentType = "text/plain";
                    context.Response.Write("Error");
                }

            }
            else if (actionName == "delete")
            {
                string idText = context.Request.Form["ID"];
                int id;
                int.TryParse(idText, out id);

                //AccountingManager.DeleteAccounting(id);
                AccountingManager.DeleteAccounting_ORM(id);
                context.Response.ContentType = "text/plain";
                context.Response.Write("Delete Succeed");
            }
            else if (actionName == "list") 
            {
                Guid userGUID = new Guid("7E85BB22-C671-4150-8B97-1A6756ACFD72");
                //string userID = "7E85BB22-C671-4150-8B97-1A6756ACFD72";

                //DataTable dataTable = AccountingManager.GetAccountingList(userID); // 用使用者ID取得此使用者的所有流水帳
                List<Accounting> sourceList = AccountingManager.GetAccountingList(userGUID);
                List<AccountingNoteViewModel> list = sourceList.Select(obj => new AccountingNoteViewModel()
                {
                    ID = obj.ID.ToString(),
                    Caption = obj.Caption,
                    Amount = obj.Amount,
                    ActType = (obj.ActType == 0) ? "支出" : "收入",
                    CreateDate = obj.CreateDate.ToString("yyyy-MM-dd")
                }).ToList();
                string jsonText = Newtonsoft.Json.JsonConvert.SerializeObject(list);

                //List<AccountingNoteViewModel> list = new List<AccountingNoteViewModel>();
                //foreach (DataRow drAccounting in dataTable.Rows)
                //{
                //    AccountingNoteViewModel model = new AccountingNoteViewModel()
                //    {
                //        ID = drAccounting["ID"].ToString(),
                //        Caption = drAccounting["Caption"].ToString(),
                //        Amount = drAccounting.Field<int>("Amount"),
                //        ActType = (drAccounting.Field<int>("ActType") == 0) ? "支出" : "收入",
                //        CreateDate = drAccounting.Field<DateTime>("CreateDate").ToString("yyyy-MM-dd")
                //    };
                //    list.Add(model);
                //}
                //string jsonText = Newtonsoft.Json.JsonConvert.SerializeObject(dataTable); // 序列化

                context.Response.ContentType = "application/json";
                context.Response.Write(jsonText);
            }
            else if (actionName == "query")
            {
                string idText = context.Request.Form["ID"];
                int id;
                int.TryParse(idText, out id);
                //string userID = "7E85BB22-C671-4150-8B97-1A6756ACFD72";
                Guid userGUID = new Guid("7E85BB22-C671-4150-8B97-1A6756ACFD72");

                //var drAccounting = AccountingManager.GetAccounting(id, userID);
                var accounting = AccountingManager.GetAccounting(id, userGUID);

                if(accounting == null)
                {
                    context.Response.StatusCode = 404;
                    context.Response.ContentType = "text/plain";
                    context.Response.Write("No data:" + idText);
                    context.Response.End();
                    return;
                }

                AccountingNoteViewModel model = new AccountingNoteViewModel()
                {
                    //ID = drAccounting["ID"].ToString(),
                    //Caption = drAccounting["Caption"].ToString(),
                    //Body = drAccounting["Body"].ToString(),
                    //CreateDate = drAccounting.Field<DateTime>("CreateDate").ToString("yyyy-M-dd"),
                    //ActType = drAccounting.Field<int>("ActType").ToString(),
                    //Amount = drAccounting.Field<int>("Amount")
                    ID = accounting.ID.ToString(),
                    Caption = accounting.Caption,
                    Body = accounting.Body,
                    Amount = accounting.Amount,
                    ActType = (accounting.ActType == 0) ? "支出" : "收入",
                    CreateDate = accounting.CreateDate.ToString("yyyy-MM-dd")
                };

                string jsonText = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                context.Response.ContentType = "application/json";
                context.Response.Write(jsonText);
            }

        }
        private void ProcessError(HttpContext context, string msg)
        {
            context.Response.StatusCode = 400;
            context.Response.ContentType = "text/plain";
            context.Response.Write(msg);
            context.Response.End();
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