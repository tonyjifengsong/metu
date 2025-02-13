#nullable disable

namespace METU.Admin.Model
{
    public partial class Admin
    {
        public int Id { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Remark { get; set; }
    }
}
