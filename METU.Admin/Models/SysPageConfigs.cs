using METU.INTERFACE.ICore;

#nullable disable

namespace METU.Admin.Model
{
    public partial class SysPageConfigs : IEntity
    {
        public string id { get; set; }
        public string SysPageid { get; set; }
        public string ControlName { get; set; }
        public string ControlCaption { get; set; }
        public string ControlType { get; set; }
        public string Placeholder { get; set; }
        public bool? Require { get; set; }
        public string Msg { get; set; }
        public string Explain { get; set; }
        public bool? Enabled { get; set; }
        public string SourceData { get; set; }
        public string InterfaceAddress { get; set; }
        public int? ControlOrder { get; set; }
        public bool? IsGroup { get; set; }
        public string GroupField { get; set; }
        public string UpdateUserid { get; set; }
        public string CreateUserid { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? CreateDate { get; set; }
        public short? IsEnabled { get; set; }
        public short? IsDeleted { get; set; }
        public string Cid { get; set; }
        public string Version { get; set; }
    }
}
