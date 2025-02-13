using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace METU.INTERFACE
{

    public interface IDBHelper
    {
        bool Add(dynamic model, string Sqlstr = "");
        bool Update(dynamic model, string Sqlstr = "");
        bool Delete(dynamic model, string Sqlstr = "");
        object Search(dynamic model, string Sqlstr = "");
        DataTable SearchDt(dynamic model, string Sqlstr = "");
        object executesql(string sqlstr);
        DataTable SearchSQL(Dictionary<string, string> dic, string Sqlstr = "");


        DataTable executeDt(string sqlstr);

    }
}
