using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace METU.ES.Elastic
{
    /// <summary>
    /// ElasticClient 扩展类
    /// </summary>
    public static class ElasticClientExtension
    {
        /// <summary>
        /// 创建索引
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="elasticClient"></param>
        /// <param name="indexName"></param>
        /// <param name="numberOfShards"></param>
        /// <param name="numberOfReplicas"></param>
        /// <returns></returns>
        public static bool CreateIndex<T>(this ElasticClient elasticClient, string indexName = "", int numberOfShards = 5, int numberOfReplicas = 1) where T : class
        {

            if (string.IsNullOrWhiteSpace(indexName))
            {
                indexName = typeof(T).Name;
            }

            if (elasticClient.Indices.Exists(indexName).Exists)
            {
                return false;
            }
            else
            {
                var indexState = new IndexState()
                {
                    Settings = new IndexSettings()
                    {
                        NumberOfReplicas = numberOfReplicas,
                        NumberOfShards = numberOfShards,
                    },
                };
                var response = elasticClient.Indices.Create(indexName, p => p.InitializeUsing(indexState).Map<T>(p => p.AutoMap()));
                return response.Acknowledged;
            }
        }

        /// <summary>
        /// 返回一个正序排列的委托
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field"></param>
        /// <returns></returns>
        public static Func<SortDescriptor<T>, SortDescriptor<T>> Sort<T>(string field) where T : class
        {
            return sd => sd.Ascending(field);
        }

        public static Func<SortDescriptor<T>, SortDescriptor<T>> Sort<T>(Expression<Func<T, object>> field) where T : class
        {
            return sd => sd.Ascending(field);
        }

        /// <summary>
        /// 返回一个倒序排列的委托
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field"></param>
        /// <returns></returns>
        public static Func<SortDescriptor<T>, SortDescriptor<T>> SortDesc<T>(string field) where T : class
        {
            return sd => sd.Descending(field);
        }

        public static Func<SortDescriptor<T>, SortDescriptor<T>> SortDesc<T>(Expression<Func<T, object>> field) where T : class
        {
            return sd => sd.Descending(field);
        }

        /// <summary>
        /// 返回一个Must条件集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<Func<QueryContainerDescriptor<T>, QueryContainer>> Must<T>() where T : class
        {
            return new List<Func<QueryContainerDescriptor<T>, QueryContainer>>();
        }

        public static List<Func<QueryContainerDescriptor<T>, QueryContainer>> Should<T>() where T : class
        {
            return new List<Func<QueryContainerDescriptor<T>, QueryContainer>>();
        }

        /// <summary>
        /// 添加Match子句
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="musts"></param>
        /// <param name="field">要查询的列</param>
        /// <param name="value">要查询的关键字</param>
        /// <param name="boost"></param>
        public static void AddMatch<T>(this List<Func<QueryContainerDescriptor<T>, QueryContainer>> musts, string field,
            string value, double? boost = null) where T : class
        {
            musts.Add(d => d.Match(mq => mq.Field(field).Query(value).Boost(boost)));
        }

        public static void AddMatch<T>(this List<Func<QueryContainerDescriptor<T>, QueryContainer>> musts,
            Expression<Func<T, object>> field, string value) where T : class
        {
            musts.Add(d => d.Match(mq => mq.Field(field).Query(value)));
        }

        /// <summary>
        /// 添加MultiMatch子句
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="musts"></param>
        /// <param name="fields">要查询的列</param>
        /// <param name="value">要查询的关键字</param>
        public static void AddMultiMatch<T>(this List<Func<QueryContainerDescriptor<T>, QueryContainer>> musts,
            string[] fields, string value) where T : class
        {
            musts.Add(d => d.MultiMatch(mq => mq.Fields(fields).Query(value)));
        }

        /// <summary>
        /// 添加MultiMatch子句
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="musts"></param>
        /// <param name="fields">例如：f=>new [] {f.xxx, f.xxx}</param>
        /// <param name="value">要查询的关键字</param>
        public static void AddMultiMatch<T>(this List<Func<QueryContainerDescriptor<T>, QueryContainer>> musts,
            Expression<Func<T, object>> fields, string value) where T : class
        {
            musts.Add(d => d.MultiMatch(mq => mq.Fields(fields).Query(value)));
        }

        /// <summary>
        /// 添加大于子句
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="musts"></param>
        /// <param name="field">要查询的列</param>
        /// <param name="value">要比较的值</param>
        public static void AddGreaterThan<T>(this List<Func<QueryContainerDescriptor<T>, QueryContainer>> musts, string field,
            double value) where T : class
        {
            musts.Add(d => d.Range(mq => mq.Field(field).GreaterThan(value)));
        }

        public static void AddGreaterThan<T>(this List<Func<QueryContainerDescriptor<T>, QueryContainer>> musts,
            Expression<Func<T, object>> field, double value) where T : class
        {
            musts.Add(d => d.Range(mq => mq.Field(field).GreaterThan(value)));
        }

        /// <summary>
        /// 添加大于等于子句
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="musts"></param>
        /// <param name="field">要查询的列</param>
        /// <param name="value">要比较的值</param>
        public static void AddGreaterThanEqual<T>(this List<Func<QueryContainerDescriptor<T>, QueryContainer>> musts, string field,
            double value) where T : class
        {
            musts.Add(d => d.Range(mq => mq.Field(field).GreaterThanOrEquals(value)));
        }

        public static void AddGreaterThanEqual<T>(this List<Func<QueryContainerDescriptor<T>, QueryContainer>> musts,
            Expression<Func<T, object>> field, double value) where T : class
        {
            musts.Add(d => d.Range(mq => mq.Field(field).GreaterThanOrEquals(value)));
        }

        /// <summary>
        /// 添加小于子句
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="musts"></param>
        /// <param name="field">要查询的列</param>
        /// <param name="value">要比较的值</param>
        public static void AddLessThan<T>(this List<Func<QueryContainerDescriptor<T>, QueryContainer>> musts, string field,
            double value) where T : class
        {
            musts.Add(d => d.Range(mq => mq.Field(field).LessThan(value)));
        }

        public static void AddLessThan<T>(this List<Func<QueryContainerDescriptor<T>, QueryContainer>> musts,
            Expression<Func<T, object>> field, double value) where T : class
        {
            musts.Add(d => d.Range(mq => mq.Field(field).LessThan(value)));
        }

        /// <summary>
        /// 添加小于等于子句
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="musts"></param>
        /// <param name="field">要查询的列</param>
        /// <param name="value">要比较的值</param>
        public static void AddLessThanEqual<T>(this List<Func<QueryContainerDescriptor<T>, QueryContainer>> musts, string field,
            double value) where T : class
        {
            musts.Add(d => d.Range(mq => mq.Field(field).LessThanOrEquals(value)));
        }

        public static void AddLessThanEqual<T>(this List<Func<QueryContainerDescriptor<T>, QueryContainer>> musts,
            Expression<Func<T, object>> field, double value) where T : class
        {
            musts.Add(d => d.Range(mq => mq.Field(field).LessThanOrEquals(value)));
        }

        /// <summary>
        /// 添加一个Term，一个列一个值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="musts"></param>
        /// <param name="field">要查询的列</param>
        /// <param name="value">要比较的值</param>
        public static void AddTerm<T>(this List<Func<QueryContainerDescriptor<T>, QueryContainer>> musts, string field,
            object value) where T : class
        {
            musts.Add(d => d.Term(field, value));
        }

        public static void AddTerm<T>(this List<Func<QueryContainerDescriptor<T>, QueryContainer>> musts,
            Expression<Func<T, object>> field, object value) where T : class
        {
            musts.Add(d => d.Term(field, value));
        }

        /// <summary>
        /// 添加一个Terms，一个列多个值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="musts"></param>
        /// <param name="field"></param>
        /// <param name="values"></param>
        public static void AddTerms<T>(this List<Func<QueryContainerDescriptor<T>, QueryContainer>> musts, string field,
            object[] values) where T : class
        {
            musts.Add(d => d.Terms(tq => tq.Field(field).Terms(values)));
        }

        public static void AddTerms<T>(this List<Func<QueryContainerDescriptor<T>, QueryContainer>> musts,
            Expression<Func<T, object>> field, object[] values) where T : class
        {
            musts.Add(d => d.Terms(tq => tq.Field(field).Terms(values)));
        }
    }
}

/*
        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="input"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public IActionResult Index(CourseEsSearchInput input, int pageIndex = 1)
{
    pageIndex = pageIndex > 0 ? pageIndex : 1;

    //var musts = new List<Func<QueryContainerDescriptor<CourseEsDto>, QueryContainer>>();
    var musts = EsUtil.Must<CourseEsDto>();
    if (!string.IsNullOrWhiteSpace(input.School))
    {
        //musts.Add(c => c.Term(cc => cc.Field("School").Value(input.School)));
        musts.AddMatch("school", input.School);
    }

    if (!string.IsNullOrWhiteSpace(input.Key))
    {
        //musts.Add(c => c.MultiMatch(cc => cc.Fields(ccc => ccc.Fields(ced => new[] {ced.Title, ced.School})).Query(input.Key)));
        musts.Add(c => c.MultiMatch(cc => cc.Query(input.Key).Fields(new[] { "title", "school" })));
    }

    var must2 = EsUtil.Must<CourseEsDto>();

    if (!string.IsNullOrWhiteSpace(input.Ver1))
    {
        //musts.Add(c => c.Term(cc => cc.Ver1, input.Ver1));
        must2.AddTerm("ver1", input.Ver1);
    }

    if (!string.IsNullOrWhiteSpace(input.Ver2))
    {
        //musts.Add(c => c.Term(cc => cc.Field(ced => ced.Ver2).Value(input.Ver2)));
        must2.AddTerm("ver2", input.Ver2);
    }

    if (!string.IsNullOrWhiteSpace(input.Ver3))
    {
        //musts.Add(c => c.Term(cc => cc.Field(ced => ced.Ver3).Value(input.Ver2)));
        must2.AddTerm("ver3", input.Ver3);
    }

    if (input.PriceStart.HasValue)
    {
        //musts.Add(c => c.Range(cc => cc.Field(ccc => ccc.Price).GreaterThan((double)input.PriceStart.Value)));
        must2.AddGreaterThan("price", (double)input.PriceStart.Value);
    }

    if (input.PriceEnd.HasValue)
    {
        //musts.Add(c => c.Range(cc => cc.Field(ccc => ccc.Price).LessThanOrEquals((double)input.PriceEnd.Value)));
        must2.AddLessThanEqual("price", (double)input.PriceEnd.Value);
    }

    var client = EsUtil.Client("http://127.0.0.1:9200", "course", "doc");
    var result = client.Search<CourseEsDto>(sd =>
        sd.Query(qcd => qcd
                .Bool(cc => cc
                    .Must(musts)
                    .Filter(must2))
                )
                .From(10 * (pageIndex - 1))
                .Take(10)
            //.Sort(sdd => sdd.Descending("price"))
            .Sort(EsUtil.Sort<CourseEsDto>(c => c.Price))
    );

    var total = result.Total;
    var data = result.Documents;
    ViewBag.Total = total;
    return View(data);
}

/// <summary>
/// 创建索引
/// </summary>
/// <returns></returns>
public IActionResult CreateIndex()
{
    var result = EsUtil.Client("http://127.0.0.1:9200").CreateIndex<CourseEsDto>("course");
    if (result)
    {
        return Content("创建成功");
    }
    else
    {
        return Content("创建失败，可能已存在同名索引");
    }
}*/