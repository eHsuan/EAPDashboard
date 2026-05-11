using System;
using System.IO;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using EAPGateway.Models;
using EAPGateway.Services;

namespace EAPGateway
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // 1. Load Config
            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");
            if (!File.Exists(configPath))
            {
                var defaultConfig = new GatewayConfig { 
                    Port = 5000,
                    PrimaryDb = "Data Source=127.0.0.1;Initial Catalog=EAP_DB;User ID=sa;Password=your_password;Connect Timeout=3",
                    SecondaryDb = "Data Source=127.0.0.1;Initial Catalog=EAP_DB_BAK;User ID=sa;Password=your_password;Connect Timeout=3"
                };
                File.WriteAllText(configPath, JsonConvert.SerializeObject(defaultConfig, Formatting.Indented));
            }
            var config = JsonConvert.DeserializeObject<GatewayConfig>(File.ReadAllText(configPath)) ?? new GatewayConfig();

            // 2. Setup Services
            var logService = new LogService();
            var dbService = new DatabaseService(config, logService);
            var retryService = new RetryBackgroundService(dbService, config, logService);

            // 3. Start Web API in Background
            var builder = WebApplication.CreateBuilder();
            builder.Services.AddControllers();
            builder.Services.AddSingleton(config);
            builder.Services.AddSingleton(logService);
            builder.Services.AddSingleton(dbService);
            builder.Services.AddHostedService(sp => retryService);
            builder.WebHost.UseUrls($"http://*:{config.Port}");
            
            var app = builder.Build();
            app.MapControllers();
            _ = app.RunAsync();

            // 4. Start WinForms UI
            logService.Info("EAPGateway Web API started.");
            System.Windows.Forms.Application.Run(new MainForm(config, dbService, logService));
        }
    }
}
