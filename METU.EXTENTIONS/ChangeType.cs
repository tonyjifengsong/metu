using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class ChangeType
    {
        /// <summary>
        /// 判断是否可为转换为指定的类型
        /// </summary>
        /// <param name="str"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool TryChangeType(this object str, Type type, out dynamic returnValue)
        {
            try
            {
                if (type.IsNullableType())
                {
                    if (str == null || str.ToString().Length == 0)
                    {
                        returnValue = null;
                    }
                    else
                    {
                        type = type.GetGenericArguments()[0];
                        returnValue = Convert.ChangeType(str, type);
                    }
                }
                else
                {
                    returnValue = Convert.ChangeType(str, type);
                }
                return true;
            }
            catch
            {
                returnValue = type.DefaultValue();
                return false;
            }
        }

        /// <summary>
        /// 判断是否为可空类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsNullableType(this Type type)
        {
            return (type.IsGenericType &&
              type.GetGenericTypeDefinition().Equals
              (typeof(Nullable<>)));
        }

        /// <summary>
        /// 默认值
        /// </summary>
        /// <param name="targetType"></param>
        /// <returns></returns>
        public static dynamic DefaultValue(this Type targetType)
        {
            return targetType.IsValueType ? Activator.CreateInstance(targetType) : null;
        }
    }
}
