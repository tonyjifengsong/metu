using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace METU.INTERFACE
{
    public interface IControllerBase<T> : IBase
    {
        IQueryable<T> GetAll();
        IQueryable<T> GetAll(T model);
        List<T> GetList(T model);
        T Get(int id);
        T GetModel(string GuidKey);
        bool Delete(string GuidKey);

        T Post(T model);

        bool Delete(int id);
        string Delete(T model);
        bool CheckInput(dynamic context);
        HttpResponseMessage DoBusiness(dynamic context);
        bool CheckReturn(dynamic obj);
        object DoWork(dynamic context);
        List<object> DoWorks(dynamic context);
        IQueryable<T> GetQuery(dynamic context);
    }
}
