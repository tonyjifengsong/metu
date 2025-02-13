using METU.INTERFACE.ICore;

#nullable disable

namespace METU.Admin.Model
{
    public partial class SysPage : IEntity
    {
        public string id { get; set; }
        public long IdorderNo { get; set; }
        public string PageName { get; set; }
        public string ObjectType { get; set; }
        public string ObjectName { get; set; }
        public string PageNameEns { get; set; }
        public string Sourcedata { get; set; }
        public string InterfaceAddress { get; set; }
        public string Cid { get; set; }
        public string UpdateUserid { get; set; }
        public string CreateUserid { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? CreateDate { get; set; }
        public short? IsEnabled { get; set; }
        public short? IsDeleted { get; set; }
        public string Version { get; set; }
    }
}
