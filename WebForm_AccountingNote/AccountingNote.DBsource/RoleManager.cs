using AccountingNote.ORM.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingNote.DBsource
{
    class RoleManager
    {
        /// <summary> 取得所有角色清單 </summary>
        /// <returns></returns>
        public static List<Role> GetRoleList()
        {
            try
            {
                using (ContextModel0906 context = new ContextModel0906())
                {
                    var query = context.Roles;
                    var list = query.ToList();
                    return list;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
                return null;
            }
        }

        /// <summary> 用名稱取得角色 </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public static Role GetRoleByName(string roleName)
        {
            try
            {
                using (ContextModel0906 context = new ContextModel0906())
                {
                    var query =
                        (from item in context.Roles
                         where item.RoleName == roleName
                         select item);

                    var obj = query.FirstOrDefault();
                    return obj;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
                return null;
            }
        }

        /// <summary> 用 id 取得角色 </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Role GetRole(Guid id)
        {
            try
            {
                using (ContextModel0906 context = new ContextModel0906())
                {
                    var query =
                        (from item in context.Roles
                         where item.ID == id
                         select item);

                    var obj = query.FirstOrDefault();
                    return obj;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
                return null;
            }
        }
    }
}
