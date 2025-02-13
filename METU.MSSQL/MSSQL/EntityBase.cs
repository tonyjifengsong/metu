using METU.INTERFACE.ICore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace METU.MSSQL.MSSQL
{
    public class EntityBase : IEntity
    {
        public EntityBase()
        {
            id = Guid.NewGuid().ToString();
        }
        public string id { get; set; }

    }
}
