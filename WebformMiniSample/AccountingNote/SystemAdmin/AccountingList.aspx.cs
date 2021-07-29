using AccountingNote.DBsource;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Ray0728am.SystemAdmin
{
    public partial class AccountingList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // check is logined
            if (this.Session["UserLoginInfo"] == null)
            {
                Response.Redirect("/Login.aspx");
                return;
            }

            string account = this.Session["UserLoginInfo"] as string;
            var dr = UserInfoManager.GetUserInfoByAccount(account);

            if (dr == null)
            {
                Response.Redirect("/Login.aspx");
                return;
            }

            // read accounting data
            var dt = AccountingManager.GetAccountingList(dr["ID"].ToString());

            if (dt.Rows.Count > 0) // 如果DB沒資料
            {
                this.gvAccountingList.DataSource = dt; // 資料繫結
                this.gvAccountingList.DataBind();
            }
            else
            {
                this.gvAccountingList.Visible = false;
                this.plcNoData.Visible = true;
            }

        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            Response.Redirect("/SystemAdmin/AccountingDetail.aspx");
        }

        protected void gvAccountingList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            var row = e.Row;

            if(row.RowType == DataControlRowType.DataRow)
            {
                //Literal ltl = row.FindControl("ltActType") as Literal;
                Label lbl = row.FindControl("lblActType") as Label;
                //ltl.Text = "OK";

                var dr = row.DataItem as DataRowView;
                int actType = dr.Row.Field<int>("ActType");
                
                switch (actType)
                {
                    case 0:
                        lbl.Text = "支出";
                        if (dr.Row.Field<int>("Amount") > 1500)
                        {
                            lbl.ForeColor = Color.Red;
                        }
                        break;
                    case 1:
                        lbl.Text = "收入";
                        if (dr.Row.Field<int>("Amount") > 1500)
                        {
                            lbl.ForeColor = Color.Blue;
                        }
                        break;
                }

                //if (actType == 0)
                //{
                //    //ltl.Text = "支出";
                //    lbl.Text = "支出";
                //}
                //else
                //{
                //    //ltl.Text = "收入";
                //    lbl.Text = "收入";
                //}
                //if (dr.Row.Field<int>("Amount") > 1500)
                //{
                //    lbl.ForeColor = Color.Red;
                //}
            }

        }
    }
}