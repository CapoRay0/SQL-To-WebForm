using AccountingNote.DBsource;
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
            var list = UserInfoManager.GetUserInfoList();
            this.gvList.DataSource = list;
            this.gvList.DataBind();
        }
    }
}