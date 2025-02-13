using METU.INTERFACE.ICore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace METU.MODEL
{
    /// <summary>
    /// 实体基类
    /// </summary>
    public class Entity :IEntity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public Entity()
        {
            TraceKey = "";
            id = Guid.NewGuid().ToString();
            Details = new List<object>();
            createdate = DateTime.Now;

        }
        /// <summary>
        ///主键
        /// </summary>
        [Key]
        public string id { get; set; }

        /// <summary>
        ///添加时间
        /// </summary>
        public DateTime createdate { get; set; }

        /// <summary>
        ///客户ID
        /// </summary>
        public string cid { get; set; }

        /// <summary>
        ///是否启用0－未启用，1－启用 默认值1－启用
        /// </summary>
        public int isenabled { get; set; }

        /// <summary>
        ///是否启用0－未删除，1－已删除 默认值0－未删除
        /// </summary>
        public int isdeleted { get; set; }

        /// <summary>
        ///更新时间
        /// </summary>
        public DateTime updatedate { get; set; }

        /// <summary>
        ///更新新人
        /// </summary>
        public string updateuserid { get; set; }

        /// <summary>
        ///创建人
        /// </summary>

        public string createuserid { get; set; }
        /// <summary>
        /// 从表一
        /// </summary>
        [NotMapped]
        public List<object> Details { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [NotMapped]


        public string TraceKey { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [NotMapped]
        public string DBConfig { get; set; }

    }
}
