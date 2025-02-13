namespace METU.Core
{
    /// <summary>
    /// 配置对象
    /// </summary>
    public  class DBConfigString
    {
        /// <summary>
        /// 命名空间名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 数据库类型
        /// </summary>
        public int Dialect { get; set; }
        /// <summary>
        /// 数据库名
        /// </summary>
        public string Database { get; set; }
        /// <summary>
        /// 数据库链接字符串
        /// </summary>
        public string ConnString { get; set; }

    }
}
