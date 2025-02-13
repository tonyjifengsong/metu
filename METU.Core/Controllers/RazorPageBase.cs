using METU.INTERFACE;
using METU.INTERFACE.EVENTS;
using METU.MSSQL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;

namespace METU.Main.Controllers
{
  public partial  class RazorPageBase :  PageModel
    {

        protected IEventInit eventaction = new EventAction();
        protected IEventPredicate eventpre = new EventPredicate();
        protected IEventFunc eventfunc = new EventFunc();
        protected IEvent events = new Events();
        protected IFEvent<object> evento = new CoreEvent<object>();

        public DataTable dtCtrols;
        public DataTable dtDataList;
        protected CoreBLL _context;
        protected   ILogger<RazorPageBase> logger;
        protected metucore me;
        protected METUCORE ME;
        [BindProperty(SupportsGet = true)]
        public string appkey { get; set; }
        /// <summary>
        /// secretkey参数
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string secretkey { get; set; }
        /// <summary>
        /// APPID参数
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string appid { get; set; }
        /// <summary>
        /// CODE参数
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string code { get; set; }
        /// <summary>
        /// barcode参数
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string barcode { get; set; }
        /// <summary>
        /// PID参数
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string pid { get; set; }
        /// <summary>
        /// CID参数
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string cid { get; set; }
        /// <summary>
        /// DOMAINID参数
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string domainid { get; set; }
        /// <summary>
        /// ID参数
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string id { get; set; }
        /// <summary>
        /// 应用 名参数
        /// </summary>
        [BindProperty(SupportsGet = true)]

        public string appname { get; set; }
        /// <summary>
        /// 服务名参数
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string servicename { get; set; }
        /// <summary>
        /// 分页大小
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public int pagesize { get; set; }
        /// <summary>
        /// 当前页码
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public int currentpage { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string version { get; set; }
        /// <summary>
        /// 方法名
        /// </summary>
        [BindProperty(SupportsGet = true)]

        public string methodname { get; set; }
        [BindProperty(SupportsGet =true)]
        public List<Dictionary<string, object>> ListDic { get; set; }

        [BindProperty(SupportsGet = true)]
        public List<Dictionary<string, string>> ListDics { get; set; }

        public DataTable dt = new DataTable();
        public DataTable dts = new DataTable();
        public Dictionary<string, object> dic = new Dictionary<string, object>();
        public Dictionary<string, string> dics = new Dictionary<string, string>();
        [BindProperty]
        public Dictionary<string, object> dicbind { get; set; }
        protected CoreBLL bll;
        [BindProperty(SupportsGet = true)]
        public string pagename { get; set; }
        [BindProperty(SupportsGet = true)]
        public string currentpagename { get; set; }
        [BindProperty(SupportsGet = true)]
        public string pagetitle { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SearchTxt { get; set; }

        [BindProperty(SupportsGet = true)]
        public string param { get; set; }
        public RazorPageBase(ILogger<RazorPageBase> _logger, CoreBLL context)
        {
            me = new metucore();
            ME = new METUCORE();
            _context = context;
            logger = _logger;
            bll = context;
        }
        public RazorPageBase(CoreBLL context)
        {
            ME = new METUCORE();
            me = new metucore();
            _context = context;
            bll = context;

        }
        public RazorPageBase()
        {
            ME = new METUCORE();
            me = new metucore();
            _context = new CoreBLL();
            bll = _context;

        }
        protected void getForm()
        {
            dic = new Dictionary<string, object>();
            dics = new Dictionary<string, string>();
            dic.AddAppName(appname);
            dic.AddMethodName(methodname);
            dic.AddServiceName(servicename);
            dics.AddAppName(appname);
            dics.AddMethodName(methodname);
            dics.AddServiceName(servicename);
            dic.AddKey("pid", pid);
            dic.AddKey("cid", cid);
            dic.AddKey("appid", appid);
            dic.AddKey("appkey", appkey);
            dic.AddKey("secretkey", secretkey);
            dic.AddKey("domainid", domainid);
            dic.AddKey("barcode", barcode);
            dic.AddKey("code", code);
            dics.AddKey("pid", pid);
            dics.AddKey("cid", cid);
            dics.AddKey("appid", appid);
            dics.AddKey("appkey", appkey);
            dics.AddKey("secretkey", secretkey);
            dics.AddKey("domainid", domainid);
            dics.AddKey("barcode", barcode);
            dics.AddKey("code", code);

            dic.AddKey("pagename", pagename);
            dic.AddKey("version", version);
            dic.AddKey("versions", version);
            dic.AddKey("SearchTxt", SearchTxt);
            dic.AddKey("param", param);
            dics.AddKey("pagename", pagename);
            dics.AddKey("versions", version);
            dics.AddKey("version", version);
            dics.AddKey("SearchTxt", SearchTxt);
            dics.AddKey("param", param);
            foreach (var itm in HttpContext.Request.Query.Keys)
            {
                dic.AddKey(itm, HttpContext.Request.Query[itm]);
                dics.AddKey(itm, HttpContext.Request.Query[itm]);

            } 
            try
            {
                foreach (string key in base.HttpContext.Request.Form.Keys)
                {
                    dic.AddKey(key, base.HttpContext.Request.Form[key]);
                    dics.AddKey(key, base.HttpContext.Request.Form[key]);
                }
            }
            catch (Exception ex)
            {
                FileHelper.Writelog(ex.Message);
            }
           
            foreach (var itm in HttpContext.Request.Headers.Keys)
            {
                dic.AddKey(itm, HttpContext.Request.Headers[itm]);
                dics.AddKey(itm, HttpContext.Request.Headers[itm]);

            }
        }
        protected Dictionary<string, object> getFormData()
        {
            dic = new Dictionary<string, object>();
            dics = new Dictionary<string, string>();
            dic.AddAppName(appname);
            dic.AddMethodName(methodname);
            dic.AddServiceName(servicename);
            dics.AddAppName(appname);
            dics.AddMethodName(methodname);
            dics.AddServiceName(servicename);
            dic.AddKey("pid", pid);
            dic.AddKey("cid", cid);
            dic.AddKey("appid", appid);
            dic.AddKey("appkey", appkey);
            dic.AddKey("secretkey", secretkey);
            dic.AddKey("domainid", domainid);
            dic.AddKey("barcode", barcode);
            dic.AddKey("code", code);
            dics.AddKey("pid", pid);
            dics.AddKey("cid", cid);
            dics.AddKey("appid", appid);
            dics.AddKey("appkey", appkey);
            dics.AddKey("secretkey", secretkey);
            dics.AddKey("domainid", domainid);
            dics.AddKey("barcode", barcode);
            dics.AddKey("code", code);
            dic.AddKey("pagename", pagename);
            dic.AddKey("version", version);
            dic.AddKey("versions", version);
            dic.AddKey("SearchTxt", SearchTxt);
            dic.AddKey("param", param);
            dics.AddKey("pagename", pagename);
            dics.AddKey("versions", version);
            dics.AddKey("version", version);
            dics.AddKey("SearchTxt", SearchTxt);
            dics.AddKey("param", param);
             foreach (var itm in HttpContext.Request.Query.Keys)
            {
                dic.AddKey(itm, HttpContext.Request.Query[itm]);
                dics.AddKey(itm, HttpContext.Request.Query[itm]);

            }
            try
            {
                foreach (string key in base.HttpContext.Request.Form.Keys)
                {
                    dic.AddKey(key, base.HttpContext.Request.Form[key]);
                    dics.AddKey(key, base.HttpContext.Request.Form[key]);
                }
            }
            catch (Exception ex)
            {
                FileHelper.Writelog(ex.Message);
            }
           
            foreach (var itm in HttpContext.Request.Headers.Keys)
            {
                dic.AddKey(itm, HttpContext.Request.Headers[itm]);
                dics.AddKey(itm, HttpContext.Request.Headers[itm]);

            }
            return dic;
        }
    }
}
