using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using WorkerService1.Models;
using WorkerService1.Services;

namespace WorkerService1
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private HttpClient client;
        private readonly DbHelper dbHelper;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
            dbHelper = new DbHelper();
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            client = new HttpClient();
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            client.Dispose();
            _logger.LogInformation("The service has been stopped...");
            return base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                List<FtpFile> ftpFiles = dbHelper.GetAllFiles();
                if(ftpFiles.Count == 0)
                    dbHelper.SeedData();
                else 
                    PrintUserInfo(ftpFiles);


                var result = await client.GetAsync("https://www.apress.com/gp");
                if (result.IsSuccessStatusCode)
                {
                    _logger.LogInformation("The website is up. Status code {StatusCode}", result.StatusCode);
                }
                else
                {
                    _logger.LogError("The website is down. Status code{StatusCode}", result.StatusCode);
                }

                await Task.Delay(5000, stoppingToken);
            }
        }

        private void PrintUserInfo(List<FtpFile> ftpFiles)
        {
            ftpFiles?.ForEach(ftpFile =>
            {
                _logger.LogInformation($"File info: Name: {ftpFile.FileName} and FileDownloaded: {ftpFile.FileDownloaded} and FileInProgress: {ftpFile.FileInProgress} ");

            });

        }
    }
}
