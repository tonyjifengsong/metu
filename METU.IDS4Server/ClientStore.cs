﻿using IdentityServer4.Models;
using IdentityServer4.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace METU.IDS4Server.Identity.Service
{
    public class ClientStore : IClientStore
    {
        public async Task<Client> FindClientByIdAsync(string clientId)
        {
            #region 用户名密码
            var memoryClients = OAuthMemoryData.GetClients();
            if (memoryClients.Any(p => p.ClientId == clientId))
            {
                return memoryClients.FirstOrDefault(p => p.ClientId == clientId);
            }
            #endregion

            #region 通过数据库查询Client 信息
            return GetClient(clientId);
            #endregion
        }

        private Client GetClient(string client)
        {
            //TODO 根据数据库查询
            return null;
        }
    }
}
