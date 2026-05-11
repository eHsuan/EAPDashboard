using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using EAPGateway.Models;

namespace EAPGateway.Services
{
    public class RetryBackgroundService : BackgroundService
    {
        private readonly DatabaseService _dbService;
        private readonly GatewayConfig _config;
        public event Action<string>? OnLog;

        public RetryBackgroundService(DatabaseService dbService, GatewayConfig config)
        {
            _dbService = dbService;
            _config = config;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await foreach (var task in _dbService.RetryReader.ReadAllAsync(stoppingToken))
            {
                bool success = false;
                int retryCount = 0;

                while (!success && !stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        retryCount++;
                        await _dbService.ExecuteActionAsync(_config.SecondaryDb, task.Action, task.Request);
                        success = true;
                        OnLog?.Invoke($"[RETRY] Successfully synced Secondary DB for {task.Action} after {retryCount} attempts.");
                    }
                    catch (Exception ex)
                    {
                        OnLog?.Invoke($"[RETRY] Failed to sync Secondary DB (Attempt {retryCount}): {ex.Message}. Waiting 5s...");
                        await Task.Delay(5000, stoppingToken);
                    }
                }
            }
        }
    }
}
