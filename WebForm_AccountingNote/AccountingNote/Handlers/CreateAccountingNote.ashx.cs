using AccountingNote.DBsource;
using AccountingNote.ORM.DBModels;
using Ray0728am.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ray0728am.Handlers
{
    /// <summary>
    /// CreateAccountingNote 的摘要描述
    /// </summary>
    public class CreateAccountingNote : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            if(context.Request.HttpMethod != "POST")
            {
                this.ProcessError(context, "POST ONLY.");
                return;
            }

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
                //context.Response.StatusCode = 400;
                //context.Response.ContentType = "text/plain";
                //context.Response.Write("caption, amount, actType is required.");
                //context.Response.End();
                return;
            }

            // 轉型
            int tempAmount, tempActType;
            if (!int.TryParse(amountText, out tempAmount) ||
                !int.TryParse(actTypeText, out tempActType))
            {
                this.ProcessError(context, "amount, actType should be a integer.");
                //context.Response.StatusCode = 400;
                //context.Response.ContentType = "text/plain";
                //context.Response.Write("amount, actType should be a integer.");
                //context.Response.End();
                return;
            }

            //建立流水帳
            //---------------資料容器--------------//
            Accounting accounting = new Accounting()
            {
                UserID = id.ToGuid(),
                ActType = tempActType,
                Amount = tempAmount,
                Caption = caption,
                Body = body,
            };
            //-----------------------------//
            AccountingManager.CreateAccounting(accounting);

            context.Response.ContentType = "text/plain";
            context.Response.Write("ok");
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