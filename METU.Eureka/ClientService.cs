using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace METU.Eureka
{
    public class ClientService : IClientService
    {
        private ILogger<ClientService> _logger;
        private readonly HttpClient _httpClient;

        public ClientService(HttpClient httpClient, ILoggerFactory logFactory = null)
        {
            _httpClient = httpClient;
            _logger = logFactory?.CreateLogger<ClientService>();
        }

        public async Task<string> GetClientName(int clientId)
        {
            var result = await _httpClient.GetStringAsync(clientId.ToString());
            _logger?.LogInformation($"GetClientName - ClientId:{clientId}");
            return result;
        }
    }
}
