using AccountingNote.DBsource;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AccountingNote.Auth
{
    /// <summary> 負責處理登入的元件 </summary>
    public class AuthManager
    {
        /// <summary> 檢查目前是否登入 </summary>
        /// <returns></returns>
        public static bool IsLogined()
        {
            if (HttpContext.Current.Session["UserLoginInfo"] == null)
                return false;
            else
                return true;
            //{
            //    Response.Redirect("/Login.aspx");
            //    return;
            //}
        }

        /// <summary> 取得已登入的使用者資訊 (如果沒有登入就回傳 null) </summary>
        /// <returns></returns>
        public static UserInfoModel GetCurrentUser()
        {
            string account = HttpContext.Current.Session["UserLoginInfo"] as string;

            if (account == null)
                return null;

            //DataRow dr = UserInfoManager.GetUserInfoByAccount(account);

            ////return dr;

            //if (dr == null)
            //{
            //    HttpContext.Current.Session["UserLoginInfo"] = null; // 無限迴圈問題
            //    return null;
            //}
            //UserInfoModel model = new UserInfoModel();
            //model.ID = dr["ID"].ToString();
            //model.Account = dr["Account"].ToString();
            //model.Name = dr["Name"].ToString();
            //model.Email = dr["Email"].ToString();

            var userInfo = UserInfoManager.GetUserInfoByAccount_ORM(account);

            if (userInfo == null)
            {
                HttpContext.Current.Session["UserLoginInfo"] = null; // 無限迴圈問題
                return null;
            }
            UserInfoModel model = new UserInfoModel();
            model.ID = userInfo.ID;
            model.Account = userInfo.Account;
            model.Name = userInfo.Name;
            model.Email = userInfo.Email;

            return model;
        }

        /// <summary> 登出 </summary>
        public static void Logout()
        {
            HttpContext.Current.Session["UserLoginInfo"] = null;
        }

        /// <summary> 嘗試登入 </summary>
        /// <param name="account"></param>
        /// <param name="pwd"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public static bool TryLogin(string account, string pwd, out string errorMsg)
        {
            // check empty
            if (string.IsNullOrWhiteSpace(account) || string.IsNullOrWhiteSpace(pwd))
            {
                errorMsg = "Account / PWD is required.";
                return false;
            }

            // read db and check
            var userInfo = UserInfoManager.GetUserInfoByAccount_ORM(account);

            //check null
            if (userInfo == null)
            {
                errorMsg = $"Account: {account} doesn't exists."; // 查不到的話
                return false;
            }

            // check account / pwd
            if (string.Compare(userInfo.Account, account, true) == 0 &&
                string.Compare(userInfo.PWD, pwd, false) == 0) // 因密碼要強制大小寫因此設定為false
            {
                HttpContext.Current.Session["UserLoginInfo"] = userInfo.Account; // 正確!!，跳頁至 UserInfo.aspx
                //Response.Redirect("/SystemAdmin/UserInfo.aspx");
                errorMsg = string.Empty;
                return true;
            }
            else
            {
                //this.ltlMsg.Text = "Login failed. Please check PWD.";
                errorMsg = "Login failed. Please check PWD.";
                return false;
            }

        }

        /// <summary> 儲存角色對應 </summary>
        /// <param name="userID"></param>
        /// <param name="roleIDs"></param>
        public static void MapUserAndRole(Guid userID, Guid[] roleIDs)
        {
            RoleManager.MappingUserAndRole(userID, roleIDs);
        }

        /// <summary> 是否被授權 </summary>
        /// <param name="userID"></param>
        /// <param name="roleIDs"></param>
        /// <returns></returns>
        public static bool IsGrant(Guid userID, Guid[] roleIDs)
        {
            return RoleManager.IsGrant(userID, roleIDs);
        }

        /// <summary> 是否被授權 </summary>
        /// <param name="userID"></param>
        /// <param name="roleIDs"></param>
        /// <returns></returns>
        public static bool IsGrant(Guid userID, string[] roleNames)
        {
            List<Guid> roleIDs = new List<Guid>();

            foreach(string roleName in roleNames)
            {
                var role = RoleManager.GetRoleByName(roleName);
                if (role == null)
                    continue;
                roleIDs.Add(role.ID);
            }

            return RoleManager.IsGrant(userID, roleIDs.ToArray());
        }

        /// <summary> 嘗試登入 </summary>
        /// <param name="account"></param>
        /// <param name="pwd"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        //public static bool TryLogin(string account, string pwd, out string errorMsg)
        //{
        //    // check empty
        //    if (string.IsNullOrWhiteSpace(account) || string.IsNullOrWhiteSpace(pwd))
        //    {
        //        errorMsg = "Account / PWD is required.";
        //        return false;
        //    }

        //    // read db and check
        //    var dr = UserInfoManager.GetUserInfoByAccount(account);

        //    //check null
        //    if (dr == null)
        //    {
        //        errorMsg = $"Account: {account} doesn't exists."; // 查不到的話
        //        return false;
        //    }

        //    // check account / pwd
        //    if (string.Compare(dr["Account"].ToString(), account, true) == 0 &&
        //        string.Compare(dr["PWD"].ToString(), pwd, false) == 0) // 因密碼要強制大小寫因此設定為false
        //    {
        //        HttpContext.Current.Session["UserLoginInfo"] = dr["Account"].ToString(); // 正確!!，跳頁至 UserInfo.aspx
        //        //Response.Redirect("/SystemAdmin/UserInfo.aspx");
        //        errorMsg = string.Empty;
        //        return true;
        //    }
        //    else
        //    {
        //        //this.ltlMsg.Text = "Login failed. Please check PWD.";
        //        errorMsg = "Login failed. Please check PWD.";
        //        return false;
        //    }

        //}
    }
}
