namespace AccountingNote.ORM.DBModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class UserRole
    {
        public Guid ID { get; set; }

        public Guid RoleID { get; set; }

        public Guid? UserInfoID { get; set; }

        public bool? IsGrant { get; set; }

        public DateTime EndDate { get; set; }

        public virtual Role Role { get; set; }

        public virtual UserInfo UserInfo { get; set; }
    }
}
