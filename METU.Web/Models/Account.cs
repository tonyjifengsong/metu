using METU.INTERFACE.ICore;

#nullable disable

namespace METU.Admin.Model
{
    public partial class Account: IEntity
    {
        public string id { get; set; }
        public int Type { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime LoginTime { get; set; }
        public string LoginIp { get; set; }
        public short Status { get; set; }
        public bool IsLock { get; set; }
        public DateTime CreatedTime { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedTime { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ClosedTime { get; set; }
        public string ClosedBy { get; set; }
        public bool Deleted { get; set; }
        public DateTime DeletedTime { get; set; }
        public string DeletedBy { get; set; }
        public string cid { get; set; }
        public string Pid { get; set; }
        public string Domainid { get; set; }
        public int isdeleted { get; set; }
        public int isenabled { get; set; }
        public DateTime createdate { get; set; }
        public DateTime updatedate { get; set; }
        public string CreateUserid { get; set; }
        public string UpdateUserid { get; set; }
      
    }
}
