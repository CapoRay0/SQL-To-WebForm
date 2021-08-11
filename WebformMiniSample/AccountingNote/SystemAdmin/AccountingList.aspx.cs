using AccountingNote.Auth;
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
            //var dr = UserInfoManager.GetUserInfoByAccount(account);

            //if (dr == null)
            //{
            //    Response.Redirect("/Login.aspx");
            //    return;
            //}


            // read accounting data
            var dt = AccountingManager.GetAccountingList(CurrentUser.ID);

            if (dt.Rows.Count > 0) // 如果DB有資料
            {
                //int totalPages = this.GetTotalPages(dt); // 取得總頁數
                var dtPaged = this.GetPagedDataTable(dt);

                //0804
                this.ucPager2.TotalSize = dt.Rows.Count;
                this.ucPager2.Bind();
                //0804

                this.gvAccountingList.DataSource = dtPaged; // 資料繫結
                this.gvAccountingList.DataBind();


                // 0804砍掉 this.ucPager.TotalSize = dt.Rows.Count; //總頁數給dt筆數就好
                // 0804砍掉 this.ucPager.Bind(); // 可以利用 Method 來跟外界(這裡)溝通
                //// 0802--------------------------------------------------------
                //var pages = (dt.Rows.Count / 10); // 計算共幾筆、共幾頁
                //if (dt.Rows.Count % 10 > 0)
                //    pages += 1;

                //this.ltpager.Text = $"共 {dt.Rows.Count} 筆，共 {pages} 頁，目前在第 {this.GetCurrentPage()} 頁<br/>";
                ////--------------------------------------------------------

                //for (var i = 1; i <= totalPages; i++) // 總頁數
                //{
                //    this.ltpager.Text += $"<a href='AccountingList.aspx?page={i}'>{i}</a>&nbsp";
                //}
            }
            else
            {
                this.gvAccountingList.Visible = false;
                this.plcNoData.Visible = true;
            }
        }

        //private int GetTotalPages(DataTable dt) // 0802
        //{
        //    int pagers = dt.Rows.Count / 10;

        //    if ((dt.Rows.Count % 10) > 0)
        //        pagers += 1;

        //    return pagers;
        //    // 1 => 0
        //    // 9 => 0
        //    //10 => 1
        //    //15 => 1
        //}

        private int GetCurrentPage()    
        {
            string pageText = Request.QueryString["Page"];

            if (string.IsNullOrWhiteSpace(pageText)) // 空的時候，給第一頁
                return 1;

            int intPage;
            if (!int.TryParse(pageText, out intPage)) // (錯誤) 數字轉換失敗，給第一頁
                return 1;

            if (intPage <= 0) // (錯誤) 0或以下，也給第一頁
                return 1;

            return intPage;
        }
        private DataTable GetPagedDataTable(DataTable dt)
        {
            DataTable dtPaged = dt.Clone(); //只拿結構
            //dt.Copy(); // 除了結構還拿資料，但0筆的話會出錯

            //0804
            int pageSize = this.ucPager2.PageSize;
            int startIndex = (this.GetCurrentPage() - 1) * pageSize;
            int endIndex = this.GetCurrentPage() * pageSize;
            //0804

            //foreach (DataRow dr in dt.Rows)
            //for (var i = 0; i < dt.Rows.Count; i++)

            //0804砍掉 int startIndex = (this.GetCurrentPage() - 1) * 10;
            //0804砍掉 int endIndex = this.GetCurrentPage() * 10;

            if (endIndex > dt.Rows.Count)
                endIndex = dt.Rows.Count;

            for (var i = startIndex; i < endIndex; i++)
            {
                DataRow dr = dt.Rows[i];
                var drNew = dtPaged.NewRow();

                foreach (DataColumn dc in dt.Columns)
                {
                    drNew[dc.ColumnName] = dr[dc];
                }

                //foreach (DataColumn dc in dtPaged.Columns)
                //    drNew[dc.ColumnName] = dr[dc.ColumnName];

                dtPaged.Rows.Add(drNew);
            }
            return dtPaged;
            // dt = DataTable: 資料表
            // dr = DataRows: 資料列
            // dc = DataColumns: 資料內容
        }



        protected void btnCreate_Click(object sender, EventArgs e)
        {
            Response.Redirect("/SystemAdmin/AccountingDetail.aspx");
        }

        protected void gvAccountingList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            var row = e.Row;

            if (row.RowType == DataControlRowType.DataRow)
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