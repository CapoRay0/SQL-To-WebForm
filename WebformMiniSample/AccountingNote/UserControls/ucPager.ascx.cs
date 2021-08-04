using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Ray0728am.UserControls
{
    public partial class ucPager : System.Web.UI.UserControl
    {
        /// <summary> 頁面 url </summary>
        public string Url { get; set; }
        /// <summary> 總筆數 </summary>
        public int TotalSize { get; set; }
        /// <summary> 頁面筆數 </summary>
        public int PageSize { get; set; }
        /// <summary> 目前頁數 </summary>
        public int CurrentPage { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            //this.Bind();
        }

        public void Bind()
        {
            int totalPages = this.GetTotalPages();

            this.ltpager.Text = $"共 {this.TotalSize} 筆，共 {totalPages} 頁，目前在第 {this.GetCurrentPage()} 頁~<br/>";

            for (var i = 1; i <= totalPages; i++) // 總頁數
            {
                this.ltpager.Text += $"<a href='{this.Url}?page={i}'>{i}</a>&nbsp";
            }
        }

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

        private int GetTotalPages()
        {
            int pagers = this.TotalSize / this.PageSize;

            if ((this.TotalSize % this.PageSize) > 0)
                pagers += 1;

            return pagers;
        }
        //AccountingList中
        //PageSize="10"
        //CurrentPage="1"
        //TotalSize="10"
    }
}