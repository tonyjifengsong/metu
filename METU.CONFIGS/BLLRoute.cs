using METU.MODEL;
using System.Diagnostics;
using System.Reflection;

namespace System
{
    /// <summary>
    /// 
    /// </summary>
    public static class BLLRoute
    {
        public static string GetMethodInfo()

        {

            string str = "";

            //取得当前方法命名空间

            str += "命名空间名:" + System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Namespace + "\n";

            //取得当前方法类全名 包括命名空间

            str += "命名空间+类名:" + System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName + "\n";

            //获得当前类名

            str += "类名:" + System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name + "\n";

            //取得当前方法名

            str += "方法名:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\n";

            str += "\n";

            StackTrace ss = new StackTrace(true);

            MethodBase mb = ss.GetFrame(1).GetMethod();

            //取得父方法命名空间

            str += mb.DeclaringType.Namespace + "\n";

            //取得父方法类名

            str += mb.DeclaringType.Name + "\n";

            //取得父方法类全名

            str += mb.DeclaringType.FullName + "\n";

            //取得父方法名

            str += mb.Name + "\n";

            return str;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="classname"></param>
        /// <param name="param"></param>
        /// <param name="methodname"></param>
        /// <param name="nspace"></param>
        /// <returns></returns>
        public static object ExecuteSSO(string classname, object param = null, string methodname = "dowork", string nspace = "METU.SSOBLL")
        {
            classname = classname + "Service";
            object rs = new object();
            string strClass = null;
            string strMethod = methodname;
            var classes = Assembly.Load(nspace).GetTypes();
            foreach (var item in classes)
            {
                if (item.Name.ToString().ToLower() == classname.ToString().ToLower())
                {
                    strClass = item.FullName;
                    break;
                }
            }
            if (strClass == null) return  Result.ERROR(0,"没有此接口！");

            Type type = Assembly.Load(nspace).GetType(strClass);
            var objs = System.Activator.CreateInstance(type);
            MethodInfo method = null;
            foreach (var itm in type.GetMethods())
            {
                if (itm.Name.ToString().ToLower() == methodname.ToLower())
                {
                    method = itm;
                    break;
                }
            }

            if (method == null) return Result.ERROR(0, "没有此接口！"); //new Result() { IsSuccess = false, DebugMessage = "没有此接口！", Message = "没有此接口！" };

            try
            {
                FileHelper.Writelog("传入参数：", classname + "_" + method.Name);

                FileHelper.Writelog(param.toJson(), classname + "_" + method.Name);
                rs = method.Invoke(objs, new object[] { param });
                FileHelper.Writelog("返回结果：", classname + "_" + method.Name);
                FileHelper.Writelog(rs.toJson(), classname + "_" + method.Name);



            }
            catch (Exception ex)
            {
                FileHelper.Writelog(ex, "ex");
                return Result.ERROR(0, "没有此接口！");// new Result() { IsSuccess = false, DebugMessage = ex.InnerException.Message, Message = ex.InnerException.Message };
            }
            return new Result(1,rs );

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="classname"></param>
        /// <param name="param"></param>
        /// <param name="methodname"></param>
        /// <param name="nspace"></param>
        /// <returns></returns>
        public static object ExecuteBLL(string classname, object param = null, string methodname = "dowork", string nspace = "METU.SSOBLL")
        {
            classname = classname + "Service";
            object rs = new object();
            string strClass = null;
            string strMethod = methodname;
            var classes = Assembly.Load(nspace).GetTypes();
            foreach (var item in classes)
            {
                if (item.Name.ToString().ToLower() == classname.ToString().ToLower())
                {
                    strClass = item.FullName;
                    break;
                }
            }
            if (strClass == null) return Result.ERROR(0, "没有此接口！"); //new Result() { IsSuccess = false, DebugMessage = "没有此接口！", Message = "没有此接口！" };

            Type type = Assembly.Load(nspace).GetType(strClass);
            var objs = System.Activator.CreateInstance(type);
            MethodInfo method = null;
            foreach (var itm in type.GetMethods())
            {
                if (itm.Name.ToString().ToLower() == methodname.ToLower())
                {
                    method = itm;
                    break;
                }
            }

            if (method == null) return Result.ERROR(0, "没有此接口！"); //new Result() { IsSuccess = false, DebugMessage = "没有此接口！", Message = "没有此接口！" };

            try
            {

                // rs = method.Invoke(objs, new object[] { param });

                FileHelper.Writelog("传入参数：", classname+"_"+method.Name);

                FileHelper.Writelog(param.toJson(), classname + "_" + method.Name);
                rs = method.Invoke(objs, new object[] { param });
                FileHelper.Writelog("返回结果：", classname + "_" + method.Name);
                FileHelper.Writelog(rs.toJson(), classname + "_" + method.Name);

            }
            catch (Exception ex)
            {
                FileHelper.Writelog(ex, "ex");
                return Result.ERROR(0, "没有此接口！"); //new Result() { IsSuccess = false, DebugMessage = ex.InnerException.Message,Message = ex.InnerException.Message };
            }
            return new Result(1, rs);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="classname"></param>
        /// <param name="param"></param>
        /// <param name="methodname"></param>
        /// <param name="nspace"></param>
        /// <returns></returns>
        public static object ExecuteAPI(string classname, object param = null, string methodname = "dowork", string nspace = "")
        {

            classname = classname + "Service";
            object rs = new object();
            string strClass = null;           // 命名空间+类名
            string strMethod = methodname;        // 方法名
            var classes = Assembly.Load(nspace).GetTypes();
            foreach (var item in classes)
            {
                if (item.Name.ToString().ToLower() == classname.ToString().ToLower())
                {
                    strClass = item.FullName;
                    break;
                }
            }
            if (strClass == null) return Result.ERROR(0, "没有此接口！"); //new Result() { Message = "没有此接口！", IsSuccess = false };

            Type type = Assembly.Load(nspace).GetType(strClass);      // 通过类名获取同名类
            var objs = System.Activator.CreateInstance(type);       // 创建实例
            MethodInfo method = null;
            foreach (var itm in type.GetMethods())
            {
                if (itm.Name.ToString().ToLower() == methodname.ToLower())
                {
                    method = itm;
                    break;
                }
            }

            if (method == null) return Result.ERROR(0, "没有此接口！"); //new Result() { Message = "没有此接口！", IsSuccess = false };
            try
            {
                FileHelper.Writelog("传入参数：", classname + "_" + method.Name);

                FileHelper.Writelog(param.toJson(), classname + "_" + method.Name);
                rs = method.Invoke(objs, new object[] { param });
                FileHelper.Writelog("返回结果：", classname + "_" + method.Name);
                FileHelper.Writelog(rs.toJson(), classname + "_" + method.Name);
              
            }
            catch (Exception ex)
            {
                FileHelper.Writelog(ex, "ex");
                return Result.ERROR(0, "没有此接口！"); //new Result() { Message = ex.InnerException.Message, DebugMessage = ex.InnerException.Message, IsSuccess = false };
            }
            return new Result(1, rs);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="classname"></param>
        /// <param name="param"></param>
        /// <param name="methodname"></param>
        /// <param name="nspace"></param>
        /// <returns></returns>
        public static object Execute(string classname, object param = null, string methodname = "dowork", string nspace = "")
        {

            object rs = new object();
            string strClass = null;           // 命名空间+类名
            string strMethod = methodname;        // 方法名
            var classes = Assembly.Load(nspace).GetTypes();
            foreach (var item in classes)
            {
                if (item.Name.ToString().ToLower() == classname.ToString().ToLower())
                {
                    strClass = item.FullName;
                    break;
                }
            }
            if (strClass == null) return Result.ERROR(0, "没有此接口！"); //new Result() { Message = "没有此接口！", IsSuccess = false };

            Type type = Assembly.Load(nspace).GetType(strClass);      // 通过类名获取同名类
            var objs = System.Activator.CreateInstance(type);       // 创建实例
            MethodInfo method = null;
            foreach (var itm in type.GetMethods())
            {
                if (itm.Name.ToString().ToLower() == methodname.ToLower())
                {
                    method = itm;
                    break;
                }
            }

            if (method == null) return Result.ERROR(0, "没有此接口！"); //new Result() { Message = "没有此接口！", IsSuccess = false };
            try
            {
                FileHelper.Writelog("传入参数：", classname + "_" + method.Name);

                FileHelper.Writelog(param.toJson(), classname + "_" + method.Name);
                rs = method.Invoke(objs, new object[] { param });
                FileHelper.Writelog("返回结果：", classname + "_" + method.Name);
                FileHelper.Writelog(rs.toJson(), classname + "_" + method.Name);

            }
            catch (Exception ex)
            {
                FileHelper.Writelog(ex, "ex");
                return Result.ERROR(0, "没有此接口！"); //new Result() { Message = ex.InnerException.Message, DebugMessage = ex.InnerException.Message, IsSuccess = false };
            }
            return new Result(1, rs);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="classname"></param>
        /// <param name="param"></param>
        /// <param name="methodname"></param>
        /// <param name="nspace"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static object ExecutePrefix(string classname, object param = null, string methodname = "dowork", string nspace = "",string prefix="Service")
        {
            classname = classname + prefix;

            object rs = new object();
            string strClass = null;           // 命名空间+类名
            string strMethod = methodname;        // 方法名
            var classes = Assembly.Load(nspace).GetTypes();
            foreach (var item in classes)
            {
                if (item.Name.ToString().ToLower() == classname.ToString().ToLower())
                {
                    strClass = item.FullName;
                    break;
                }
            }
            if (strClass == null) return Result.ERROR(0, "没有此接口！"); //new Result() { Message = "没有此接口！", IsSuccess = false };

            Type type = Assembly.Load(nspace).GetType(strClass);      // 通过类名获取同名类
            var objs = System.Activator.CreateInstance(type);       // 创建实例
            MethodInfo method = null;
            foreach (var itm in type.GetMethods())
            {
                if (itm.Name.ToString().ToLower() == methodname.ToLower())
                {
                    method = itm;
                    break;
                }
            }

            if (method == null) return Result.ERROR(0, "没有此接口！"); //new Result() { Message = "没有此接口！", IsSuccess = false };
            try
            {
                FileHelper.Writelog("传入参数：", classname + "_" + method.Name);

                FileHelper.Writelog(param.toJson(), classname + "_" + method.Name);
                rs = method.Invoke(objs, new object[] { param });
                FileHelper.Writelog("返回结果：", classname + "_" + method.Name);
                FileHelper.Writelog(rs.toJson(), classname + "_" + method.Name);

            }
            catch (Exception ex)
            {
                FileHelper.Writelog(ex, "ex");
                return Result.ERROR(0, "没有此接口！"); //new Result() { Message = ex.InnerException.Message, DebugMessage = ex.InnerException.Message, IsSuccess = false };
            }
            return new Result(1, rs);

        }
    }
}
