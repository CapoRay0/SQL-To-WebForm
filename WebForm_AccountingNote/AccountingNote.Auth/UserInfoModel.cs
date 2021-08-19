using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingNote.Auth
{
    public class UserInfoModel
    {
        public string ID { get; set; }
        public string Account { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }


        public Guid/*?*/ UserGuid
        {
            get
            {
                if (Guid.TryParse(this.ID, out Guid tempGuid))
                {
                    return tempGuid;
                }
                else
                {
                    //return null;
                    return Guid.Empty;
                    // 因為 Guid 是實質型別，因此不能直接return null
                }
            }
        }
    }
}
