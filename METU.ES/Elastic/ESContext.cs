using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace METU.ES.Elastic
{
   public  class ESContext : BaseEsContext<ESModel>
    {
        /// <summary>
        /// 索引名称
        /// </summary>
        public  string IndexName => "indexName";
        public ESContext(IEsClientProvider provider) : base(provider)
        {
        }
        /// <summary>
        /// 获取地址
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<ESModel> Get(string id, int pageIndex, int pageSize)
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
        public List<ESModel> GetAll()
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
        public bool DeleteByQuery(string city)
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
