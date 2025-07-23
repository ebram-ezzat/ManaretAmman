using BusinessLogicLayer.Services.FireBaseNotifications;
using BusinessLogicLayer.Services.Notification;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ManaretAmman.HostedServices
{
    public class NotificationHostedService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IConfiguration _configuration;
        private readonly int ProjectId;
        private readonly int DelayInMinutes;

        public NotificationHostedService(IServiceScopeFactory scopeFactory, IConfiguration configuration)
        {
            _scopeFactory = scopeFactory;
            _configuration = configuration;
            ProjectId = _configuration.GetValue<int>("HostedServices:ProjectId");
            DelayInMinutes = _configuration.GetValue<int>("HostedServices:Delay");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using PeriodicTimer timer = new(TimeSpan.FromMinutes(DelayInMinutes));
            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                await GetNotificationsAsync();
            }
           
        }

        private async Task GetNotificationsAsync()
        {
            try
            {
                var stopWatch = Stopwatch.StartNew();
                using var scope = _scopeFactory.CreateScope();
                var fireBaseNotification = scope.ServiceProvider.GetRequiredService<IFireBaseNotification>();

                var notifications = await fireBaseNotification.GetNotificationsForFireBaseAsync(ProjectId);

                foreach (var item in notifications)
                {
                    if (item.Token is not null)
                    {
                        var message = await fireBaseNotification.SendNotificationAsync(item.Token, item.Typedesc, item.Notes);
                    }

                }

                await fireBaseNotification.UpdateNotificationFireBase(notifications);
                stopWatch.Stop();
            }
            catch (Exception ex)
            {

            }
       
        }
    }
}
