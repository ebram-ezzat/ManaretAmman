using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.DTO.Notification;

namespace BusinessLogicLayer.Services.FireBaseNotifications
{
    public interface IFireBaseNotification
    {
        public Task<string> SendNotificationAsync(string deviceToken, string title, string body);
        Task<List<RemiderOutputNotifications>> GetNotificationsForFireBaseAsync(int ProjectId);
        Task UpdateNotificationFireBase(List<RemiderOutputNotifications> remiderOutputs);
    }
}
