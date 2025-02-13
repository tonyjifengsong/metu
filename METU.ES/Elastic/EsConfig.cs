using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace METU.ES.Elastic
{
    public class EsConfig : IOptions<EsConfig>
    {
        public List<string> Urls { get; set; }

        public EsConfig Value => this;
    }
}
