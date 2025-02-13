using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Security.Claims
{
  public static  class ClaimExtentions
    {
        /// <summary>
        /// Object To Claim
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static List< Claim > ObjectToClaimList(this object obj)
        {
            List<Claim> rs = new List<Claim>();
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic = obj.ToDictionary();
            foreach(var itm in dic)
            {
                rs.Add(new Claim(itm.Key,itm.Value));
            }
            return rs;
        }
        public static List<Claim>  ToClaimList(this object obj)
        {
            List<Claim> rs = new List<Claim>();
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic = obj.ToDictionary();
            foreach (var itm in dic)
            {
                rs.Add(new Claim(itm.Key, itm.Value));
            }
            return rs;
        }
    }
}
