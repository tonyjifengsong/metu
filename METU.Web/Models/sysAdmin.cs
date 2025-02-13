using METU.INTERFACE.ICore;

#nullable disable

namespace METU.Admin.Model
{
    public partial class SysAdmin : IEntity
    {
       
        public string id { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Remark { get; set; }
    }
}
