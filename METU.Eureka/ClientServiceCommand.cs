﻿using Microsoft.Extensions.Logging;
using Steeltoe.CircuitBreaker.Hystrix;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace METU.Eureka
{
    public class ClientServiceCommand : HystrixCommand<string>
    {
        IClientService _clientService;
        ILogger<ClientServiceCommand> _logger;
        private int _clientId;

        public ClientServiceCommand(IHystrixCommandOptions options, IClientService clientService,
            ILogger<ClientServiceCommand> logger) : base(options)
        {
            _clientService = clientService;
            _logger = logger;
            IsFallbackUserDefined = true;
        }

        public async Task<string> GetClientName(int clientId)
        {
            _clientId = clientId;
            return await ExecuteAsync();
        }

        protected override async Task<string> RunAsync()
        {
            var result = await _clientService.GetClientName(_clientId);
            _logger.LogInformation("Run: {0}", result);
            return result;
        }

        protected override async Task<string> RunFallbackAsync()
        {
            _logger.LogWarning("RunFallback");
            return await Task.FromResult("Sorry, the service is unavaliable now. Please try again later.");
        }
    }
}
