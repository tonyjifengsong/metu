using METU.INTERFACE.ICore;

#nullable disable

namespace METU.Admin.Model
{
    public partial class SysPageService : IEntity
    {
        public string id { get; set; }
        public string Domainid { get; set; }
        public string Servicename { get; set; }
        public string ServiceEvents { get; set; }
        public string Remarks { get; set; }
        public string ReturnType { get; set; }
        public DateTime? CreatedTime { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedTime { get; set; }
        public string ModifiedBy { get; set; }
        public string Cid { get; set; }
        public string UpdateUserid { get; set; }
        public string CreateUserid { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? CreateDate { get; set; }
        public short? IsEnabled { get; set; }
        public short? IsDeleted { get; set; }
        public string Version { get; set; }
        public string Methodname { get; set; }
    }
}
