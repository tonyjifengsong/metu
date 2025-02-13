using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace METU.ES
{
    [ElasticsearchType(IdProperty = "Id")]
    public  class ESModel
    {
        [Keyword]
        public string Id { get; set; }
        [Keyword]
        public string Index { get; set; }
        [Keyword]
        public string Type { get; set; }
        [Keyword]
        public string Body { get; set; }
        [Keyword]
        public string Content { get; set; }
        [Keyword]
        public DateTime CreateDate { get; set; }
        [Keyword] 
        public string userid { get; set; }
        [Keyword]
        public string CID { get; set; }
        [Keyword]
        public object Data { get; set; }
        [Keyword]
        public string KeyWords { get; set; }
        [Keyword]
        public object Doc { get; set; }
        [Keyword]
        public float price { get; set; }
        [Keyword]
        public int num { get; set; }
        [Keyword]
        public decimal sums { get; set; }
        
       
        public string estype { get; set; }
    }
}
