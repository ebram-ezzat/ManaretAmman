using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services.FireBaseNotifications
{
    public interface IFireBaseNotification
    {
        public Task<string> SendNotificationAsync(string deviceToken, string title, string body);
    }
}
