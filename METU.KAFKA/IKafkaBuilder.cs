using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace METU.KAFKA
{
    public interface IKafkaBuilder
    {         /// <summary>
              /// Gets the <see cref="IServiceCollection"/> where Logging services are configured.
              /// </summary>
        IServiceCollection Services { get; }
    }

}
