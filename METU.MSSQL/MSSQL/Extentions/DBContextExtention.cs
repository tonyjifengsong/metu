using METU.CACHES;
using METU.MODEL;
using METU.MSSQL;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Reflection;

namespace Microsoft.EntityFrameworkCore
{/// <summary>
/// 
/// </summary>
    public static class DBContextExtention
    {
        public static class MSGBox
        {
            /// <summary>
            /// 异常信息
            /// </summary>
            /// <param name="Msg"></param>
            public static void EXMessage(string Msg, string MSGCode = "0")
            {
                throw new DebugException(MSGCode, Msg);
            }


        }
        /// <summary>
        /// 
        /// </summary>

        static DbConnection _dbConnection = null;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static DbConnection CreateDbConnection(this DbContext context)
        {
            DbConnection dbConn = context.Database.GetDbConnection();// DbHelper.CreateDbConnection(DbHelper.DbConnectionString, DbHelper.DbProviderName);
            _dbConnection = dbConn;
            return dbConn;
        }/// <summary>
         /// 
         /// </summary>
         /// <param name="context"></param>
         /// <param name="sql"></param>
         /// <param name="parameters"></param>
         /// <returns></returns>


        [Obsolete]
        public static int ExecuteSQL(this DbContext context, string sql, params object[] parameters)
        {
            FileHelper.Writelog(sql);
            FileHelper.Writelog(parameters);

            var result = context.Database.ExecuteSqlRaw(sql, parameters);
            return result;
        }
        /// <summary>
        /// 通过ＳＱＬ语句返回影响结果行数
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sql">ＳＱＬ语句</param>
        /// <returns></returns>
        public static int ExecuteSQL(this DbContext context, string sql)
        {
            FileHelper.Writelog(sql);


            int rows = -1;

            if (sql == null) return 0;
            using (DbConnection conn = context.Database.GetDbConnection())
            {
                if (conn.State != System.Data.ConnectionState.Open)
                {
                    conn.Open();
                }
                using (DbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandTimeout = 4;
                    cmd.Connection = conn;
                    cmd.CommandText = sql;
                    try
                    {

                        rows = cmd.ExecuteNonQuery();
                        return rows;
                    }
                    catch (DbException ee)
                    {
                        rows = -1;
                        conn.Close();
                        conn.Dispose();
                        throw new Exception(ee.Message); ;



                    }
                }
            }

        }
        #region 跨库事务 初始化变量
        public static DbConnection Muticonn = null;
        public static DbCommand Muticmd = null;
        public static DbTransaction Mutitx = null;
        #endregion
        /// <summary>
        /// 跨库事务执行开始
        /// </summary>
        /// <param name="context"></param>
        /// <param name="SQLStringList"></param>
        public static int MutiTranBegin(this DbContext context, string[] SQLStringList = null)
        {
            FileHelper.Writelog(SQLStringList);

            if (SQLStringList == null) return -1;
            if (SQLStringList.Length == 0) return -1;
            try
            {
                context.Database.SetCommandTimeout(4);
                Muticonn = context.Database.GetDbConnection();
            }
            catch
            {
                return -1;
            }

            if (Muticonn.State != System.Data.ConnectionState.Open)
            {
                Muticonn.Open();
            }
            try
            {
                Muticmd = Muticonn.CreateCommand();
            }
            catch
            {
                return -1;
            }

            Muticmd.Connection = Muticonn;
            try
            {
                Mutitx = Muticonn.BeginTransaction();
            }
            catch
            {
                return -1;
            }

            Muticmd.Transaction = Mutitx;
            int rs = 0;
            try
            {
                for (int n = 0; n < SQLStringList.Length; n++)
                {
                    string strsql = SQLStringList[n].ToString();
                    if (strsql.Trim().Length > 1)
                    {
                        Muticmd.CommandText = strsql;
                        Muticmd.ExecuteNonQuery();
                    }
                    rs++;
                }
                //tx.Commit();
            }
            catch (DbException ex)
            {
                Mutitx.Rollback();
                Muticonn.Close();
                Muticonn.Dispose();
                //throw new Exception(ex.Message); ;
                return -1;
            }

            if (rs > 0) return rs;


            return 1;
        }
        /// <summary>
        /// 跨库事务执行开始
        /// </summary>
        /// <param name="context"></param>
        /// <param name="SQLStringList"></param>
        public static int MutiTranCommit(this DbContext context)
        {
            try
            {
                Mutitx.Commit();
                return 1;
            }
            catch
            {
                Mutitx.Rollback();
                Muticonn.Close();
                Muticonn.Dispose();
                return -1;
            }


        }

        /// <summary>
        /// 执行Transaction ＳＱＬ语句,不返回值
        /// </summary>
        /// <param name="context"></param>
        /// <param name="SQLStringList"></param>
        public static void ExecuteSqlTran(this DbContext context, string[] SQLStringList = null)
        {
            FileHelper.Writelog(SQLStringList);

            if (SQLStringList == null) return;
            using (DbConnection conn = context.Database.GetDbConnection())
            {
                if (conn.State != System.Data.ConnectionState.Open)
                {
                    conn.Open();
                }
                using (DbCommand cmd = conn.CreateCommand())
                {
                    cmd.Connection = conn;
                    using (DbTransaction tx = conn.BeginTransaction())
                    {
                        cmd.Transaction = tx;
                        try
                        {
                            for (int n = 0; n < SQLStringList.Length; n++)
                            {
                                string strsql = SQLStringList[n].ToString();
                                if (strsql.Trim().Length > 1)
                                {
                                    cmd.CommandTimeout = 4;
                                    cmd.CommandText = strsql;
                                    cmd.ExecuteNonQuery();
                                }
                            }
                            tx.Commit();
                        }
                        catch (DbException ex)
                        {
                            tx.Rollback();
                            conn.Close();
                            conn.Dispose();
                            throw new Exception(ex.Message); ;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="SQLStringList"></param>
        public static void ExecuteSqlTran(this DbContext context, IList<string> SQLStringList = null)
        {

            if (SQLStringList == null) return;
            using (DbConnection conn = context.Database.GetDbConnection())
            {
                FileHelper.Writelog(SQLStringList);

                if (conn.State != System.Data.ConnectionState.Open)
                {
                    conn.Open();
                }
                using (DbCommand cmd = conn.CreateCommand())
                {
                    cmd.Connection = conn;
                    using (DbTransaction tx = conn.BeginTransaction())
                    {
                        cmd.Transaction = tx;
                        try
                        {
                            for (int n = 0; n < SQLStringList.Count; n++)
                            {
                                string strsql = SQLStringList[n].ToString();
                                if (strsql.Trim().Length > 1)
                                {
                                    cmd.CommandTimeout = 4;
                                    cmd.CommandText = strsql;
                                    cmd.ExecuteNonQuery();
                                }
                            }
                            tx.Commit();
                        }
                        catch (DbException ex)
                        {
                            tx.Rollback();
                            conn.Close();
                            conn.Dispose();
                            throw new Exception(ex.Message); ;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="SQLString">计算查询结果语句</param>
        /// <returns>查询结果（object）</returns>
        public static object GetSingle(this DbContext context, string SQLString = null)
        {

            if (SQLString == null) return null;
            using (DbConnection conn = context.Database.GetDbConnection())
            {
                FileHelper.Writelog(SQLString);

                if (conn.State != System.Data.ConnectionState.Open)
                {
                    conn.Open();
                }
                using (DbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandTimeout = 4;
                    cmd.Connection = conn;
                    cmd.CommandText = SQLString;
                    try
                    {

                        object obj = cmd.ExecuteScalar();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (DbException e)
                    {
                        conn.Close();
                        conn.Dispose();
                        throw new Exception(e.Message); ;
                    }
                }
            }
        }


        /// <summary>
        /// 执行查询语句，返回SqlDataReader
        /// </summary>
        /// <param name="strSQL">查询语句</param>
        /// <returns>SqlDataReader</returns>
        public static DbDataReader ExecuteReader(this DbContext context, string strSQL = null)
        {

            if (strSQL == null) return null;
            DbConnection conn = context.Database.GetDbConnection();
            if (conn.State != System.Data.ConnectionState.Open)
            {
                FileHelper.Writelog(strSQL, "SQL");

                conn.Open();
            }
            DbCommand cmd = conn.CreateCommand();
            cmd.Connection = conn; cmd.CommandTimeout = 4;
            cmd.CommandText = strSQL;
            try
            {

                DbDataReader myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return myReader;
            }
            catch (System.Data.Common.DbException e)
            {
                conn.Close();
                conn.Dispose();
                throw new Exception(e.Message); ;
            }

        }
    



        #region 执行带参数的SQL语句

        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSql(this DbContext context, string SQLString, DbParameter[] cmdParms = null)
        {
            FileHelper.Writelog(SQLString);
            FileHelper.Writelog(cmdParms);

            if (cmdParms == null) return ExecuteSQL(context, SQLString);
            using (DbConnection connection = context.Database.GetDbConnection())
            {

                using (DbCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandTimeout = 4;
                    cmd.Connection = connection;
                    cmd.CommandText = SQLString;
                    try
                    {
                        PrepareCommand(context, cmd, null, SQLString, cmdParms);
                        int rows = cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                        return rows;
                    }
                    catch (DbException E)
                    {
                        connection.Close();
                        connection.Dispose();
                        return -1;
                    }
                }
            }
        }

        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">SQL语句的哈希表（key为sql语句，value是该语句的SqlParameter[]）</param>
        public static void ExecuteSqlTrans(this DbContext context, string[] SQLStringList)
        {

            using (DbConnection conn = context.Database.GetDbConnection())
            {
                using (DbTransaction trans = conn.BeginTransaction())
                {
                    FileHelper.Writelog(SQLStringList);

                    using (DbCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandTimeout = 4;
                        try
                        {
                            //循环
                            foreach (string myDE in SQLStringList)
                            {
                                string cmdText = myDE.ToString();
                                PrepareCommand(context, cmd, trans, cmdText, null);
                                int val = cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                            }
                            trans.Commit();
                        }
                        catch (DbException ex)
                        {
                            trans.Rollback();
                            conn.Close();
                            conn.Dispose();
                            return;
                        }
                    }
                }
            }
        }
        public static void ExecuteSqlTran(this DbContext context, Dictionary<string, DbParameter[]> dics)
        {

            using (DbConnection conn = context.Database.GetDbConnection())
            {
                FileHelper.Writelog(dics);

                using (DbTransaction trans = conn.BeginTransaction())
                {
                    using (DbCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandTimeout = 4;
                        try
                        {
                            //循环
                            foreach (var myDE in dics)
                            {
                                string cmdText = myDE.Key.ToString();
                                DbParameter[] cmdParms = myDE.Value;
                                PrepareCommand(context, cmd, trans, cmdText, cmdParms);
                                int val = cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                            }
                            trans.Commit();
                        }
                        catch (DbException ex)
                        {
                            trans.Rollback();
                            conn.Close();
                            conn.Dispose();
                            return;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）,返回首行首列的值;
        /// </summary>
        /// <param name="SQLString">计算查询结果语句</param>
        /// <returns>查询结果（object）</returns>
        public static object GetSingle(this DbContext context, string SQLString, DbParameter[] cmdParms = null)
        {

            if (cmdParms == null) return GetSingle(context, SQLString);
            using (DbConnection connection = context.Database.GetDbConnection())
            {
                FileHelper.Writelog(SQLString);
                FileHelper.Writelog(cmdParms);


                using (DbCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandTimeout = 4;
                    try
                    {
                        PrepareCommand(context, cmd, null, SQLString, cmdParms);
                        object obj = cmd.ExecuteScalar();
                        cmd.Parameters.Clear();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (DbException e)
                    {
                        connection.Close();
                        connection.Dispose();
                        return null;
                    }
                }
            }
        }

        /// <summary>
        /// 执行查询语句，返回SqlDataReader
        /// </summary>
        /// <param name="strSQL">查询语句</param>
        /// <returns>SqlDataReader</returns>
        public static DbDataReader ExecuteReader(this DbContext context, string SQLString, DbParameter[] cmdParms)
        {

            DbConnection connection = context.Database.GetDbConnection();

            DbCommand cmd = connection.CreateCommand(); cmd.CommandTimeout = 4;
            try
            {
                FileHelper.Writelog(SQLString);
                FileHelper.Writelog(cmdParms);

                PrepareCommand(context, cmd, null, SQLString, cmdParms);
                cmd.CommandTimeout = 4;
                DbDataReader myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return myReader;
            }
            catch (DbException e)
            {
                connection.Close();
                connection.Dispose();
                return null;
            }

        }



        private static void PrepareCommand(this DbContext context, DbCommand cmd, DbTransaction trans, string cmdText, DbParameter[] cmdParms)
        {
            DbConnection conn = context.Database.GetDbConnection();

            if (conn.State != ConnectionState.Open)
            {
                FileHelper.Writelog(cmdText);
                FileHelper.Writelog(cmdParms);

                conn.Open();
            }
            cmd.CommandTimeout = 4;
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
            {
                cmd.Transaction = trans;
            }
            cmd.CommandType = CommandType.Text;//cmdType;
            if (cmdParms != null)
            {
                foreach (DbParameter parm in cmdParms)
                {
                    cmd.Parameters.Add(parm);
                }
            }
        }

        #endregion

        #region 存储过程操作
        /// <summary>
        /// 执行存储过程;
        /// </summary>
        /// <param name="storeProcName">存储过程名</param>
        /// <param name="parameters">所需要的参数</param>
        /// <returns>返回受影响的行数</returns>
        public static int RunProcedureExecuteSql(this DbContext context, string storeProcName, DbParameter[] parameters)
        {

            using (DbConnection connection = context.Database.GetDbConnection())
            {

                DbCommand cmd = BuildQueryCommand(context, storeProcName, parameters);
                cmd.CommandTimeout = 4;
                int rows = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                connection.Close();
                return rows;
            }
        }

        /// <summary>
        /// 执行存储过程,返回首行首列的值
        /// </summary>
        /// <param name="storeProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>返回首行首列的值</returns>
        public static Object RunProcedureGetSingle(this DbContext context, string storeProcName, DbParameter[] parameters)
        {

            using (DbConnection connection = context.Database.GetDbConnection())
            {

                try
                {
                    DbCommand cmd = BuildQueryCommand(context, storeProcName, parameters);
                    object obj = cmd.ExecuteScalar();
                    cmd.Parameters.Clear();
                    if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                    {
                        return null;
                    }
                    else
                    {
                        return obj;
                    }
                }
                catch (DbException e)
                {
                    connection.Close();
                    connection.Dispose();
                    return null;
                }
            }
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>SqlDataReader</returns>
        public static DbDataReader RunProcedureGetDataReader(this DbContext context, string storedProcName, DbParameter[] parameters = null)
        {

            if (parameters == null) return null;
            DbConnection connection = context.Database.GetDbConnection();

            DbDataReader returnReader;
            DbCommand cmd = BuildQueryCommand(context, storedProcName, parameters);
            cmd.CommandType = CommandType.StoredProcedure;
            returnReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            cmd.Parameters.Clear();
            return returnReader;
        }


        /// <summary>
        /// 执行多个存储过程，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">存储过程的哈希表（value为存储过程语句，key是该语句的DbParameter[]）</param>
        public static bool RunProcedureTran(this DbContext context, string[] SQLStringList = null)
        {

            if (SQLStringList == null) return false;
            using (DbConnection connection = context.Database.GetDbConnection())
            {
                FileHelper.Writelog(SQLStringList);

                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (DbTransaction trans = connection.BeginTransaction())
                {
                    using (DbCommand cmd = connection.CreateCommand())
                    {
                        try
                        {
                            //循环
                            foreach (string myDE in SQLStringList)
                            {
                                cmd.Connection = connection;
                                string storeName = myDE.ToString();

                                cmd.Transaction = trans;
                                cmd.CommandText = storeName;
                                cmd.CommandType = CommandType.StoredProcedure;

                                int val = cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                            }
                            trans.Commit();
                            return true;
                        }
                        catch
                        {
                            trans.Rollback();
                            connection.Close();
                            connection.Dispose();
                            return false;
                        }
                    }
                }
            }
        }
        public static bool RunProcedureTran(this DbContext context, Dictionary<string, DbParameter[]> Dics = null)
        {

            if (Dics == null) return false;
            using (DbConnection connection = context.Database.GetDbConnection())
            {
                FileHelper.Writelog(Dics);

                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (DbTransaction trans = connection.BeginTransaction())
                {
                    using (DbCommand cmd = connection.CreateCommand())
                    {
                        try
                        {
                            //循环
                            foreach (var myDE in Dics)
                            {
                                cmd.Connection = connection;
                                string storeName = myDE.Key.ToString();

                                DbParameter[] cmdParms = myDE.Value; ;

                                cmd.Transaction = trans;
                                cmd.CommandText = storeName;
                                cmd.CommandType = CommandType.StoredProcedure;
                                if (cmdParms != null)
                                {
                                    foreach (DbParameter parameter in cmdParms)
                                    {
                                        cmd.Parameters.Add(parameter);
                                    }
                                }
                                int val = cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                            }
                            trans.Commit();
                            return true;
                        }
                        catch
                        {
                            trans.Rollback();
                            connection.Close();
                            connection.Dispose();
                            return false;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 执行多个存储过程，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">存储过程的哈希表（value为存储过程语句，key是该语句的DbParameter[]）</param>
        public static bool RunProcedureTran(this DbContext context, Dictionary<string, Dictionary<string, DbParameter>> Dics = null)
        {

            if (Dics == null) return false;


            using (DbConnection connection = context.Database.GetDbConnection())
            {
                FileHelper.Writelog(Dics);

                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (DbTransaction trans = connection.BeginTransaction())
                {
                    using (DbCommand cmd = connection.CreateCommand())
                    {
                        try
                        {
                            //循环
                            foreach (var item in Dics)
                            {
                                cmd.Connection = connection;
                                string storeName = item.Key.ToString();
                                Dictionary<string, DbParameter> cmdParms = new Dictionary<string, DbParameter>();
                                cmdParms = item.Value;
                                cmd.Transaction = trans;
                                cmd.CommandText = storeName;
                                cmd.CommandType = CommandType.StoredProcedure;
                                if (cmdParms != null)
                                {
                                    foreach (DbParameter parameter in cmdParms.Values)
                                    {
                                        cmd.Parameters.Add(parameter);
                                    }
                                }
                                int val = cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                            }
                            trans.Commit();
                            return true;
                        }
                        catch
                        {
                            trans.Rollback();
                            connection.Close();
                            connection.Dispose();
                            return false;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 构建 SqlCommand 对象(用来返回一个结果集，而不是一个整数值)
        /// </summary>
        /// <param name="connection">数据库连接</param>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>SqlCommand</returns>
        private static DbCommand BuildQueryCommand(this DbContext context, string storedProcName, DbParameter[] parameters = null)
        {
            DbConnection connection = context.Database.GetDbConnection();
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
            FileHelper.Writelog(storedProcName);
            FileHelper.Writelog(parameters);


            DbCommand command = connection.CreateCommand();
            command.CommandText = storedProcName;
            command.Connection = connection;
            command.CommandType = CommandType.StoredProcedure;
            if (parameters != null)
            {
                foreach (DbParameter parameter in parameters)
                {
                    command.Parameters.Add(parameter);
                }
            }
            return command;
        }
        #endregion


        #region 获取DbCommand对象
        /// <summary>
        /// 根据存储过程名称来构建当前数据库链接的DbCommand对象
        /// </summary>
        /// <param name="storedProcedure">存储过程名称</param>
        public static DbCommand GetStoredProcedureCommond(this DbContext context, string storedProcedure)
        {
            _dbConnection = context.CreateDbConnection();
            DbCommand dbCmd = _dbConnection.CreateCommand();
            dbCmd.CommandTimeout = 4;
            dbCmd.CommandText = storedProcedure;
            dbCmd.CommandType = CommandType.StoredProcedure;

            return dbCmd;
        }

        /// <summary>
        /// 根据SQL语句来构建当前数据库链接的DbCommand对象
        /// </summary>
        /// <param name="sqlQuery">SQL查询语句</param>
        public static DbCommand GetSqlStringCommond(string sqlQuery)
        {
            FileHelper.Writelog(sqlQuery);

            DbCommand dbCmd = _dbConnection.CreateCommand();
            dbCmd.CommandTimeout = 4;
            dbCmd.CommandText = sqlQuery;
            dbCmd.CommandType = CommandType.Text;

            return dbCmd;
        }
        #endregion

        #region 添加DbCommand参数
        /// <summary>
        /// 把参数集合添加到DbCommand对象中
        /// </summary>
        /// <param name="cmd">数据库命令操作对象</param>
        /// <param name="dbParameterCollection">数据库操作集合</param>
        public static void AddParameterCollection(this DbContext context, DbCommand cmd, DbParameterCollection dbParameterCollection)
        {
            if (cmd != null)
            {
                foreach (DbParameter dbParameter in dbParameterCollection)
                {
                    cmd.CommandTimeout = 4;
                    cmd.Parameters.Add(dbParameter);
                }
            }
        }

        /// <summary>
        /// 把输出参数添加到DbCommand对象中
        /// </summary>
        /// <param name="cmd">数据库命令操作对象</param>
        /// <param name="parameterName">参数名称</param>
        /// <param name="dbType">参数的类型</param>
        /// <param name="size">参数的大小</param>
        public static void AddOutParameter(this DbContext context, DbCommand cmd, string parameterName, DbType dbType, int size)
        {
            if (cmd != null)
            {
                DbParameter dbParameter = cmd.CreateParameter();

                dbParameter.DbType = dbType;
                dbParameter.ParameterName = parameterName;
                dbParameter.Size = size;
                dbParameter.Direction = ParameterDirection.Output;
                cmd.CommandTimeout = 4;
                cmd.Parameters.Add(dbParameter);
            }
        }

        /// <summary>
        /// 把输入参数添加到DbCommand对象中
        /// </summary>
        /// <param name="cmd">数据库命令操作对象</param>
        /// <param name="parameterName">参数名称</param>
        /// <param name="dbType">参数的类型</param>
        /// <param name="value">参数值</param>
        public static void AddInParameter(this DbContext context, DbCommand cmd, string parameterName, DbType dbType, object value)
        {
            if (cmd != null)
            {
                DbParameter dbParameter = cmd.CreateParameter();

                dbParameter.DbType = dbType;
                dbParameter.ParameterName = parameterName;
                dbParameter.Value = value;
                dbParameter.Direction = ParameterDirection.Input;
                cmd.CommandTimeout = 4;
                cmd.Parameters.Add(dbParameter);
            }
        }

        /// <summary>
        /// 把返回参数添加到DbCommand对象中
        /// </summary>
        /// <param name="cmd">数据库命令操作对象</param>
        /// <param name="parameterName">参数名称</param>
        /// <param name="dbType">参数的类型</param>
        public static void AddReturnParameter(this DbContext context, DbCommand cmd, string parameterName, DbType dbType)
        {
            if (cmd != null)
            {
                DbParameter dbParameter = cmd.CreateParameter();

                dbParameter.DbType = dbType;
                dbParameter.ParameterName = parameterName;
                dbParameter.Direction = ParameterDirection.ReturnValue;
                cmd.CommandTimeout = 4;
                cmd.Parameters.Add(dbParameter);
            }
        }

        /// <summary>
        /// 根据参数名称从DbCommand对象中获取相应的参数对象
        /// </summary>
        /// <param name="cmd">数据库命令操作对象</param>
        /// <param name="parameterName">参数名称</param>
        public static DbParameter GetParameter(this DbContext context, DbCommand cmd, string parameterName)
        {
            if (cmd != null && cmd.Parameters.Count > 0)
            {
                cmd.CommandTimeout = 4;
                DbParameter param = cmd.Parameters[parameterName];

                return param;
            }

            return null;
        }
        #endregion

        #region 执行SQL脚本语句
        ///// <summary>
        ///// 执行相应的SQL命令，返回一个DataSet数据集合
        ///// </summary>
        ///// <param name="sqlQuery">需要执行的SQL语句</param>
        ///// <returns>返回一个DataSet数据集合</returns>
        public static DataSet ExecuteDataSet(this DbContext context, string sqlQuery)
        {
            DataSet ds = new DataSet();
            _dbConnection = context.CreateDbConnection();
            if (!string.IsNullOrEmpty(sqlQuery))
            {
                DataTable dt = new DataTable();
                FileHelper.Writelog(sqlQuery);
                try
                {
                    context.Database.SetCommandTimeout(3);
                    var result = context.ExecuteReader(sqlQuery);
                    if (result != null)
                        dt.Load(result);
                    if (dt != null)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            ds.Tables.Add(dt);
                        }
                    }

                }
                catch
                {
                    return null;
                }
            }

            return ds;
        }

        /// <summary>
        /// 执行相应的SQL命令，返回一个DataTable数据集
        /// </summary>
        /// <param name="sqlQuery">需要执行的SQL语句</param>
        /// <returns>返回一个DataTable数据集</returns>
        public static DataTable  ExecuteDataTable(this DbContext context, string sqlQuery)
        {

            DataTable dt = new DataTable();

            if (!string.IsNullOrEmpty(sqlQuery))
            {
                FileHelper.Writelog(sqlQuery);
                try
                {
                    context.Database.SetCommandTimeout(3);
                    var result = context.ExecuteReader(sqlQuery);
                    if (result != null)
                    {

                        dt.Load(result);




                    }
                    else
                    {
                        dt = null;
                    }

                }
                catch
                {
                    return null;
                }
            }
            return dt;
        }
 
        #endregion
        #region 创建DbProviderFactory对象(静态方法)
        /// <summary>
        /// 根据配置的数据库提供程序的DbProviderName名称来创建一个数据库配置的提供程序DbProviderFactory对象
        /// </summary>
        public static DbProviderFactory CreateDbProviderFactory()
        {
            DbProviderFactory dbFactory = CreateDbProviderFactory(DbProviderName);

            _dbFactory = dbFactory;
            return _dbFactory;
        }
        public static string DbProviderName = "System.Data.SqlClient";// System.Configuration.ConfigurationManager.AppSettings["DbProviderName"];

        /// <summary>
        /// 根据参数名称创建一个数据库提供程序DbProviderFactory对象
        /// </summary>
        /// <param name="dbProviderName">数据库提供程序的名称</param>
        public static DbProviderFactory CreateDbProviderFactory(string dbProviderName)
        {
            DbProviderFactory dbFactory = DbProviderFactories.GetFactory(dbProviderName);
            _dbFactory = dbFactory;
            return dbFactory;
        }
        #endregion
        #region 执行DbCommand命令
        /// <summary>
        /// 当前默认配置的数据库提供程序DbProviderFactory
        /// </summary>
        private static DbProviderFactory _dbFactory = null;
        /// <summary>
        /// 执行相应的命令，返回一个DataSet数据集合
        /// </summary>
        /// <param name="cmd">需要执行的DbCommand命令对象</param>
        /// <returns>返回一个DataSet数据集合</returns>
        public static DataSet ExecuteDataSet(this DbContext context, DbCommand cmd)
        {
            _dbFactory = CreateDbProviderFactory();
            DataSet ds = new DataSet();

            if (cmd != null)
            {
                DbDataAdapter dbDataAdapter = _dbFactory.CreateDataAdapter();
                dbDataAdapter.SelectCommand = cmd;

                dbDataAdapter.Fill(ds);
            }

            return ds;
        }

        /// <summary>
        /// 执行相应的命令，返回一个DataTable数据集合
        /// </summary>
        /// <param name="cmd">需要执行的DbCommand命令对象</param>
        /// <returns>返回一个DataTable数据集合</returns>
        public static DataTable  ExecuteDataTable(this DbContext context, DbCommand cmd)
        {

            DataTable dataTable = new DataTable();
            CreateDbProviderFactory();
            if (cmd != null)
            {
                DbDataAdapter dbDataAdapter = _dbFactory.CreateDataAdapter();
                dbDataAdapter.SelectCommand = cmd;

                dbDataAdapter.Fill(dataTable);
            }

            return dataTable;
        }

        /// <summary>
        /// 执行相应的命令，返回一个DbDataReader数据对象，如果没有则返回null值
        /// </summary>
        /// <param name="cmd">需要执行的DbCommand命令对象</param>
        /// <returns>返回一个DbDataReader数据对象，如果没有则返回null值</returns>
        public static DbDataReader ExecuteReader(this DbContext context, DbCommand cmd)
        {
            CreateDbProviderFactory();
            if (cmd != null && cmd.Connection != null)
            {
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.CommandTimeout = 4;
                    cmd.Connection.Open();
                }

                DbDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);//当reader读取结束时自动关闭数据库链接

                return reader;
            }

            return null;
        }

        /// <summary>
        /// 执行相应的命令，返回影响的数据记录数，如果不成功则返回-1
        /// </summary>
        /// <param name="cmd">需要执行的DbCommand命令对象</param>
        /// <returns>返回影响的数据记录数，如果不成功则返回-1</returns>
        public static int ExecuteNonQuery(this DbContext context, DbCommand cmd)
        {
            if (cmd != null && cmd.Connection != null)
            {
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.CommandTimeout = 4;
                    cmd.Connection.Open();
                }

                int retVal = cmd.ExecuteNonQuery();

                cmd.Connection.Close();

                return retVal;
            }

            return -1;
        }

        /// <summary>
        /// 执行相应的命令，返回结果集中的第一行第一列的值，如果不成功则返回null值
        /// </summary>
        /// <param name="cmd">需要执行的DbCommand命令对象</param>
        /// <returns>返回结果集中的第一行第一列的值，如果不成功则返回null值</returns>
        public static object ExecuteScalar(this DbContext context, DbCommand cmd)
        {
            if (cmd != null && cmd.Connection != null)
            {
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.CommandTimeout = 4;
                    cmd.Connection.Open();
                }

                object retVal = cmd.ExecuteScalar();

                cmd.Connection.Close();

                return retVal;
            }

            return null;
        }
        #endregion

        #region 执行DbTransaction事务
        /// <summary>
        /// 以事务的方式执行相应的命令，返回一个DataSet数据集合
        /// </summary>
        /// <param name="cmd">需要执行的DbCommand命令对象</param>
        /// <param name="trans">数据库事务对象</param>
        /// <returns>返回一个DataSet数据集合</returns>
        public static DataSet ExecuteDataSet(this DbContext context, DbCommand cmd, Trans trans = null)
        {
            trans = new Trans(context);
            DataSet ds = new DataSet();
            CreateDbProviderFactory();

            if (cmd != null)
            {
                cmd.CommandTimeout = 4;
                cmd.Connection = context.Database.GetDbConnection();
                cmd.Transaction = context.Database.GetDbConnection().BeginTransaction();

                DbDataAdapter dbDataAdapter = _dbFactory.CreateDataAdapter();
                dbDataAdapter.SelectCommand = cmd;

                dbDataAdapter.Fill(ds);
            }

            return ds;
        }

        /// <summary>
        /// 以事务的方式执行相应的命令，返回一个DataTable数据集合
        /// </summary>
        /// <param name="cmd">需要执行的DbCommand命令对象</param>
        /// <param name="trans">数据库事务对象</param>
        /// <returns>返回一个DataTable数据集合</returns>
        public static DataTable  ExecuteDataTable(this DbContext context, DbCommand cmd, Trans trans = null)
        {
            trans = new Trans(context);
            DataTable dataTable = new DataTable();
            CreateDbProviderFactory();

            if (cmd != null)
            {
                cmd.CommandTimeout = 4;
                cmd.Connection = trans.Connection;
                cmd.Transaction = trans.Transaction;

                DbDataAdapter dbDataAdapter = _dbFactory.CreateDataAdapter();
                dbDataAdapter.SelectCommand = cmd;

                dbDataAdapter.Fill(dataTable);
            }

            return dataTable;
        }

        /// <summary>
        /// 以事务的方式执行相应的命令，返回一个DbDataReader数据对象，如果没有则返回null值
        /// </summary>
        /// <param name="cmd">需要执行的DbCommand命令对象</param>
        /// <param name="trans">数据库事务对象</param>
        /// <returns>返回一个DbDataReader数据对象，如果没有则返回null值</returns>
        public static DbDataReader ExecuteReader(this DbContext context, DbCommand cmd, Trans trans = null)
        {
            trans = new Trans(context);
            if (cmd != null)
            {
                cmd.Connection.Close();
                cmd.CommandTimeout = 4;
                cmd.Connection = trans.Connection;
                cmd.Transaction = trans.Transaction;

                DbDataReader reader = cmd.ExecuteReader();

                return reader;
            }

            return null;
        }

        /// <summary>
        /// 以事务的方式执行相应的命令，返回影响的数据记录数，如果不成功则返回-1
        /// </summary>
        /// <param name="cmd">需要执行的DbCommand命令对象</param>
        /// <param name="trans">数据库事务对象</param>
        /// <returns>返回影响的数据记录数，如果不成功则返回-1</returns>
        public static int ExecuteNonQuery(this DbContext context, DbCommand cmd, Trans trans = null)
        {
            trans = new Trans(context);
            if (cmd != null)
            {
                cmd.Connection.Close();
                cmd.CommandTimeout = 4;
                cmd.Connection = trans.Connection;
                cmd.Transaction = trans.Transaction;

                int retVal = cmd.ExecuteNonQuery();

                return retVal;
            }

            return -1;
        }

        /// <summary>
        /// 以事务的方式执行相应的命令，返回结果集中的第一行第一列的值，如果不成功则返回null值
        /// </summary>
        /// <param name="cmd">需要执行的DbCommand命令对象</param>
        /// <param name="trans">数据库事务对象</param>
        /// <returns>返回结果集中的第一行第一列的值，如果不成功则返回null值</returns>
        public static object ExecuteScalar(this DbContext context, DbCommand cmd, Trans trans = null)
        {
            trans = new Trans(context);
            if (cmd != null)
            {
                cmd.Connection.Close();
                cmd.CommandTimeout = 4;
                cmd.Connection = trans.Connection;
                cmd.Transaction = trans.Transaction;

                object retVal = cmd.ExecuteScalar();

                return retVal;
            }

            return null;
        }
        #endregion
        private static void CombineParams(ref DbCommand command, params object[] parameters)
        {
            if (parameters != null)
            {
                foreach (DbParameter parameter in parameters)
                {
                    if (!parameter.ParameterName.Contains("@"))
                        parameter.ParameterName = $"@{parameter.ParameterName}";
                    command.Parameters.Add(parameter);
                }
            }
        }

        private static DbCommand CreateCommand(DatabaseFacade facade, string sql, out DbConnection dbConn, params object[] parameters)
        {
            DbConnection conn = facade.GetDbConnection();
            dbConn = conn;
            conn.Open();
            DbCommand cmd = conn.CreateCommand();
            cmd.CommandTimeout = 4;
            cmd.CommandText = sql;
            CombineParams(ref cmd, parameters);

            return cmd;
        }

        public static DataTable SqlQuery(this DatabaseFacade facade, string sql, params object[] parameters)
        {
            try
            {
                DbCommand cmd = CreateCommand(facade, sql, out DbConnection conn, parameters); cmd.CommandTimeout = 4;
                DbDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                reader.Close();
                conn.Close();
                return dt;
            }
            catch
            {
                return null;
            }

        }

        public static IEnumerable<T> SqlQuery<T>(this DatabaseFacade facade, string sql, params object[] parameters) where T : class, new()
        {
            FileHelper.Writelog(sql);

            DataTable dt = SqlQuery(facade, sql, parameters);
            return dt.ToEnumerable<T>();
        }

        public static IEnumerable<T> ToEnumerable<T>(this DataTable dt) where T : class, new()
        {
            PropertyInfo[] propertyInfos = typeof(T).GetProperties();
            T[] ts = new T[dt.Rows.Count];
            int i = 0;
            foreach (DataRow row in dt.Rows)
            {
                T t = new T();
                foreach (PropertyInfo p in propertyInfos)
                {
                    if (dt.Columns.IndexOf(p.Name) != -1 && row[p.Name] != DBNull.Value)
                        p.SetValue(t, row[p.Name], null);
                }
                ts[i] = t;
                i++;
            }
            return ts;
        }
        #region extention
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public static dynamic DynCore(this DbContext context, Dictionary<string, string> dic)
        {
           
            string servicename = dic.ServiceName();          

            string sqlstr = BaseSQL.SQL_SysPageService_List;//@"SELECT[ServiceEvents],[ReturnType]FROM[dbo].[SysPageService]where  [IsEnabled]=1 and [IsDeleted]=0 and [servicename]=N'{0}'";

            sqlstr = sqlstr.Replace("{0}", servicename); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");

            var dt =   ExecuteDataTable(context,sqlstr.Str2SQL());
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
                                var rsdt =  ExecuteDataTable(context,sqlstr.Str2SQL());

                                return new Result(rsdt.ToListDictionary());
                            }

                            if (dt.Rows[0][1].ToString() == "0" || dt.Rows[0][1].ToString().ToLower() == "table" || dt.Rows[0][1].ToString().ToLower() == "list")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt =  ExecuteDataTable(context,sqlstr.Str2SQL());

                                return new Result(rsdt.ToListDictionary());
                            }
                            if (dt.Rows[0][1].ToString() == "object")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt =  ExecuteDataTable(context,sqlstr.Str2SQL());

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
                                var rsdt = ExecuteSql(context,sqlstr.Str2SQL());
                                return new Result(rsdt > 0);
                            }
                            if (dt.Rows[0][1].ToString() == "int")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = ExecuteDataTable(context,sqlstr.Str2SQL());
                                return new Result(rsdt);
                            } 
                            else
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt =  ExecuteDataTable(context,sqlstr.Str2SQL());
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

        public static dynamic Core(this DbContext context,   Dictionary<string, string> dic)
        {
            
            string servicename = dic.ServiceName();
          
          

            string sqlstr = BaseSQL.SQL_SysPageService_List;//@"SELECT[ServiceEvents],[ReturnType]FROM[dbo].[SysPageService]where  [IsEnabled]=1 and [IsDeleted]=0 and [servicename]=N'{0}'";

            sqlstr = sqlstr.Replace("{0}", servicename); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");

            var dt =    ExecuteDataTable(context,sqlstr.Str2SQL());
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
                                var rsdt =  ExecuteDataTable(context,sqlstr.Str2SQL());

                                return new Result(rsdt.ToListDictionary());
                            }

                            if (dt.Rows[0][1].ToString() == "0" || dt.Rows[0][1].ToString().ToLower() == "table" || dt.Rows[0][1].ToString().ToLower() == "list")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt =  ExecuteDataTable(context,sqlstr.Str2SQL());

                                return new Result(rsdt.ToListDictionary());
                            }
                            if (dt.Rows[0][1].ToString() == "object")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt =  ExecuteDataTable(context,sqlstr.Str2SQL());

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
                                var rsdt = ExecuteSql(context,sqlstr.Str2SQL());
                                return new Result(rsdt > 0);
                            }
                            if (dt.Rows[0][1].ToString() == "int")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = ExecuteSql(context,sqlstr.Str2SQL());
                                return new Result(rsdt);
                            } 
                            else
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt =  ExecuteDataTable(context,sqlstr.Str2SQL());
                                return new Result(rsdt.ToListDictionary());
                            }



                        }

            return Result.ERROR(0, "操作失败！");
        }
      /// <summary>
      /// 
      /// </summary>
      /// <param name="context"></param>
      /// <param name="sqlstr"></param>
      /// <param name="dic"></param>
      /// <returns></returns>
        public static object CoreExecuteSQL(this DbContext context, string sqlstr, Dictionary<string, string> dic)
        {
            sqlstr = dic.ToSQL(sqlstr);
            sqlstr = sqlstr.Replace("=n'", "=N'");
            FileHelper.Writelog(sqlstr, "SQL");
            return ExecuteDataTable(context, sqlstr.Str2SQL());
        }

        /// <summary>
        ///  执行业通用务逻辑,返回整型
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dic"></param>
        /// <returns></returns>


        public static int Coreint(this DbContext context,   Dictionary<string, string> dic)
        { 
            string servicename = dic.ServiceName();
     

            string sqlstr = BaseSQL.SQL_SysPageService_List;//@"SELECT[ServiceEvents],[ReturnType]FROM[dbo].[SysPageService]where  [IsEnabled]=1 and [IsDeleted]=0 and [servicename]=N'{0}'";

            sqlstr = sqlstr.Replace("{0}", servicename); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");

            var dt =   ExecuteDataTable(context,sqlstr.Str2SQL());
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
                                var rsdt =  ExecuteDataTable(context,sqlstr.Str2SQL());

                                return rsdt.Rows.Count;
                            }

                            if (dt.Rows[0][1].ToString() == "0" || dt.Rows[0][1].ToString().ToLower() == "table" || dt.Rows[0][1].ToString().ToLower() == "list")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt =  ExecuteDataTable(context,sqlstr.Str2SQL());

                                return rsdt.Rows.Count;
                            }
                            if (dt.Rows[0][1].ToString() == "object")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt =  ExecuteDataTable(context,sqlstr.Str2SQL());

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
                                var rsdt = ExecuteSql(context,sqlstr.Str2SQL());
                                return rsdt;
                            }
                            if (dt.Rows[0][1].ToString() == "int")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = ExecuteSql(context,sqlstr.Str2SQL());
                                return rsdt;
                            }
                             
                            else
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt =  ExecuteDataTable(context,sqlstr.Str2SQL());
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

        public static bool Corebool(this DbContext context,   Dictionary<string, string> dic)
        {
             
            string servicename = dic.ServiceName();
               

            string sqlstr = BaseSQL.SQL_SysPageService_List;//@"SELECT[ServiceEvents],[ReturnType]FROM[dbo].[SysPageService]where  [IsEnabled]=1 and [IsDeleted]=0 and [servicename]=N'{0}'";


            sqlstr = sqlstr.Replace("{0}", servicename); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");

            var dt =    ExecuteDataTable(context,sqlstr.Str2SQL());
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
                                var rsdt =  ExecuteDataTable(context,sqlstr.Str2SQL());

                                return rsdt.Rows.Count > 0;
                            }

                            if (dt.Rows[0][1].ToString() == "0" || dt.Rows[0][1].ToString().ToLower() == "table" || dt.Rows[0][1].ToString().ToLower() == "list")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt =  ExecuteDataTable(context,sqlstr.Str2SQL());

                                return rsdt.Rows.Count > 0;
                            }
                            if (dt.Rows[0][1].ToString() == "object")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt =  ExecuteDataTable(context,sqlstr.Str2SQL());

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
                                var rsdt = ExecuteSql(context,sqlstr.Str2SQL());
                                return rsdt > 0;
                            }
                            if (dt.Rows[0][1].ToString() == "int")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = ExecuteSql(context,sqlstr.Str2SQL());
                                return rsdt > 0;
                            } 
                            else
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt =  ExecuteDataTable(context,sqlstr.Str2SQL());
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
        public static object Coreobject(this DbContext context,   Dictionary<string, string> dic)
        {
           
            string servicename = dic.ServiceName();
          

            string sqlstr = BaseSQL.SQL_SysPageService_List;//@"SELECT[ServiceEvents],[ReturnType]FROM[dbo].[SysPageService]where  [IsEnabled]=1 and [IsDeleted]=0 and [servicename]=N'{0}'";


            sqlstr = sqlstr.Replace("{0}", servicename); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");

            var dt =   ExecuteDataTable(context,sqlstr.Str2SQL());
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
                                var rsdt =  ExecuteDataTable(context,sqlstr.Str2SQL());

                                return new Result(rsdt.ToListDictionary());
                            }

                            if (dt.Rows[0][1].ToString() == "0" || dt.Rows[0][1].ToString().ToLower() == "table" || dt.Rows[0][1].ToString().ToLower() == "list")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt =  ExecuteDataTable(context,sqlstr.Str2SQL());

                                return new Result(rsdt.ToListDictionary());
                            }
                            if (dt.Rows[0][1].ToString() == "object")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt =  ExecuteDataTable(context,sqlstr.Str2SQL());

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
                                var rsdt = ExecuteSql(context,sqlstr.Str2SQL());
                                return new Result(rsdt > 0);
                            }
                            if (dt.Rows[0][1].ToString() == "int")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = ExecuteDataTable(context,sqlstr.Str2SQL());
                                return new Result(rsdt);
                            } 
                            else
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt =  ExecuteDataTable(context,sqlstr.Str2SQL());
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
        public static Result CoreBasic(this DbContext context,   Dictionary<string, string> dic)
        {
            string servicename = dic.ServiceName();
         
            string sqlstr = BaseSQL.SQL_SysPageService_List;//@"SELECT[ServiceEvents],[ReturnType]FROM[dbo].[SysPageService]where  [IsEnabled]=1 and [IsDeleted]=0 and [servicename]=N'{0}'";


            sqlstr = sqlstr.Replace("{0}", servicename); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");

            var dt =    ExecuteDataTable(context,sqlstr.Str2SQL());
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
                                var rsdt =  ExecuteDataTable(context,sqlstr.Str2SQL());

                                return new Result(rsdt.ToListDictionary());
                            }

                            if (dt.Rows[0][1].ToString() == "0" || dt.Rows[0][1].ToString().ToLower() == "table" || dt.Rows[0][1].ToString().ToLower() == "list")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt =  ExecuteDataTable(context,sqlstr.Str2SQL());

                                return new Result(rsdt.ToListDictionary());
                            }
                            if (dt.Rows[0][1].ToString() == "object")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt =  ExecuteDataTable(context,sqlstr.Str2SQL());

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
                                var rsdt = ExecuteSql(context,sqlstr.Str2SQL());
                                return new Result(rsdt > 0);
                            }
                            if (dt.Rows[0][1].ToString() == "int")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = ExecuteDataTable(context,sqlstr.Str2SQL());
                                return new Result(rsdt);
                            } 
                            else
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt =  ExecuteDataTable(context,sqlstr.Str2SQL());
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
        public static Result CoreService(this DbContext context, string servicename, Dictionary<string, object> dic)
        {
           
            string sqlstr = BaseSQL.SQL_SysPageService_List;//@"SELECT[ServiceEvents],[ReturnType]FROM[dbo].[SysPageService]where  [IsEnabled]=1 and [IsDeleted]=0 and [servicename]=N'{0}'";


            sqlstr = sqlstr.Replace("{0}", servicename); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");

            var dt = ExecuteDataTable(context, sqlstr.Str2SQL());
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
                                var rsdt = ExecuteDataTable(context, sqlstr.Str2SQL());

                                return new Result(rsdt.ToListDictionary());
                            }

                            if (dt.Rows[0][1].ToString() == "0" || dt.Rows[0][1].ToString().ToLower() == "table" || dt.Rows[0][1].ToString().ToLower() == "list")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = ExecuteDataTable(context, sqlstr.Str2SQL());

                                return new Result(rsdt.ToListDictionary());
                            }
                            if (dt.Rows[0][1].ToString() == "object")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = ExecuteDataTable(context, sqlstr.Str2SQL());

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
                                var rsdt = ExecuteSql(context, sqlstr.Str2SQL());
                                return new Result(rsdt > 0);
                            }
                            if (dt.Rows[0][1].ToString() == "int")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = ExecuteDataTable(context, sqlstr.Str2SQL());
                                return new Result(rsdt);
                            }
                            else
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = ExecuteDataTable(context, sqlstr.Str2SQL());
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
        public static Result CoreList(this DbContext context,   int pageindex, int pagesize, Dictionary<string, string> dic=null)
        {
           
            string servicename = dic.ServiceName();
           
            Result rs = new Result();
            rs.Data.Add("pageindex", pageindex);
            rs.Data.Add("pagesize", pagesize);
            
            string sqlstr = BaseSQL.SQL_SysPageService_List;//@"SELECT[ServiceEvents],[ReturnType]FROM[dbo].[SysPageService]where  [IsEnabled]=1 and [IsDeleted]=0 and [servicename]=N'{0}'";

            sqlstr = sqlstr.Replace("{0}", servicename); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");

            var dt =    ExecuteDataTable(context,sqlstr.Str2SQL());
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
                                var rsdt =  ExecuteDataTable(context,sqlstr.Str2SQL());

                                return new Result(rsdt.ToListDictionary());
                            }

                            if (dt.Rows[0][1].ToString() == "0" || dt.Rows[0][1].ToString().ToLower() == "table" || dt.Rows[0][1].ToString().ToLower() == "list")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt =  ExecuteDataTable(context,sqlstr.Str2SQL());

                                return new Result(rsdt.ToListDictionary());
                            }
                            if (dt.Rows[0][1].ToString() == "object")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt =  ExecuteDataTable(context,sqlstr.Str2SQL());

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
                                var rsdt = ExecuteSql(context,sqlstr.Str2SQL());
                                return new Result(rsdt > 0);
                            }
                            if (dt.Rows[0][1].ToString() == "int")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = ExecuteDataTable(context,sqlstr.Str2SQL());
                                return new Result(rsdt);
                            } 
                            else
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt =  ExecuteDataTable(context,sqlstr.Str2SQL());
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
    public    static Result DoCore(this DbContext context,   Dictionary<string, string> dic)
        {         
            string servicename = dic.ServiceName();
            string sqlstr = BaseSQL.SQL_SysPageService_List;//@"SELECT[ServiceEvents],[ReturnType]FROM[dbo].[SysPageService]where  [IsEnabled]=1 and [IsDeleted]=0 and [servicename]=N'{0}'";

            sqlstr = sqlstr.Replace("{0}", servicename); 

            var dt =    ExecuteDataTable(context,sqlstr.Str2SQL());
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
                                var rsdt =  ExecuteDataTable(context,sqlstr.Str2SQL());

                                return new Result(rsdt.ToListDictionary());
                            }

                            if (dt.Rows[0][1].ToString() == "0" || dt.Rows[0][1].ToString().ToLower() == "table" || dt.Rows[0][1].ToString().ToLower() == "list")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt =  ExecuteDataTable(context,sqlstr.Str2SQL());

                                return new Result(rsdt.ToListDictionary());
                            }
                            if (dt.Rows[0][1].ToString() == "object")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt =  ExecuteDataTable(context,sqlstr.Str2SQL());

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
                                var rsdt = ExecuteSql(context,sqlstr.Str2SQL());
                                return new Result(rsdt > 0);
                            }
                            if (dt.Rows[0][1].ToString() == "int")
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt = ExecuteDataTable(context,sqlstr.Str2SQL());
                                return new Result(rsdt);
                            }
                            
                            else
                            {
                                FileHelper.Writelog(sqlstr, "SQL"); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
                                var rsdt =  ExecuteDataTable(context,sqlstr.Str2SQL());
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
        public static dynamic Reports(this DbContext context,   string tname)
        {
            string sqlstr = BaseSQL.SQL_GET_PAGE_LIST_COMMON;
            sqlstr = sqlstr.Replace("{0}", tname);


            return  ExecuteDataTable(context,sqlstr.ToLower());
        }

        /// <summary>
        /// 根据数据库中的表名;获取表中的数据；参数可以为空；地址组合：{数据库配置名}APP/{数据库中表名}/getlist
        /// </summary>
        /// <param name="id">数据库配置名</param>
        /// <param name="tname">数据库中表名</param>
        /// <param name="dic">传入参数</param>
        /// <returns></returns>

        public static object configServices(this DbContext context,   Dictionary<string, string> dic)
        {
            
            string methodname = dic.ServiceName();
            methodname = methodname + "getconfig";
            dic.AddServiceName(methodname);
            return DoCore(context,   dic);
        }

        /// <summary>
        /// [页面控件信息]通用接口（获取实体字段及描述）;通过配置页面英文名获取
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tname"></param>
        /// <returns></returns>
        public static object PageInfo(this DbContext context, string methodname)
        {
            string sqlstr = BaseSQL.SQL_GET_PAGECONTROL_LIST; 
            sqlstr = sqlstr.Replace("{0}", methodname);
            sqlstr = sqlstr.Replace("=n'", "=N'");
            FileHelper.Writelog(sqlstr, "SQL");
            return  ExecuteDataTable(context,sqlstr.Str2SQL());
        }
        /// <summary>
        /// [页面控件信息内容保存到后端] 根据数据库中的中页面配置名;添加数据到指定接口；参数可以为空；后缀：save
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public static object sysconfigServicessave(this DbContext context,   Dictionary<string, string> dic)
        {
           
            string methodname = dic.ServiceName();
            methodname = methodname + "save";
            dic.AddServiceName(methodname);
            return DoCore(context,   dic);
        }

        /// <summary>
        /// [根据页面控件信息获取对应的数据]根据数据库中的中页面配置名;通过接口更新数据；参数可以为空；
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public static object sysconfigServicesgets(this DbContext context,   Dictionary<string, string> dic)
        {
           
            string methodname = dic.ServiceName();
            methodname = methodname + "get";
            dic.AddServiceName(methodname);
            return DoCore(context,   dic);
        }

        /// <summary>
        /// [列表]根据数据库中的表名;获取表中的数据；参数可以为空；
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public static object sysconfigServices(this DbContext context,   Dictionary<string, string> dic)
        {
           
            string methodname = dic.ServiceName();
            methodname = methodname + "getlist";
            dic.AddServiceName(methodname);
            return DoCore(context,   dic);
        }


        /// <summary>
        /// [添加] 根据数据库中的中页面配置名;添加数据到指定接口；参数可以为空
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public static object sysconfigServicesadd(this DbContext context,   Dictionary<string, string> dic)
        {
        
            string methodname = dic.MethodName();
            methodname = methodname + "add";
            dic.AddMethodName(methodname);
            return DoCore(context, dic);
        }

        /// <summary>
        ///  [更新]根据数据库中的中页面配置名;通过接口更新数据；参数可以为空；
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public static object sysconfigServicesedit(this DbContext context,   Dictionary<string, string> dic)
        {
           
            string methodname = dic.ServiceName();
            methodname = methodname + "edit";
            dic.AddServiceName(methodname);
            return DoCore(context, dic);
        }
        /// <summary>
        /// [更新]根据数据库中的中页面配置名;通过接口更新数据；参数可以为空；
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public static object sysconfigServicesupdate(this DbContext context,   Dictionary<string, string> dic)
        { 
            string methodname = dic.ServiceName();
            methodname = methodname + "update";
            dic.AddServiceName(methodname);
            return DoCore(context,  dic);
        }
        /// <summary>
        /// [删除]根据数据库中的中页面配置名;通过接口删除（逻辑删除）指定数据；参数可以为空
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dic"></param>
        /// <returns></returns>

        public static object sysconfigServicesdelete(this DbContext context,   Dictionary<string, string> dic)
        {
            
            string methodname = dic.ServiceName();
            methodname = methodname + "delete";
            dic.AddServiceName(methodname);
            return DoCore(context, dic);
        }
        /// <summary>
        /// [删除]根据数据库中的中页面配置名;通过接口删除(物理删除)指定数据；参数可以为空；地址组合：{数据库配置名}APP/{数据库中页面配置名}/delete
        /// </summary>
        /// <param name="id">数据库配置名</param>
        /// <param name="tname">数据库中页面配置名</param>
        /// <param name="dic">传入参数</param>
        /// <returns></returns>
        public static object sysconfigServicesremovebykey(this DbContext context,   Dictionary<string, string> dic)
        {
           
            string methodname = dic.ServiceName();
            methodname = methodname + "removebykey";
            dic.AddServiceName(methodname);
            return DoCore(context, dic);
        }
        /// <summary>
        ///  [获取数据]根据数据库中的中页面配置名;通过接口获取指定数据；参数可以为空；
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public static object sysconfigServicesget(this DbContext context,   Dictionary<string, string> dic)
        {
        
            string methodname = dic.ServiceName();
            methodname = methodname + "gets";
            dic.AddServiceName(methodname);
            return DoCore(context, dic);
        }

        /// <summary>
        ///  [获取数据]根据数据库中的中页面配置名;通过接口获取指定数据；参数可以为空；
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dic"></param>
        /// <returns></returns>

        public static object sysconfigServiceslist(this DbContext context,   Dictionary<string, string> dic)
        { 
            string methodname = dic.ServiceName();
            methodname = methodname + "list";
            dic.AddServiceName(methodname);
            return DoCore(context, dic);
        }
        /// <summary>
        /// 根据数据库配置，服务配置 执行对应的业务逻辑；地址组合：{数据库配置名}APP/{数据库中表SysPageService中字段servicename服务名}Service
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public static object Services(this DbContext context,   Dictionary<string, string> dic)
        { 
            string methodname = dic.ServiceName();
            methodname = methodname + "service";
            dic.AddServiceName(methodname);
            return DoCore(context, dic);
        }

        /// <summary>
        /// 多ＳＱＬ语句执行，非事务；服务名：{ctrl}bntservice
        /// </summary>
        /// <param name="context"></param>
        /// <param name="ctrl"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static Result BatServiceNotTransRecords(this DbContext context,   string ctrl, Dictionary<string, object> model)
        {
            var dic = model.ToDictionary();          

            string sqlstr = BaseSQL.SQL_SysPageService_List;//@"SELECT[ServiceEvents],[ReturnType]FROM[dbo].[SysPageService]where [servicename]='{0}bntservice'";
            string result = "";
            sqlstr = sqlstr.Replace("{0}", ctrl); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
            FileHelper.Writelog(sqlstr, "SQL");
            var dt =  ExecuteDataTable(context,sqlstr.ToLower());
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
                                var rsdt = ExecuteSQL(context,sqlstr.Str2SQL());


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
        public static Result ExecuteSQLServiceNotTransRecords(this DbContext context,   string ctrl, Dictionary<string, object> model)
        { 
            var dic = model.ToDictionary();
            string sqlstr = BaseSQL.SQL_SysPageService_List;//@"SELECT[ServiceEvents],[ReturnType]FROM[dbo].[SysPageService]where [servicename]='{0}sqlexecute'";
            string result = "";
            sqlstr = sqlstr.Replace("{0}", ctrl); sqlstr = sqlstr.ToLower().Replace("=n'", "=N'"); sqlstr = sqlstr.Replace(",n'", ",N'");
            FileHelper.Writelog(sqlstr, "SQL");
            var dt =  ExecuteDataTable(context,sqlstr.ToLower());
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
                            var rsdt = ExecuteSQL(context,sqlstr.Str2SQL());


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
        public static object ReplaceTemplate(this DbContext context,   string ctrl, Dictionary<string, string> dic)
        {           
            string sqlstr = BaseSQL.SQL_SysPageService_List;//@"SELECT[ServiceEvents],[ReturnType]FROM[dbo].[SysPageService]where [servicename]='{0}sqlexecute'";

            sqlstr = sqlstr.Replace("{0}", ctrl); 
            FileHelper.Writelog(sqlstr, "SQL");
            var dt =  ExecuteDataTable(context,sqlstr.ToLower());
            return new Result(dt);

        }

        #endregion


    }


    public class Trans : IDisposable
    {
        #region 字段属性
        private DbConnection connection = null;
        /// <summary>
        /// 获取当前数据库链接对象
        /// </summary>
        public DbConnection Connection
        {
            get
            {
                return connection;
            }
        }

        private DbTransaction transaction = null;
        /// <summary>
        /// 获取当前数据库事务对象
        /// </summary>
        public DbTransaction Transaction
        {
            get
            {
                return transaction;
            }
        }
        #endregion

        #region 构造函数


        /// <summary>
        /// 根据数据库连接字符串来创建此事务对象
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="dbProviderName">数据库提供程序的名称</param>
        public Trans(DbContext context)
        {
            if (context != null)
            {
                connection = context.Database.GetDbConnection();


                transaction = Connection.BeginTransaction();
            }
            else
            {
                throw new ArgumentNullException("connectionString", "数据库链接串参数值不能为空!");
            }
        }
        #endregion

        #region 方法函数
        /// <summary>
        /// 提交此数据库事务操作
        /// </summary>
        public void Commit()
        {
            Transaction.Commit();

            Close();
        }

        /// <summary>
        /// 回滚此数据库事务操作
        /// </summary>
        public void RollBack()
        {
            Transaction.Rollback();

            Close();
        }

        /// <summary>
        /// 关闭此数据库事务链接
        /// </summary>
        public void Close()
        {
            if (Connection.State != System.Data.ConnectionState.Closed)
            {
                Connection.Close();
            }
        }
        #endregion

        #region IDisposable 成员
        /// <summary>
        /// 执行与释放或重置非托管资源相关的应用程序定义的任务。
        /// </summary>
        public void Dispose()
        {
            Close();
        }
        #endregion
    }
}
