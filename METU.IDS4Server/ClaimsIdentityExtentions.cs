using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Security.Claims
{
  public static  class ClaimsIdentityExtentions
    {
        public static ClaimsIdentity ObjectToClaimsIdentity(this ClaimsIdentity ci,object obj)
        {
            List<Claim> rs =obj.ObjectToClaimList();
           
            foreach (var itm in rs)
            {
                ci.AddClaim(itm);
            }
            return ci;
        }
        public static void ObjectToClaimsIdentit(this ClaimsIdentity ci, object obj)
        {
            List<Claim> rs = obj.ObjectToClaimList();

            foreach (var itm in rs)
            {
                ci.AddClaim(itm);
            }
           
        }

        public static ClaimsIdentity ObjectToClaimsIdentity( this object obj)
        {
            ClaimsIdentity ci = new ClaimsIdentity();
            List<Claim> rs = obj.ObjectToClaimList();

            foreach (var itm in rs)
            {
                ci.AddClaim(itm);
            }
            return ci;
        }

        public static ClaimsIdentity ToClaimsIdentity(this object obj)
        {
            ClaimsIdentity ci = new ClaimsIdentity();
            List<Claim> rs = obj.ObjectToClaimList();

            foreach (var itm in rs)
            {
                ci.AddClaim(itm);
            }
            return ci;
        }
    }
}
