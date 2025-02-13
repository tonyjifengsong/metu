﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace System
{
    /// <summary>
    /// <see cref="Expression"/>解析器。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ExpressionParser<T>
    {
        public ParameterExpression Parameter { get; } = Expression.Parameter(typeof(T));
        public Expression<Func<T, bool>> ParserConditions(IEnumerable<Condition> conditions)
        {
            //将条件转化成表达是的Body
            var query = ParseExpressionBody(conditions);
            return Expression.Lambda<Func<T, bool>>(query, Parameter);
        }

        private Expression ParseExpressionBody(IEnumerable<Condition> conditions)
        {
            if (conditions == null || conditions.Count() == 0)
            {
                return Expression.Constant(true, typeof(bool));
            }
            else if (conditions.Count() == 1)
            {
                return ParseCondition(conditions.First());
            }
            else
            {
                Expression left = ParseCondition(conditions.First());
                Expression right = ParseExpressionBody(conditions.Skip(1));
                return Expression.AndAlso(left, right);
            }
        }

        private Expression ParseCondition(Condition condition)
        {
            Expression key = Expression.Property(Parameter, condition.Key);
            //通过Tuple元组，实现Sql参数化。
            Expression value = ToTuple(condition.Value, key.Type);

            switch (condition.QuerySymbol)
            {
                case ConditionSymbolEnum.Contains:
                    return Expression.Call(key, typeof(string).GetMethod("Contains", new Type[] { typeof(string) }), value);
                case ConditionSymbolEnum.Equal:
                    return Expression.Equal(key, value);
                case ConditionSymbolEnum.Greater:
                    return Expression.GreaterThan(key, value);
                case ConditionSymbolEnum.GreaterEqual:
                    return Expression.GreaterThanOrEqual(key, value);
                case ConditionSymbolEnum.Less:
                    return Expression.LessThan(key, value);
                case ConditionSymbolEnum.LessEqual:
                    return Expression.LessThanOrEqual(key, value);
                case ConditionSymbolEnum.NotEqual:
                    return Expression.NotEqual(key, value);
                case ConditionSymbolEnum.In:
                    return ParaseIn(condition);
                case ConditionSymbolEnum.Between:
                    return ParaseBetween(condition);
                default:
                    throw new NotImplementedException("不支持此操作。");
            }
        }

        private Expression ParaseBetween(Condition conditions)
        {
            Expression key = Expression.Property(Parameter, conditions.Key);
            var valueArr = conditions.Value.ToString().Split(',');
            if (valueArr.Length != 2)
            {
                throw new NotImplementedException("ParaseBetween参数错误");
            }

            Expression expression = Expression.Constant(true, typeof(bool));
            if (double.TryParse(valueArr[0], out double v1)
                && double.TryParse(valueArr[1], out double v2))
            {
                Expression startvalue = ToTuple(v1, typeof(double));
                Expression start = Expression.GreaterThanOrEqual(key, Expression.Convert(startvalue, key.Type));
                Expression endvalue = ToTuple(v2, typeof(double));
                Expression end = Expression.LessThanOrEqual(key, Expression.Convert(endvalue, key.Type));
                return Expression.AndAlso(start, end);
            }
            else if (DateTime.TryParse(valueArr[0], out DateTime v3)
                && DateTime.TryParse(valueArr[1], out DateTime v4))
            {
                Expression startvalue = ToTuple(v3, typeof(DateTime));
                Expression start = Expression.GreaterThanOrEqual(key, Expression.Convert(startvalue, key.Type));
                Expression endvalue = ToTuple(v4, typeof(DateTime));
                Expression end = Expression.LessThanOrEqual(key, Expression.Convert(endvalue, key.Type));
                return Expression.AndAlso(start, end);
            }
            else
            {
                throw new NotImplementedException("ParaseBetween参数错误");
            }
        }

        private Expression ParaseIn(Condition conditions)
        {
            Expression key = Expression.Property(Parameter, conditions.Key);
            var valueArr = conditions.Value.ToString().Split(',');
            Expression expression = Expression.Constant(false, typeof(bool));
            foreach (var itemVal in valueArr)
            {
                //Expression value = Expression.Constant(itemVal);
                Expression value = ToTuple(itemVal, typeof(string));
                Expression right = Expression.Equal(key, Expression.Convert(value, key.Type));
                expression = Expression.Or(expression, right);
            }
            return expression;
        }

        private Expression ToTuple(object value, Type type)
        {
            var tuple = Tuple.Create(value);
            return Expression.Convert(
                 Expression.Property(Expression.Constant(tuple), nameof(tuple.Item1))
                 , type);
        }

    }


    /// <summary>
    /// 条件实体。
    /// </summary>
    public class Condition
    {
        /// <summary>
        /// 
        /// </summary>
        public Condition()
        {
            QuerySymbol = ConditionSymbolEnum.Equal;
            Order = OrderEnum.None;
        }
        /// <summary>
        /// 字段名称
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ConditionSymbolEnum QuerySymbol { get; set; }

        public OrderEnum Order { get; set; }
    }

    /// <summary>
    /// 排序方式枚举。
    /// </summary>
    public enum OrderEnum
    {
        /// <summary>
        /// 不排序。
        /// </summary>
        None,
        /// <summary>
        /// 升序。
        /// </summary>
        Asc,
        /// <summary>
        /// 降序。
        /// </summary>
        Desc
    }

    /// <summary>
    /// 条件标志。
    /// </summary>
    public enum ConditionSymbolEnum
    {/// <summary>
     /// 
     /// </summary>
        Contains,
        /// <summary>
        /// 
        /// </summary>
        Equal,
        /// <summary>
        /// 
        /// </summary>
        NotEqual,
        /// <summary>
        /// 
        /// </summary>
        Greater,
        /// <summary>
        /// 
        /// </summary>
        GreaterEqual,
        /// <summary>
        /// 
        /// </summary>
        Less,
        /// <summary>
        /// 
        /// </summary>
        LessEqual,
        /// <summary>
        /// 
        /// </summary>
        In,
        /// <summary>
        /// 
        /// </summary>
        Between
    }

    public static partial class QueryableExtensions
    {
        /// <summary>
        /// 添加查询条件。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="conditions"></param>
        /// <returns></returns>
        public static IQueryable<T> QueryConditions<T>(this IQueryable<T> query, IEnumerable<Condition> conditions)
        {
            var parser = new ExpressionParser<T>();
            var filter = parser.ParserConditions(conditions);
            query = query.Where(filter);
            foreach (var orderinfo in conditions.Where(c => c.Order != OrderEnum.None))
            {
                var t = typeof(T);
                var propertyInfo = t.GetProperty(orderinfo.Key);
                var parameter = Expression.Parameter(t);
                Expression propertySelector = Expression.Property(parameter, propertyInfo);

                var orderby = Expression.Lambda<Func<T, object>>(Expression.Convert(propertySelector, typeof(object)), parameter);
                if (orderinfo.Order == OrderEnum.Desc)
                    query = query.OrderByDescending(orderby);
                else
                    query = query.OrderBy(orderby);

            }
            return query;
        }

        /// <summary>
        /// 分页。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <param name="itemCount"></param>
        /// <returns></returns>
        public static IQueryable<T> Pager<T>(this IQueryable<T> query, int pageindex, int pagesize, out int itemCount)
        {
            itemCount = query.Count();
            return query.Skip((pageindex - 1) * pagesize).Take(pagesize);
        }
    }
}