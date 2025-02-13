using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace METU.ES.Elastic
{
   public  class ESBaseContext : IBaseEsContext 
    {
        /// <summary>
        /// 索引名称
        /// </summary>
        public  string IndexName => "indexName";
        public ESBaseContext(IEsClientProvider provider) 
        { _EsClientProvider = provider;
        }
        protected IEsClientProvider _EsClientProvider;
      
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="tList"></param>
        /// <returns></returns>
        public bool Insert<T>(string IndexName, T obj) where T : class
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
        public bool InsertMany<T>(string IndexName, List<T> tList) where T : class
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
        public long GetTotalCount<T>(string IndexName) where T : class
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
        public bool DeleteById<T>(string IndexName, string id) where T : class
        {
            var client = _EsClientProvider.GetClient(IndexName);
            var response = client.Delete<T>(id);
            return response.IsValid;
        }
        /// <summary>
        /// 获取地址
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 获取所有地址
        /// </summary>
        /// <returns></returns>
        public List<ESModel> GetAll(string IndexName)
        {
            var client = _EsClientProvider.GetClient(IndexName);
            var searchDescriptor = new SearchDescriptor<ESModel>();
            // searchDescriptor = searchDescriptor.Index(IndexName).Query(p => p.MatchAll());
            searchDescriptor = searchDescriptor.Query(p => p.MatchAll());
            var response = client.Search<ESModel>(searchDescriptor);
            return response.Documents.ToList();
        }
        /// <summary>
        /// 删除指定城市的数据
        /// </summary>
        /// <param name="city">ID</param>
        /// <returns></returns>
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
