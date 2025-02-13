using METU.IDS4Server.Identity.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace METU.IDS4Server
{
  public  class UserStore
    {
        public async Task<object> FindBySubjectId(string SubjectId)
        {
            #region 用户名密码
            var memoryClients = OAuthMemoryData.GetTestUsers();
            if (memoryClients.Any(p => p.SubjectId == SubjectId))
            {
                return memoryClients.FirstOrDefault(p => p.SubjectId == SubjectId);
            }
            #endregion

            #region 通过数据库查询Client 信息
            return GetClient(SubjectId);
            #endregion
        }

        private object GetClient(string subjectId)
        {
            throw new NotImplementedException();
        }

        private object GetUsers(string SubjectId)
        {
            //TODO 根据数据库查询
            return null;
        }
    }
}
