using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public struct FilterOperators
    {

        public const string Equal = "Equal";
        public const string NotEqual = "NotEqual";
        public const string LessThan = "LessThan";
        public const string GreaterThan = "GreaterThan";
        public const string LessThanOrEqual = "LessThanOrEqual";
        public const string GreaterThanOrEqual = "GreaterThanOrEqual";
        public const string Contains = "Contains";
        public const string StartsWith = "StartsWith";
        public const string EndsWith = "EndsWith";
        public const string Between = "Between";

        public static Dictionary<string, string> Operators = new Dictionary<string, string>()
    {
        {Equal,"等于"},{NotEqual,"不等于"},
        {LessThan,"小于"},{GreaterThan,"大于"},
        {LessThanOrEqual,"小于或等于"},{GreaterThanOrEqual,"大于或等于"},
        {Contains,"包含"},{StartsWith,"开头包含"},{EndsWith,"结尾包含"},
        {Between,"区间"}
    };
    }
    public static class ExpressionExtentions
    {
        /// <summary>
        /// 获取查询表达式树 (zuowenjun.cn)
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="fieldName"></param>
        /// <param name="operatorName"></param>
        /// <param name="value"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public static Expression<Func<TEntity, bool>> GetQueryExpression<TEntity>(string fieldName, string operatorName, string value, string value2) where TEntity : class
        {
            PropertyInfo fieldInfo = typeof(TEntity).GetProperty(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);
            Type pType = fieldInfo.PropertyType;

            if (string.IsNullOrEmpty(operatorName))
            {
                // throw new ArgumentException("运算符不能为空！", "operatorName");
            }

            dynamic convertedValue;

            if (!value.TryChangeType(pType, out convertedValue))
            {
                // throw new ArgumentException(string.Format("【{0}】的查询值类型不正确,必须为{1}类型！", General.GetDisplayName(fieldInfo), pType.FullName), "value");
            }

            ParameterExpression expParameter = Expression.Parameter(typeof(TEntity), "f");
            MemberExpression expl = Expression.Property(expParameter, fieldInfo);
            ConstantExpression expr = Expression.Constant(convertedValue, pType);

            Expression expBody = null;
            Type expType = typeof(Expression);

            var expMethod = expType.GetMethod(operatorName, new[] { expType, expType });
            if (expMethod != null)
            {
                expBody = (Expression)expMethod.Invoke(null, new object[] { expl, expr });
            }
            else if (FilterOperators.Between == operatorName)
            {
                dynamic convertedValue2;
                if (!value2.TryChangeType(pType, out convertedValue2))
                {
                    // throw new ArgumentException(string.Format("【{0}】的查询值2类型不正确！",fieldInfo.Name);
                }

                ConstantExpression expr2 = Expression.Constant(convertedValue2, pType);
                expBody = Expression.GreaterThanOrEqual(expl, expr);
                expBody = Expression.AndAlso(expBody, Expression.LessThanOrEqual(expl, expr2));
            }
            else if (new[] { FilterOperators.Contains, FilterOperators.StartsWith, FilterOperators.EndsWith }.Contains(operatorName))
            {
                expBody = Expression.Call(expl, typeof(string).GetMethod(operatorName, new Type[] { typeof(string) }), expr);
            }
            else
            {
                // throw new ArgumentException("无效的运算符！", "operatorName");
            }

            Expression<Func<TEntity, bool>> lamExp = Expression.Lambda<Func<TEntity, bool>>(expBody, expParameter);

            return lamExp;
        }
    }
}
