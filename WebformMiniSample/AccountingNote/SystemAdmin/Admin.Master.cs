using AccountingNote.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Ray0728am.SystemAdmin
{
    public partial class Admin : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!AuthManager.IsLogined()) // Session存不存在，如果尚未登入，導至登入頁
            {
                Response.Redirect("/Login.aspx");
                return;
            }

            var CurrentUser = AuthManager.GetCurrentUser();

            if (CurrentUser == null) // 如果帳號不存在，導至登入頁 (有可能被管理者砍帳號)
            {
                //this.Session["UserLoginInfo"] = null; // 才不會無限迴圈，導來導去
                Response.Redirect("/Login.aspx");
                return;
            }
        }
    }
}