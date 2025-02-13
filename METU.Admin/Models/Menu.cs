using METU.INTERFACE.ICore;

#nullable disable

namespace METU.Admin.Model
{
    public partial class Menu : IEntity
    {
        public string id { get; set; }
        public string Domainid { get; set; }
        public string ModuleCode { get; set; }
        public short? Type { get; set; }
        public string Parentid { get; set; }
        public string Name { get; set; }
        public string RouteName { get; set; }
        public string RouteParams { get; set; }
        public string RouteQuery { get; set; }
        public string Icon { get; set; }
        public string IconColor { get; set; }
        public string Url { get; set; }
        public int? Level { get; set; }
        public bool? Show { get; set; }
        public int? Sort { get; set; }
        public short? Target { get; set; }
        public string DialogWidth { get; set; }
        public string DialogHeight { get; set; }
        public bool? DialogFullscreen { get; set; }
        public string Remarks { get; set; }
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
    }
}
