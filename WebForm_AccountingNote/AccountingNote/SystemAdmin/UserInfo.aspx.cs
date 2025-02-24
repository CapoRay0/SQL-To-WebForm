﻿using AccountingNote.Auth;
using AccountingNote.DBsource;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Ray0728am.SystemAdmin
{
    public partial class UserInfo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack) // 可能是按鈕跳回本頁，所以要判斷 postback
            {
                //if (this.Session["UserLoginInfo"] == null)
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

                // 帳號存在則印出來
                this.ltAccount.Text = CurrentUser.Account;
                this.ltName.Text = CurrentUser.Name;
                this.ltEmail.Text = CurrentUser.Email;

                //string account = this.Session["UserLoginInfo"] as string;

                //DataRow dr = UserInfoManager.GetUserInfoByAccount(account);

                //if (dr == null) // 如果帳號不存在，導至登入頁 (有可能被管理者砍帳號)
                //{
                //    this.Session["UserLoginInfo"] = null; // 才不會無限迴圈，導來導去
                //    Response.Redirect("/Login.aspx");
                //    return;
                //}

                //// 帳號存在則印出來
                //this.ltAccount.Text = dr["Account"].ToString();
                //this.ltName.Text = dr["Name"].ToString();
                //this.ltEmail.Text = dr["Email"].ToString();
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            //this.Session["UserLoginInfo"] = null;
            AuthManager.Logout(); // 登出，並導至登入頁
            Response.Redirect("/Login.aspx");
        }
    }
}