using METU.CACHES;
using METU.CONFIGS;
using METU.MODEL;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace METU.MSSQL
{ /// <summary>
  /// 任意系统接口调用方法
  /// </summary>

    public partial class metucore
    {
        CoreBLL bll;
        public metucore()
        {

            bll = new CoreBLL();
        }
        /// <summary>
        /// 执行业通用务逻辑 [Route("{appname}/{servicename}service/{methodname}api")]
        /// </summary>
        /// <param name="id">企业用户名 （英文字符或手机号，唯一不重复）</param>
        /// <param name="appname">企业用户下的应用分类（ERP、CRM、WMS、MES、MTS、TMS、SSO、GPS等）</param>
        /// <param name="servicename">应该下的服务分类名</param>
        /// <param name="methodname">服务中具体业务名</param>
        /// <param name="dic">传入参数</param>
        /// <param name="version"></param>
        /// <returns></returns>

        public dynamic DynCore(Dictionary<string, string> dic, string version = "0", string id = "metu", string appname = "metu", string servicename = "service", string methodname = "call")
        {
            if (id == null)
            {
                return Result.ERROR(0, "应用未注册！");
            }
            if (version == null)
            {
                version = "0";
            }
            if (version.Trim().Length == 0)
            {
                version = "0";
            }
            if (appname == null)
            {
                appname = "metu";
            }
            if (appname.Trim().Length == 0)
            {
                appname = "metu";
            }
            if (servicename == null)
            {
                servicename = "service";
            }
            if (servicename.Trim().Length == 0)
            {
                servicename = "service";
            }
            if (methodname == null)
            {
                methodname = "call";
            }
            if (methodname.Trim().Length == 0)
            {
                methodname = "call";
            }
            FileHelper.Writelog(dic, "SQL"); bool configtag = false;
            foreach (var itm in CommonCache.ConfigCaches.Keys)
            {
                if (itm.ToUpper() == id.ToUpper())
                {
                    configtag = true;
                    break;
                }
            }
            CoreBLL cbll = new CoreBLL();

            if (!configtag)
            {
                string sqlconfig = BaseSQL.SQL_GET_PAGE_DATA_LIST;// "SELECT top(1)  [ConfigName],[ConfigValue]FROM  [dbo].[SYSpageDICConfig] aa, [dbo].[SYSpageDic] bb where aa.CID=bb.CID and aa.DICID=bb.ID  and  lower(bb.SortCode)='dbconfig' and aa.[ConfigName]='{0}' and  bb.Cname=N'{1}'and  bb.appname=N'{2}' and  bb.version=N'{3}'";
                sqlconfig = sqlconfig.Replace("{0}", id);
                sqlconfig = sqlconfig.Replace("{1}", appname);
                sqlconfig = sqlconfig.Replace("{2}", appname);
                sqlconfig = sqlconfig.Replace("{3}", version);
                var configdt = cbll.db.ExecuteDataTable(sqlconfig);
                bool exitf = true;
                if (configdt != null) if (configdt.Rows.Count > ConstNum.Zero) if (configdt.Rows[0][0] != null && configdt.Rows[0][1] != null) if (configdt.Rows[0][1].ToString().Trim().Length > ConstNum.Zero)
                            {
                                CommonCache.ConfigCaches.Add(configdt.Rows[0][0].ToString().ToUpper(), configdt.Rows[0][1].ToString());
                                exitf = false;
                            }
                if (exitf) return Result.ERROR(0, "未找到相关配置数据");
            }
            bll = new CoreBLL((id).ToUpper());

            string sqlstr = BaseSQL.SQL_SysPageService_List;// @"SELECT[ServiceEvents],[ReturnType]FROM[dbo].[SysPageService]where  [IsEnabled]=1 and [IsDeleted]=0 and [servicename]=N'{0}'";

            if (version != null) if (version.Trim().Length > 1)
                {
                    sqlstr = sqlstr + " and [version]=N'" + version + "'  ";
                }
            if (methodname != null) if (methodname.Trim().Length > 1)
                {
                    sqlstr = sqlstr + " and [methodname]=N'" + methodname + "'  ";
                }
            sqlstr = sqlstr.Replace("{0}", servicename); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");

            var dt = cbll.db.ExecuteDataTable(sqlstr.Str2SQL());
            if (dt != null) if (dt.Rows.Count > ConstNum.Zero) if (dt.Rows[0][0] != null) if (dt.Rows[0][0].ToString().Trim().Length > 10)
                        {
                            sqlstr = dt.Rows[0][0].ToString();

                            if (dic != null) if (dic.Count > ConstNum.Zero)
                                {
                                    foreach (var itm in dic)
                                    {
                                        if (itm.Value != null)
                                        {
                                            sqlstr = sqlstr.Replace("{" + itm.Key.ToUpper() + "}", itm.Value.Replace("'", "''")).Replace("{" + itm.Key.ToLower() + "}", itm.Value.Replace("'", "''"));
                                        }
                                    }
                                }
                            if (dt.Rows[0][1] == null)
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = bll.db.ExecuteDataTable(sqlstr.Str2SQL());

                                return new Result(rsdt.ToListDictionary());
                            }

                            if (dt.Rows[0][1].ToString() == "0" || dt.Rows[0][1].ToString().ToLower() == "table" || dt.Rows[0][1].ToString().ToLower() == "list")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = bll.db.ExecuteDataTable(sqlstr.Str2SQL());

                                return new Result(rsdt.ToListDictionary());
                            }
                            if (dt.Rows[0][1].ToString() == "object")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = bll.db.ExecuteDataTable(sqlstr.Str2SQL());

                                if (rsdt == null)
                                {
                                    return new Result(null); ;
                                }
                                if (rsdt.Rows.Count == 0)
                                {
                                    return new Result(0);
                                }
                                if (rsdt.Columns.Count == 0)
                                {
                                    return new Result(0);
                                }
                                if (rsdt.Rows.Count > 0 && rsdt.Columns.Count > 0)
                                {
                                    return new Result(rsdt.Rows[0][0]);
                                }
                                return new Result(null);
                            }
                            if (dt.Rows[0][1].ToString() == "bool")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = bll.db.Execute(sqlstr.Str2SQL());
                                return new Result(rsdt > 0);
                            }
                            if (dt.Rows[0][1].ToString() == "int")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = bll.db.Execute(sqlstr.Str2SQL());
                                return new Result(rsdt);
                            }
                            if (dt.Rows[0][1].ToString() == "WEBAPI")
                            {
                                return new Result(WebAPIHelper.CallAPI(sqlstr, dic)); ;
                            }
                            else
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = bll.db.ExecuteDataTable(sqlstr.Str2SQL());
                                return new Result(rsdt.ToListDictionary());
                            }



                        }

            return Result.ERROR(0, "操作失败！");
        }

        /// <summary>
        /// 执行业通用务逻辑  [Route("{appname}/{servicename}cdservice/{methodname}api")]      
        /// </summary>
        /// <param name="id">企业用户名 （英文字符或手机号，唯一不重复）</param>
        /// <param name="appname">企业用户下的应用分类（ERP、CRM、WMS、MES、MTS、TMS、SSO、GPS等）</param>
        /// <param name="servicename">应该下的服务分类名</param>
        /// <param name="methodname">服务中具体业务名</param>
        /// <param name="dic">传入参数</param>
        /// <param name="version"></param>
        /// <returns></returns>

        public dynamic Core(Dictionary<string, string> dic, string version = "0", string id = "metu", string appname = "metu", string servicename = "service", string methodname = "call")
        {
            if (version == null)
            {
                version = "0";
            }
            if (version.Trim().Length == 0)
            {
                version = "0";
            }
            if (appname == null)
            {
                appname = "metu";
            }
            if (appname.Trim().Length == 0)
            {
                appname = "metu";
            }
            if (servicename == null)
            {
                servicename = "service";
            }
            if (servicename.Trim().Length == 0)
            {
                servicename = "service";
            }
            if (methodname == null)
            {
                methodname = "call";
            }
            if (methodname.Trim().Length == 0)
            {
                methodname = "call";
            }
            if (id == null)
            {
                return Result.ERROR(0, "应用未注册！");
            }
            FileHelper.Writelog(dic, "SQL"); bool configtag = false;
            foreach (var itm in CommonCache.ConfigCaches.Keys)
            {
                if (itm.ToUpper() == id.ToUpper())
                {
                    configtag = true;
                    break;
                }
            }
            CoreBLL cbll = new CoreBLL();

            if (!configtag)
            {
                string sqlconfig = BaseSQL.SQL_GET_PAGE_DATA_LIST;//"SELECT top(1)  [ConfigName],[ConfigValue]FROM  [dbo].[SYSpageDICConfig] aa, [dbo].[SYSpageDic] bb where aa.CID=bb.CID and aa.DICID=bb.ID  and  lower(bb.SortCode)='dbconfig' and aa.[ConfigName]='{0}' and  bb.Cname=N'{1}'and  bb.appname=N'{2}' and  bb.version=N'{3}'";
                sqlconfig = sqlconfig.Replace("{0}", id);
                sqlconfig = sqlconfig.Replace("{1}", appname);
                sqlconfig = sqlconfig.Replace("{2}", appname);
                sqlconfig = sqlconfig.Replace("{3}", version);
                var configdt = cbll.db.ExecuteDataTable(sqlconfig);
                bool exitf = true;
                if (configdt != null) if (configdt.Rows.Count > ConstNum.Zero) if (configdt.Rows[0][0] != null && configdt.Rows[0][1] != null) if (configdt.Rows[0][1].ToString().Trim().Length > ConstNum.Zero)
                            {
                                CommonCache.ConfigCaches.Add(configdt.Rows[0][0].ToString().ToUpper(), configdt.Rows[0][1].ToString());
                                exitf = false;
                            }
                if (exitf) return Result.ERROR(0, "未找到相关配置数据");
            }
            bll = new CoreBLL((id).ToUpper());

            string sqlstr = BaseSQL.SQL_SysPageService_List;//@"SELECT[ServiceEvents],[ReturnType]FROM[dbo].[SysPageService]where  [IsEnabled]=1 and [IsDeleted]=0 and [servicename]=N'{0}'";

            if (version != null) if (version.Trim().Length > 1)
                {
                    sqlstr = sqlstr + " and [version]=N'" + version + "'  ";
                }
            if (methodname != null) if (methodname.Trim().Length > 1)
                {
                    sqlstr = sqlstr + " and [methodname]=N'" + methodname + "'  ";
                }
            sqlstr = sqlstr.Replace("{0}", servicename); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");

            var dt = cbll.db.ExecuteDataTable(sqlstr.Str2SQL());
            if (dt != null) if (dt.Rows.Count > ConstNum.Zero) if (dt.Rows[0][0] != null) if (dt.Rows[0][0].ToString().Trim().Length > 10)
                        {
                            sqlstr = dt.Rows[0][0].ToString();

                            if (dic != null) if (dic.Count > ConstNum.Zero)
                                {
                                    foreach (var itm in dic)
                                    {
                                        if (itm.Value != null)
                                        {
                                            sqlstr = sqlstr.Replace("{" + itm.Key.ToUpper() + "}", itm.Value.Replace("'", "''")).Replace("{" + itm.Key.ToLower() + "}", itm.Value.Replace("'", "''"));
                                        }
                                    }
                                }
                            if (dt.Rows[0][1] == null)
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = bll.db.ExecuteDataTable(sqlstr.Str2SQL());

                                return new Result(rsdt.ToListDictionary());
                            }

                            if (dt.Rows[0][1].ToString() == "0" || dt.Rows[0][1].ToString().ToLower() == "table" || dt.Rows[0][1].ToString().ToLower() == "list")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = bll.db.ExecuteDataTable(sqlstr.Str2SQL());

                                return new Result(rsdt.ToListDictionary());
                            }
                            if (dt.Rows[0][1].ToString() == "object")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = bll.db.ExecuteDataTable(sqlstr.Str2SQL());

                                if (rsdt == null)
                                {
                                    return new Result(null); ;
                                }
                                if (rsdt.Rows.Count == 0)
                                {
                                    return new Result(0);
                                }
                                if (rsdt.Columns.Count == 0)
                                {
                                    return new Result(0);
                                }
                                if (rsdt.Rows.Count > 0 && rsdt.Columns.Count > 0)
                                {
                                    return new Result(rsdt.Rows[0][0]);
                                }
                                return new Result(null);
                            }
                            if (dt.Rows[0][1].ToString() == "bool")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = bll.db.Execute(sqlstr.Str2SQL());
                                return new Result(rsdt > 0);
                            }
                            if (dt.Rows[0][1].ToString() == "int")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = bll.db.Execute(sqlstr.Str2SQL());
                                return new Result(rsdt);
                            }
                            if (dt.Rows[0][1].ToString() == "WEBAPI")
                            {
                                return new Result(WebAPIHelper.CallAPI(sqlstr, dic)); ;
                            }
                            else
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = bll.db.ExecuteDataTable(sqlstr.Str2SQL());
                                return new Result(rsdt.ToListDictionary());
                            }



                        }

            return Result.ERROR(0, "操作失败！");
        }
        /// <summary>
        /// 执行业通用务逻辑,返回整型 [Route("{appname}/{servicename}iservice/{methodname}api")]
        /// </summary>
        /// <param name="id">企业用户名 （英文字符或手机号，唯一不重复）</param>
        /// <param name="appname">企业用户下的应用分类（ERP、CRM、WMS、MES、MTS、TMS、SSO、GPS等）</param>
        /// <param name="servicename">应该下的服务分类名</param>
        /// <param name="methodname">服务中具体业务名</param>
        /// <param name="dic">传入参数</param>
        /// <param name="version"></param>
        /// <returns></returns>


        public int Coreint(Dictionary<string, string> dic, string version = "0", string id = "metu", string appname = "metu", string servicename = "service", string methodname = "call")
        {
            if (id == null)
            {
                return -1;
            }
            if (version == null)
            {
                version = "0";
            }
            if (version.Trim().Length == 0)
            {
                version = "0";
            }
            if (appname == null)
            {
                appname = "metu";
            }
            if (appname.Trim().Length == 0)
            {
                appname = "metu";
            }
            if (servicename == null)
            {
                servicename = "service";
            }
            if (servicename.Trim().Length == 0)
            {
                servicename = "service";
            }
            if (methodname == null)
            {
                methodname = "call";
            }
            if (methodname.Trim().Length == 0)
            {
                methodname = "call";
            }
            FileHelper.Writelog(dic, "SQL"); bool configtag = false;
            foreach (var itm in CommonCache.ConfigCaches.Keys)
            {
                if (itm.ToUpper() == id.ToUpper())
                {
                    configtag = true;
                    break;
                }
            }
            CoreBLL cbll = new CoreBLL();

            if (!configtag)
            {
                string sqlconfig = BaseSQL.SQL_GET_PAGE_DATA_LIST;//"SELECT top(1)  [ConfigName],[ConfigValue]FROM  [dbo].[SYSpageDICConfig] aa, [dbo].[SYSpageDic] bb where aa.CID=bb.CID and aa.DICID=bb.ID  and  lower(bb.SortCode)='dbconfig' and aa.[ConfigName]='{0}' and  bb.Cname=N'{1}'and  bb.appname=N'{2}' and  bb.version=N'{3}'";
                sqlconfig = sqlconfig.Replace("{0}", id);
                sqlconfig = sqlconfig.Replace("{1}", appname);
                sqlconfig = sqlconfig.Replace("{2}", appname);
                sqlconfig = sqlconfig.Replace("{3}", version);
                var configdt = cbll.db.ExecuteDataTable(sqlconfig);
                bool exitf = true;
                if (configdt != null) if (configdt.Rows.Count > ConstNum.Zero) if (configdt.Rows[0][0] != null && configdt.Rows[0][1] != null) if (configdt.Rows[0][1].ToString().Trim().Length > ConstNum.Zero)
                            {
                                CommonCache.ConfigCaches.Add(configdt.Rows[0][0].ToString().ToUpper(), configdt.Rows[0][1].ToString());
                                exitf = false;
                            }
                if (exitf) return -1;
            }
            bll = new CoreBLL((id).ToUpper());

            string sqlstr = BaseSQL.SQL_SysPageService_List;//@"SELECT[ServiceEvents],[ReturnType]FROM[dbo].[SysPageService]where  [IsEnabled]=1 and [IsDeleted]=0 and [servicename]=N'{0}'";

            if (version != null) if (version.Trim().Length > 1)
                {
                    sqlstr = sqlstr + " and [version]=N'" + version + "'  ";
                }
            if (methodname != null) if (methodname.Trim().Length > 1)
                {
                    sqlstr = sqlstr + " and [methodname]=N'" + methodname + "'  ";
                }
            sqlstr = sqlstr.Replace("{0}", servicename); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");

            var dt = cbll.db.ExecuteDataTable(sqlstr.Str2SQL());
            if (dt != null) if (dt.Rows.Count > ConstNum.Zero) if (dt.Rows[0][0] != null) if (dt.Rows[0][0].ToString().Trim().Length > 10)
                        {
                            sqlstr = dt.Rows[0][0].ToString();

                            if (dic != null) if (dic.Count > ConstNum.Zero)
                                {
                                    foreach (var itm in dic)
                                    {
                                        if (itm.Value != null)
                                        {
                                            sqlstr = sqlstr.Replace("{" + itm.Key.ToUpper() + "}", itm.Value.Replace("'", "''")).Replace("{" + itm.Key.ToLower() + "}", itm.Value.Replace("'", "''"));
                                        }
                                    }
                                }
                            if (dt.Rows[0][1] == null)
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = bll.db.ExecuteDataTable(sqlstr.Str2SQL());

                                return rsdt.Rows.Count;
                            }

                            if (dt.Rows[0][1].ToString() == "0" || dt.Rows[0][1].ToString().ToLower() == "table" || dt.Rows[0][1].ToString().ToLower() == "list")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = bll.db.ExecuteDataTable(sqlstr.Str2SQL());

                                return rsdt.Rows.Count;
                            }
                            if (dt.Rows[0][1].ToString() == "object")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = bll.db.ExecuteDataTable(sqlstr.Str2SQL());

                                if (rsdt == null)
                                {
                                    return -1;
                                }
                                if (rsdt.Rows.Count == 0)
                                {
                                    return 0;
                                }
                                if (rsdt.Columns.Count == 0)
                                {
                                    return 0;
                                }
                                if (rsdt.Rows.Count > 0 && rsdt.Columns.Count > 0)
                                {
                                    return rsdt.Rows.Count;
                                }
                                return -1;
                            }
                            if (dt.Rows[0][1].ToString() == "bool")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = bll.db.Execute(sqlstr.Str2SQL());
                                return rsdt;
                            }
                            if (dt.Rows[0][1].ToString() == "int")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = bll.db.Execute(sqlstr.Str2SQL());
                                return rsdt;
                            }
                            if (dt.Rows[0][1].ToString() == "WEBAPI")
                            {
                                WebAPIHelper.CallAPI(sqlstr, dic);
                                return 1;

                            }
                            else
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = bll.db.ExecuteDataTable(sqlstr.Str2SQL());
                                return rsdt.Rows.Count;
                            }



                        }

            return -1;
        }
        /// <summary>
        /// 执行业通用务逻辑,返回Boolean[Route("{appname}/{servicename}bservice/{methodname}api")]
        /// </summary>
        /// <param name="id">企业用户名 （英文字符或手机号，唯一不重复）</param>
        /// <param name="appname">企业用户下的应用分类（ERP、CRM、WMS、MES、MTS、TMS、SSO、GPS等）</param>
        /// <param name="servicename">应该下的服务分类名</param>
        /// <param name="methodname">服务中具体业务名</param>
        /// <param name="dic">传入参数</param>
        /// <param name="version"></param>
        /// <returns></returns>


        public bool Corebool(Dictionary<string, string> dic, string version = "0", string id = "metu", string appname = "metu", string servicename = "service", string methodname = "call")
        {
            if (id == null)
            {
                return false;
            }
            if (version == null)
            {
                version = "0";
            }
            if (version.Trim().Length == 0)
            {
                version = "0";
            }
            if (appname == null)
            {
                appname = "metu";
            }
            if (appname.Trim().Length == 0)
            {
                appname = "metu";
            }
            if (servicename == null)
            {
                servicename = "service";
            }
            if (servicename.Trim().Length == 0)
            {
                servicename = "service";
            }
            if (methodname == null)
            {
                methodname = "call";
            }
            if (methodname.Trim().Length == 0)
            {
                methodname = "call";
            }
            FileHelper.Writelog(dic, "SQL"); bool configtag = false;
            foreach (var itm in CommonCache.ConfigCaches.Keys)
            {
                if (itm.ToUpper() == id.ToUpper())
                {
                    configtag = true;
                    break;
                }
            }
            CoreBLL cbll = new CoreBLL();

            if (!configtag)
            {
                string sqlconfig = BaseSQL.SQL_GET_PAGE_DATA_LIST;//"SELECT top(1)  [ConfigName],[ConfigValue]FROM  [dbo].[SYSpageDICConfig] aa, [dbo].[SYSpageDic] bb where aa.CID=bb.CID and aa.DICID=bb.ID  and  lower(bb.SortCode)='dbconfig' and aa.[ConfigName]='{0}' and  bb.Cname=N'{1}'and  bb.appname=N'{2}' and  bb.version=N'{3}'";
                sqlconfig = sqlconfig.Replace("{0}", id);
                sqlconfig = sqlconfig.Replace("{1}", appname);
                sqlconfig = sqlconfig.Replace("{2}", appname);
                sqlconfig = sqlconfig.Replace("{3}", version);
                var configdt = cbll.db.ExecuteDataTable(sqlconfig);
                bool exitf = true;
                if (configdt != null) if (configdt.Rows.Count > ConstNum.Zero) if (configdt.Rows[0][0] != null && configdt.Rows[0][1] != null) if (configdt.Rows[0][1].ToString().Trim().Length > ConstNum.Zero)
                            {
                                CommonCache.ConfigCaches.Add(configdt.Rows[0][0].ToString().ToUpper(), configdt.Rows[0][1].ToString());
                                exitf = false;
                            }
                if (exitf) return false;
            }
            bll = new CoreBLL((id).ToUpper());

            string sqlstr = BaseSQL.SQL_SysPageService_List;//@"SELECT[ServiceEvents],[ReturnType]FROM[dbo].[SysPageService]where  [IsEnabled]=1 and [IsDeleted]=0 and [servicename]=N'{0}'";

            if (version != null) if (version.Trim().Length > 1)
                {
                    sqlstr = sqlstr + " and [version]=N'" + version + "'  ";
                }
            if (methodname != null) if (methodname.Trim().Length > 1)
                {
                    sqlstr = sqlstr + " and [methodname]=N'" + methodname + "'  ";
                }
            sqlstr = sqlstr.Replace("{0}", servicename); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");

            var dt = cbll.db.ExecuteDataTable(sqlstr.Str2SQL());
            if (dt != null) if (dt.Rows.Count > ConstNum.Zero) if (dt.Rows[0][0] != null) if (dt.Rows[0][0].ToString().Trim().Length > 10)
                        {
                            sqlstr = dt.Rows[0][0].ToString();

                            if (dic != null) if (dic.Count > ConstNum.Zero)
                                {
                                    foreach (var itm in dic)
                                    {
                                        if (itm.Value != null)
                                        {
                                            sqlstr = sqlstr.Replace("{" + itm.Key.ToUpper() + "}", itm.Value.Replace("'", "''")).Replace("{" + itm.Key.ToLower() + "}", itm.Value.Replace("'", "''"));
                                        }
                                    }
                                }
                            if (dt.Rows[0][1] == null)
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = bll.db.ExecuteDataTable(sqlstr.Str2SQL());

                                return rsdt.Rows.Count > 0;
                            }

                            if (dt.Rows[0][1].ToString() == "0" || dt.Rows[0][1].ToString().ToLower() == "table" || dt.Rows[0][1].ToString().ToLower() == "list")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = bll.db.ExecuteDataTable(sqlstr.Str2SQL());

                                return rsdt.Rows.Count > 0;
                            }
                            if (dt.Rows[0][1].ToString() == "object")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = bll.db.ExecuteDataTable(sqlstr.Str2SQL());

                                if (rsdt == null)
                                {
                                    return false;
                                }
                                if (rsdt.Rows.Count == 0)
                                {
                                    return false;
                                }
                                if (rsdt.Columns.Count == 0)
                                {
                                    return false;
                                }
                                if (rsdt.Rows.Count > 0 && rsdt.Columns.Count > 0)
                                {
                                    return rsdt.Rows.Count > 0;
                                }
                                return false;
                            }
                            if (dt.Rows[0][1].ToString() == "bool")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = bll.db.Execute(sqlstr.Str2SQL());
                                return rsdt > 0;
                            }
                            if (dt.Rows[0][1].ToString() == "int")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = bll.db.Execute(sqlstr.Str2SQL());
                                return rsdt > 0;
                            }
                            if (dt.Rows[0][1].ToString() == "WEBAPI")
                            {
                                WebAPIHelper.CallAPI(sqlstr, dic);
                                return true;

                            }
                            else
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = bll.db.ExecuteDataTable(sqlstr.Str2SQL());
                                return rsdt.Rows.Count > 0;
                            }



                        }

            return false;
        }
        /// <summary>
        /// 执行业通用务逻辑,返回对象数据[Route("{appname}/{servicename}oservice/{methodname}api")]
        /// </summary>
        /// <param name="id">企业用户名 （英文字符或手机号，唯一不重复）</param>
        /// <param name="appname">企业用户下的应用分类（ERP、CRM、WMS、MES、MTS、TMS、SSO、GPS等）</param>
        /// <param name="servicename">应该下的服务分类名</param>
        /// <param name="methodname">服务中具体业务名</param>
        /// <param name="dic">传入参数</param>
        /// <param name="version"></param>
        /// <returns></returns>

        public object Coreobject(Dictionary<string, string> dic, string version = "0", string id = "metu", string appname = "metu", string servicename = "service", string methodname = "call")
        {
            if (id == null)
            {
                return Result.ERROR(0, "应用未注册！");
            }
            if (version == null)
            {
                version = "0";
            }
            if (version.Trim().Length == 0)
            {
                version = "0";
            }
            if (appname == null)
            {
                appname = "metu";
            }
            if (appname.Trim().Length == 0)
            {
                appname = "metu";
            }
            if (servicename == null)
            {
                servicename = "service";
            }
            if (servicename.Trim().Length == 0)
            {
                servicename = "service";
            }
            if (methodname == null)
            {
                methodname = "call";
            }
            if (methodname.Trim().Length == 0)
            {
                methodname = "call";
            }
            FileHelper.Writelog(dic, "SQL"); bool configtag = false;
            foreach (var itm in CommonCache.ConfigCaches.Keys)
            {
                if (itm.ToUpper() == id.ToUpper())
                {
                    configtag = true;
                    break;
                }
            }
            CoreBLL cbll = new CoreBLL();

            if (!configtag)
            {
                string sqlconfig = BaseSQL.SQL_GET_PAGE_DATA_LIST;//"SELECT top(1)  [ConfigName],[ConfigValue]FROM  [dbo].[SYSpageDICConfig] aa, [dbo].[SYSpageDic] bb where aa.CID=bb.CID and aa.DICID=bb.ID  and  lower(bb.SortCode)='dbconfig' and aa.[ConfigName]='{0}' and  bb.Cname=N'{1}'and  bb.appname=N'{2}' and  bb.version=N'{3}'";
                sqlconfig = sqlconfig.Replace("{0}", id);
                sqlconfig = sqlconfig.Replace("{1}", appname);
                sqlconfig = sqlconfig.Replace("{2}", appname);
                sqlconfig = sqlconfig.Replace("{3}", version);
                var configdt = cbll.db.ExecuteDataTable(sqlconfig);
                bool exitf = true;
                if (configdt != null) if (configdt.Rows.Count > ConstNum.Zero) if (configdt.Rows[0][0] != null && configdt.Rows[0][1] != null) if (configdt.Rows[0][1].ToString().Trim().Length > ConstNum.Zero)
                            {
                                CommonCache.ConfigCaches.Add(configdt.Rows[0][0].ToString().ToUpper(), configdt.Rows[0][1].ToString());
                                exitf = false;
                            }
                if (exitf) return Result.ERROR(0, "未找到相关配置数据");
            }
            bll = new CoreBLL((id).ToUpper());

            string sqlstr = BaseSQL.SQL_SysPageService_List;//@"SELECT[ServiceEvents],[ReturnType]FROM[dbo].[SysPageService]where  [IsEnabled]=1 and [IsDeleted]=0 and [servicename]=N'{0}'";

            if (version != null) if (version.Trim().Length > 1)
                {
                    sqlstr = sqlstr + " and [version]=N'" + version + "'  ";
                }
            if (methodname != null) if (methodname.Trim().Length > 1)
                {
                    sqlstr = sqlstr + " and [methodname]=N'" + methodname + "'  ";
                }
            sqlstr = sqlstr.Replace("{0}", servicename); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");

            var dt = cbll.db.ExecuteDataTable(sqlstr.Str2SQL());
            if (dt != null) if (dt.Rows.Count > ConstNum.Zero) if (dt.Rows[0][0] != null) if (dt.Rows[0][0].ToString().Trim().Length > 10)
                        {
                            sqlstr = dt.Rows[0][0].ToString();

                            if (dic != null) if (dic.Count > ConstNum.Zero)
                                {
                                    foreach (var itm in dic)
                                    {
                                        if (itm.Value != null)
                                        {
                                            sqlstr = sqlstr.Replace("{" + itm.Key.ToUpper() + "}", itm.Value.Replace("'", "''")).Replace("{" + itm.Key.ToLower() + "}", itm.Value.Replace("'", "''"));
                                        }
                                    }
                                }
                            if (dt.Rows[0][1] == null)
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = bll.db.ExecuteDataTable(sqlstr.Str2SQL());

                                return new Result(rsdt.ToListDictionary());
                            }

                            if (dt.Rows[0][1].ToString() == "0" || dt.Rows[0][1].ToString().ToLower() == "table" || dt.Rows[0][1].ToString().ToLower() == "list")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = bll.db.ExecuteDataTable(sqlstr.Str2SQL());

                                return new Result(rsdt.ToListDictionary());
                            }
                            if (dt.Rows[0][1].ToString() == "object")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = bll.db.ExecuteDataTable(sqlstr.Str2SQL());

                                if (rsdt == null)
                                {
                                    return new Result(null); ;
                                }
                                if (rsdt.Rows.Count == 0)
                                {
                                    return new Result(0);
                                }
                                if (rsdt.Columns.Count == 0)
                                {
                                    return new Result(0);
                                }
                                if (rsdt.Rows.Count > 0 && rsdt.Columns.Count > 0)
                                {
                                    return new Result(rsdt.Rows[0][0]);
                                }
                                return new Result(null);
                            }
                            if (dt.Rows[0][1].ToString() == "bool")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = bll.db.Execute(sqlstr.Str2SQL());
                                return new Result(rsdt > 0);
                            }
                            if (dt.Rows[0][1].ToString() == "int")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = bll.db.Execute(sqlstr.Str2SQL());
                                return new Result(rsdt);
                            }
                            if (dt.Rows[0][1].ToString() == "WEBAPI")
                            {
                                return new Result(WebAPIHelper.CallAPI(sqlstr, dic)); ;
                            }
                            else
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = bll.db.ExecuteDataTable(sqlstr.Str2SQL());
                                return new Result(rsdt.ToListDictionary());
                            }



                        }

            return Result.ERROR(0, "操作失败！");
        }
        /// <summary>
        /// 执行业通用务逻辑,返回统一格式result[Route("{appname}/{servicename}rservice/{methodname}api")]
        /// </summary>
        /// <param name="id">企业用户名 （英文字符或手机号，唯一不重复）</param>
        /// <param name="appname">企业用户下的应用分类（ERP、CRM、WMS、MES、MTS、TMS、SSO、GPS等）</param>
        /// <param name="servicename">应该下的服务分类名</param>
        /// <param name="methodname">服务中具体业务名</param>
        /// <param name="dic">传入参数</param>
        /// <param name="version"></param>
        /// <returns></returns>


        public Result CoreBasic(Dictionary<string, string> dic, string version = "0", string id = "metu", string appname = "metu", string servicename = "service", string methodname = "call")
        {
            if (id == null)
            {
                return Result.ERROR(0, "应用未注册！");
            }
            if (version == null)
            {
                version = "0";
            }
            if (version.Trim().Length == 0)
            {
                version = "0";
            }
            if (appname == null)
            {
                appname = "metu";
            }
            if (appname.Trim().Length == 0)
            {
                appname = "metu";
            }
            if (servicename == null)
            {
                servicename = "service";
            }
            if (servicename.Trim().Length == 0)
            {
                servicename = "service";
            }
            if (methodname == null)
            {
                methodname = "call";
            }
            if (methodname.Trim().Length == 0)
            {
                methodname = "call";
            }
            FileHelper.Writelog(dic, "SQL"); bool configtag = false;
            foreach (var itm in CommonCache.ConfigCaches.Keys)
            {
                if (itm.ToUpper() == id.ToUpper())
                {
                    configtag = true;
                    break;
                }
            }
            CoreBLL cbll = new CoreBLL();

            if (!configtag)
            {
                string sqlconfig = BaseSQL.SQL_GET_PAGE_DATA_LIST;//"SELECT top(1)  [ConfigName],[ConfigValue]FROM  [dbo].[SYSpageDICConfig] aa, [dbo].[SYSpageDic] bb where aa.CID=bb.CID and aa.DICID=bb.ID  and  lower(bb.SortCode)='dbconfig' and aa.[ConfigName]='{0}' and  bb.Cname=N'{1}'and  bb.appname=N'{2}' and  bb.version=N'{3}'";
                sqlconfig = sqlconfig.Replace("{0}", id);
                sqlconfig = sqlconfig.Replace("{1}", appname);
                sqlconfig = sqlconfig.Replace("{2}", appname);
                sqlconfig = sqlconfig.Replace("{3}", version);
                var configdt = cbll.db.ExecuteDataTable(sqlconfig);
                bool exitf = true;
                if (configdt != null) if (configdt.Rows.Count > ConstNum.Zero) if (configdt.Rows[0][0] != null && configdt.Rows[0][1] != null) if (configdt.Rows[0][1].ToString().Trim().Length > ConstNum.Zero)
                            {
                                CommonCache.ConfigCaches.Add(configdt.Rows[0][0].ToString().ToUpper(), configdt.Rows[0][1].ToString());
                                exitf = false;
                            }
                if (exitf) return Result.ERROR(0, "未找到相关配置数据");
            }
            bll = new CoreBLL((id).ToUpper());

            string sqlstr = BaseSQL.SQL_SysPageService_List;//@"SELECT[ServiceEvents],[ReturnType]FROM[dbo].[SysPageService]where  [IsEnabled]=1 and [IsDeleted]=0 and [servicename]=N'{0}'";

            if (version != null) if (version.Trim().Length > 1)
                {
                    sqlstr = sqlstr + " and [version]=N'" + version + "'  ";
                }
            if (methodname != null) if (methodname.Trim().Length > 1)
                {
                    sqlstr = sqlstr + " and [methodname]=N'" + methodname + "'  ";
                }
            sqlstr = sqlstr.Replace("{0}", servicename); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");

            var dt = cbll.db.ExecuteDataTable(sqlstr.Str2SQL());
            if (dt != null) if (dt.Rows.Count > ConstNum.Zero) if (dt.Rows[0][0] != null) if (dt.Rows[0][0].ToString().Trim().Length > 10)
                        {
                            sqlstr = dt.Rows[0][0].ToString();

                            if (dic != null) if (dic.Count > ConstNum.Zero)
                                {
                                    foreach (var itm in dic)
                                    {
                                        if (itm.Value != null)
                                        {
                                            sqlstr = sqlstr.Replace("{" + itm.Key.ToUpper() + "}", itm.Value.Replace("'", "''")).Replace("{" + itm.Key.ToLower() + "}", itm.Value.Replace("'", "''"));
                                        }
                                    }
                                }
                            if (dt.Rows[0][1] == null)
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = bll.db.ExecuteDataTable(sqlstr.Str2SQL());

                                return new Result(rsdt.ToListDictionary());
                            }

                            if (dt.Rows[0][1].ToString() == "0" || dt.Rows[0][1].ToString().ToLower() == "table" || dt.Rows[0][1].ToString().ToLower() == "list")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = bll.db.ExecuteDataTable(sqlstr.Str2SQL());

                                return new Result(rsdt.ToListDictionary());
                            }
                            if (dt.Rows[0][1].ToString() == "object")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = bll.db.ExecuteDataTable(sqlstr.Str2SQL());

                                if (rsdt == null)
                                {
                                    return new Result(null); ;
                                }
                                if (rsdt.Rows.Count == 0)
                                {
                                    return new Result(0);
                                }
                                if (rsdt.Columns.Count == 0)
                                {
                                    return new Result(0);
                                }
                                if (rsdt.Rows.Count > 0 && rsdt.Columns.Count > 0)
                                {
                                    return new Result(rsdt.Rows[0][0]);
                                }
                                return new Result(null);
                            }
                            if (dt.Rows[0][1].ToString() == "bool")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = bll.db.Execute(sqlstr.Str2SQL());
                                return new Result(rsdt > 0);
                            }
                            if (dt.Rows[0][1].ToString() == "int")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = bll.db.Execute(sqlstr.Str2SQL());
                                return new Result(rsdt);
                            }
                            if (dt.Rows[0][1].ToString() == "WEBAPI")
                            {
                                return new Result(WebAPIHelper.CallAPI(sqlstr, dic)); ;
                            }
                            else
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = bll.db.ExecuteDataTable(sqlstr.Str2SQL());
                                return new Result(rsdt.ToListDictionary());
                            }



                        }

            return Result.ERROR(0, "操作失败！");
        }
        /// <summary>
        /// 执行业通用务逻辑,返回统一格式result[Route("{appname}/{servicename}rservice/{methodname}list/{pageindex}/{pagesize}")]
        /// </summary>
        /// <param name="id">企业用户名 （英文字符或手机号，唯一不重复）</param>
        /// <param name="appname">企业用户下的应用分类（ERP、CRM、WMS、MES、MTS、TMS、SSO、GPS等）</param>
        /// <param name="servicename">应该下的服务分类名</param>
        /// <param name="methodname">服务中具体业务名</param>
        /// <param name="dic">传入参数</param>
        /// <returns></returns>


        public Result CoreList(Dictionary<string, string> dic, int pageindex, int pagesize, string version = "0", string id = "metu", string appname = "metu", string servicename = "service", string methodname = "call")
        {
            if (id == null)
            {
                return Result.ERROR(0, "应用未注册！");
            }
            if (version == null)
            {
                version = "0";
            }
            if (version.Trim().Length == 0)
            {
                version = "0";
            }
            if (appname == null)
            {
                appname = "metu";
            }
            if (appname.Trim().Length == 0)
            {
                appname = "metu";
            }
            if (servicename == null)
            {
                servicename = "service";
            }
            if (servicename.Trim().Length == 0)
            {
                servicename = "service";
            }
            if (methodname == null)
            {
                methodname = "call";
            }
            if (methodname.Trim().Length == 0)
            {
                methodname = "call";
            }
            Result rs = new Result();
            rs.Data.Add("pageindex", pageindex);
            rs.Data.Add("pagesize", pagesize);
            if (id == null)
            {
                return Result.ERROR(0, "应用未注册！");
            }
            FileHelper.Writelog(dic, "SQL"); bool configtag = false;
            foreach (var itm in CommonCache.ConfigCaches.Keys)
            {
                if (itm.ToUpper() == id.ToUpper())
                {
                    configtag = true;
                    break;
                }
            }
            CoreBLL cbll = new CoreBLL();

            if (!configtag)
            {
                string sqlconfig = BaseSQL.SQL_GET_PAGE_DATA_LIST;//"SELECT top(1)  [ConfigName],[ConfigValue]FROM  [dbo].[SYSpageDICConfig] aa, [dbo].[SYSpageDic] bb where aa.CID=bb.CID and aa.DICID=bb.ID  and  lower(bb.SortCode)='dbconfig' and aa.[ConfigName]='{0}' and  bb.Cname=N'{1}'and  bb.appname=N'{2}' and  bb.version=N'{3}'";
                sqlconfig = sqlconfig.Replace("{0}", id);
                sqlconfig = sqlconfig.Replace("{1}", appname);
                sqlconfig = sqlconfig.Replace("{2}", appname);
                sqlconfig = sqlconfig.Replace("{3}", version);
                var configdt = cbll.db.ExecuteDataTable(sqlconfig);
                bool exitf = true;
                if (configdt != null) if (configdt.Rows.Count > ConstNum.Zero) if (configdt.Rows[0][0] != null && configdt.Rows[0][1] != null) if (configdt.Rows[0][1].ToString().Trim().Length > ConstNum.Zero)
                            {
                                CommonCache.ConfigCaches.Add(configdt.Rows[0][0].ToString().ToUpper(), configdt.Rows[0][1].ToString());
                                exitf = false;
                            }
                if (exitf) return Result.ERROR(0, "未找到相关配置数据");
            }
            bll = new CoreBLL((id).ToUpper());

            string sqlstr = BaseSQL.SQL_SysPageService_List;//@"SELECT[ServiceEvents],[ReturnType]FROM[dbo].[SysPageService]where  [IsEnabled]=1 and [IsDeleted]=0 and [servicename]=N'{0}'";

            if (version != null) if (version.Trim().Length > 1)
                {
                    sqlstr = sqlstr + " and [version]=N'" + version + "'  ";
                }
            if (methodname != null) if (methodname.Trim().Length > 1)
                {
                    sqlstr = sqlstr + " and [methodname]=N'" + methodname + "'  ";
                }
            sqlstr = sqlstr.Replace("{0}", servicename); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");

            var dt = cbll.db.ExecuteDataTable(sqlstr.Str2SQL());
            if (dt != null) if (dt.Rows.Count > ConstNum.Zero) if (dt.Rows[0][0] != null) if (dt.Rows[0][0].ToString().Trim().Length > 10)
                        {
                            sqlstr = dt.Rows[0][0].ToString();

                            if (dic != null) if (dic.Count > ConstNum.Zero)
                                {
                                    foreach (var itm in dic)
                                    {
                                        if (itm.Value != null)
                                        {
                                            sqlstr = sqlstr.Replace("{" + itm.Key.ToUpper() + "}", itm.Value.Replace("'", "''")).Replace("{" + itm.Key.ToLower() + "}", itm.Value.Replace("'", "''"));
                                        }
                                    }
                                }
                            if (dt.Rows[0][1] == null)
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = bll.db.ExecuteDataTable(sqlstr.Str2SQL());

                                return new Result(rsdt.ToListDictionary());
                            }

                            if (dt.Rows[0][1].ToString() == "0" || dt.Rows[0][1].ToString().ToLower() == "table" || dt.Rows[0][1].ToString().ToLower() == "list")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = bll.db.ExecuteDataTable(sqlstr.Str2SQL());

                                return new Result(rsdt.ToListDictionary());
                            }
                            if (dt.Rows[0][1].ToString() == "object")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = bll.db.ExecuteDataTable(sqlstr.Str2SQL());

                                if (rsdt == null)
                                {
                                    return new Result(null); ;
                                }
                                if (rsdt.Rows.Count == 0)
                                {
                                    return new Result(0);
                                }
                                if (rsdt.Columns.Count == 0)
                                {
                                    return new Result(0);
                                }
                                if (rsdt.Rows.Count > 0 && rsdt.Columns.Count > 0)
                                {
                                    return new Result(rsdt.Rows[0][0]);
                                }
                                return new Result(null);
                            }
                            if (dt.Rows[0][1].ToString() == "bool")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = bll.db.Execute(sqlstr.Str2SQL());
                                return new Result(rsdt > 0);
                            }
                            if (dt.Rows[0][1].ToString() == "int")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = bll.db.Execute(sqlstr.Str2SQL());
                                return new Result(rsdt);
                            }
                            if (dt.Rows[0][1].ToString() == "WEBAPI")
                            {
                                return new Result(WebAPIHelper.CallAPI(sqlstr, dic)); ;
                            }
                            else
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = bll.db.ExecuteDataTable(sqlstr.Str2SQL());
                                return new Result(rsdt.ToListDictionary());
                            }



                        }

            return Result.ERROR(0, "操作失败！");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="version"></param>
        /// <param name="id"></param>
        /// <param name="appname"></param>
        /// <param name="servicename"></param>
        /// <param name="methodname"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public Result DoCore(string version, string id, string appname, string servicename, string methodname, Dictionary<string, string> dic)
        {
            if (id == null)
            {
                return Result.ERROR(0, "应用未注册！");
            }
            if (version == null)
            {
                version = "0";
            }
            if (version.Trim().Length == 0)
            {
                version = "0";
            }
            if (appname == null)
            {
                appname = "metu";
            }
            if (appname.Trim().Length == 0)
            {
                appname = "metu";
            }
            if (servicename == null)
            {
                servicename = "service";
            }
            if (servicename.Trim().Length == 0)
            {
                servicename = "service";
            }
            if (methodname == null)
            {
                methodname = "call";
            }
            if (methodname.Trim().Length == 0)
            {
                methodname = "call";
            }
            FileHelper.Writelog(dic, "SQL"); bool configtag = false;
            foreach (var itm in CommonCache.ConfigCaches.Keys)
            {
                if (itm.ToUpper() == id.ToUpper())
                {
                    configtag = true;
                    break;
                }
            }
            CoreBLL cbll = new CoreBLL();

            if (!configtag)
            {
                string sqlconfig = BaseSQL.SQL_GET_PAGE_DATA_LIST;//"SELECT top(1)  [ConfigName],[ConfigValue]FROM  [dbo].[SYSpageDICConfig] aa, [dbo].[SYSpageDic] bb where aa.CID=bb.CID and aa.DICID=bb.ID  and  lower(bb.SortCode)='dbconfig' and aa.[ConfigName]='{0}' and  bb.Cname=N'{1}'and  bb.appname=N'{2}' and  bb.version=N'{3}'";
                sqlconfig = sqlconfig.Replace("{0}", id);
                sqlconfig = sqlconfig.Replace("{1}", appname);
                sqlconfig = sqlconfig.Replace("{2}", appname);
                sqlconfig = sqlconfig.Replace("{3}", version);
                var configdt = cbll.db.ExecuteDataTable(sqlconfig);
                bool exitf = true;
                if (configdt != null) if (configdt.Rows.Count > ConstNum.Zero) if (configdt.Rows[0][0] != null && configdt.Rows[0][1] != null) if (configdt.Rows[0][1].ToString().Trim().Length > ConstNum.Zero)
                            {
                                CommonCache.ConfigCaches.Add(configdt.Rows[0][0].ToString().ToUpper(), configdt.Rows[0][1].ToString());
                                exitf = false;
                            }
                if (exitf) return Result.ERROR(0, "未找到相关配置数据");
            }
            bll = new CoreBLL((id).ToUpper());

            string sqlstr = BaseSQL.SQL_SysPageService_List;//@"SELECT[ServiceEvents],[ReturnType]FROM[dbo].[SysPageService]where  [IsEnabled]=1 and [IsDeleted]=0 and [servicename]=N'{0}'";

            if (version != null) if (version.Trim().Length > 1)
                {
                    sqlstr = sqlstr + " and [version]=N'" + version + "'  ";
                }
            if (methodname != null) if (methodname.Trim().Length > 1)
                {
                    sqlstr = sqlstr + " and [methodname]=N'" + methodname + "'  ";
                }
            sqlstr = sqlstr.Replace("{0}", servicename);
            sqlstr = sqlstr.ToLower().Replace("=n'", "=N'");
            sqlstr = sqlstr.Replace(",n'", ",N'");

            var dt = cbll.db.ExecuteDataTable(sqlstr.Str2SQL());
            if (dt != null) if (dt.Rows.Count > ConstNum.Zero) if (dt.Rows[0][0] != null) if (dt.Rows[0][0].ToString().Trim().Length > 10)
                        {
                            sqlstr = dt.Rows[0][0].ToString();

                            if (dic != null) if (dic.Count > ConstNum.Zero)
                                {
                                    foreach (var itm in dic)
                                    {
                                        if (itm.Value != null)
                                        {
                                            sqlstr = sqlstr.Replace("{" + itm.Key.ToUpper() + "}", itm.Value.Replace("'", "''")).Replace("{" + itm.Key.ToLower() + "}", itm.Value.Replace("'", "''"));
                                        }
                                    }
                                }
                            if (dt.Rows[0][1] == null)
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = bll.db.ExecuteDataTable(sqlstr.Str2SQL());

                                return new Result(rsdt.ToListDictionary());
                            }

                            if (dt.Rows[0][1].ToString() == "0" || dt.Rows[0][1].ToString().ToLower() == "table" || dt.Rows[0][1].ToString().ToLower() == "list")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = bll.db.ExecuteDataTable(sqlstr.Str2SQL());

                                return new Result(rsdt.ToListDictionary());
                            }
                            if (dt.Rows[0][1].ToString() == "object")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = bll.db.ExecuteDataTable(sqlstr.Str2SQL());

                                if (rsdt == null)
                                {
                                    return new Result(null); ;
                                }
                                if (rsdt.Rows.Count == 0)
                                {
                                    return new Result(0);
                                }
                                if (rsdt.Columns.Count == 0)
                                {
                                    return new Result(0);
                                }
                                if (rsdt.Rows.Count > 0 && rsdt.Columns.Count > 0)
                                {
                                    return new Result(rsdt.Rows[0][0]);
                                }
                                return new Result(null);
                            }
                            if (dt.Rows[0][1].ToString() == "bool")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = bll.db.Execute(sqlstr.Str2SQL());
                                return new Result(rsdt > 0);
                            }
                            if (dt.Rows[0][1].ToString() == "int")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = bll.db.Execute(sqlstr.Str2SQL());
                                return new Result(rsdt);
                            }
                            if (dt.Rows[0][1].ToString() == "WEBAPI")
                            {
                                return new Result(WebAPIHelper.CallAPI(sqlstr, dic)); ;
                            }
                            else
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = bll.db.ExecuteDataTable(sqlstr.Str2SQL());
                                return new Result(rsdt.ToListDictionary());
                            }

                        }

            return Result.ERROR(0, "操作失败！");
        }
        /// <summary>
        /// 根据数据库中的表名获取页面控件的基本信息（获取实体字段及描述）地址组合：{数据库配置名}APP/{数据库中表名}/getpageinfo[Route("{appname}/{servicename}service/{methodname}api/Reports")]
        /// </summary>
        /// <param name="tname"></param>
        /// <param name="id">数据库配置名</param>
        /// <param name="version"></param>
        /// <param name="appname">企业用户下的应用分类（ERP、CRM、WMS、MES、MTS、TMS、SSO、GPS等）</param>
        /// <param name="servicename">应该下的服务分类名</param>
        /// <param name="methodname">服务中具体业务名</param>
        /// <returns></returns>



        public dynamic Reports(string version, string id, string appname, string servicename, string methodname, string tname)
        {
            if (id == null)
            {
                return Result.ERROR(0, "应用未注册！");
            }

            if (version == null)
            {
                version = "0";
            }
            if (version.Trim().Length == 0)
            {
                version = "0";
            }
            if (appname == null)
            {
                appname = "metu";
            }
            if (appname.Trim().Length == 0)
            {
                appname = "metu";
            }
            if (servicename == null)
            {
                servicename = "service";
            }
            if (servicename.Trim().Length == 0)
            {
                servicename = "service";
            }
            if (methodname == null)
            {
                methodname = "call";
            }
            if (methodname.Trim().Length == 0)
            {
                methodname = "call";
            }
            bool configtag = false;
            foreach (var itm in CommonCache.ConfigCaches.Keys)
            {
                if (itm.ToUpper() == id.ToUpper())
                {
                    configtag = true;
                    break;
                }
            }
            CoreBLL cbll = new CoreBLL();

            if (!configtag)
            {
                string sqlconfig = BaseSQL.SQL_GET_PAGE_DATA_LIST;//"SELECT top(1)  [ConfigName],[ConfigValue]FROM  [dbo].[SYSpageDICConfig] aa, [dbo].[SYSpageDic] bb where aa.CID=bb.CID and aa.DICID=bb.ID  and  lower(bb.SortCode)='dbconfig' and aa.[ConfigName]='{0}' and  bb.Cname=N'{1}' and  bb.appname=N'{2}' and  bb.version=N'{3}'";
                sqlconfig = sqlconfig.Replace("{0}", id);
                sqlconfig = sqlconfig.Replace("{1}", appname);
                sqlconfig = sqlconfig.Replace("{2}", appname);
                sqlconfig = sqlconfig.Replace("{3}", version);
                var configdt = cbll.db.ExecuteDataTable(sqlconfig);
                bool exitf = true;
                if (configdt != null) if (configdt.Rows.Count > ConstNum.Zero) if (configdt.Rows[0][0] != null && configdt.Rows[0][1] != null) if (configdt.Rows[0][1].ToString().Trim().Length > ConstNum.Zero)
                            {
                                CommonCache.ConfigCaches.Add(configdt.Rows[0][0].ToString().ToUpper(), configdt.Rows[0][1].ToString());
                                exitf = false;
                            }
                if (exitf) return Result.ERROR(0, "未找到相关配置数据");
            }

            bll = new CoreBLL((id).ToUpper());
            string sqlstr = BaseSQL.SQL_GET_PAGE_LIST_COMMON; 
            sqlstr = sqlstr.Replace("{0}", tname);


            return bll.db.ExecuteDataTable(sqlstr.ToLower());
        }

        /// <summary>
        /// 根据数据库中的表名;获取表中的数据；参数可以为空；地址组合：{数据库配置名}APP/{数据库中表名}/getlist[Route("{appname}/{servicename}service/{methodname}api/getconfig")]
        /// </summary>
        /// <param name="dic">需要传入的参数对象</param>
        /// <param name="version"></param>
        /// <param name="id">数据库配置名</param>
        /// <param name="appname">企业用户下的应用分类（ERP、CRM、WMS、MES、MTS、TMS、SSO、GPS等）</param>
        /// <param name="servicename">应该下的服务分类名</param>
        /// <param name="methodname">服务中具体业务名</param>
        /// <returns></returns>

        public object configServices(Dictionary<string, string> dic, string version = "0", string id = "metu", string appname = "metu", string servicename = "service", string methodname = "call")
        {
            if (id == null)
            {
                return Result.ERROR(0, "应用未注册！");
            }
            if (version == null)
            {
                version = "0";
            }
            if (version.Trim().Length == 0)
            {
                version = "0";
            }
            if (appname == null)
            {
                appname = "metu";
            }
            if (appname.Trim().Length == 0)
            {
                appname = "metu";
            }
            if (servicename == null)
            {
                servicename = "service";
            }
            if (servicename.Trim().Length == 0)
            {
                servicename = "service";
            }
            if (methodname == null)
            {
                methodname = "call";
            }
            if (methodname.Trim().Length == 0)
            {
                methodname = "call";
            }
            methodname = methodname + "getconfig";
            return DoCore(version, id, appname, servicename, methodname, dic);
        }

        /// <summary>
        ///  [页面控件信息]通用接口（获取实体字段及描述）;通过配置页面英文名获取[Route("{appname}/{servicename}service/{methodname}api/getpageinfo")]
        /// </summary>
        /// <param name="version"></param>
        /// <param name="id"></param>
        /// <param name="appname"></param>
        /// <param name="servicename"></param>
        /// <param name="methodname"></param>
        /// <returns></returns>
        public object PageControlInfo(string version, string id, string appname, string servicename)
        {
            if (id == null)
            {
                return Result.ERROR(0, "应用未注册！");
            }
            if (version == null)
            {
                version = "0";
            }
            if (version.Trim().Length == 0)
            {
                version = "0";
            }
            if (appname == null)
            {
                appname = "metu";
            }
            if (appname.Trim().Length == 0)
            {
                appname = "metu";
            }
            if (servicename == null)
            {
                servicename = "service";
            }
            if (servicename.Trim().Length == 0)
            {
                servicename = "service";
            }
           
            bool configtag = false;
            foreach (var itm in CommonCache.ConfigCaches.Keys)
            {
                if (itm.ToUpper() == id.ToUpper())
                {
                    configtag = true;
                    break;
                }
            }
            CoreBLL cbll = new CoreBLL();

            if (!configtag)
            {
                string sqlconfig = BaseSQL.SQL_GET_PAGE_DATA_LIST;//"SELECT top(1)  [ConfigName],[ConfigValue]FROM  [dbo].[SYSpageDICConfig] aa, [dbo].[SYSpageDic] bb where aa.CID=bb.CID and aa.DICID=bb.ID  and  lower(bb.SortCode)='dbconfig' and aa.[ConfigName]='{0}' and  bb.Cname=N'{1}'and  bb.appname=N'{2}' and  bb.version=N'{3}'";
                sqlconfig = sqlconfig.Replace("{0}", id);
                sqlconfig = sqlconfig.Replace("{1}", appname);
                sqlconfig = sqlconfig.Replace("{2}", appname);
                sqlconfig = sqlconfig.Replace("{3}", version);
                var configdt = cbll.db.ExecuteDataTable(sqlconfig);
                bool exitf = true;
                if (configdt != null) if (configdt.Rows.Count > ConstNum.Zero) if (configdt.Rows[0][0] != null && configdt.Rows[0][1] != null) if (configdt.Rows[0][1].ToString().Trim().Length > ConstNum.Zero)
                            {
                                CommonCache.ConfigCaches.Add(configdt.Rows[0][0].ToString().ToUpper(), configdt.Rows[0][1].ToString());
                                exitf = false;
                            }
                if (exitf) return Result.ERROR(0, "未找到相关配置数据");
            }
            bll = new CoreBLL((id).ToUpper());

            string sqlstr = BaseSQL.SQL_GET_PAGECONTROL_LIST;
            sqlstr = sqlstr.Replace("{0}", servicename);
                sqlstr = sqlstr.Replace("{1}", version);
                sqlstr = sqlstr.Replace("{2}", appname);
                sqlstr = sqlstr.Replace("=n'", "=N'");
                FileHelper.Writelog(sqlstr, "SQL");
            
            return new Result(bll.db.ExecuteDataTable(sqlstr.Str2SQL()));
        }
        /// <summary>
        ///[页面控件信息内容保存到后端] 根据数据库中的中页面配置名;添加数据到指定接口；参数可以为空；地址组合：{数据库配置名}APP/{数据库中页面配置名}/add [Route("{appname}/{servicename}service/{methodname}api/save")]
        /// </summary>
        /// <param name="id">数据库配置名</param>
        /// <param name="dic">需要传入的参数对象</param>
        /// <param name="version"></param>
        /// <param name="appname">企业用户下的应用分类（ERP、CRM、WMS、MES、MTS、TMS、SSO、GPS等）</param>
        /// <param name="servicename">应该下的服务分类名</param>
        /// <param name="methodname">服务中具体业务名</param>
        /// <returns></returns>



        public object sysconfigServicessave(Dictionary<string, string> dic, string version = "0", string id = "metu", string appname = "metu", string servicename = "service", string methodname = "call")
        {
            if (id == null)
            {
                return Result.ERROR(0, "应用未注册！");
            }
            if (version == null)
            {
                version = "0";
            }
            if (version.Trim().Length == 0)
            {
                version = "0";
            }
            if (appname == null)
            {
                appname = "metu";
            }
            if (appname.Trim().Length == 0)
            {
                appname = "metu";
            }
            if (servicename == null)
            {
                servicename = "service";
            }
            if (servicename.Trim().Length == 0)
            {
                servicename = "service";
            }
            if (methodname == null)
            {
                methodname = "call";
            }
            if (methodname.Trim().Length == 0)
            {
                methodname = "call";
            }
            methodname = methodname + "save";
            return DoCore(version, id, appname, servicename, methodname, dic);
        }

        /// <summary>
        /// [根据页面控件信息获取对应的数据]根据数据库中的中页面配置名;通过接口更新数据；参数可以为空；地址组合：{数据库配置名}APP/{数据库中页面配置名}/edit[Route("{appname}/{servicename}service/{methodname}api/get")]
        /// </summary>
        /// <param name="id">数据库配置名</param>
        /// <param name="dic">需要传入的参数对象</param>
        /// <param name="version"></param>
        /// <param name="appname">企业用户下的应用分类（ERP、CRM、WMS、MES、MTS、TMS、SSO、GPS等）</param>
        /// <param name="servicename">应该下的服务分类名</param>
        /// <param name="methodname">服务中具体业务名</param>
        /// <returns></returns>


        public object sysconfigServicesgets(Dictionary<string, string> dic, string version = "0", string id = "metu", string appname = "metu", string servicename = "service", string methodname = "call")
        {
            if (id == null)
            {
                return Result.ERROR(0, "应用未注册！");
            }
            if (version == null)
            {
                version = "0";
            }
            if (version.Trim().Length == 0)
            {
                version = "0";
            }
            if (appname == null)
            {
                appname = "metu";
            }
            if (appname.Trim().Length == 0)
            {
                appname = "metu";
            }
            if (servicename == null)
            {
                servicename = "service";
            }
            if (servicename.Trim().Length == 0)
            {
                servicename = "service";
            }
            if (methodname == null)
            {
                methodname = "call";
            }
            if (methodname.Trim().Length == 0)
            {
                methodname = "call";
            }
            methodname = methodname + "get";
            return DoCore(version, id, appname, servicename, methodname, dic);
        }

        /// <summary>
        /// [列表]根据数据库中的表名;获取表中的数据；参数可以为空；地址组合：{数据库配置名}APP/{数据库中页面配置名}/sysgetlist[Route("{appname}/{servicename}service/{methodname}api/getlist")]
        /// </summary>
        /// <param name="id">数据库配置名</param>
        /// <param name="dic">需要传入的参数对象</param>
        /// <param name="version"></param>
        /// <param name="appname">企业用户下的应用分类（ERP、CRM、WMS、MES、MTS、TMS、SSO、GPS等）</param>
        /// <param name="servicename">应该下的服务分类名</param>
        /// <param name="methodname">服务中具体业务名</param>
        /// <returns></returns>



        public object sysconfigServices(Dictionary<string, string> dic, string version = "0", string id = "metu", string appname = "metu", string servicename = "service", string methodname = "call")
        {
            if (id == null)
            {
                return Result.ERROR(0, "应用未注册！");
            }
            if (version == null)
            {
                version = "0";
            }
            if (version.Trim().Length == 0)
            {
                version = "0";
            }
            if (appname == null)
            {
                appname = "metu";
            }
            if (appname.Trim().Length == 0)
            {
                appname = "metu";
            }
            if (servicename == null)
            {
                servicename = "service";
            }
            if (servicename.Trim().Length == 0)
            {
                servicename = "service";
            }
            if (methodname == null)
            {
                methodname = "call";
            }
            if (methodname.Trim().Length == 0)
            {
                methodname = "call";
            }
            methodname = methodname + "getlist";
            return DoCore(version, id, appname, servicename, methodname, dic);
        }


        /// <summary>
        ///[添加] 根据数据库中的中页面配置名;添加数据到指定接口；参数可以为空；地址组合：{数据库配置名}APP/{数据库中页面配置名}/add [Route("{appname}/{servicename}service/{methodname}api/add")]
        /// </summary>
        /// <param name="id">数据库配置名</param>
        /// <param name="dic">需要传入的参数对象</param>
        /// <param name="version"></param>
        /// <param name="appname">企业用户下的应用分类（ERP、CRM、WMS、MES、MTS、TMS、SSO、GPS等）</param>
        /// <param name="servicename">应该下的服务分类名</param>
        /// <param name="methodname">服务中具体业务名</param>
        /// <returns></returns>


        public object sysconfigServicesadd(Dictionary<string, string> dic, string version = "0", string id = "metu", string appname = "metu", string servicename = "service", string methodname = "call")
        {
            if (id == null)
            {
                return Result.ERROR(0, "应用未注册！");
            }
            if (version == null)
            {
                version = "0";
            }
            if (version.Trim().Length == 0)
            {
                version = "0";
            }
            if (appname == null)
            {
                appname = "metu";
            }
            if (appname.Trim().Length == 0)
            {
                appname = "metu";
            }
            if (servicename == null)
            {
                servicename = "service";
            }
            if (servicename.Trim().Length == 0)
            {
                servicename = "service";
            }
            if (methodname == null)
            {
                methodname = "call";
            }
            if (methodname.Trim().Length == 0)
            {
                methodname = "call";
            }
            methodname = methodname + "add";
            return DoCore(version, id, appname, servicename, methodname, dic);
        }

        /// <summary>
        /// [更新]根据数据库中的中页面配置名;通过接口更新数据；参数可以为空；地址组合：{数据库配置名}APP/{数据库中页面配置名}/edit[Route("{appname}/{servicename}service/{methodname}api/edit")]
        /// </summary>
        /// <param name="id">数据库配置名</param>
        /// <param name="dic">需要传入的参数对象</param>
        /// <param name="version"></param>
        /// <param name="appname">企业用户下的应用分类（ERP、CRM、WMS、MES、MTS、TMS、SSO、GPS等）</param>
        /// <param name="servicename">应该下的服务分类名</param>
        /// <param name="methodname">服务中具体业务名</param>
        /// <returns></returns>



        public object sysconfigServicesedit(Dictionary<string, string> dic, string version = "0", string id = "metu", string appname = "metu", string servicename = "service", string methodname = "call")
        {
            if (id == null)
            {
                return Result.ERROR(0, "应用未注册！");
            }
            if (version == null)
            {
                version = "0";
            }
            if (version.Trim().Length == 0)
            {
                version = "0";
            }
            if (appname == null)
            {
                appname = "metu";
            }
            if (appname.Trim().Length == 0)
            {
                appname = "metu";
            }
            if (servicename == null)
            {
                servicename = "service";
            }
            if (servicename.Trim().Length == 0)
            {
                servicename = "service";
            }
            if (methodname == null)
            {
                methodname = "call";
            }
            if (methodname.Trim().Length == 0)
            {
                methodname = "call";
            }
            methodname = methodname + "edit";
            return DoCore(version, id, appname, servicename, methodname, dic);
        }
        /// <summary>
        /// [更新]根据数据库中的中页面配置名;通过接口更新数据；参数可以为空；地址组合：{数据库配置名}APP/{数据库中页面配置名}/edit[Route("{appname}/{servicename}service/{methodname}api/UPDATE")]
        /// </summary>
        /// <param name="id">数据库配置名</param>
        /// <param name="dic">需要传入的参数对象</param>
        /// <param name="version"></param>
        /// <param name="appname">企业用户下的应用分类（ERP、CRM、WMS、MES、MTS、TMS、SSO、GPS等）</param>
        /// <param name="servicename">应该下的服务分类名</param>
        /// <param name="methodname">服务中具体业务名</param>
        /// <returns></returns>



        public object sysconfigServicesupdate(Dictionary<string, string> dic, string version = "0", string id = "metu", string appname = "metu", string servicename = "service", string methodname = "call")
        {
            if (id == null)
            {
                return Result.ERROR(0, "应用未注册！");
            }
            if (version == null)
            {
                version = "0";
            }
            if (version.Trim().Length == 0)
            {
                version = "0";
            }
            if (appname == null)
            {
                appname = "metu";
            }
            if (appname.Trim().Length == 0)
            {
                appname = "metu";
            }
            if (servicename == null)
            {
                servicename = "service";
            }
            if (servicename.Trim().Length == 0)
            {
                servicename = "service";
            }
            if (methodname == null)
            {
                methodname = "call";
            }
            if (methodname.Trim().Length == 0)
            {
                methodname = "call";
            }
            methodname = methodname + "update";
            return DoCore(version, id, appname, servicename, methodname, dic);
        }
        /// <summary>
        /// [删除]根据数据库中的中页面配置名;通过接口删除（逻辑删除）指定数据；参数可以为空；地址组合：{数据库配置名}APP/{数据库中页面配置名}/delete[Route("{appname}/{servicename}service/{methodname}api/delete")]
        /// </summary>
        /// <param name="id">数据库配置名</param>
        /// <param name="dic">需要传入的参数对象</param>
        /// <param name="version"></param>
        /// <param name="appname">企业用户下的应用分类（ERP、CRM、WMS、MES、MTS、TMS、SSO、GPS等）</param>
        /// <param name="servicename">应该下的服务分类名</param>
        /// <param name="methodname">服务中具体业务名</param>
        /// <returns></returns>



        public object sysconfigServicesdelete(Dictionary<string, string> dic, string version = "0", string id = "metu", string appname = "metu", string servicename = "service", string methodname = "call")
        {
            if (id == null)
            {
                return Result.ERROR(0, "应用未注册！");
            }
            if (version == null)
            {
                version = "0";
            }
            if (version.Trim().Length == 0)
            {
                version = "0";
            }
            if (appname == null)
            {
                appname = "metu";
            }
            if (appname.Trim().Length == 0)
            {
                appname = "metu";
            }
            if (servicename == null)
            {
                servicename = "service";
            }
            if (servicename.Trim().Length == 0)
            {
                servicename = "service";
            }
            if (methodname == null)
            {
                methodname = "call";
            }
            if (methodname.Trim().Length == 0)
            {
                methodname = "call";
            }
            methodname = methodname + "delete";
            return DoCore(version, id, appname, servicename, methodname, dic);
        }
        /// <summary>
        /// [删除]根据数据库中的中页面配置名;通过接口删除(物理删除)指定数据；参数可以为空；地址组合：{数据库配置名}APP/{数据库中页面配置名}/delete [Route("{appname}/{servicename}service/{methodname}api/removebykey")]
        /// </summary>
        /// <param name="id">数据库配置名</param>
        /// <param name="dic">需要传入的参数对象</param>
        /// <param name="version"></param>
        /// <param name="appname">企业用户下的应用分类（ERP、CRM、WMS、MES、MTS、TMS、SSO、GPS等）</param>
        /// <param name="servicename">应该下的服务分类名</param>
        /// <param name="methodname">服务中具体业务名</param>
        /// <returns></returns>


        public object sysconfigServicesremovebykey(Dictionary<string, string> dic, string version = "0", string id = "metu", string appname = "metu", string servicename = "service", string methodname = "call")
        {
            if (id == null)
            {
                return Result.ERROR(0, "应用未注册！");
            }
            if (version == null)
            {
                version = "0";
            }
            if (version.Trim().Length == 0)
            {
                version = "0";
            }
            if (appname == null)
            {
                appname = "metu";
            }
            if (appname.Trim().Length == 0)
            {
                appname = "metu";
            }
            if (servicename == null)
            {
                servicename = "service";
            }
            if (servicename.Trim().Length == 0)
            {
                servicename = "service";
            }
            if (methodname == null)
            {
                methodname = "call";
            }
            if (methodname.Trim().Length == 0)
            {
                methodname = "call";
            }
            methodname = methodname + "removebykey";
            return DoCore(version, id, appname, servicename, methodname, dic);
        }
        /// <summary>
        /// [获取数据]根据数据库中的中页面配置名;通过接口获取指定数据；参数可以为空；地址组合：{数据库配置名}APP/{数据库中页面配置名}/get [Route("{appname}/{servicename}service/{methodname}api/gets")]
        /// </summary>
        /// <param name="id">数据库配置名</param>
        /// <param name="dic">需要传入的参数对象</param>
        /// <param name="version"></param>
        /// <param name="appname">企业用户下的应用分类（ERP、CRM、WMS、MES、MTS、TMS、SSO、GPS等）</param>
        /// <param name="servicename">应该下的服务分类名</param>
        /// <param name="methodname">服务中具体业务名</param>
        /// <returns></returns>



        public object sysconfigServicesget(Dictionary<string, string> dic, string version = "0", string id = "metu", string appname = "metu", string servicename = "service", string methodname = "call")
        {
            if (id == null)
            {
                return Result.ERROR(0, "应用未注册！");
            }
            if (version == null)
            {
                version = "0";
            }
            if (version.Trim().Length == 0)
            {
                version = "0";
            }
            if (appname == null)
            {
                appname = "metu";
            }
            if (appname.Trim().Length == 0)
            {
                appname = "metu";
            }
            if (servicename == null)
            {
                servicename = "service";
            }
            if (servicename.Trim().Length == 0)
            {
                servicename = "service";
            }
            if (methodname == null)
            {
                methodname = "call";
            }
            if (methodname.Trim().Length == 0)
            {
                methodname = "call";
            }
            methodname = methodname + "gets";
            return DoCore(version, id, appname, servicename, methodname, dic);
        }

        /// <summary>
        /// [获取数据]根据数据库中的中页面配置名;通过接口获取指定数据；参数可以为空；地址组合：{数据库配置名}APP/{数据库中页面配置名}/get [Route("{appname}/{servicename}service/{methodname}api/list")]
        /// </summary>
        /// <param name="id">数据库配置名</param>
        /// <param name="dic">需要传入的参数对象</param>
        /// <param name="version"></param>
        /// <param name="appname">企业用户下的应用分类（ERP、CRM、WMS、MES、MTS、TMS、SSO、GPS等）</param>
        /// <param name="servicename">应该下的服务分类名</param>
        /// <param name="methodname">服务中具体业务名</param>
        /// <returns></returns>


        public object sysconfigServiceslist(Dictionary<string, string> dic, string version = "0", string id = "metu", string appname = "metu", string servicename = "service", string methodname = "call")
        {
            if (id == null)
            {
                return Result.ERROR(0, "应用未注册！");
            }
            if (version == null)
            {
                version = "0";
            }
            if (version.Trim().Length == 0)
            {
                version = "0";
            }
            if (appname == null)
            {
                appname = "metu";
            }
            if (appname.Trim().Length == 0)
            {
                appname = "metu";
            }
            if (servicename == null)
            {
                servicename = "service";
            }
            if (servicename.Trim().Length == 0)
            {
                servicename = "service";
            }
            if (methodname == null)
            {
                methodname = "call";
            }
            if (methodname.Trim().Length == 0)
            {
                methodname = "call";
            }
            methodname = methodname + "list";
            return DoCore(version, id, appname, servicename, methodname, dic);
        }
        /// <summary>
        /// 根据数据库配置，服务配置 执行对应的业务逻辑；地址组合：{数据库配置名}APP/{数据库中表SysPageService中字段servicename服务名}Service [Route("{appname}/{servicename}service/{methodname}api/Service")]
        /// </summary>
        /// <param name="id">数据库配置名</param>
        /// <param name="dic">需要传入的参数对象</param>
        /// <param name="version"></param>
        /// <param name="appname">企业用户下的应用分类（ERP、CRM、WMS、MES、MTS、TMS、SSO、GPS等）</param>
        /// <param name="servicename">应该下的服务分类名</param>
        /// <param name="methodname">服务中具体业务名</param>
        /// <returns></returns>



        public object Services(Dictionary<string, string> dic, string version = "0", string id = "metu", string appname = "metu", string servicename = "service", string methodname = "call")
        {
            if (id == null)
            {
                return Result.ERROR(0, "应用未注册！");
            }
            if (version == null)
            {
                version = "0";
            }
            if (version.Trim().Length == 0)
            {
                version = "0";
            }
            if (appname == null)
            {
                appname = "metu";
            }
            if (appname.Trim().Length == 0)
            {
                appname = "metu";
            }
            if (servicename == null)
            {
                servicename = "service";
            }
            if (servicename.Trim().Length == 0)
            {
                servicename = "service";
            }
            if (methodname == null)
            {
                methodname = "call";
            }
            if (methodname.Trim().Length == 0)
            {
                methodname = "call";
            }
            methodname = methodname + "service";
            return DoCore(version, id, appname, servicename, methodname, dic);
        }

        /// <summary>
        /// 不同系统模块间服务的调用 [Route("{ctrl}/{actname}")]
        /// </summary>
        /// <param name="id">数据库配置名</param>
        /// <param name="ctrl">控制器名</param>
        /// <param name="actname">方法名</param>
        /// <param name="model">需要传入的参数对象</param>
        /// <param name="version"></param>
        /// <param name="appname">企业用户下的应用分类（ERP、CRM、WMS、MES、MTS、TMS、SSO、GPS等）</param>
        /// <param name="servicename">应该下的服务分类名</param>
        /// <param name="methodname">服务中具体业务名</param>
        /// <returns></returns>



        public object Login(string actname, Dictionary<string, object> model, string ctrl, string version = "0", string id = "metu", string appname = "metu", string servicename = "metu", string methodname = "call")
        {
            if (id == null)
            {
                return Result.ERROR(0, "应用未注册！");
            }
            if (version == null)
            {
                version = "0";
            }
            if (version.Trim().Length == 0)
            {
                version = "0";
            }
            if (appname == null)
            {
                appname = "metu";
            }
            if (appname.Trim().Length == 0)
            {
                appname = "metu";
            }
            if (servicename == null)
            {
                servicename = "service";
            }
            if (servicename.Trim().Length == 0)
            {
                servicename = "service";
            }
            if (methodname == null)
            {
                methodname = "call";
            }
            if (methodname.Trim().Length == 0)
            {
                methodname = "call";
            }
            CommonCache.ConfigCaches[id.ToUpper()].validatedIsString("请确认是否已经配置好对应的接口地址！");
            string urlroot = CommonCache.ConfigCaches[id.ToUpper()].ToString();
            urlroot = urlroot + ctrl + "/" + actname;

            var rs = WebAPIHelper.CallAPI(urlroot, model);
            return rs;
        }
        /// <summary>
        /// 外部接口地址的调用 [Route("{ctrl}/webservice")]
        /// </summary>
        /// <param name="id">数据库配置名</param>
        /// <param name="version"></param>
        /// <param name="appname">企业用户下的应用分类（ERP、CRM、WMS、MES、MTS、TMS、SSO、GPS等）</param>
        /// <param name="servicename">应该下的服务分类名</param>
        /// <param name="methodname">服务中具体业务名</param>
        /// <param name="ctrl"></param>
        /// <param name="model"></param>
        /// <returns></returns>




        public object Loginwebservice(Dictionary<string, object> model, string ctrl, string version = "0", string id = "metu", string appname = "metu", string servicename = "metu", string methodname = "call")
        {
            if (id == null)
            {
                return Result.ERROR(0, "应用未注册！");
            }
            if (version == null)
            {
                version = "0";
            }
            if (version.Trim().Length == 0)
            {
                version = "0";
            }
            if (appname == null)
            {
                appname = "metu";
            }
            if (appname.Trim().Length == 0)
            {
                appname = "metu";
            }
            if (servicename == null)
            {
                servicename = "service";
            }
            if (servicename.Trim().Length == 0)
            {
                servicename = "service";
            }
            if (methodname == null)
            {
                methodname = "call";
            }
            if (methodname.Trim().Length == 0)
            {
                methodname = "call";
            }
            CommonCache.ConfigCaches[id.ToUpper()].validatedIsString("请确认是否已经配置好对应的接口地址！");
            string urlroot = CommonCache.ConfigCaches[id.ToUpper()].ToString();
            urlroot = urlroot + ctrl.Replace("--", "/");

            var rs = WebAPIHelper.CallAPI(urlroot, model);
            return rs;
        }

        /// <summary>
        /// 外部接口地址的调用 [Route("{ctrl}API")]
        /// </summary>
        /// <param name="id">数据库配置名</param>
        /// <param name="version"></param>
        /// <param name="appname">企业用户下的应用分类（ERP、CRM、WMS、MES、MTS、TMS、SSO、GPS等）</param>
        /// <param name="servicename">应该下的服务分类名</param>
        /// <param name="methodname">服务中具体业务名</param>
        /// <param name="ctrl"></param>
        /// <param name="model"></param>
        /// <returns></returns>



        public object Callwebservice(Dictionary<string, object> model, string ctrl, string version = "0", string id = "metu", string appname = "metu", string servicename = "metu", string methodname = "call")
        {
            if (id == null)
            {
                return Result.ERROR(0, "应用未注册！");
            }
            if (version == null)
            {
                version = "0";
            }
            if (version.Trim().Length == 0)
            {
                version = "0";
            }
            if (appname == null)
            {
                appname = "metu";
            }
            if (appname.Trim().Length == 0)
            {
                appname = "metu";
            }
            if (servicename == null)
            {
                servicename = "service";
            }
            if (servicename.Trim().Length == 0)
            {
                servicename = "service";
            }
            if (methodname == null)
            {
                methodname = "call";
            }
            if (methodname.Trim().Length == 0)
            {
                methodname = "call";
            }
            CommonCache.ConfigCaches[id.ToUpper()].validatedIsString("请确认是否已经配置好对应的接口地址！");
            string urlroot = CommonCache.ConfigCaches[id.ToUpper()].ToString();
            urlroot = urlroot + ctrl.Replace("--", "/");

            var rs = WebAPIHelper.CallAPI(urlroot, model);

            return rs;
        }

        /// <summary>
        /// 多ＳＱＬ语句执行，非事务；服务名：{ctrl}bntservice [Route("{appname}/{servicename}service/{methodname}api/{ctrl}/bntservice")]
        /// </summary>
        /// <param name="id">数据库配置名</param>
        /// <param name="version"></param>
        /// <param name="appname">企业用户下的应用分类（ERP、CRM、WMS、MES、MTS、TMS、SSO、GPS等）</param>
        /// <param name="servicename">应该下的服务分类名</param>
        /// <param name="methodname">服务中具体业务名</param>
        /// <param name="ctrl"></param>
        /// <param name="model"></param>
        /// <returns></returns>




        public Result BatServiceNotTransRecords(Dictionary<string, object> model, string ctrl, string version = "0", string id = "metu", string appname = "metu", string servicename = "metu", string methodname = "call")
        {
            if (id == null)
            {
                return Result.ERROR(0, "应用未注册！");
            }
            if (version == null)
            {
                version = "0";
            }
            if (version.Trim().Length == 0)
            {
                version = "0";
            }
            if (appname == null)
            {
                appname = "metu";
            }
            if (appname.Trim().Length == 0)
            {
                appname = "metu";
            }
            if (servicename == null)
            {
                servicename = "service";
            }
            if (servicename.Trim().Length == 0)
            {
                servicename = "service";
            }
            if (methodname == null)
            {
                methodname = "call";
            }
            if (methodname.Trim().Length == 0)
            {
                methodname = "call";
            }
            bool configtag = false;
            foreach (var itm in CommonCache.ConfigCaches.Keys)
            {
                if (itm.ToUpper() == id.ToUpper())
                {
                    configtag = true;
                    break;
                }
            }
            CoreBLL cbll = new CoreBLL();

            if (!configtag)
            {
                string sqlconfig = BaseSQL.SQL_GET_PAGE_DATA_LIST;//"SELECT top(1)  [ConfigName],[ConfigValue]FROM  [dbo].[SYSpageDICConfig] aa, [dbo].[SYSpageDic] bb where aa.CID=bb.CID and aa.DICID=bb.ID  and  lower(bb.SortCode)='dbconfig' and aa.[ConfigName]='{0}' and  bb.Cname=N'{1}'and  bb.appname=N'{2}' and  bb.version=N'{3}'";
                sqlconfig = sqlconfig.Replace("{0}", id);
                sqlconfig = sqlconfig.Replace("{1}", appname);
                sqlconfig = sqlconfig.Replace("{2}", appname);
                sqlconfig = sqlconfig.Replace("{3}", version);
                var configdt = cbll.db.ExecuteDataTable(sqlconfig);
                bool exitf = true;
                if (configdt != null) if (configdt.Rows.Count > ConstNum.Zero) if (configdt.Rows[0][0] != null && configdt.Rows[0][1] != null) if (configdt.Rows[0][1].ToString().Trim().Length > ConstNum.Zero)
                            {
                                CommonCache.ConfigCaches.Add(configdt.Rows[0][0].ToString().ToUpper(), configdt.Rows[0][1].ToString());
                                exitf = false;
                            }
                if (exitf) return Result.ERROR(0, "未找到相关配置数据");
            }

            if (model == null)
            {
                MSGBox.EXMessage("传入参数不可以为空！");
            }
            var dic = model.ToDictionary();

            bll = new CoreBLL((id).ToUpper());

            string sqlstr = BaseSQL.SQL_SysPageService_List;//@"SELECT[ServiceEvents],[ReturnType]FROM[dbo].[SysPageService]where [servicename]='{0}bntservice'";
            string result = "";
            sqlstr = sqlstr.Replace("{0}", ctrl); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
            FileHelper.Writelog(sqlstr, "SQL");
            var dt = bll.db.ExecuteDataTable(sqlstr.ToLower());
            if (dt != null) if (dt.Rows.Count > ConstNum.Zero)
                {
                    List<string> sqltrans = new List<string>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i][0] != null) if (dt.Rows[i][0].ToString().Trim().Length > 10)
                            {
                                sqlstr = dt.Rows[i][0].ToString();

                                if (dic != null) if (dic.Count > ConstNum.Zero)
                                    {
                                        foreach (var itm in dic)
                                        {
                                            if (itm.Value != null)
                                            {
                                                sqlstr = sqlstr.Replace("{" + itm.Key.ToUpper() + "}", itm.Value.Replace("'", "''")).Replace("{" + itm.Key.ToLower() + "}", itm.Value.Replace("'", "''"));
                                            }
                                            else
                                            {

                                            }
                                        }
                                    }

                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = bll.db.Execute(sqlstr.Str2SQL());


                                if (rsdt < 0)
                                {
                                    result = "业务执行失败！";
                                }


                            }
                    }



                }


            return new Result(result);

        }


        /// <summary>
        /// SQL事务执行；服务名：{ctrl}sqlexecute [Route("{appname}/{servicename}service/{methodname}api/{ctrl}/sqlexecute")]
        /// </summary>
        /// <param name="id">数据库配置名</param>
        /// <param name="version"></param>
        /// <param name="appname">企业用户下的应用分类（ERP、CRM、WMS、MES、MTS、TMS、SSO、GPS等）</param>
        /// <param name="servicename">应该下的服务分类名</param>
        /// <param name="methodname">服务中具体业务名</param>
        /// <param name="ctrl"></param>
        /// <param name="model"></param>
        /// <returns></returns>




        public Result ExecuteSQLServiceNotTransRecords(Dictionary<string, object> model, string ctrl, string version = "0", string id = "metu", string appname = "metu", string servicename = "metu", string methodname = "call")
        {
            if (id == null)
            {
                return Result.ERROR(0, "应用未注册！");
            }
            if (version == null)
            {
                version = "0";
            }
            if (version.Trim().Length == 0)
            {
                version = "0";
            }
            if (appname == null)
            {
                appname = "metu";
            }
            if (appname.Trim().Length == 0)
            {
                appname = "metu";
            }
            if (servicename == null)
            {
                servicename = "service";
            }
            if (servicename.Trim().Length == 0)
            {
                servicename = "service";
            }
            if (methodname == null)
            {
                methodname = "call";
            }
            if (methodname.Trim().Length == 0)
            {
                methodname = "call";
            }
            bool configtag = false;
            foreach (var itm in CommonCache.ConfigCaches.Keys)
            {
                if (itm.ToUpper() == id.ToUpper())
                {
                    configtag = true;
                    break;
                }
            }
            CoreBLL cbll = new CoreBLL();

            if (!configtag)
            {
                string sqlconfig = BaseSQL.SQL_GET_PAGE_DATA_LIST;//"SELECT top(1)  [ConfigName],[ConfigValue]FROM  [dbo].[SYSpageDICConfig] aa, [dbo].[SYSpageDic] bb where aa.CID=bb.CID and aa.DICID=bb.ID  and  lower(bb.SortCode)='dbconfig' and aa.[ConfigName]='{0}' and  bb.Cname=N'{1}'and  bb.appname=N'{2}' and  bb.version=N'{3}'";
                sqlconfig = sqlconfig.Replace("{0}", id);
                sqlconfig = sqlconfig.Replace("{1}", appname);
                sqlconfig = sqlconfig.Replace("{2}", appname);
                sqlconfig = sqlconfig.Replace("{3}", version);
                var configdt = cbll.db.ExecuteDataTable(sqlconfig);
                bool exitf = true;
                if (configdt != null) if (configdt.Rows.Count > ConstNum.Zero) if (configdt.Rows[0][0] != null && configdt.Rows[0][1] != null) if (configdt.Rows[0][1].ToString().Trim().Length > ConstNum.Zero)
                            {
                                CommonCache.ConfigCaches.Add(configdt.Rows[0][0].ToString().ToUpper(), configdt.Rows[0][1].ToString());
                                exitf = false;
                            }
                if (exitf) return Result.ERROR(0, "未找到相关配置数据");
            }

            if (model == null)
            {
                MSGBox.EXMessage("传入参数不可以为空！");
            }
            var dic = model.ToDictionary();

            bll = new CoreBLL((id).ToUpper());

            string sqlstr = BaseSQL.SQL_SysPageService_List;//@"SELECT[ServiceEvents],[ReturnType]FROM[dbo].[SysPageService]where [servicename]='{0}sqlexecute'";
            string result = "";
            sqlstr = sqlstr.Replace("{0}", ctrl); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
            FileHelper.Writelog(sqlstr, "SQL");
            var dt = bll.db.ExecuteDataTable(sqlstr.ToLower());
            if (dt != null) if (dt.Rows.Count > ConstNum.Zero)
                {

                    if (dt.Rows[0][0] != null) if (dt.Rows[0][0].ToString().Trim().Length > 10)
                        {
                            sqlstr = dt.Rows[0][0].ToString();

                            if (dic != null) if (dic.Count > ConstNum.Zero)
                                {
                                    foreach (var itm in dic)
                                    {
                                        if (itm.Value != null)
                                        {
                                            sqlstr = sqlstr.Replace("{" + itm.Key.ToUpper() + "}", itm.Value.Replace("'", "''")).Replace("{" + itm.Key.ToLower() + "}", itm.Value.Replace("'", "''"));
                                        }
                                        else
                                        {

                                        }
                                    }
                                }

                            FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                            var rsdt = bll.db.Execute(sqlstr.Str2SQL());


                            if (rsdt < 0)
                            {
                                result = "业务执行失败！";
                            }


                        }



                }

            return new Result(result);

        }

        /// <summary>
        /// 通用导出功能 [Route("{appname}/{servicename}service/{methodname}api/{ctrl}template")]
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ctrl"></param>
        /// <param name="file"></param>
        /// <param name="version"></param>
        /// <param name="appname">企业用户下的应用分类（ERP、CRM、WMS、MES、MTS、TMS、SSO、GPS等）</param>
        /// <param name="servicename">应该下的服务分类名</param>
        /// <param name="methodname">服务中具体业务名</param>
        /// <returns></returns>
        public Result ReplaceTemplate(Dictionary<string, string> file, string ctrl, string version = "0", string id = "metu", string appname = "metu", string servicename = "service", string methodname = "call")
        {

            if (id == null)
            {
                return Result.ERROR(0, "应用未注册！");
            }
            if (version == null)
            {
                version = "0";
            }
            if (version.Trim().Length == 0)
            {
                version = "0";
            }
            if (appname == null)
            {
                appname = "metu";
            }
            if (appname.Trim().Length == 0)
            {
                appname = "metu";
            }
            if (servicename == null)
            {
                servicename = "service";
            }
            if (servicename.Trim().Length == 0)
            {
                servicename = "service";
            }
            if (methodname == null)
            {
                methodname = "call";
            }
            if (methodname.Trim().Length == 0)
            {
                methodname = "call";
            }
            bool configtag = false;
            foreach (var itm in CommonCache.ConfigCaches.Keys)
            {
                if (itm.ToUpper() == id.ToUpper())
                {
                    configtag = true;
                    break;
                }
            }
            CoreBLL cbll = new CoreBLL();

            if (!configtag)
            {
                string sqlconfig = BaseSQL.SQL_GET_PAGE_DATA_LIST;//"SELECT top(1)  [ConfigName],[ConfigValue]FROM  [dbo].[SYSpageDICConfig] aa, [dbo].[SYSpageDic] bb where aa.CID=bb.CID and aa.DICID=bb.ID  and  lower(bb.SortCode)='dbconfig' and aa.[ConfigName]='{0}' and  bb.Cname=N'{1}'and  bb.appname=N'{2}' and  bb.version=N'{3}'";
                sqlconfig = sqlconfig.Replace("{0}", id);
                sqlconfig = sqlconfig.Replace("{1}", appname);
                sqlconfig = sqlconfig.Replace("{2}", appname);
                sqlconfig = sqlconfig.Replace("{3}", version);
                var configdt = cbll.db.ExecuteDataTable(sqlconfig);
                bool exitf = true;
                if (configdt != null) if (configdt.Rows.Count > ConstNum.Zero) if (configdt.Rows[0][0] != null && configdt.Rows[0][1] != null) if (configdt.Rows[0][1].ToString().Trim().Length > ConstNum.Zero)
                            {
                                CommonCache.ConfigCaches.Add(configdt.Rows[0][0].ToString().ToUpper(), configdt.Rows[0][1].ToString());
                                exitf = false;
                            }
                if (exitf) return Result.ERROR(0, "未找到相关配置数据");
            }

            string path = AppDomain.CurrentDomain.BaseDirectory + "wwwroot\\";
            string localPath = path + id.ToString().Replace("-", "\\") + "\\";
            if (!Directory.Exists(Path.GetDirectoryName(localPath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(localPath));
            }

            string filenamestr = DateTime.Now.ToString("yyyyMMddHHmmssffffff") + ".docx";

            var filePath = localPath + filenamestr;
            bll = new CoreBLL((id).ToUpper());

            string sqlstr = BaseSQL.SQL_SysPageService_List;//@"SELECT[ServiceEvents],[ReturnType]FROM[dbo].[SysPageService]where [servicename]='{0}sqlexecute'";

            sqlstr = sqlstr.Replace("{0}", ctrl); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
            FileHelper.Writelog(sqlstr, "SQL");
            var dt = bll.db.ExecuteDataTable(sqlstr.ToLower());
            return new Result(dt);

        }
        #region 添加返回DataTable方法
        /// <summary>
        ///  执行业通用务逻辑
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dic"></param>
        /// <returns></returns>

        public DataTable CoreService(Dictionary<string, string> dic)
        {
           
            string version = "0";
            string id = "metu";
            string appname = "metu";
            string servicename = "metu";
            string methodname = "call";
            version = dic.Versions();
            servicename = dic.ServiceName();
            methodname = dic.MethodName();
            appname = dic.AppName();
            if (id == null)
            {
                return new DataTable();
            }
            if (version == null)
            {
                version = "0";
            }
            if (version.Trim().Length == 0)
            {
                version = "0";
            }
            if (appname == null)
            {
                appname = "metu";
            }
            if (appname.Trim().Length == 0)
            {
                appname = "metu";
            }
            if (servicename == null)
            {
                servicename = "service";
            }
            if (servicename.Trim().Length == 0)
            {
                servicename = "service";
            }
            if (methodname == null)
            {
                methodname = "call";
            }
            if (methodname.Trim().Length == 0)
            {
                methodname = "call";
            }
            bool configtag = false;
            foreach (var itm in CommonCache.ConfigCaches.Keys)
            {
                if (itm.ToUpper() == id.ToUpper())
                {
                    configtag = true;
                    break;
                }
            }
            CoreBLL cbll = new CoreBLL();

            if (!configtag)
            {
                string sqlconfig = BaseSQL.SQL_GET_PAGE_DATA_LIST;//"SELECT top(1)  [ConfigName],[ConfigValue]FROM  [dbo].[SYSpageDICConfig] aa, [dbo].[SYSpageDic] bb where aa.CID=bb.CID and aa.DICID=bb.ID  and  lower(bb.SortCode)='dbconfig' and aa.[ConfigName]='{0}' and  bb.Cname=N'{1}'and  bb.appname=N'{2}' and  bb.version=N'{3}'";
                sqlconfig = sqlconfig.Replace("{0}", id);
                sqlconfig = sqlconfig.Replace("{1}", appname);
                sqlconfig = sqlconfig.Replace("{2}", appname);
                sqlconfig = sqlconfig.Replace("{3}", version);
                var configdt = cbll.db.ExecuteDataTable(sqlconfig);
                bool exitf = true;
                if (configdt != null) if (configdt.Rows.Count > ConstNum.Zero) if (configdt.Rows[0][0] != null && configdt.Rows[0][1] != null) if (configdt.Rows[0][1].ToString().Trim().Length > ConstNum.Zero)
                            {
                                CommonCache.ConfigCaches.Add(configdt.Rows[0][0].ToString().ToUpper(), configdt.Rows[0][1].ToString());
                                exitf = false;
                            }
                if (exitf) return new DataTable();
            }
            bll = new CoreBLL((id).ToUpper());
            
            string sqlstr = BaseSQL.SQL_SysPageService_List;//@"SELECT[ServiceEvents],[ReturnType]FROM[dbo].[SysPageService]where  [IsEnabled]=1 and [IsDeleted]=0 and [servicename]=N'{0}'";

            sqlstr = sqlstr.Replace("{0}", servicename);
            if (version != null) if (version.Trim().Length > 1)
                {
                    sqlstr = sqlstr + " and [version]=N'" + version + "'  ";
                }
            if (methodname != null) if (methodname.Trim().Length > 1)
                {
                    sqlstr = sqlstr + " and [methodname]=N'" + methodname + "'  ";
                }
            sqlstr = sqlstr.ToLower().Replace("=n'", "=N'");
            sqlstr = sqlstr.Replace(",n'", ",N'");

            var dt = cbll.db.ExecuteDataTable(sqlstr.Str2SQL());
            if (dt != null) if (dt.Rows.Count > ConstNum.Zero) if (dt.Rows[0][0] != null) if (dt.Rows[0][0].ToString().Trim().Length > 10)
                        {
                            sqlstr = dt.Rows[0][0].ToString();

                            if (dic != null) if (dic.Count > ConstNum.Zero)
                                {
                                    foreach (var itm in dic)
                                    {
                                        if (itm.Value != null)
                                        {
                                            sqlstr = sqlstr.Replace("{" + itm.Key.ToUpper() + "}", itm.Value.Replace("'", "''")).Replace("{" + itm.Key.ToLower() + "}", itm.Value.Replace("'", "''"));
                                        }
                                    }
                                }
                            FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                            var rsdt = bll.db.ExecuteDataTable(sqlstr.Str2SQL());

                            if (rsdt == null)
                            {
                                return new DataTable();
                            }
                            if (rsdt.Rows.Count == 0)
                            {
                                return rsdt;
                            }
                            if (rsdt.Columns.Count == 0)
                            {
                                return rsdt;
                            }
                            if (rsdt.Rows.Count > 0 && rsdt.Columns.Count > 0)
                            {
                                return rsdt;
                            }
                            return new DataTable();


                        }

            return new DataTable();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="servicename"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public DataTable CoreService(string servicename, Dictionary<string, string> dic)
        {
            #region 获取数据库连接字符串
            string version = "0";
            string id = "metu";
            string appname = "metu";
            
            string methodname = "call";
            version = dic.Versions();
            
            methodname = dic.MethodName();
            appname = dic.AppName();
            if (id == null)
            {
                return new DataTable();
            }
            if (version == null)
            {
                version = "0";
            }
            if (version.Trim().Length == 0)
            {
                version = "0";
            }
            if (appname == null)
            {
                appname = "metu";
            }
            if (appname.Trim().Length == 0)
            {
                appname = "metu";
            }
            if (servicename == null)
            {
                servicename = "service";
            }
            if (servicename.Trim().Length == 0)
            {
                servicename = "service";
            }
            if (methodname == null)
            {
                methodname = "call";
            }
            if (methodname.Trim().Length == 0)
            {
                methodname = "call";
            }
            bool configtag = false;
            foreach (var itm in CommonCache.ConfigCaches.Keys)
            {
                if (itm.ToUpper() == id.ToUpper())
                {
                    configtag = true;
                    break;
                }
            }
            CoreBLL cbll = new CoreBLL();

            if (!configtag)
            {
                string sqlconfig = BaseSQL.SQL_GET_PAGE_DATA_LIST;//"SELECT top(1)  [ConfigName],[ConfigValue]FROM  [dbo].[SYSpageDICConfig] aa, [dbo].[SYSpageDic] bb where aa.CID=bb.CID and aa.DICID=bb.ID  and  lower(bb.SortCode)='dbconfig' and aa.[ConfigName]='{0}' and  bb.Cname=N'{1}'and  bb.appname=N'{2}' and  bb.version=N'{3}'";
                sqlconfig = sqlconfig.Replace("{0}", id);
                sqlconfig = sqlconfig.Replace("{1}", appname);
                sqlconfig = sqlconfig.Replace("{2}", appname);
                sqlconfig = sqlconfig.Replace("{3}", version);
                var configdt = cbll.db.ExecuteDataTable(sqlconfig);
                bool exitf = true;
                if (configdt != null) if (configdt.Rows.Count > ConstNum.Zero) if (configdt.Rows[0][0] != null && configdt.Rows[0][1] != null) if (configdt.Rows[0][1].ToString().Trim().Length > ConstNum.Zero)
                            {
                                CommonCache.ConfigCaches.Add(configdt.Rows[0][0].ToString().ToUpper(), configdt.Rows[0][1].ToString());
                                exitf = false;
                            }
                if (exitf) return new DataTable();
            }
            bll = new CoreBLL((id).ToUpper());
            #endregion

            string sqlstr = BaseSQL.SQL_SysPageService_List;//@"SELECT[ServiceEvents],[ReturnType]FROM[dbo].[SysPageService]where  [IsEnabled]=1 and [IsDeleted]=0 and [servicename]=N'{0}'";

            sqlstr = sqlstr.Replace("{0}", servicename);
            if (methodname != null) if (methodname.Trim().Length > 1)
                {
                    sqlstr = sqlstr + " and [methodname]=N'" + methodname + "'  ";
                }
            sqlstr = sqlstr.ToLower().Replace("=n'", "=N'");
            sqlstr = sqlstr.Replace(",n'", ",N'");

            var dt = cbll.db.ExecuteDataTable(sqlstr.Str2SQL());
            if (dt != null) if (dt.Rows.Count > ConstNum.Zero) if (dt.Rows[0][0] != null) if (dt.Rows[0][0].ToString().Trim().Length > 10)
                        {
                            sqlstr = dt.Rows[0][0].ToString();

                            if (dic != null) if (dic.Count > ConstNum.Zero)
                                {
                                    foreach (var itm in dic)
                                    {
                                        if (itm.Value != null)
                                        {
                                            sqlstr = sqlstr.Replace("{" + itm.Key.ToUpper() + "}", itm.Value.Replace("'", "''")).Replace("{" + itm.Key.ToLower() + "}", itm.Value.Replace("'", "''"));
                                        }
                                    }
                                }
                            FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                            var rsdt = bll.db.ExecuteDataTable(sqlstr.Str2SQL());

                            if (rsdt == null)
                            {
                                return new DataTable();
                            }
                            if (rsdt.Rows.Count == 0)
                            {
                                return rsdt;
                            }
                            if (rsdt.Columns.Count == 0)
                            {
                                return rsdt;
                            }
                            if (rsdt.Rows.Count > 0 && rsdt.Columns.Count > 0)
                            {
                                return rsdt;
                            }
                            return new DataTable();


                        }

            return new DataTable();
        }
        /// <summary>
        /// [页面控件信息]通用接口（获取实体字段及描述）;通过配置页面英文名获取
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tname"></param>
        /// <returns></returns>
        public DataTable CoreServicePageControlsInfo(string PageNameens)
        {
            string sqlstr = BaseSQL.SQL_GET_PAGECONTROL_LIST;
            sqlstr = sqlstr.Replace("{0}", PageNameens);
            sqlstr = sqlstr.Replace("=n'", "=N'");
            FileHelper.Writelog(sqlstr, "SQL");
            return bll.db.ExecuteDataTable(sqlstr.Str2SQL());
        }
        public DataTable CoreServicePageDataInfo(string servicename, Dictionary<string, string> dic)
        {

            #region 获取数据库连接字符串
            string version = "0";
            string id = "metu";
            string appname = "metu";

            string methodname = "call";
            version = dic.Versions();

            methodname = dic.MethodName();
            appname = dic.AppName();
            if (id == null)
            {
                return new DataTable();
            }
            if (version == null)
            {
                version = "0";
            }
            if (version.Trim().Length == 0)
            {
                version = "0";
            }
            if (appname == null)
            {
                appname = "metu";
            }
            if (appname.Trim().Length == 0)
            {
                appname = "metu";
            }
            if (servicename == null)
            {
                servicename = "service";
            }
            if (servicename.Trim().Length == 0)
            {
                servicename = "service";
            }
            if (methodname == null)
            {
                methodname = "call";
            }
            if (methodname.Trim().Length == 0)
            {
                methodname = "call";
            }
            bool configtag = false;
            foreach (var itm in CommonCache.ConfigCaches.Keys)
            {
                if (itm.ToUpper() == id.ToUpper())
                {
                    configtag = true;
                    break;
                }
            }
            CoreBLL cbll = new CoreBLL();

            if (!configtag)
            {
                string sqlconfig = BaseSQL.SQL_GET_PAGE_DATA_LIST;//"SELECT top(1)  [ConfigName],[ConfigValue]FROM  [dbo].[SYSpageDICConfig] aa, [dbo].[SYSpageDic] bb where aa.CID=bb.CID and aa.DICID=bb.ID  and  lower(bb.SortCode)='dbconfig' and aa.[ConfigName]='{0}' and  bb.Cname=N'{1}'and  bb.appname=N'{2}' and  bb.version=N'{3}'";
                sqlconfig = sqlconfig.Replace("{0}", id);
                sqlconfig = sqlconfig.Replace("{1}", appname);
                sqlconfig = sqlconfig.Replace("{2}", appname);
                sqlconfig = sqlconfig.Replace("{3}", version);
                var configdt = cbll.db.ExecuteDataTable(sqlconfig);
                bool exitf = true;
                if (configdt != null) if (configdt.Rows.Count > ConstNum.Zero) if (configdt.Rows[0][0] != null && configdt.Rows[0][1] != null) if (configdt.Rows[0][1].ToString().Trim().Length > ConstNum.Zero)
                            {
                                CommonCache.ConfigCaches.Add(configdt.Rows[0][0].ToString().ToUpper(), configdt.Rows[0][1].ToString());
                                exitf = false;
                            }
                if (exitf) return new DataTable();
            }
            bll = new CoreBLL((id).ToUpper());
            #endregion

            string sqlstr = BaseSQL.SQL_SysPageService_List;//@"SELECT[ServiceEvents],[ReturnType]FROM[dbo].[SysPageService]where  [IsEnabled]=1 and [IsDeleted]=0 and [servicename]=N'{0}'";

            sqlstr = sqlstr.Replace("{0}", servicename);
            if (methodname != null) if (methodname.Trim().Length > 1)
                {
                    sqlstr = sqlstr + " and [methodname]=N'" + methodname + "'  ";
                }
            sqlstr = sqlstr.ToLower().Replace("=n'", "=N'");
            sqlstr = sqlstr.Replace(",n'", ",N'");
            FileHelper.Writelog(sqlstr, "SQL");
            var dt = cbll.db.ExecuteDataTable(sqlstr.Str2SQL());
            if (dt != null) if (dt.Rows.Count > ConstNum.Zero) if (dt.Rows[0][0] != null) if (dt.Rows[0][0].ToString().Trim().Length > 10)
                        {
                            sqlstr = dt.Rows[0][0].ToString();

                            if (dic != null) if (dic.Count > ConstNum.Zero)
                                {
                                    foreach (var itm in dic)
                                    {
                                        if (itm.Value != null)
                                        {
                                            sqlstr = sqlstr.Replace("{" + itm.Key.ToUpper() + "}", itm.Value.Replace("'", "''")).Replace("{" + itm.Key.ToLower() + "}", itm.Value.Replace("'", "''"));
                                        }
                                    }
                                }
                            sqlstr = sqlstr.ToLower().Replace("=n'", "=N'");
                            sqlstr = sqlstr.Replace(",n'", ",N'");
                             FileHelper.Writelog(sqlstr, "SQL");
                            var rsdt = bll.db.ExecuteDataTable(sqlstr.Str2SQL());

                            if (rsdt == null)
                            {
                                return new DataTable();
                            }
                            if (rsdt.Rows.Count == 0)
                            {
                                return rsdt;
                            }
                            if (rsdt.Columns.Count == 0)
                            {
                                return rsdt;
                            }
                            if (rsdt.Rows.Count > 0 && rsdt.Columns.Count > 0)
                            {
                                return rsdt;
                            }
                            return new DataTable();


                        }

            return new DataTable();
        }
        /// <summary>
        ///  执行业通用务逻辑
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dic"></param>
        /// <returns></returns>

        public DataTable CoreService(Dictionary<string, object> dic)
        {
            #region 获取数据库连接字符串
            string version = "0";
            string id = "metu";
            string appname = "metu";
            string servicename = "";
            string methodname = "call";
            if (dic.Versions() != null) version = dic.Versions().ToString();

            if (dic.MethodName() != null) methodname = dic.MethodName().ToString();
            if (dic.AppName() != null) appname = dic.AppName().ToString();
            if (id == null)
            {
                return new DataTable();
            }
            if (version == null)
            {
                version = "0";
            }
            if (version.Trim().Length == 0)
            {
                version = "0";
            }
            if (appname == null)
            {
                appname = "metu";
            }
            if (appname.Trim().Length == 0)
            {
                appname = "metu";
            }
            if (servicename == null)
            {
                servicename = "service";
            }
            if (servicename.Trim().Length == 0)
            {
                servicename = "service";
            }
            if (methodname == null)
            {
                methodname = "call";
            }
            if (methodname.Trim().Length == 0)
            {
                methodname = "call";
            }
            bool configtag = false;
            foreach (var itm in CommonCache.ConfigCaches.Keys)
            {
                if (itm.ToUpper() == id.ToUpper())
                {
                    configtag = true;
                    break;
                }
            }
            CoreBLL cbll = new CoreBLL();

            if (!configtag)
            {
                string sqlconfig = BaseSQL.SQL_GET_PAGE_DATA_LIST;//"SELECT top(1)  [ConfigName],[ConfigValue]FROM  [dbo].[SYSpageDICConfig] aa, [dbo].[SYSpageDic] bb where aa.CID=bb.CID and aa.DICID=bb.ID  and  lower(bb.SortCode)='dbconfig' and aa.[ConfigName]='{0}' and  bb.Cname=N'{1}'and  bb.appname=N'{2}' and  bb.version=N'{3}'";
                sqlconfig = sqlconfig.Replace("{0}", id);
                sqlconfig = sqlconfig.Replace("{1}", appname);
                sqlconfig = sqlconfig.Replace("{2}", appname);
                sqlconfig = sqlconfig.Replace("{3}", version);
                var configdt = cbll.db.ExecuteDataTable(sqlconfig);
                bool exitf = true;
                if (configdt != null) if (configdt.Rows.Count > ConstNum.Zero) if (configdt.Rows[0][0] != null && configdt.Rows[0][1] != null) if (configdt.Rows[0][1].ToString().Trim().Length > ConstNum.Zero)
                            {
                                CommonCache.ConfigCaches.Add(configdt.Rows[0][0].ToString().ToUpper(), configdt.Rows[0][1].ToString());
                                exitf = false;
                            }
                if (exitf) return new DataTable();
            }
            bll = new CoreBLL((id).ToUpper());
            #endregion

            string sqlstr = BaseSQL.SQL_SysPageService_List;//@"SELECT[ServiceEvents],[ReturnType]FROM[dbo].[SysPageService]where  [IsEnabled]=1 and [IsDeleted]=0 and [servicename]=N'{0}'";

            sqlstr = sqlstr.Replace("{0}", servicename);
            if (methodname != null) if (methodname.Trim().Length > 1)
                {
                    sqlstr = sqlstr + " and [methodname]=N'" + methodname + "'  ";
                }
            sqlstr = sqlstr.ToLower().Replace("=n'", "=N'");
            sqlstr = sqlstr.Replace(",n'", ",N'");

            var dt = cbll.db.ExecuteDataTable(sqlstr.Str2SQL());
            if (dt != null) if (dt.Rows.Count > ConstNum.Zero) if (dt.Rows[0][0] != null) if (dt.Rows[0][0].ToString().Trim().Length > 10)
                        {
                            sqlstr = dt.Rows[0][0].ToString();

                            if (dic != null) if (dic.Count > ConstNum.Zero)
                                {
                                    foreach (var itm in dic)
                                    {
                                        if (itm.Value != null)
                                        {
                                            sqlstr = sqlstr.Replace("{" + itm.Key.ToUpper() + "}", itm.Value.ToString().Replace("'", "''")).Replace("{" + itm.Key.ToLower() + "}", itm.Value.ToString().Replace("'", "''"));
                                        }
                                    }
                                }
                            FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                            var rsdt = bll.db.ExecuteDataTable(sqlstr.Str2SQL());

                            if (rsdt == null)
                            {
                                return new DataTable();
                            }
                            if (rsdt.Rows.Count == 0)
                            {
                                return rsdt;
                            }
                            if (rsdt.Columns.Count == 0)
                            {
                                return rsdt;
                            }
                            if (rsdt.Rows.Count > 0 && rsdt.Columns.Count > 0)
                            {
                                return rsdt;
                            }
                            return new DataTable();


                        }

            return new DataTable();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="servicename"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public DataTable CoreServiceDataTable(string servicename, Dictionary<string, object> dic)
        {
            #region 获取数据库连接字符串
            string version = "0";
            string id = "metu";
            string appname = "metu";

            string methodname = "call";
            if (dic.Versions() != null) version = dic.Versions().ToString();

            if (dic.MethodName() != null) methodname = dic.MethodName().ToString();
            if (dic.AppName() != null) appname = dic.AppName().ToString();
            if (id == null)
            {
                return new DataTable();
            }
            if (version == null)
            {
                version = "0";
            }
            if (version.Trim().Length == 0)
            {
                version = "0";
            }
            if (appname == null)
            {
                appname = "metu";
            }
            if (appname.Trim().Length == 0)
            {
                appname = "metu";
            }
            if (servicename == null)
            {
                servicename = "service";
            }
            if (servicename.Trim().Length == 0)
            {
                servicename = "service";
            }
            if (methodname == null)
            {
                methodname = "call";
            }
            if (methodname.Trim().Length == 0)
            {
                methodname = "call";
            }
            bool configtag = false;
            foreach (var itm in CommonCache.ConfigCaches.Keys)
            {
                if (itm.ToUpper() == id.ToUpper())
                {
                    configtag = true;
                    break;
                }
            }
            CoreBLL cbll = new CoreBLL();

            if (!configtag)
            {
                string sqlconfig = BaseSQL.SQL_GET_PAGE_DATA_LIST;//"SELECT top(1)  [ConfigName],[ConfigValue]FROM  [dbo].[SYSpageDICConfig] aa, [dbo].[SYSpageDic] bb where aa.CID=bb.CID and aa.DICID=bb.ID  and  lower(bb.SortCode)='dbconfig' and aa.[ConfigName]='{0}' and  bb.Cname=N'{1}'and  bb.appname=N'{2}' and  bb.version=N'{3}'";
                sqlconfig = sqlconfig.Replace("{0}", id);
                sqlconfig = sqlconfig.Replace("{1}", appname);
                sqlconfig = sqlconfig.Replace("{2}", appname);
                sqlconfig = sqlconfig.Replace("{3}", version);
                var configdt = cbll.db.ExecuteDataTable(sqlconfig);
                bool exitf = true;
                if (configdt != null) if (configdt.Rows.Count > ConstNum.Zero) if (configdt.Rows[0][0] != null && configdt.Rows[0][1] != null) if (configdt.Rows[0][1].ToString().Trim().Length > ConstNum.Zero)
                            {
                                CommonCache.ConfigCaches.Add(configdt.Rows[0][0].ToString().ToUpper(), configdt.Rows[0][1].ToString());
                                exitf = false;
                            }
                if (exitf) return new DataTable();
            }
            bll = new CoreBLL((id).ToUpper());
            #endregion


            string sqlstr = BaseSQL.SQL_SysPageService_List;//@"SELECT[ServiceEvents],[ReturnType]FROM[dbo].[SysPageService]where  [IsEnabled]=1 and [IsDeleted]=0 and [servicename]=N'{0}'";

            sqlstr = sqlstr.Replace("{0}", servicename);
            if (methodname != null) if (methodname.Trim().Length > 1)
                {
                    sqlstr = sqlstr + " and [methodname]=N'" + methodname + "'  ";
                }
            sqlstr = sqlstr.ToLower().Replace("=n'", "=N'");
            sqlstr = sqlstr.Replace(",n'", ",N'");

            var dt = cbll.db.ExecuteDataTable(sqlstr.Str2SQL());
            if (dt != null) if (dt.Rows.Count > ConstNum.Zero) if (dt.Rows[0][0] != null) if (dt.Rows[0][0].ToString().Trim().Length > 10)
                        {
                            sqlstr = dt.Rows[0][0].ToString();

                            if (dic != null) if (dic.Count > ConstNum.Zero)
                                {
                                    foreach (var itm in dic)
                                    {
                                        if (itm.Value != null)
                                        {
                                            sqlstr = sqlstr.Replace("{" + itm.Key.ToUpper() + "}", itm.Value.ToString().Replace("'", "''")).Replace("{" + itm.Key.ToLower() + "}", itm.Value.ToString().Replace("'", "''"));
                                        }
                                    }
                                }
                            FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                            var rsdt = bll.db.ExecuteDataTable(sqlstr.Str2SQL());

                            if (rsdt == null)
                            {
                                return new DataTable();
                            }
                            if (rsdt.Rows.Count == 0)
                            {
                                return rsdt;
                            }
                            if (rsdt.Columns.Count == 0)
                            {
                                return rsdt;
                            }
                            if (rsdt.Rows.Count > 0 && rsdt.Columns.Count > 0)
                            {
                                return rsdt;
                            }
                            return new DataTable();


                        }

            return new DataTable();
        }

        public DataTable CoreServicePageDataInfo(string servicename, Dictionary<string, object> dic)
        {
            #region 获取数据库连接字符串
            string version = "0";
            string id = "metu";
            string appname = "metu";

            string methodname = "call";
            if(dic.Versions()!=null)version = dic.Versions().ToString();

           if(dic.MethodName()!=null) methodname = dic.MethodName().ToString();
            if(dic.AppName()!=null)appname = dic.AppName().ToString();
            if (id == null)
            {
                return new DataTable();
            }
            if (version == null)
            {
                version = "0";
            }
            if (version.Trim().Length == 0)
            {
                version = "0";
            }
            if (appname == null)
            {
                appname = "metu";
            }
            if (appname.Trim().Length == 0)
            {
                appname = "metu";
            }
            if (servicename == null)
            {
                servicename = "service";
            }
            if (servicename.Trim().Length == 0)
            {
                servicename = "service";
            }
            if (methodname == null)
            {
                methodname = "call";
            }
            if (methodname.Trim().Length == 0)
            {
                methodname = "call";
            }
            bool configtag = false;
            foreach (var itm in CommonCache.ConfigCaches.Keys)
            {
                if (itm.ToUpper() == id.ToUpper())
                {
                    configtag = true;
                    break;
                }
            }
            CoreBLL cbll = new CoreBLL();

            if (!configtag)
            {
                string sqlconfig = BaseSQL.SQL_GET_PAGE_DATA_LIST;//"SELECT top(1)  [ConfigName],[ConfigValue]FROM  [dbo].[SYSpageDICConfig] aa, [dbo].[SYSpageDic] bb where aa.CID=bb.CID and aa.DICID=bb.ID  and  lower(bb.SortCode)='dbconfig' and aa.[ConfigName]='{0}' and  bb.Cname=N'{1}'and  bb.appname=N'{2}' and  bb.version=N'{3}'";
                sqlconfig = sqlconfig.Replace("{0}", id);
                sqlconfig = sqlconfig.Replace("{1}", appname);
                sqlconfig = sqlconfig.Replace("{2}", appname);
                sqlconfig = sqlconfig.Replace("{3}", version);
                var configdt = cbll.db.ExecuteDataTable(sqlconfig);
                bool exitf = true;
                if (configdt != null) if (configdt.Rows.Count > ConstNum.Zero) if (configdt.Rows[0][0] != null && configdt.Rows[0][1] != null) if (configdt.Rows[0][1].ToString().Trim().Length > ConstNum.Zero)
                            {
                                CommonCache.ConfigCaches.Add(configdt.Rows[0][0].ToString().ToUpper(), configdt.Rows[0][1].ToString());
                                exitf = false;
                            }
                if (exitf) return new DataTable();
            }
            bll = new CoreBLL((id).ToUpper());
            #endregion


            string sqlstr = BaseSQL.SQL_SysPageService_List;//@"SELECT[ServiceEvents],[ReturnType]FROM[dbo].[SysPageService]where  [IsEnabled]=1 and [IsDeleted]=0 and [servicename]=N'{0}'";

            sqlstr = sqlstr.Replace("{0}", servicename);
            if (methodname != null) if (methodname.Trim().Length > 1)
                {
                    sqlstr = sqlstr + " and [methodname]=N'" + methodname + "'  ";
                }
            sqlstr = sqlstr.ToLower().Replace("=n'", "=N'");
            sqlstr = sqlstr.Replace(",n'", ",N'");
            FileHelper.Writelog(sqlstr, "SQL");
            var dt = cbll.db.ExecuteDataTable(sqlstr.Str2SQL());
            if (dt != null) 
                if (dt.Rows.Count > ConstNum.Zero) 
                    if (dt.Rows[0][0] != null) 
                        if (dt.Rows[0][0].ToString().Trim().Length > 10)
                        {
                            sqlstr = dt.Rows[0][0].ToString();
                            if (dic != null) if (dic.Count > ConstNum.Zero)
                                {
                                    foreach (var itm in dic)
                                    {
                                        if (itm.Value != null)
                                        {
                                            sqlstr = sqlstr.Replace("{" + itm.Key.ToUpper() + "}", itm.Value.ToString().Replace("'", "''")).Replace("{" + itm.Key.ToLower() + "}", itm.Value.ToString().Replace("'", "''"));
                                        }
                                    }
                                }
                            sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); 
                            sqlstr = sqlstr.Replace(",n'", ",N'");
                            FileHelper.Writelog(sqlstr, "SQL"); 
                           var rsdt = bll.db.ExecuteDataTable(sqlstr.Str2SQL());

                            if (rsdt == null)
                            {
                                return new DataTable();
                            }
                            if (rsdt.Rows.Count == 0)
                            {
                                return rsdt;
                            }
                            if (rsdt.Columns.Count == 0)
                            {
                                return rsdt;
                            }
                            if (rsdt.Rows.Count > 0 && rsdt.Columns.Count > 0)
                            {
                                return rsdt;
                            }
                            return new DataTable();


                        }

            return new DataTable();
        }
        public object CoreService(string version, string id, string appname, string servicename,string methodname, Dictionary<string, object> dic)
        {
            #region   params
            if (id == null)
            {
                return Result.ERROR(0, "应用未注册！");
            }
            if (version == null)
            {
                version = "0";
            }
            if (version.Trim().Length == 0)
            {
                version = "0";
            }
            if (appname == null)
            {
                appname = "metu";
            }
            if (appname.Trim().Length == 0)
            {
                appname = "metu";
            }
            if (servicename == null)
            {
                servicename = "service";
            }
            if (servicename.Trim().Length == 0)
            {
                servicename = "service";
            }

            bool configtag = false;
            foreach (var itm in CommonCache.ConfigCaches.Keys)
            {
                if (itm.ToUpper() == id.ToUpper())
                {
                    configtag = true;
                    break;
                }
            }
            CoreBLL cbll = new CoreBLL();
            #endregion
            if (!configtag)
            {
                string sqlconfig = BaseSQL.SQL_GET_PAGE_DATA_LIST;//"SELECT top(1)  [ConfigName],[ConfigValue]FROM  [dbo].[SYSpageDICConfig] aa, [dbo].[SYSpageDic] bb where aa.CID=bb.CID and aa.DICID=bb.ID  and  lower(bb.SortCode)='dbconfig' and aa.[ConfigName]='{0}' and  bb.Cname=N'{1}'and  bb.appname=N'{2}' and  bb.version=N'{3}'";
                sqlconfig = sqlconfig.Replace("{0}", id);
                sqlconfig = sqlconfig.Replace("{1}", appname);
                sqlconfig = sqlconfig.Replace("{2}", appname);
                sqlconfig = sqlconfig.Replace("{3}", version);
                var configdt = cbll.db.ExecuteDataTable(sqlconfig);
                bool exitf = true;
                if (configdt != null) if (configdt.Rows.Count > ConstNum.Zero) if (configdt.Rows[0][0] != null && configdt.Rows[0][1] != null) if (configdt.Rows[0][1].ToString().Trim().Length > ConstNum.Zero)
                            {
                                CommonCache.ConfigCaches.Add(configdt.Rows[0][0].ToString().ToUpper(), configdt.Rows[0][1].ToString());
                                exitf = false;
                            }
                if (exitf) return Result.ERROR(0, "未找到相关配置数据");
            }
            bll = new CoreBLL((id).ToUpper());

            string sqlstr = BaseSQL.SQL_SysPageService_List;//@"SELECT[ServiceEvents],[ReturnType]FROM[dbo].[SysPageService]where  [IsEnabled]=1 and [IsDeleted]=0 and [servicename]=N'{0}'";

            sqlstr = sqlstr.Replace("{0}", servicename);
            if (methodname != null) if (methodname.Trim().Length > 1)
                {
                    sqlstr = sqlstr + " and [methodname]=N'" + methodname + "'  ";
                }
            sqlstr = sqlstr.ToLower().Replace("=n'", "=N'");
            sqlstr = sqlstr.Replace(",n'", ",N'");

            var dt = cbll.db.ExecuteDataTable(sqlstr.Str2SQL());
            if (dt != null) if (dt.Rows.Count > ConstNum.Zero) if (dt.Rows[0][0] != null) if (dt.Rows[0][0].ToString().Trim().Length > 10)
                        {
                            sqlstr = dt.Rows[0][0].ToString();

                            if (dic != null) if (dic.Count > ConstNum.Zero)
                                {
                                    foreach (var itm in dic)
                                    {
                                        if (itm.Value != null)
                                        {
                                            sqlstr = sqlstr.Replace("{" + itm.Key.ToUpper() + "}", itm.Value.ToString().Replace("'", "''")).Replace("{" + itm.Key.ToLower() + "}", itm.Value.ToString().Replace("'", "''"));
                                        }
                                    }
                                }
                            sqlstr = sqlstr.ToLower().Replace("=n'", "=N'");
                            sqlstr = sqlstr.Replace(",n'", ",N'");
                            FileHelper.Writelog(sqlstr, "SQL"); 
                            var rsdt = bll.db.ExecuteDataTable(sqlstr.Str2SQL());

                            if (rsdt == null)
                            {
                                return new DataTable();
                            }
                            if (rsdt.Rows.Count == 0)
                            {
                                return rsdt;
                            }
                            if (rsdt.Columns.Count == 0)
                            {
                                return rsdt;
                            }
                            if (rsdt.Rows.Count > 0 && rsdt.Columns.Count > 0)
                            {
                                return rsdt;
                            }
                            return new DataTable();
                        }

            return new DataTable();
        }
        public DataTable CoreDtService(string version, string id, string appname, string servicename, string methodname, Dictionary<string, object> dic)
        {
            #region   params
            if (id == null)
            {
                return new DataTable();
            }
            if (version == null)
            {
                version = "0";
            }
            if (version.Trim().Length == 0)
            {
                version = "0";
            }
            if (appname == null)
            {
                appname = "metu";
            }
            if (appname.Trim().Length == 0)
            {
                appname = "metu";
            }
            if (servicename == null)
            {
                servicename = "service";
            }
            if (servicename.Trim().Length == 0)
            {
                servicename = "service";
            }

            bool configtag = false;
            foreach (var itm in CommonCache.ConfigCaches.Keys)
            {
                if (itm.ToUpper() == id.ToUpper())
                {
                    configtag = true;
                    break;
                }
            }
            CoreBLL cbll = new CoreBLL();
            #endregion
            if (!configtag)
            {
                string sqlconfig = BaseSQL.SQL_GET_PAGE_DATA_LIST;//"SELECT top(1)  [ConfigName],[ConfigValue]FROM  [dbo].[SYSpageDICConfig] aa, [dbo].[SYSpageDic] bb where aa.CID=bb.CID and aa.DICID=bb.ID  and  lower(bb.SortCode)='dbconfig' and aa.[ConfigName]='{0}' and  bb.Cname=N'{1}'and  bb.appname=N'{2}' and  bb.version=N'{3}'";
                sqlconfig = sqlconfig.Replace("{0}", id);
                sqlconfig = sqlconfig.Replace("{1}", appname);
                sqlconfig = sqlconfig.Replace("{2}", appname);
                sqlconfig = sqlconfig.Replace("{3}", version);
                var configdt = cbll.db.ExecuteDataTable(sqlconfig);
                bool exitf = true;
                if (configdt != null) if (configdt.Rows.Count > ConstNum.Zero) if (configdt.Rows[0][0] != null && configdt.Rows[0][1] != null) if (configdt.Rows[0][1].ToString().Trim().Length > ConstNum.Zero)
                            {
                                CommonCache.ConfigCaches.Add(configdt.Rows[0][0].ToString().ToUpper(), configdt.Rows[0][1].ToString());
                                exitf = false;
                            }
                if (exitf) return new DataTable();
            }
            bll = new CoreBLL((id).ToUpper());

            string sqlstr = BaseSQL.SQL_SysPageService_List;//@"SELECT[ServiceEvents],[ReturnType]FROM[dbo].[SysPageService]where  [IsEnabled]=1 and [IsDeleted]=0 and [servicename]=N'{0}'";

            sqlstr = sqlstr.Replace("{0}", servicename);
            if (methodname != null) if (methodname.Trim().Length > 1)
                {
                    sqlstr = sqlstr + " and [methodname]=N'" + methodname + "'  ";
                }
            sqlstr = sqlstr.ToLower().Replace("=n'", "=N'");
            sqlstr = sqlstr.Replace(",n'", ",N'");

            var dt = cbll.db.ExecuteDataTable(sqlstr.Str2SQL());
            if (dt != null) if (dt.Rows.Count > ConstNum.Zero) if (dt.Rows[0][0] != null) if (dt.Rows[0][0].ToString().Trim().Length > 10)
                        {
                            sqlstr = dt.Rows[0][0].ToString();

                            if (dic != null) if (dic.Count > ConstNum.Zero)
                                {
                                    foreach (var itm in dic)
                                    {
                                        if (itm.Value != null)
                                        {
                                            sqlstr = sqlstr.Replace("{" + itm.Key.ToUpper() + "}", itm.Value.ToString().Replace("'", "''")).Replace("{" + itm.Key.ToLower() + "}", itm.Value.ToString().Replace("'", "''"));
                                        }
                                    }
                                }
                            sqlstr = sqlstr.ToLower().Replace("=n'", "=N'");
                            sqlstr = sqlstr.Replace(",n'", ",N'");
                            FileHelper.Writelog(sqlstr, "SQL");
                            var rsdt = bll.db.ExecuteDataTable(sqlstr.Str2SQL());

                            if (rsdt == null)
                            {
                                return new DataTable();
                            }
                            if (rsdt.Rows.Count == 0)
                            {
                                return rsdt;
                            }
                            if (rsdt.Columns.Count == 0)
                            {
                                return rsdt;
                            }
                            if (rsdt.Rows.Count > 0 && rsdt.Columns.Count > 0)
                            {
                                return rsdt;
                            }
                            return new DataTable();
                        }

            return new DataTable();
        }
        /// <summary>
        /// [页面控件信息]通用接口（获取实体字段及描述）;通过配置页面英文名获取
        /// </summary>
        /// <param name="version"></param>
        /// <param name="id">企业用户名 （英文字符或手机号，唯一不重复）</param>
        /// <param name="appname">企业用户下的应用分类（ERP、CRM、WMS、MES、MTS、TMS、SSO、GPS等）</param>
        /// <param name="servicename">应该下的服务分类名</param>
        /// <param name="methodname">服务中具体业务名</param>
        /// <returns></returns>

        public DataTable PageInfo(string version, string id, string appname, string servicename)
        {
            #region   params
            if (id == null)
            {
                return new DataTable();
            }
            if (version == null)
            {
                version = "0";
            }
            if (version.Trim().Length == 0)
            {
                version = "0";
            }
            if (appname == null)
            {
                appname = "metu";
            }
            if (appname.Trim().Length == 0)
            {
                appname = "metu";
            }
            if (servicename == null)
            {
                servicename = "service";
            }
            if (servicename.Trim().Length == 0)
            {
                servicename = "service";
            }

            bool configtag = false;
            foreach (var itm in CommonCache.ConfigCaches.Keys)
            {
                if (itm.ToUpper() == id.ToUpper())
                {
                    configtag = true;
                    break;
                }
            }
            CoreBLL cbll = new CoreBLL();
            #endregion
            if (!configtag)
            {
                string sqlconfig = BaseSQL.SQL_GET_PAGE_DATA_LIST;//"SELECT top(1)  [ConfigName],[ConfigValue]FROM  [dbo].[SYSpageDICConfig] aa, [dbo].[SYSpageDic] bb where aa.CID=bb.CID and aa.DICID=bb.ID  and  lower(bb.SortCode)='dbconfig' and aa.[ConfigName]='{0}' and  bb.Cname=N'{1}'and  bb.appname=N'{2}' and  bb.version=N'{3}'";
                sqlconfig = sqlconfig.Replace("{0}", id);
                sqlconfig = sqlconfig.Replace("{1}", appname);
                sqlconfig = sqlconfig.Replace("{2}", appname);
                sqlconfig = sqlconfig.Replace("{3}", version);
                var configdt = cbll.db.ExecuteDataTable(sqlconfig);
                bool exitf = true;
                if (configdt != null) if (configdt.Rows.Count > ConstNum.Zero) if (configdt.Rows[0][0] != null && configdt.Rows[0][1] != null) if (configdt.Rows[0][1].ToString().Trim().Length > ConstNum.Zero)
                            {
                                CommonCache.ConfigCaches.Add(configdt.Rows[0][0].ToString().ToUpper(), configdt.Rows[0][1].ToString());
                                exitf = false;
                            }
                if (exitf) return new DataTable();
            }
            bll = new CoreBLL((id).ToUpper());

            string sqlstr = BaseSQL.SQL_GET_PAGECONTROL_LIST;
            sqlstr = sqlstr.Replace("{0}", servicename);
            sqlstr = sqlstr.Replace("{1}", version);
            sqlstr = sqlstr.Replace("{2}", appname);
            sqlstr = sqlstr.Replace("=n'", "=N'");
            FileHelper.Writelog(sqlstr, "SQL");
            return bll.db.ExecuteDataTable(sqlstr.Str2SQL());
        }


        #endregion

    }
}
