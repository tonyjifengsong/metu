using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Created by tony
/// </summary>
namespace System
{
    /// <summary>
    /// 数据库字段的用途。
    /// </summary>
    public enum DBFieldType
    {
        /// <summary>
        /// 未定义。
        /// </summary>
        None = 0x00,
        /// <summary>
        /// 用于主键。
        /// </summary>
        PrimaryKey = 0x01,
        /// <summary>
        /// 用于唯一键。
        /// </summary>
        UniqueKey = 0x02,
        /// <summary>
        /// 由系统控制该字段的值。
        /// </summary>
        BySystem = 0x04,
        /// <summary>
        /// 字段
        /// </summary>
        Field = 0x08,
        /// <summary>
        ///属性
        /// </summary>
        Property = 0x16,
        /// <summary>
        /// 扩展
        /// </summary>
        Extentions = 0x32,
        /// <summary>
        /// 其他值。
        /// </summary>
        Others = 0x64,
    }
}
