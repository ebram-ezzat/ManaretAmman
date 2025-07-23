using BusinessLogicLayer.Services.FireBaseNotifications;
using BusinessLogicLayer.Services.Notification;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

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
            while (!stoppingToken.IsCancellationRequested)
            {
                await GetNotificationsAsync();

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }

        private async Task GetNotificationsAsync()
        {
            using var scope = _scopeFactory.CreateScope();
            var fireBaseNotification = scope.ServiceProvider.GetRequiredService<IFireBaseNotification>();

            var notifications = await fireBaseNotification.GetNotificationsForFireBaseAsync(ProjectId);

            foreach (var item in notifications)
            {
                await fireBaseNotification.SendNotificationAsync(item.Token, item.Typedesc, item.Notes);
            }

            await fireBaseNotification.UpdateNotificationFireBase(notifications);
        }
    }
}
