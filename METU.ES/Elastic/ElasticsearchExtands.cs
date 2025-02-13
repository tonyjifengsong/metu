﻿using Elasticsearch.Net;
using Nest;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticsearchLearn.ElasticSearch
{
    public static class ElasticsearchExtands
    {
        /// <summary>
        /// 封装后的linq的查询方式
        /// </summary>
        /// <typeparam name="T">实体泛型</typeparam>
        /// <param name="indexName">index的名称</param>
        /// <param name="selector">linq内容</param>
        /// <returns></returns>
        public static async Task<List<T>> SearchByLinqAsync<T>(this IElasticClient elasticSearchServer, string indexName, Func<QueryContainerDescriptor<T>, QueryContainer> selector = null) where T : class
        {
            var list = await elasticSearchServer.SearchAsync<T>(option => option.Index(indexName.ToLower()).Query(selector));
            return list.Documents.ToList();
        }

        /// <summary>
        /// 封装后的Json的查询方式
        /// </summary>
        /// <param name="indexName">index的名称</param>
        /// <param name="jsonString">json字符串</param>
        /// <returns>返回Jobject的内容</returns>
        public static async Task<JToken> SearchByJsonAsync(this IElasticLowLevelClient elasticSearchServer, string indexName, string jsonString)
        {
            var stringRespones = await elasticSearchServer.SearchAsync<StringResponse>(indexName.ToLower(), jsonString);
            var jobject = JObject.Parse(stringRespones.Body);
            var total = Convert.ToInt32(jobject["hits"]["total"]["value"].ToString());
            if (total > 0)
            {
                string json = string.Empty;
                var sourceArg = jobject["hits"]["hits"];
                foreach (var source in sourceArg)
                {
                    string sourceJson = source["_source"].ToString().Substring(1, source["_source"].ToString().Length - 1);
                    sourceJson = "{ \"_id\":\"" + source["_id"] + "\"," + sourceJson;
                    if (json.Length <= 0)
                        json += sourceJson;
                    else
                        json += "," + sourceJson;


                }
                return JToken.Parse("[" + json + "]");
            }
            return null;
        }

        /// <summary>
        /// 通过索引与id检查文档是否已经存在
        /// </summary>
        /// <param name="index">索引</param>
        /// <param name="id">id</param>
        /// <returns></returns>
        public static async Task<bool> SourceExistsAsync(this IElasticLowLevelClient elasticSearchServer, string index, string id)
        {
            bool flag = false;
            StringResponse resStr = null;
            try
            {
                //elasticSearchServer.ElasticJsonClient.Indices.Exists()
                resStr = await elasticSearchServer.SourceExistsAsync<StringResponse>(index, id);
                if (resStr.HttpStatusCode == 200)
                {
                    flag = true;
                }
            }
            catch (Exception ex)
            {
            }
            return flag;
        }

        /// <summary>
        /// 检测索引是否已经存在
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static async Task<bool> IsIndexExsit(this IElasticLowLevelClient elasticSearchServer, string index)
        {
            bool flag = false;
            StringResponse resStr = null;
            try
            {
                resStr = await elasticSearchServer.Indices.ExistsAsync<StringResponse>(index);
                if (resStr.HttpStatusCode == 200)
                {
                    flag = true;
                }
            }
            catch (Exception ex)
            {
            }
            return flag;
        }

        /// <summary>
        /// 创建index
        /// </summary>
        /// <param name="indexName"></param>
        /// <param name="shards">分片数量，即数据块最小单元</param>
        /// <returns></returns>
        public static async Task<bool> CreateIndexAsync(this IElasticLowLevelClient elasticSearchServer, string indexName, int shards = 5)
        {
            var isHaveIndex = await IsIndexExsit(elasticSearchServer, indexName.ToLower());
            if (!isHaveIndex)
            {
                var stringResponse = await elasticSearchServer.Indices.CreateAsync<StringResponse>(indexName.ToLower(),
                        PostData.String($"{{\"settings\" : {{\"index\" : {{\"number_of_replicas\" : 0, \"number_of_shards\":\"{shards}\",\"refresh_interval\":\"-1\"}}}}}}"));
                var resObj = JObject.Parse(stringResponse.Body);
                if ((bool)resObj["acknowledged"])
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
            return false;
        }


        /// <summary>
        /// 删除index
        /// </summary>
        /// <param name="indexName"></param>
        /// <returns></returns>
        public static async Task<bool> DeleteIndexAsync(this IElasticLowLevelClient elasticSearchServer, string indexName)
        {
            var stringRespones = await elasticSearchServer.Indices.DeleteAsync<StringResponse>(indexName.ToLower());
            var resObj = JObject.Parse(stringRespones.Body);
            if ((bool)resObj["acknowledged"])
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 插入单个文档
        /// </summary>
        /// <param name="indexName">索引名称</param>
        /// <param name="objectDocment">文档内容</param>
        /// <param name="_id">自定义_id</param>
        /// <returns></returns>
        public static async Task<bool> InsertDocumentAsync(this IElasticLowLevelClient elasticSearchServer, string indexName, object objectDocment, string _id = "")
        {
            var stringRespones = new StringResponse();
            if (_id.Length > 0)
                stringRespones = await elasticSearchServer.IndexAsync<StringResponse>(indexName.ToLower(), _id, PostData.String(JsonConvert.SerializeObject(objectDocment)));
            else
                stringRespones = await elasticSearchServer.IndexAsync<StringResponse>(indexName.ToLower(), PostData.String(JsonConvert.SerializeObject(objectDocment)));
            var resObj = JObject.Parse(stringRespones.Body);
            if ((int)resObj["_shards"]["successful"] > 0)
            {
                return true;
            }
            return false;
        }


        /// <summary>
        /// 删除单个文档
        /// </summary>
        /// <param name="indexName">索引名称</param>
        /// <param name="_id">要删除的id</param>
        /// <returns></returns>
        public static async Task<bool> DeleteDocumentAsync(this IElasticLowLevelClient elasticSearchServer, string indexName, string _id)
        {
            bool flag = false;
            StringResponse resStr = null;
            try
            {
                resStr = await elasticSearchServer.DeleteAsync<StringResponse>(indexName.ToLower(), _id);
                var resObj = JObject.Parse(resStr.Body);
                if ((int)resObj["_shards"]["total"] == 0 || (int)resObj["_shards"]["successful"] > 0)
                {
                    flag = true;
                }
            }
            catch (Exception ex)
            {
            }

            return flag;
        }

        /// <summary>
        /// 更新文档  
        /// </summary>
        /// <param name="indexName">索引名称</param>
        /// <param name="_id">文档id</param>
        /// <param name="objectDocment">文档内容</param>
        /// <returns></returns>
        public static async Task<bool> UpdateDocumentAsync(this IElasticLowLevelClient elasticSearchServer, string indexName, string id, object objectDocment)
        {
            bool flag = false;
            try
            {

                string json = JsonConvert.SerializeObject(objectDocment);

                var updateToJson = "{\"doc\":" + json + "}";

                var stringRespones = await elasticSearchServer.UpdateAsync<StringResponse>(indexName, id, PostData.String(updateToJson));
                var resObj = JObject.Parse(stringRespones.Body);
                if ((int)resObj["_shards"]["successful"] > 0)
                {
                    return true;
                }
            }
            catch { }
            return flag;
        }

        /// <summary>
        /// 通过Bulk更新文档  
        /// </summary>
        /// <param name="indexName">索引名称</param>
        /// <param name="_id">文档id</param>
        /// <param name="objectDocment">文档内容</param>
        /// <returns></returns>
        public static async Task<bool> UpdateDocumentByBulkAsync(this IElasticLowLevelClient elasticSearchServer, string indexName, string _id, object objectDocment)
        {
            bool flag = false;
            try
            {
                string json = JsonConvert.SerializeObject(objectDocment);
                if (json.IndexOf("[") == 0)
                {
                    var objectDocmentOne = JToken.Parse(json);
                    json = JsonConvert.SerializeObject(objectDocmentOne[0]);
                }
                int idInt = json.IndexOf("\"_id");
                if (idInt > 0)
                {
                    string idJson = json.Substring(idInt, json.IndexOf(_id) + _id.Length + 1);
                    json = json.Replace(idJson, "");
                }
                List<string> list = new List<string>();
                list.Add("{\"update\":{\"_id\":\"" + _id + "\"}}");
                list.Add("{\"doc\":" + json + "}");

                var stringRespones = await elasticSearchServer.BulkAsync<StringResponse>(indexName.ToLower(), PostData.MultiJson(list));
                var resObj = JObject.Parse(stringRespones.Body);
                if (!(bool)resObj["errors"])
                {
                    return true;
                }
            }
            catch { }
            return flag;
        }
    }
}