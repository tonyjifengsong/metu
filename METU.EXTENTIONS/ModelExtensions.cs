using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace System
{/// <summary>
/// 
/// </summary>
    public static class ModelExtensions
    {
        /// <summary>
        /// 获取枚举变量值的 Description 属性
        /// </summary>
        /// <param name="obj">枚举变量</param>
        /// <returns>如果包含 Description 属性，则返回 Description 属性的值，否则返回枚举变量值的名称</returns>
        public static string GetDescription(this object obj)
        {
            return GetDescription(obj, false);
        }

        /// <summary>
        /// 获取枚举变量值的 Description 属性
        /// </summary>
        /// <param name="obj">枚举变量</param>
        /// <param name="isTop">是否改变为返回该类、枚举类型的头 Description 属性，而不是当前的属性或枚举变量值的 Description 属性</param>
        /// <returns>如果包含 Description 属性，则返回 Description 属性的值，否则返回枚举变量值的名称</returns>
        public static string GetDescription(this object obj, bool isTop)
        {
            if (obj == null)
            {
                return string.Empty;
            }
            try
            {
                Type _enumType = obj.GetType();
                DescriptionAttribute dna = null;
                if (isTop)
                {
                    dna = (DescriptionAttribute)Attribute.GetCustomAttribute(_enumType, typeof(DescriptionAttribute));
                }
                else
                {
                    FieldInfo fi = _enumType.GetField(Enum.GetName(_enumType, obj));
                    dna = (DescriptionAttribute)Attribute.GetCustomAttribute(
                       fi, typeof(DescriptionAttribute));
                }
                if (dna != null && string.IsNullOrEmpty(dna.Description) == false)
                    return dna.Description;
            }
            catch (Exception ex)
            {

            }
            return obj.ToString();
        }

        /// <summary>
        /// 获取变量值的 Description 属性
        /// </summary>
        /// <param name="obj">变量</param>
        /// <returns>如果包含 Description 属性，则返回 Description 属性的值，否则返回枚举变量值的名称</returns>
        public static string Description(this object obj)
        {
            return Description(obj, false);
        }

        /// <summary>
        /// 获取变量值的 Description 属性
        /// </summary>
        /// <param name="obj">变量</param>
        /// <param name="isTop">是否改变为返回该类、枚举类型的头 Description 属性，而不是当前的属性或枚举变量值的 Description 属性</param>
        /// <returns>如果包含 Description 属性，则返回 Description 属性的值，否则返回枚举变量值的名称</returns>
        public static string Description(this object obj, bool isTop)
        {
            if (obj == null)
            {
                return string.Empty;
            }
            try
            {
                Type _enumType = obj.GetType();
                DescriptionAttribute dna = null;
                if (isTop)
                {
                    dna = (DescriptionAttribute)Attribute.GetCustomAttribute(_enumType, typeof(DescriptionAttribute));
                }
                else
                {
                    FieldInfo fi = _enumType.GetField(Enum.GetName(_enumType, obj));
                    dna = (DescriptionAttribute)Attribute.GetCustomAttribute(
                       fi, typeof(DescriptionAttribute));


                }
                if (dna != null && string.IsNullOrEmpty(dna.Description) == false)
                    return dna.Description;
            }
            catch (Exception ex)
            {
                //  FileHelpers.Writelog(ex, "extex");
            }
            return obj.ToString();
        }

        /// <summary>
                /// 获取枚举值的Description
                /// </summary>
                /// <typeparam name="T"></typeparam>
                /// <param name="value"></param>
                /// <returns></returns>
        public static string GetDescription<T>(this T value) where T : struct
        {
            string result = value.ToString();
            Type type = typeof(T);
            FieldInfo info = type.GetField(value.ToString());
            var attributes = info.GetCustomAttributes(typeof(DescriptionAttribute), true);
            if (attributes != null && attributes.FirstOrDefault() != null)
            {
                result = (attributes.First() as DescriptionAttribute).Description;
            }

            return result;
        }

        /// <summary>
                /// 根据Description获取枚举值
                /// </summary>
                /// <typeparam name="T"></typeparam>
                /// <param name="value"></param>
                /// <returns></returns>
        public static T GetValueByDescription<T>(this string description) where T : struct
        {
            Type type = typeof(T);
            foreach (var field in type.GetFields())
            {
                if (field.Name == description)
                {
                    return (T)field.GetValue(null);
                }

                var attributes = (DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), true);
                if (attributes != null && attributes.FirstOrDefault() != null)
                {
                    if (attributes.First().Description == description)
                    {
                        return (T)field.GetValue(null);
                    }
                }
            }

            throw new ArgumentException(string.Format("{0} 未能找到对应的枚举.", description), "Description");
        }

        /// <summary>
                /// 获取string获取枚举值
                /// </summary>
                /// <typeparam name="T"></typeparam>
                /// <param name="value"></param>
                /// <returns></returns>
        public static T GetValue<T>(this string value) where T : struct
        {
            T result;
            if (Enum.TryParse(value, true, out result))
            {
                return result;
            }

            throw new ArgumentException(string.Format("{0} 未能找到对应的枚举.", value), "Value");
        }


    }
}
