using AccountingNote.Auth;
using AccountingNote.DBsource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Ray0728am
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Session["UserLoginInfo"] != null)
            {
                this.plcLogin.Visible = false;
                Response.Redirect("/SystemAdmin/UserInfo.aspx"); // 已經登入過了，導頁到 UserInfo
            }
            else
            {
                this.plcLogin.Visible = true;
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            //string db_Account = "admin";
            //string db_Password = "12345";

            string inp_Account = this.txtAccount.Text; // inp 為 input
            string inp_PWD = this.txtPWD.Text;

            string msg;
            if (!AuthManager.TryLogin(inp_Account, inp_PWD, out msg))
            {
                this.ltlMsg.Text = msg;
                return;
            }

            Response.Redirect("/SystemAdmin/UserInfo.aspx");

            //// check empty
            //if (string.IsNullOrWhiteSpace(inp_Account) || string.IsNullOrWhiteSpace(inp_PWD))
            //{
            //    this.ltlMsg.Text = "Account / PWD is required.";
            //    return;
            //}

            //var dr = UserInfoManager.GetUserInfoByAccount(inp_Account); // 到DB查資料

            ////check null
            //if (dr == null)
            //{
            //    this.ltlMsg.Text = "Account doesn't exists."; // 查不到的話
            //    return;
            //}

            //// check account / pwd
            //if (string.Compare(dr["Account"].ToString(), inp_Account, true) == 0 && 
            //    string.Compare(dr["PWD"].ToString(), inp_PWD, false) == 0) // 因密碼要強制大小寫因此設定為false
            //{
            //    this.Session["UserLoginInfo"] = dr["Account"].ToString(); // 正確!!，跳頁至 UserInfo.aspx
            //    Response.Redirect("/SystemAdmin/UserInfo.aspx");
            //}
            //else
            //{
            //    this.ltlMsg.Text = "Login failed. Please check PWD.";
            //    return;
            //}

        }
    }
}