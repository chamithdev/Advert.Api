using Advert.Api.Services;
using Microsoft.Extensions.HealthChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Advert.Api.HealthChecks
{
    public class StorageHealthCheck : IHealthCheck
    {
        private readonly IAdvertStorageService advertStorageService;

        public StorageHealthCheck(IAdvertStorageService advertStorageService)
        {
            this.advertStorageService = advertStorageService;
        }

        public async ValueTask<IHealthCheckResult> CheckAsync(CancellationToken cancellationToken = default)
        {
            bool isAlive = await this.advertStorageService.CheckHealthAsync();
            if(isAlive)
            {
                return HealthCheckResult.FromStatus(CheckStatus.Healthy,String.Empty);
            }
            return HealthCheckResult.FromStatus(CheckStatus.Unhealthy, String.Empty);
        }
    }
}
