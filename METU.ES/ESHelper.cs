using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace METU.ES
{
   public class ESHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="index"></param>
        /// <param name="t"></param>
        public static bool Insert(string url, string index, object t)
        {
            var settings = new ConnectionSettings(new Uri(url)).DefaultIndex(index);
            var client = new ElasticClient(settings);
          var rs=  client.IndexDocument(t);
            return rs.IsValid;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="index"></param>
        /// <param name="docs"></param>
        public static bool InsertMany(string url, string index, IEnumerable<object> docs)
        {
            var settings = new ConnectionSettings(new Uri(url)).DefaultIndex(index);
            var client = new ElasticClient(settings);
          var rs=  client.IndexMany(docs);
            return rs.IsValid;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="index"></param>
        /// <param name="field"></param>
        /// <param name="fieldValue"></param>
        /// <param name="baseLine"></param>
        /// <param name="count"></param>
        public static object Search(string url, string index, string field, string fieldValue, decimal baseLine, int count)
        {
            var settings = new ConnectionSettings(new Uri(url)).DefaultIndex(index);
            var client = new ElasticClient(settings);
      
            var r = client.Search<dynamic>(
                s =>
                    s.Query(
                        q => q.Bool(
                            b => b.Filter(new NumericRangeQuery() { Field = field, GreaterThan = Convert.ToDouble(baseLine) })
                                .Must(new QueryContainer[] { new TermQuery() { Field = field, Value = fieldValue }, new MatchQuery() { Field = field, Operator = Operator.Or, Query = fieldValue } })
                            )
                        )
                    //排序
                    .Sort(sort => sort.Descending(field))
                    //字段过滤
                    .Source(source => source.Includes(f => f.Fields(new string[] { "field1", "field2" }))).Size(count)
            );
            return r;
        }

    }
}
