using AccountingNote.Auth;
using AccountingNote.DBsource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Ray0728am.SystemAdmin
{
    public partial class AccountingDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // check is logined
            //if (this.Session["UserLoginInfo"] == null)
            if (!AuthManager.IsLogined())
            {
                Response.Redirect("/Login.aspx");
                return;
            }

            var CurrentUser = AuthManager.GetCurrentUser();

            if (CurrentUser == null) // 如果帳號不存在，導至登入頁 (有可能被管理者砍帳號)
            {
                this.Session["UserLoginInfo"] = null; // 才不會無限迴圈，導來導去
                Response.Redirect("/Login.aspx");
                return;
            }
            //string account = this.Session["UserLoginInfo"] as string;
            //var drUserInfo = UserInfoManager.GetUserInfoByAccount(account);

            //if (drUserInfo == null)
            //{
            //    Response.Redirect("/Login.aspx");
            //    return;
            //}

            if (!this.IsPostBack)
            {
                // Check is create mode or edit mode
                if (this.Request.QueryString["ID"] == null) //參數ID沒有帶到本頁，ID=null >> 新增模式
                {
                    this.btnDelete.Visible = false;
                }
                else // 編輯模式
                {
                    this.btnDelete.Visible = true;

                    string idText = this.Request.QueryString["ID"]; // 讀取ID
                    int id;
                    if (int.TryParse(idText, out id)) // 檢查是否能轉型成數字
                    {
                        var drAccounting = AccountingManager.GetAccounting(id, CurrentUser.ID);

                        if (drAccounting == null)
                        {
                            this.ltmsg.Text = "Data doesn't exist";
                            this.btnSave.Visible = false;
                            this.btnDelete.Visible = false;
                        }
                        else // 把原本的資料帶入編輯頁面以供使用者編輯!!
                        {
                            this.ddlActType.SelectedValue = drAccounting["ActType"].ToString();
                            this.txtAmount.Text = drAccounting["Amount"].ToString();
                            this.txtCaption.Text = drAccounting["Caption"].ToString();
                            this.txtDesc.Text = drAccounting["Body"].ToString();
                        }
                    }
                    else
                    {
                        this.ltmsg.Text = "ID is required.";
                        this.btnSave.Visible = false;
                        this.btnDelete.Visible = false;
                    }
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            List<string> msgList = new List<string>();
            if (!this.CheckInput(out msgList))
            {
                this.ltmsg.Text = string.Join("<br/>", msgList);
                return;
            }

            UserInfoModel currentUser = AuthManager.GetCurrentUser();
            if (currentUser == null)
            {
                Response.Redirect("/Login.aspx");
                return;
            }
            //string account = this.Session["UserLoginInfo"] as string;
            //var dr = UserInfoManager.GetUserInfoByAccount(account);

            //if (dr == null)
            //{
            //    Response.Redirect("/Login.aspx");
            //    return;
            //}

            var CurrentUser = AuthManager.GetCurrentUser();
            // Input txt
            string userID = CurrentUser.ID;
            string actTypeText = this.ddlActType.SelectedValue;
            string amountText = this.txtAmount.Text;
            string caption = this.txtCaption.Text;
            string body = this.txtDesc.Text;

            int amount = Convert.ToInt32(amountText);
            int actType = Convert.ToInt32(actTypeText);

            // Check is create mode or edit mode
            string idText = this.Request.QueryString["ID"];
            if (string.IsNullOrWhiteSpace(idText))
            {
                // Execute 'Insert into db'
                AccountingManager.CreateAccounting(userID, caption, amount, actType, body);
            }
            else
            {
                int id;
                if (int.TryParse(idText, out id))
                {
                    // Execute 'update db'
                    AccountingManager.UpdateAccounting(id, userID, caption, amount, actType, body);
                }
            }

            Response.Redirect("/SystemAdmin/AccountingList.aspx");
        }

        private bool CheckInput(out List<string> errorMsgList)
        {
            List<string> msgList = new List<string>();

            // Type 必填
            if (this.ddlActType.SelectedValue != "0" && this.ddlActType.SelectedValue != "1")
            {
                msgList.Add("Type must be 0 or 1.");
                errorMsgList = msgList;
                return false;
            }

            // Amount 必填
            if (string.IsNullOrWhiteSpace(this.txtAmount.Text))
            {
                msgList.Add("Amount is required");
            }
            else
            {
                int tempInt;
                if (!int.TryParse(this.txtAmount.Text, out tempInt))
                {
                    msgList.Add("Amount must be a number.");
                }
                if (tempInt < 0 || tempInt > 1000000)
                {
                    msgList.Add("Amount must between 0 and 1,000,000. ");
                }
            }

            errorMsgList = msgList;

            if (msgList.Count == 0)
                return true;
            else
                return false;
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string idText = this.Request.QueryString["ID"];

            if (string.IsNullOrWhiteSpace(idText))
                return;

            int id;
            if (int.TryParse(idText, out id))
            {
                // Execute 'delete db'
                AccountingManager.DeleteAccounting(id);
            }
            Response.Redirect("/SystemAdmin/AccountingList.aspx");
        }
    }
}