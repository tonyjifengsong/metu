using METU.CACHES;
using METU.INTERFACE;
using METU.MODEL;
using METU.MSSQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Microsoft.EntityFrameworkCore
{/// <summary>
 /// 
 /// </summary>
    public  class DBHelpers : IDBHelper, ICBLL
  
    {/// <summary>
     /// 
     /// </summary>
        BaseDbContext dbh = null;
         /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public DBHelpers(DbContextOptions options)
        {
           　
            　
            dbh = new BaseDbContext(options);
        }

       
        /// <summary>
        /// 执行SQL语句，并返回影响行数
        /// </summary>
        /// <param name="sqlstr">原始SQL语句</param>
        /// <returns></returns>
        public int Execute(string sqlstr) {
            FileHelper.Writelog(sqlstr, "SQL");
            return dbh.ExecuteSql(sqlstr);
        }
              /// <summary>
        /// 通过SQL返回表格
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(string sqlQuery) { FileHelper.Writelog(sqlQuery, "SQL"); return dbh.ExecuteDataTable(sqlQuery); }
        /// <summary>
        /// 通过SQL返回多张表
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(string sqlQuery) { FileHelper.Writelog(sqlQuery, "SQL"); return dbh.ExecuteDataSet(sqlQuery); }
        /// <summary>
        /// 执行事务
        /// </summary>
        /// <param name="SQLStringList"></param>
        public void ExecuteSqlTrans(IList<string> SQLStringList = null) { FileHelper.Writelog(SQLStringList, "SQL"); dbh.ExecuteSqlTran(SQLStringList); }
        /// <summary>
        /// 执行带参数SQL语句
        /// </summary>
        /// <param name="SQLString"></param>
        /// <param name="cmdParms"></param>
        /// <returns></returns>
        public int ExecuteSql(string SQLString, DbParameter[] cmdParms = null) { FileHelper.Writelog(SQLString, "SQL"); return dbh.ExecuteSql(SQLString, cmdParms); }
        /// <summary>
        /// 执行SQL语句，返回Object值
        /// </summary>
        /// <param name="SQLString"></param>
        /// <returns></returns>
        public object GetSingle(string SQLString = null) { FileHelper.Writelog(SQLString, "SQL"); return dbh.ExecuteSql(SQLString); }

        public bool Add(dynamic model, string Sqlstr = "")
        {

            if (Sqlstr == null)
            {
                if (model == null)
                    return false;
                dbh.Add(model);
                return dbh.SaveChanges() > 0;
            }
            else
            {
                var dic = model.ToDictionary();
                Sqlstr = dic.ToSQL(Sqlstr);
                FileHelper.Writelog(Sqlstr, "SQL");
                return dbh.ExecuteSql(Sqlstr) > 0;
            }
        }

        public bool Update(dynamic model, string Sqlstr = "")
        {
            if (Sqlstr == null)
            {
                if (model == null)
                    return false;
                dbh.Update(model);
                return dbh.SaveChanges() > 0;
            }
            else
            {
                var dic = model.ToDictionary();
                Sqlstr = dic.ToSQL(Sqlstr);
                FileHelper.Writelog(Sqlstr, "SQL");
                return dbh.ExecuteSql(Sqlstr) > 0;
            }
        }

        public bool Delete(dynamic model, string Sqlstr = "")
        {
            if (Sqlstr == null)
            {
                if (model == null)
                    return false;
                dbh.Remove(model);
                return dbh.SaveChanges() > 0;
            }
            else
            {
                var dic = model.ToDictionary();
                Sqlstr = dic.ToSQL(Sqlstr);
                FileHelper.Writelog(Sqlstr, "SQL");
                return dbh.ExecuteSql(Sqlstr) > 0;
            }
        }

        public object Search(dynamic model, string Sqlstr = "")
        {
            if (Sqlstr == null)
            {
                if (model == null)
                    return null;

                return null;
            }
            else
            {
                var dic = model.ToDictionary();
                Sqlstr = dic.ToSQL(Sqlstr);
                FileHelper.Writelog(Sqlstr, "SQL");
                return dbh.ExecuteDataTable(Sqlstr);
            }
        }

        public DataTable SearchDt(dynamic model, string Sqlstr = "")
        {
            if (Sqlstr == null)
            {
                if (model == null)
                    return null;

                return null;
            }
            else
            {
                var dic = model.ToDictionary();
                Sqlstr = dic.ToSQL(Sqlstr);
                FileHelper.Writelog(Sqlstr, "SQL");
                return dbh.ExecuteDataTable(Sqlstr);
            }
        }

        public object executesql(string sqlstr)
        {
            if (sqlstr == null)
            {

                return null;
            }
            else
            {
                FileHelper.Writelog(sqlstr, "SQL");
                return dbh.ExecuteDataTable(sqlstr);
            }
        }

        public DataTable SearchSQL(Dictionary<string, string> dic, string Sqlstr = "")
        {
            if (Sqlstr == null)
            {
                if (dic == null)
                    return null;

                return null;
            }
            else
            {

                Sqlstr = dic.ToSQL(Sqlstr);
                FileHelper.Writelog(Sqlstr, "SQL");
                return dbh.ExecuteDataTable(Sqlstr);
            }
        }

        public DataTable executeDt(string sqlstr)
        {
            if (sqlstr == null)
            {

                return null;
            }
            else
            {
                FileHelper.Writelog(sqlstr, "SQL");
                return dbh.ExecuteDataTable(sqlstr);
            }
        }
        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="Dic">SQL模板参数</param>
        /// <param name="functionname">SQL模板名称</param>
        /// <returns>返回执行结果OBJECT</returns>
        public object ExecuteSQL(Dictionary<string, string> Dic, string functionname = null)
        {
            DataTable dt = new DataTable();
            string templatename = "";
            templatename = functionname;
            string templatecontent = MESCache.AppCaches.GetStringValueUpperKey(templatename);
            if (Dic.GetValueUpperKey("SQL").CheckStringValue()) templatecontent = Dic.GetValueUpperKey("SQL");

            if (!templatecontent.CheckStringValue())
            {

                dt = GetTemplateDt(templatename);
                if (dt.IsValidated()) templatecontent = dt.GetStringValue();
                templatecontent = templatecontent.ToCamstarXml(Dic);
            }
            var result = dbh.ExecuteSQL(templatecontent);
            return result;
        }

        /// <summary>
        /// Daniel 2018/1/7 直接执行SQL
        /// </summary>
        /// <param name="strSQL"></param>
        /// <returns></returns>
        public DataTable ExecuteSQL(string strSQL)
        {
            DataTable dt = new DataTable("MyTable");
            dt = dbh.ExecuteDataTable(strSQL);
            return dt;
        }

        public object GetScalar(string strSQL)
        {
            object result = dbh.ExecuteDataTable(strSQL);
            return result == null ? "" : result;
        }


        public string ErrorMessage { get; set; }

        public Dictionary<string, string> MessageDic = new Dictionary<string, string>();

        /// <summary>
        /// 拆分结果数据
        /// </summary>
        /// <param name="result">Camstar返回结果</param>
        /// <returns>返回处理后Dictionary</returns>
        public virtual Dictionary<string, string> SplitResult(string result)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            var edp = result.XMLSearchInnerXmlNode("__errorDescription");
            dic.Add("errorDescription".ToUpper(), edp);
            dic.ADDErrorKey(edp);
            if (edp == null) { dic.Add("Result".ToUpper(), "True"); }
            else
            {
                if (edp.ToString().Length == 0)
                {
                    dic.Add("Result".ToUpper(), "True");
                }
                else
                {
                    dic.Add("Result".ToUpper(), "False");
                }
            }
            dic.Add("CompletionMsg".ToUpper(), result.XMLSearchInnerXmlNode("CompletionMsg"));
            dic.Add("Results".ToUpper(), result);
            return dic;
        }
        /// <summary>
        /// 获取模板的ＳＱＬ语句
        /// </summary>
        /// <param name="templatename">模板名称</param>
        /// <returns></returns>
        public DataTable GetTemplateDt(string templatename = "")
        {
            string templatestr = string.IsNullOrEmpty(templatename) ? "BaseTemplate" : templatename.Trim();
            templatestr = templatestr.ToUpper();
            string templatecontent = MESCache.AppCaches.GetStringValueUpperKey(templatestr);
            if (!templatecontent.CheckStringValue())
            {
                string templatesqlstr = "select templatecontent  from METUTemplate  where UPPER(templatename)='" + templatestr + "'";

                DataTable dt = dbh.ExecuteDataTable(templatesqlstr);
                MESCache.AppCaches.AddKey(templatename, dt.GetStringValue());
                return dt;
            }
            else
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("templatecontent");
                DataRow dr = dt.NewRow();
                dr["templatecontent"] = templatecontent;
                dt.Rows.Add(dr);
                return dt;
            }

        }

        /// <summary>
        /// 判断返回值是否符合要求
        /// </summary>
        /// <param name="obj">返回值对象</param>
        /// <returns>返回是否符合要求，默认返回false</returns>
        public virtual bool CheckReturn(dynamic obj)
        {
            string str = "";
            try
            {
                str = (string)obj;
                var ret = str.XMLSearchInnerTextNode("__errorDescription");
                if (ret == null)
                {
                    if (str.IndexOf(">") < 0 && str.CheckStringValue()) return false;

                    return true;
                }
                if (ret.Trim().ToString().Length == 0)
                {
                    if (str.IndexOf(">") < 0 && str.CheckStringValue()) return false;

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                FileHelper.Writelog(ex.Message);
                str = "";
            }

            return false;
        }


        /// <summary>
        /// 通过模板名称及模板参数返回执行结果
        /// </summary>
        /// <param name="Dic">模板参数</param>
        /// <param name="functionname">模板名称</param>
        /// <returns>返回True或False</returns>
        public virtual bool ExecuteByTemplate(Dictionary<string, string> Dic, string functionname = null)
        {
            DataTable dt = new DataTable();
            string templatename = "";
            templatename = functionname;
            string templatecontent = MESCache.AppCaches.GetStringValueUpperKey(templatename);
            if (!templatecontent.CheckStringValue())
            {
                dt = GetTemplateDt(templatename);

                templatecontent = dt.GetStringValue();
                templatecontent = Dic.ToSQL(templatecontent);
            }
            dt = dbh.ExecuteDataTable(templatecontent);

            if (dt == null) return false;
            if (dt.Rows.Count < 1) return false;
            return true;
        }
        /// <summary>
        /// 通过模板名称及模板参数返回执行结果
        /// </summary>
        /// <param name="Dic">模板参数</param>
        /// <param name="functionname">模板名称</param>
        /// <returns>返回DataTable</returns>
        public virtual DataTable ExecuteDtByTemplate(Dictionary<string, string> Dic, string functionname = null)
        {
            DataTable dt = new DataTable();
            string templatename = functionname, templatecontent = "";
            string strSQL = Dic.ContainsKey("SQL") ? Dic["SQL"] : "";

            if (strSQL.Length > 1)
            {
                foreach (var itm in Dic)
                {
                    if (itm.Key.ToUpper() == "SQL")
                    {
                        templatecontent = Dic["SQL"];
                    }
                }

                dt.TableName = "myDataTable";   //注意:从Oracle执行SQL语句获取DataTable必须给表取名,否则报错。
                dt = dbh.ExecuteDataTable(templatecontent);

            }
            else
            {
                templatecontent = MESCache.AppCaches.GetValueUpperKey(templatename).ToString();
                if (!templatecontent.CheckStringValue())
                {
                    if (templatename.CheckStringValue())
                    {
                        dt = GetTemplateDt(templatename);

                        templatecontent = dt.GetStringValue();
                        MESCache.AppCaches.AddKey(templatename, templatecontent);
                    }
                    else
                    {
                        templatecontent = templatename;
                    }
                }
                templatecontent = Dic.ToSQL(templatecontent);
                dt = dbh.ExecuteDataTable(templatecontent);
            }

            return dt;
        }

        /// <summary>
        /// 添加数据到数据库
        /// </summary>
        /// <param name="model">实体对象</param>
        /// <param name="Sqlstr">ＳＱＬ模板(不输入ＳＱＬ，则使用模板库中ＳＱＬ模板，使用模板库中的ＳＱＬ时，实体对象的ＴＥＭＰＬＡＴＥ字段不能为空)</param>
        /// <param name="CamstarFlag">是否启用CamstarXML操作（默认不使用Camstar）</param>
        /// <returns></returns>
        public bool Add(dynamic model, string Sqlstr = "", bool CamstarFlag = false)
        {

            if (!Sqlstr.CheckStringValue())
            {
                var dic = model.ToDictionary();
                string key = dic["TEMPLATENAME"];
                if (key.CheckStringValue())
                {
                    Sqlstr = MESCache.AppCaches.GetValueUpperKey(key).ToString();
                }
                else
                {
                    Sqlstr = "";

                }
                if (!Sqlstr.CheckStringValue())
                {
                    var dt = GetTemplateDt(dic["TEMPLATENAME"]);

                    Sqlstr = dt.GetStringValue();

                }
                Sqlstr = GetTemplateDt(dic["TEMPLATENAME"]);
                if (!Sqlstr.CheckStringValue()) return false;
            }
            else
            {

                if (Sqlstr.Substring(Sqlstr.Length - 3, 3).ToUpper() == "SQL")
                {
                    string key = Sqlstr;
                    if (key.CheckStringValue())
                    {
                        Sqlstr = MESCache.AppCaches.GetValueUpperKey(key).ToString();
                    }
                    else
                    {
                        Sqlstr = "";

                    }
                    if (!Sqlstr.CheckStringValue())
                    {
                        var dt = GetTemplateDt(Sqlstr);

                        Sqlstr = dt.GetStringValue();

                    }

                }
                if (!Sqlstr.CheckStringValue()) return false;
            }
            bool result = false;
            if (CamstarFlag)
            {
                result = Add(model, Sqlstr);

            }
            else
            {
                result = Add(model, Sqlstr);
            }
            return result;
        }

        /// <summary>
        /// 删除数据库中数据（可以批量删除）
        /// </summary>
        /// <param name="model">实体对象</param>
        /// <param name="Sqlstr">ＳＱＬ模板(不输入ＳＱＬ，则使用模板库中ＳＱＬ模板，实体对象的ＴＥＭＰＬＡＴＥ字段不能为空)</param>
        /// <param name="CamstarFlag">是否启用CamstarXML操作（默认不使用Camstar）</param>
        /// <returns></returns>
        public bool Delete(dynamic model, string Sqlstr = "", bool CamstarFlag = false)
        {
            var dic = model.ToDictionary();

            if (!Sqlstr.CheckStringValue())
            {
                Sqlstr = GetTemplateDt(dic["TEMPLATENAME"]);
                if (!Sqlstr.CheckStringValue()) return false;
            }
            else
            {

                if (Sqlstr.Substring(Sqlstr.Length - 3, 3).ToUpper() == "SQL")
                {
                    string key = Sqlstr;
                    if (key.CheckStringValue())
                    {
                        Sqlstr = MESCache.AppCaches.GetValueUpperKey(key).ToString();
                    }
                    else
                    {
                        Sqlstr = "";

                    }
                    if (!Sqlstr.CheckStringValue())
                    {
                        var dt = GetTemplateDt(Sqlstr);

                        Sqlstr = dt.GetStringValue();

                    }

                }
                if (!Sqlstr.CheckStringValue()) return false;
            }
            bool result = false;
            if (CamstarFlag)
            {
                result = Delete(model, Sqlstr);
            }
            else
            {
                result = Delete(model, Sqlstr);
            }
            return result;
        }

        /// <summary>
        /// 更新数据库中数据，（可以批量更新）
        /// </summary>
        /// <param name="model">实体对象</param>
        /// <param name="Sqlstr">ＳＱＬ模板(不输入ＳＱＬ，则使用模板库中ＳＱＬ模板，实体对象的ＴＥＭＰＬＡＴＥ字段不能为空)</param>
        /// <param name="CamstarFlag">是否启用CamstarXML操作（默认不使用Camstar）</param>
        /// <returns></returns>
        public bool Update(dynamic model, string Sqlstr = "", bool CamstarFlag = false)
        {
            if (!Sqlstr.CheckStringValue())
            {
                var dic = model.ToDictionary();
                Sqlstr = GetTemplateDt(dic["TEMPLATENAME"]);
                if (!Sqlstr.CheckStringValue()) return false;
            }
            else
            {

                if (Sqlstr.Substring(Sqlstr.Length - 3, 3).ToUpper() == "SQL")
                {
                    string key = Sqlstr;
                    if (key.CheckStringValue())
                    {
                        Sqlstr = MESCache.AppCaches.GetValueUpperKey(key).ToString();
                    }
                    else
                    {
                        Sqlstr = "";

                    }
                    if (!Sqlstr.CheckStringValue())
                    {
                        var dt = GetTemplateDt(Sqlstr);

                        Sqlstr = dt.GetStringValue();

                    }

                }
                if (!Sqlstr.CheckStringValue()) return false;
            }
            bool result = false;
            if (CamstarFlag)
            {
                result = Update(model, Sqlstr);
            }
            else
            {
                result = Update(model, Sqlstr);

            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model">实体对象</param>
        /// <param name="sqlstr">ＳＱＬ模板(不输入ＳＱＬ，则使用模板库中ＳＱＬ模板，实体对象的ＴＥＭＰＬＡＴＥ字段不能为空)</param>
        /// <param name="CamstarFlag">是否启用CamstarXML操作（默认不使用Camstar）</param>
        /// <returns></returns>
        public DataTable ExecuteSQL(dynamic model, string sqlstr = "", bool CamstarFlag = false)
        {
            if (!sqlstr.CheckStringValue())
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                if (model as string != null)
                {
                    sqlstr = model;

                }
                else
                {
                    dic = model.ToDictionary();
                    sqlstr = GetTemplateDt(dic.GetTemplate()).GetStringValue();
                }

                if (!sqlstr.CheckStringValue()) return new DataTable();
            }
            if (sqlstr.ToUpper().EndsWith("SQL"))
            {
                string key = sqlstr;
                if (key.CheckStringValue())
                {
                    sqlstr = MESCache.AppCaches.GetValueUpperKey(key).ToString();
                }
                else
                {
                    sqlstr = "";

                }
                if (!sqlstr.CheckStringValue())
                {
                    var dt = GetTemplateDt(key);

                    sqlstr = dt.GetStringValue();

                }

            }
            if (!sqlstr.CheckStringValue()) return new DataTable();

            DataTable result = new DataTable();
            if (CamstarFlag)
            {
                result = dbh.ExecuteDataTable(sqlstr);
            }
            else
            {
                result = dbh.ExecuteDataTable(sqlstr);
            }
            result.TableName = "result";
            return result;
        }

        #region extention
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public dynamic DynCore(Dictionary<string, string> dic)
        {

            string servicename = dic.ServiceName();

            string sqlstr = BaseSQL.SQL_SysPageService_List;//@"SELECT[ServiceEvents],[ReturnType]FROM[dbo].[SysPageService]where  [IsEnabled]=1 and [IsDeleted]=0 and [servicename]=N'{0}'";

            sqlstr = sqlstr.Replace("{0}", servicename); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");

            var dt = dbh.ExecuteDataTable(sqlstr.Str2SQL());
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
                                var rsdt = dbh.ExecuteDataTable(sqlstr.Str2SQL());

                                return new Result(rsdt.ToListDictionary());
                            }

                            if (dt.Rows[0][1].ToString() == "0" || dt.Rows[0][1].ToString().ToLower() == "table" || dt.Rows[0][1].ToString().ToLower() == "list")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteDataTable(sqlstr.Str2SQL());

                                return new Result(rsdt.ToListDictionary());
                            }
                            if (dt.Rows[0][1].ToString() == "object")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteDataTable(sqlstr.Str2SQL());

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
                                var rsdt = dbh.ExecuteSql(sqlstr.Str2SQL());
                                return new Result(rsdt > 0);
                            }
                            if (dt.Rows[0][1].ToString() == "int")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteDataTable(sqlstr.Str2SQL());
                                return new Result(rsdt);
                            }
                            else
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteDataTable(sqlstr.Str2SQL());
                                return new Result(rsdt.ToListDictionary());
                            }



                        }

            return Result.ERROR(0, "操作失败！");
        }

        /// <summary>
        ///  执行业通用务逻辑
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dic"></param>
        /// <returns></returns>

        public dynamic Core(Dictionary<string, string> dic)
        {

            string servicename = dic.ServiceName();



            string sqlstr = BaseSQL.SQL_SysPageService_List;//@"SELECT[ServiceEvents],[ReturnType]FROM[dbo].[SysPageService]where  [IsEnabled]=1 and [IsDeleted]=0 and [servicename]=N'{0}'";

            sqlstr = sqlstr.Replace("{0}", servicename); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");

            var dt = dbh.ExecuteDataTable(sqlstr.Str2SQL());
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
                                var rsdt = dbh.ExecuteDataTable(sqlstr.Str2SQL());

                                return new Result(rsdt.ToListDictionary());
                            }

                            if (dt.Rows[0][1].ToString() == "0" || dt.Rows[0][1].ToString().ToLower() == "table" || dt.Rows[0][1].ToString().ToLower() == "list")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteDataTable(sqlstr.Str2SQL());

                                return new Result(rsdt.ToListDictionary());
                            }
                            if (dt.Rows[0][1].ToString() == "object")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteDataTable(sqlstr.Str2SQL());

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
                                var rsdt = dbh.ExecuteSql(sqlstr.Str2SQL());
                                return new Result(rsdt > 0);
                            }
                            if (dt.Rows[0][1].ToString() == "int")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteSql(sqlstr.Str2SQL());
                                return new Result(rsdt);
                            }
                            else
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteDataTable(sqlstr.Str2SQL());
                                return new Result(rsdt.ToListDictionary());
                            }



                        }

            return Result.ERROR(0, "操作失败！");
        }

        /// <summary>
        ///  执行业通用务逻辑,返回整型
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dic"></param>
        /// <returns></returns>



        public int Coreint(Dictionary<string, string> dic)
        {
            string servicename = dic.ServiceName();


            string sqlstr = BaseSQL.SQL_SysPageService_List;//@"SELECT[ServiceEvents],[ReturnType]FROM[dbo].[SysPageService]where  [IsEnabled]=1 and [IsDeleted]=0 and [servicename]=N'{0}'";

            sqlstr = sqlstr.Replace("{0}", servicename); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");

            var dt = dbh.ExecuteDataTable(sqlstr.Str2SQL());
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
                                var rsdt = dbh.ExecuteDataTable(sqlstr.Str2SQL());

                                return rsdt.Rows.Count;
                            }

                            if (dt.Rows[0][1].ToString() == "0" || dt.Rows[0][1].ToString().ToLower() == "table" || dt.Rows[0][1].ToString().ToLower() == "list")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteDataTable(sqlstr.Str2SQL());

                                return rsdt.Rows.Count;
                            }
                            if (dt.Rows[0][1].ToString() == "object")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteDataTable(sqlstr.Str2SQL());

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
                                var rsdt = dbh.ExecuteSql(sqlstr.Str2SQL());
                                return rsdt;
                            }
                            if (dt.Rows[0][1].ToString() == "int")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteSql(sqlstr.Str2SQL());
                                return rsdt;
                            }

                            else
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteDataTable(sqlstr.Str2SQL());
                                return rsdt.Rows.Count;
                            }



                        }

            return -1;
        }
        /// <summary>
        /// 执行业通用务逻辑,返回Boolean
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dic"></param>
        /// <returns></returns>

        public bool Corebool(Dictionary<string, string> dic)
        {

            string servicename = dic.ServiceName();


            string sqlstr = BaseSQL.SQL_SysPageService_List;//@"SELECT[ServiceEvents],[ReturnType]FROM[dbo].[SysPageService]where  [IsEnabled]=1 and [IsDeleted]=0 and [servicename]=N'{0}'";


            sqlstr = sqlstr.Replace("{0}", servicename); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");

            var dt = dbh.ExecuteDataTable(sqlstr.Str2SQL());
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
                                var rsdt = dbh.ExecuteDataTable(sqlstr.Str2SQL());

                                return rsdt.Rows.Count > 0;
                            }

                            if (dt.Rows[0][1].ToString() == "0" || dt.Rows[0][1].ToString().ToLower() == "table" || dt.Rows[0][1].ToString().ToLower() == "list")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteDataTable(sqlstr.Str2SQL());

                                return rsdt.Rows.Count > 0;
                            }
                            if (dt.Rows[0][1].ToString() == "object")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteDataTable(sqlstr.Str2SQL());

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
                                var rsdt = dbh.ExecuteSql(sqlstr.Str2SQL());
                                return rsdt > 0;
                            }
                            if (dt.Rows[0][1].ToString() == "int")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteSql(sqlstr.Str2SQL());
                                return rsdt > 0;
                            }
                            else
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteDataTable(sqlstr.Str2SQL());
                                return rsdt.Rows.Count > 0;
                            }



                        }

            return false;
        }
        /// <summary>
        ///  执行业通用务逻辑,返回对象数据
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public object Coreobject(Dictionary<string, string> dic)
        {

            string servicename = dic.ServiceName();


            string sqlstr = BaseSQL.SQL_SysPageService_List;//@"SELECT[ServiceEvents],[ReturnType]FROM[dbo].[SysPageService]where  [IsEnabled]=1 and [IsDeleted]=0 and [servicename]=N'{0}'";


            sqlstr = sqlstr.Replace("{0}", servicename); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");

            var dt = dbh.ExecuteDataTable(sqlstr.Str2SQL());
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
                                var rsdt = dbh.ExecuteDataTable(sqlstr.Str2SQL());

                                return new Result(rsdt.ToListDictionary());
                            }

                            if (dt.Rows[0][1].ToString() == "0" || dt.Rows[0][1].ToString().ToLower() == "table" || dt.Rows[0][1].ToString().ToLower() == "list")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteDataTable(sqlstr.Str2SQL());

                                return new Result(rsdt.ToListDictionary());
                            }
                            if (dt.Rows[0][1].ToString() == "object")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteDataTable(sqlstr.Str2SQL());

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
                                var rsdt = dbh.ExecuteSql(sqlstr.Str2SQL());
                                return new Result(rsdt > 0);
                            }
                            if (dt.Rows[0][1].ToString() == "int")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteDataTable(sqlstr.Str2SQL());
                                return new Result(rsdt);
                            }
                            else
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteDataTable(sqlstr.Str2SQL());
                                return new Result(rsdt.ToListDictionary());
                            }



                        }

            return Result.ERROR(0, "操作失败！");
        }
        /// <summary>
        ///  执行业通用务逻辑,返回统一格式result
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public Result CoreBasic(Dictionary<string, string> dic)
        {
            string servicename = dic.ServiceName();

            string sqlstr = BaseSQL.SQL_SysPageService_List;//@"SELECT[ServiceEvents],[ReturnType]FROM[dbo].[SysPageService]where  [IsEnabled]=1 and [IsDeleted]=0 and [servicename]=N'{0}'";


            sqlstr = sqlstr.Replace("{0}", servicename); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");

            var dt = dbh.ExecuteDataTable(sqlstr.Str2SQL());
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
                                var rsdt = dbh.ExecuteDataTable(sqlstr.Str2SQL());

                                return new Result(rsdt.ToListDictionary());
                            }

                            if (dt.Rows[0][1].ToString() == "0" || dt.Rows[0][1].ToString().ToLower() == "table" || dt.Rows[0][1].ToString().ToLower() == "list")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteDataTable(sqlstr.Str2SQL());

                                return new Result(rsdt.ToListDictionary());
                            }
                            if (dt.Rows[0][1].ToString() == "object")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteDataTable(sqlstr.Str2SQL());

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
                                var rsdt = dbh.ExecuteSql(sqlstr.Str2SQL());
                                return new Result(rsdt > 0);
                            }
                            if (dt.Rows[0][1].ToString() == "int")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteDataTable(sqlstr.Str2SQL());
                                return new Result(rsdt);
                            }
                            else
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteDataTable(sqlstr.Str2SQL());
                                return new Result(rsdt.ToListDictionary());
                            }



                        }

            return Result.ERROR(0, "操作失败！");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="servicename"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public Result CoreService(string servicename, Dictionary<string, object> dic)
        {

            string sqlstr = BaseSQL.SQL_SysPageService_List;//@"SELECT[ServiceEvents],[ReturnType]FROM[dbo].[SysPageService]where  [IsEnabled]=1 and [IsDeleted]=0 and [servicename]=N'{0}'";


            sqlstr = sqlstr.Replace("{0}", servicename); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");

            var dt = dbh.ExecuteDataTable(sqlstr.Str2SQL());
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
                            if (dt.Rows[0][1] == null)
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteDataTable(sqlstr.Str2SQL());

                                return new Result(rsdt.ToListDictionary());
                            }

                            if (dt.Rows[0][1].ToString() == "0" || dt.Rows[0][1].ToString().ToLower() == "table" || dt.Rows[0][1].ToString().ToLower() == "list")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteDataTable(sqlstr.Str2SQL());

                                return new Result(rsdt.ToListDictionary());
                            }
                            if (dt.Rows[0][1].ToString() == "object")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteDataTable(sqlstr.Str2SQL());

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
                                var rsdt = dbh.ExecuteSql(sqlstr.Str2SQL());
                                return new Result(rsdt > 0);
                            }
                            if (dt.Rows[0][1].ToString() == "int")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteDataTable(sqlstr.Str2SQL());
                                return new Result(rsdt);
                            }
                            else
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteDataTable(sqlstr.Str2SQL());
                                return new Result(rsdt.ToListDictionary());
                            }



                        }

            return Result.ERROR(0, "操作失败！");
        }
        /// <summary>
        /// 执行业通用务逻辑,返回统一格式result
        /// </summary>
        /// <param name="context"></param>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public Result CoreList(int pageindex, int pagesize, Dictionary<string, string> dic = null)
        {

            string servicename = dic.ServiceName();

            Result rs = new Result();
            rs.Data.Add("pageindex", pageindex);
            rs.Data.Add("pagesize", pagesize);

            string sqlstr = BaseSQL.SQL_SysPageService_List;//@"SELECT[ServiceEvents],[ReturnType]FROM[dbo].[SysPageService]where  [IsEnabled]=1 and [IsDeleted]=0 and [servicename]=N'{0}'";

            sqlstr = sqlstr.Replace("{0}", servicename); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");

            var dt = dbh.ExecuteDataTable(sqlstr.Str2SQL());
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
                                var rsdt = dbh.ExecuteDataTable(sqlstr.Str2SQL());

                                return new Result(rsdt.ToListDictionary());
                            }

                            if (dt.Rows[0][1].ToString() == "0" || dt.Rows[0][1].ToString().ToLower() == "table" || dt.Rows[0][1].ToString().ToLower() == "list")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteDataTable(sqlstr.Str2SQL());

                                return new Result(rsdt.ToListDictionary());
                            }
                            if (dt.Rows[0][1].ToString() == "object")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteDataTable(sqlstr.Str2SQL());

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
                                var rsdt = dbh.ExecuteSql(sqlstr.Str2SQL());
                                return new Result(rsdt > 0);
                            }
                            if (dt.Rows[0][1].ToString() == "int")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteDataTable(sqlstr.Str2SQL());
                                return new Result(rsdt);
                            }
                            else
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteDataTable(sqlstr.Str2SQL());
                                return new Result(rsdt.ToListDictionary());
                            }



                        }

            return Result.ERROR(0, "操作失败！");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public Result DoCore(Dictionary<string, string> dic)
        {
            string servicename = dic.ServiceName();
            string sqlstr = BaseSQL.SQL_SysPageService_List;//@"SELECT[ServiceEvents],[ReturnType]FROM[dbo].[SysPageService]where  [IsEnabled]=1 and [IsDeleted]=0 and [servicename]=N'{0}'";

            sqlstr = sqlstr.Replace("{0}", servicename);

            var dt = dbh.ExecuteDataTable(sqlstr.Str2SQL());
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
                                var rsdt = dbh.ExecuteDataTable(sqlstr.Str2SQL());

                                return new Result(rsdt.ToListDictionary());
                            }

                            if (dt.Rows[0][1].ToString() == "0" || dt.Rows[0][1].ToString().ToLower() == "table" || dt.Rows[0][1].ToString().ToLower() == "list")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteDataTable(sqlstr.Str2SQL());

                                return new Result(rsdt.ToListDictionary());
                            }
                            if (dt.Rows[0][1].ToString() == "object")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteDataTable(sqlstr.Str2SQL());

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
                                var rsdt = dbh.ExecuteSql(sqlstr.Str2SQL());
                                return new Result(rsdt > 0);
                            }
                            if (dt.Rows[0][1].ToString() == "int")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteDataTable(sqlstr.Str2SQL());
                                return new Result(rsdt);
                            }

                            else
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = dbh.ExecuteDataTable(sqlstr.Str2SQL());
                                return new Result(rsdt.ToListDictionary());
                            }

                        }

            return Result.ERROR(0, "操作失败！");
        }
        /// <summary>
        /// 根据数据库中的表名获取页面控件的基本信息（获取实体字段及描述）
        /// </summary>
        /// <param name="context"></param>
        /// <param name="tname">数据库中表名</param>
        /// <returns></returns>
        public dynamic Reports(string tname)
        {
            string sqlstr = BaseSQL.SQL_GET_PAGE_LIST_COMMON;
            sqlstr = sqlstr.Replace("{0}", tname);


            return dbh.ExecuteDataTable(sqlstr.ToLower());
        }

        /// <summary>
        /// 根据数据库中的表名;获取表中的数据；参数可以为空；地址组合：{数据库配置名}APP/{数据库中表名}/getlist
        /// </summary>
        /// <param name="id">数据库配置名</param>
        /// <param name="tname">数据库中表名</param>
        /// <param name="dic">传入参数</param>
        /// <returns></returns>

        public object configServices(Dictionary<string, string> dic)
        {

            string methodname = dic.ServiceName();
            methodname = methodname + "getconfig";
            dic.AddServiceName(methodname);
            return DoCore(dic);
        }

        /// <summary>
        /// [页面控件信息]通用接口（获取实体字段及描述）;通过配置页面英文名获取
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tname"></param>
        /// <returns></returns>
        public object PageInfo(string methodname)
        {
            string sqlstr = BaseSQL.SQL_GET_PAGECONTROL_LIST;
            sqlstr = sqlstr.Replace("{0}", methodname);
            sqlstr = sqlstr.Replace("=n'", "=N'");
            FileHelper.Writelog(sqlstr, "SQL");
            return dbh.ExecuteDataTable(sqlstr.Str2SQL());
        }
        /// <summary>
        /// [页面控件信息内容保存到后端] 根据数据库中的中页面配置名;添加数据到指定接口；参数可以为空；后缀：save
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public object sysconfigServicessave(Dictionary<string, string> dic)
        {

            string methodname = dic.ServiceName();
            methodname = methodname + "save";
            dic.AddServiceName(methodname);
            return DoCore(dic);
        }

        /// <summary>
        /// [根据页面控件信息获取对应的数据]根据数据库中的中页面配置名;通过接口更新数据；参数可以为空；
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public object sysconfigServicesgets(Dictionary<string, string> dic)
        {

            string methodname = dic.ServiceName();
            methodname = methodname + "get";
            dic.AddServiceName(methodname);
            return DoCore(dic);
        }

        /// <summary>
        /// [列表]根据数据库中的表名;获取表中的数据；参数可以为空；
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public object sysconfigServices(Dictionary<string, string> dic)
        {

            string methodname = dic.ServiceName();
            methodname = methodname + "getlist";
            dic.AddServiceName(methodname);
            return DoCore(dic);
        }


        /// <summary>
        /// [添加] 根据数据库中的中页面配置名;添加数据到指定接口；参数可以为空
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public object sysconfigServicesadd(Dictionary<string, string> dic)
        {

            string methodname = dic.MethodName();
            methodname = methodname + "add";
            dic.AddMethodName(methodname);
            return DoCore(dic);
        }

        /// <summary>
        ///  [更新]根据数据库中的中页面配置名;通过接口更新数据；参数可以为空；
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public object sysconfigServicesedit(Dictionary<string, string> dic)
        {

            string methodname = dic.ServiceName();
            methodname = methodname + "edit";
            dic.AddServiceName(methodname);
            return DoCore(dic);
        }
        /// <summary>
        /// [更新]根据数据库中的中页面配置名;通过接口更新数据；参数可以为空；
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public object sysconfigServicesupdate(Dictionary<string, string> dic)
        {
            string methodname = dic.ServiceName();
            methodname = methodname + "update";
            dic.AddServiceName(methodname);
            return DoCore(dic);
        }
        /// <summary>
        /// [删除]根据数据库中的中页面配置名;通过接口删除（逻辑删除）指定数据；参数可以为空
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dic"></param>
        /// <returns></returns>

        public object sysconfigServicesdelete(Dictionary<string, string> dic)
        {

            string methodname = dic.ServiceName();
            methodname = methodname + "delete";
            dic.AddServiceName(methodname);
            return DoCore(dic);
        }
        /// <summary>
        /// [删除]根据数据库中的中页面配置名;通过接口删除(物理删除)指定数据；参数可以为空；地址组合：{数据库配置名}APP/{数据库中页面配置名}/delete
        /// </summary>
        /// <param name="id">数据库配置名</param>
        /// <param name="tname">数据库中页面配置名</param>
        /// <param name="dic">传入参数</param>
        /// <returns></returns>
        public object sysconfigServicesremovebykey(Dictionary<string, string> dic)
        {

            string methodname = dic.ServiceName();
            methodname = methodname + "removebykey";
            dic.AddServiceName(methodname);
            return DoCore(dic);
        }
        /// <summary>
        ///  [获取数据]根据数据库中的中页面配置名;通过接口获取指定数据；参数可以为空；
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public object sysconfigServicesget(Dictionary<string, string> dic)
        {

            string methodname = dic.ServiceName();
            methodname = methodname + "gets";
            dic.AddServiceName(methodname);
            return DoCore(dic);
        }

        /// <summary>
        ///  [获取数据]根据数据库中的中页面配置名;通过接口获取指定数据；参数可以为空；
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dic"></param>
        /// <returns></returns>

        public object sysconfigServiceslist(Dictionary<string, string> dic)
        {
            string methodname = dic.ServiceName();
            methodname = methodname + "list";
            dic.AddServiceName(methodname);
            return DoCore(dic);
        }
        /// <summary>
        /// 根据数据库配置，服务配置 执行对应的业务逻辑；地址组合：{数据库配置名}APP/{数据库中表SysPageService中字段servicename服务名}Service
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public object Services(Dictionary<string, string> dic)
        {
            string methodname = dic.ServiceName();
            methodname = methodname + "service";
            dic.AddServiceName(methodname);
            return DoCore(dic);
        }

        /// <summary>
        /// 多ＳＱＬ语句执行，非事务；服务名：{ctrl}bntservice
        /// </summary>
        /// <param name="context"></param>
        /// <param name="ctrl"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public Result BatServiceNotTransRecords(string ctrl, Dictionary<string, object> model)
        {
            var dic = model.ToDictionary();

            string sqlstr = BaseSQL.SQL_SysPageService_List;//@"SELECT[ServiceEvents],[ReturnType]FROM[dbo].[SysPageService]where [servicename]='{0}bntservice'";
            string result = "";
            sqlstr = sqlstr.Replace("{0}", ctrl); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
            FileHelper.Writelog(sqlstr, "SQL");
            var dt = dbh.ExecuteDataTable(sqlstr.ToLower());
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
                                var rsdt = dbh.ExecuteSQL(sqlstr.Str2SQL());


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
        /// SQL事务执行；服务名：{ctrl}sqlexecute
        /// </summary>
        /// <param name="context"></param>
        /// <param name="ctrl"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public Result ExecuteSQLServiceNotTransRecords(string ctrl, Dictionary<string, object> model)
        {
            var dic = model.ToDictionary();
            string sqlstr = BaseSQL.SQL_SysPageService_List;//@"SELECT[ServiceEvents],[ReturnType]FROM[dbo].[SysPageService]where [servicename]='{0}sqlexecute'";
            string result = "";
            sqlstr = sqlstr.Replace("{0}", ctrl); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
            FileHelper.Writelog(sqlstr, "SQL");
            var dt = dbh.ExecuteDataTable(sqlstr.ToLower());
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
                            var rsdt = dbh.ExecuteSQL(sqlstr.Str2SQL());


                            if (rsdt < 0)
                            {
                                result = "业务执行失败！";
                            }


                        }



                }

            return new Result(result);

        }

        /// <summary>
        ///  通用导出功能
        /// </summary>
        /// <param name="context"></param>
        /// <param name="ctrl"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public object ReplaceTemplate(string ctrl, Dictionary<string, string> dic)
        {
            string sqlstr = BaseSQL.SQL_SysPageService_List;//@"SELECT[ServiceEvents],[ReturnType]FROM[dbo].[SysPageService]where [servicename]='{0}sqlexecute'";

            sqlstr = sqlstr.Replace("{0}", ctrl);
            FileHelper.Writelog(sqlstr, "SQL");
            var dt = dbh.ExecuteDataTable(sqlstr.ToLower());
            return new Result(dt);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sqlstr"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public object CoreExecuteSQL(string sqlstr, Dictionary<string, string> dic)
        {
            sqlstr = dic.ToSQL(sqlstr);
            sqlstr = sqlstr.Replace("=n'", "=N'");
            FileHelper.Writelog(sqlstr, "SQL");
            return dbh.ExecuteDataTable(sqlstr.Str2SQL());
        }

        #endregion

    }
}
