using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Security.Claims
{
  public static  class ClaimsPrincipalExtentions
    {
        public static ClaimsPrincipal ObjectToClaimsPrinc(this ClaimsPrincipal cp, object obj)
        {
            ClaimsIdentity ci = new ClaimsIdentity();

            ci.ObjectToClaimsIdentity(obj);
            cp.AddIdentity(ci);
            return cp;
        }
        public static void ObjectToClaimsPrincipal(this ClaimsPrincipal cp, object obj)
        {
            ClaimsIdentity ci = new ClaimsIdentity();

            ci.ObjectToClaimsIdentity(obj);
            cp.AddIdentity(ci);
        }
        public static ClaimsPrincipal ObjectToClaimsPrincipal( this object obj)
        {
            ClaimsIdentity ci = new ClaimsIdentity();
            ClaimsPrincipal cp = new ClaimsPrincipal();
            ci.ObjectToClaimsIdentity(obj);
            cp.AddIdentity(ci);
            return cp;
        }
        public static ClaimsPrincipal ToClaimsPrincipal(this object obj)
        {
            ClaimsIdentity ci = new ClaimsIdentity();
            ClaimsPrincipal cp = new ClaimsPrincipal();
            ci.ObjectToClaimsIdentity(obj);
            cp.AddIdentity(ci);
            return cp;
        }
    }
}
