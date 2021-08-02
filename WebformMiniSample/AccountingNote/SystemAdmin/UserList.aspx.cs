using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Ray0728am.SystemAdmin
{
    public partial class UserList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!AccountingNote.Auth.AuthManager.IsLogined())
            {
                return;
            }

            var cUser = AccountingNote.Auth.AuthManager.GetCurrentUser();

            this.GridView1.DataSource = AccountingNote.DBsource.AccountingManager.GetAccountingList(cUser.ID);
            this.GridView1.DataBind();
        }
    }
}