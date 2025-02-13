using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace METU.ES.Elastic
{

    /// <summary>
    /// es操作基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseEsContext<T> : IBaseEsContext where T : class
    {
        protected IEsClientProvider _EsClientProvider;
        public   string IndexName { get; }
        public BaseEsContext(IEsClientProvider provider)
        {
            IndexName = "indexname";
            _EsClientProvider = provider;
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="tList"></param>
        /// <returns></returns>
        public bool Insert(string IndexName,T obj)
        {
            List<T> tList = new List<T>();
            tList.Add(obj);
            var client = _EsClientProvider.GetClient(IndexName);
            if (!client.Indices.Exists(IndexName).Exists)
            {
                client.CreateIndex<T>(IndexName);
            }
            var response = client.IndexMany(tList);
            //var response = client.Bulk(p=>p.Index(IndexName).IndexMany(tList));
            return response.IsValid;
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="tList"></param>
        /// <returns></returns>
        public bool InsertMany(string IndexName, List<T> tList)
        {
            var client = _EsClientProvider.GetClient(IndexName);
            if (!client.Indices.Exists(IndexName).Exists)
            {
                client.CreateIndex<T>(IndexName);
            }
            var response = client.IndexMany(tList);
            //var response = client.Bulk(p=>p.Index(IndexName).IndexMany(tList));
            return response.IsValid;
        }

        /// <summary>
        /// 获取总数
        /// </summary>
        /// <returns></returns>
        public long GetTotalCount(string IndexName)
        {
            var client = _EsClientProvider.GetClient(IndexName);
            var search = new SearchDescriptor<T>().MatchAll(); //指定查询字段 .Source(p => p.Includes(x => x.Field("Id")));
            var response = client.Search<T>(search);
            return response.Total;
        }
        /// <summary>
        /// 根据Id删除数据
        /// </summary>
        /// <returns></returns>
        public bool DeleteById(string IndexName, string id)
        {
            var client = _EsClientProvider.GetClient(IndexName);
            var response = client.Delete<T>(id);
            return response.IsValid;
        }

        public bool Insert<T1>(string IndexName, T1 obj) where T1 : class
        {
            List<T1> tList = new List<T1>();
            tList.Add(obj);
            var client = _EsClientProvider.GetClient(IndexName);
            if (!client.Indices.Exists(IndexName).Exists)
            {
                client.CreateIndex<T1>(IndexName);
            }
            var response = client.IndexMany(tList);
            //var response = client.Bulk(p=>p.Index(IndexName).IndexMany(tList));
            return response.IsValid;
        }

        public bool InsertMany<T1>(string IndexName, List<T1> tList) where T1 : class
        {

            var client = _EsClientProvider.GetClient(IndexName);
            if (!client.Indices.Exists(IndexName).Exists)
            {
                client.CreateIndex<T1>(IndexName);
            }
            var response = client.IndexMany(tList);
            //var response = client.Bulk(p=>p.Index(IndexName).IndexMany(tList));
            return response.IsValid;
        }

        public long GetTotalCount<T1>(string IndexName) where T1 : class
        {
            var client = _EsClientProvider.GetClient(IndexName);
            var search = new SearchDescriptor<T1>().MatchAll(); //指定查询字段 .Source(p => p.Includes(x => x.Field("Id")));
            var response = client.Search<T1>(search);
            return response.Total;
        }

        public bool DeleteById<T1>(string IndexName, string id) where T1 : class
        {
            var client = _EsClientProvider.GetClient(IndexName);
            var response = client.Delete<T1>(id);
            return response.IsValid;
        }

        public List<ESModel> Get(string IndexName, string id, int pageIndex, int pageSize)
        {
            var client = _EsClientProvider.GetClient(IndexName);
            var musts = new List<Func<QueryContainerDescriptor<ESModel>, QueryContainer>>();
            musts.Add(p => p.Term(m => m.Field(x => x.Index).Value(id)));
            var search = new SearchDescriptor<ESModel>();
            // search = search.Index(IndexName).Query(p => p.Bool(m => m.Must(musts))).From((pageIndex - 1) * pageSize).Take(pageSize);
            search = search.Query(p => p.Bool(m => m.Must(musts))).From((pageIndex - 1) * pageSize).Take(pageSize);
            var response = client.Search<ESModel>(search);
            return response.Documents.ToList();
        }

        public List<ESModel> GetAll(string IndexName)
        {
            var client = _EsClientProvider.GetClient(IndexName);
            var searchDescriptor = new SearchDescriptor<ESModel>();
            // searchDescriptor = searchDescriptor.Index(IndexName).Query(p => p.MatchAll());
            searchDescriptor = searchDescriptor.Query(p => p.MatchAll());
            var response = client.Search<ESModel>(searchDescriptor);
            return response.Documents.ToList();
        }

        public bool DeleteByQuery(string IndexName, string city)
        {
            var client = _EsClientProvider.GetClient(IndexName);
            var musts = new List<Func<QueryContainerDescriptor<ESModel>, QueryContainer>>();
            musts.Add(p => p.Term(m => m.Field(f => f.Id).Value(city)));
            var search = new DeleteByQueryDescriptor<ESModel>().Index(IndexName);
            search = search.Query(p => p.Bool(m => m.Must(musts)));
            var response = client.DeleteByQuery<ESModel>(p => search);
            return response.IsValid;
        }
    }
}
