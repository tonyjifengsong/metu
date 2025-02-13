using IdentityServer4;
using IdentityServer4.Models;
using METU.MODEL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace METU.IDS4Server
{
    public class IDS4Config
    {

        static CoreBLL bll = new CoreBLL();
        //下载ids4的依赖：install-package IdentityServer4  -version 2.1.1
        // scopes define the resources in your system
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                 new IdentityResources.Address(),
                  new IdentityResources.Email(),
                   new IdentityResources.Phone(),

            };
        }
        //所有可以访问的Resource
        public static IEnumerable<ApiResource> GetApiResources()
        {
            List<ApiResource> rs = new List<ApiResource>();
            var dt = bll.db.ExecuteDataTable("select * from ID4Resources");
            var lst = dt.ToList<IDS4Client>();
            foreach (var itm in lst)
            {
                if (itm.name == null) continue;
                if (itm.name.Trim().Length == 0) continue;
                if (itm.auth2claims != null && itm.auth2claims.Trim().Length > 2)
                {
                    if (itm.descriptions == null || itm.descriptions.Trim().Length == 0)
                    {
                        List<string> lsts = itm.auth2claims.ToList();
                        ApiResource rsitm = new ApiResource(itm.name, lsts);
                        rs.Add(rsitm);
                    }
                    else
                    {
                        List<string> lsts = itm.auth2claims.ToList();
                        ApiResource rsitm = new ApiResource(itm.name, itm.descriptions, lsts);
                        rs.Add(rsitm);
                    }
                }
                else
                {
                    if (itm.descriptions == null || itm.descriptions.Trim().Length == 0)
                    {

                        ApiResource rsitm = new ApiResource(itm.name);
                        rs.Add(rsitm);
                    }
                    else
                    {

                        ApiResource rsitm = new ApiResource(itm.name, itm.descriptions);
                        rs.Add(rsitm);
                    }
                }
            }
            rs.Add(new ApiResource("api1", "Tony API 1"));
            return rs;
        }
        // clients want to access resources (aka scopes)
        public static IEnumerable<Client> GetClients()
        {
            List<Client> rs = new List<Client>();

            var dt = bll.db.ExecuteDataTable("select * from ID4Clients");
            var lst = dt.ToList<IDS4Client>();
            foreach (var itm in lst)
            {
                Client ct = new Client();
                ct.ClientId = itm.ClientId;
                ct.ClientName = itm.ClientName;
                ICollection<string> ic = new Collection<string>();
                ic.Add(IdentityServerConstants.StandardScopes.OpenId);
                ic.Add(IdentityServerConstants.StandardScopes.Profile);
                ic.Add(IdentityServerConstants.StandardScopes.Address);
                ic.Add(IdentityServerConstants.StandardScopes.Email);
                ic.Add(IdentityServerConstants.StandardScopes.Phone);
                ct.AllowedGrantTypes = GrantTypes.Implicit;
                ct.RequireConsent = itm.RequireConsent;
                ct.RedirectUris = itm.RedirectUris.ToIcollection();
                ct.PostLogoutRedirectUris = itm.PostLogoutRedirectUris.ToIcollection();
                ct.AllowedScopes = ic;
                ct.AllowedCorsOrigins = itm.AllowedCorsOrigins.ToIcollection();
                ct.EnableLocalLogin = itm.EnableLocalLogin;
                ICollection<ClientClaim> icm = new Collection<ClientClaim>();
                Dictionary<string, string> dicc = itm.Claims.ToDictinary();
                foreach (var itms in dicc)
                {
                    ClientClaim ccm = new ClientClaim(itms.Key, itms.Value);
                    icm.Add(ccm);
                }
                ct.Claims = icm;              
                rs.Add(ct);
            }
            return rs;

        }
    }
}
