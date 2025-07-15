using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;

namespace BusinessLogicLayer.Services.FireBaseNotifications
{
    public class FireBaseNotification : IFireBaseNotification
    {
        public FireBaseNotification()
        {
            if (FirebaseApp.DefaultInstance == null)
            {
                FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromFile("manaretamman-firebase-adminsdk.json")
                });
            }
        }
        public async Task<string> SendNotificationAsync(string deviceToken, string title, string body)
        {
            var message = new Message()
            {
                Token = deviceToken,               
                Notification = new FirebaseAdmin.Messaging.Notification
                {
                    Title = title,
                    Body = body
                },
                Android = new AndroidConfig
                {
                    Priority = Priority.High
                },
               
            };

            string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
            return response; // Message ID
        }
    }
}
