using METU.INTERFACE.ICore;

#nullable disable

namespace METU.Admin.Model
{
    public partial class SyspageDic : IEntity
    {
        public string SetKey { get; set; }
        public string SortName { get; set; }
        public string SortCode { get; set; }
        public string Rank { get; set; }
        public string Parentid { get; set; }
        public string id { get; set; }
        public string Pid { get; set; }
        public string Cid { get; set; }
        public string UpdateUserid { get; set; }
        public string CreateUserid { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? CreateDate { get; set; }
        public short? IsEnabled { get; set; }
        public short? IsDeleted { get; set; }
        public string Version { get; set; }
        public string Cname { get; set; }
        public string AppName { get; set; }
    }
}
