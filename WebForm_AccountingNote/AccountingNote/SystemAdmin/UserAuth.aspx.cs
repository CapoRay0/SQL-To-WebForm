using AccountingNote.DBsource;
using AccountingNote.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Ray0728am.SystemAdmin
{
    public partial class UserAuth : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                string userIDText = Request.QueryString["ID"];

                if (string.IsNullOrWhiteSpace(userIDText)) return;

                Guid userID = Guid.Parse(userIDText);
                var mUser = UserInfoManager.GetUserInfo(userID);

                if (mUser == null)
                {
                    Response.Redirect("default.aspx");
                    return;
                }

                this.ltAccount.Text = mUser.Account;

                this.ckbRoleList.DataSource = RoleManager.GetRoleList();
                this.ckbRoleList.DataBind();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            List<string> willSaveIDList = new List<string>();

            foreach(ListItem li in this.ckbRoleList.Items)
            {
                if (li.Selected)
                    willSaveIDList.Add(li.Value);
            }

            if (willSaveIDList.Count == 0)
                return;

            var roleList = willSaveIDList.Select(obj => Guid.Parse(obj)).ToArray();

            string userIDText = Request.QueryString["ID"];
            Guid userID = Guid.Parse(userIDText);

            AuthManager.MapUserAndRole(userID, roleList);
        }
    }
}