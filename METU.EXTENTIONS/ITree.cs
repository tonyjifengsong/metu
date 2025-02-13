using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    /// <summary>
    /// 树形结构接口
    /// </summary>
    public interface ITree<T>
    {
        /// <summary>
        /// 主键值
        /// </summary>
        string id { get; set; }
        /// <summary>
        /// 上级菜单ID
        /// </summary>
        string parentid { get; set; }
        /// <summary>
        /// 子菜单
        /// </summary>
        List<T> sublist { get; set; }
    }
}
