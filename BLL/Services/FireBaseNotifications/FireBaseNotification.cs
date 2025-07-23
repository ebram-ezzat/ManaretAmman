using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.DTO.Notification;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using DataAccessLayer.Models;
namespace BusinessLogicLayer.Services.FireBaseNotifications
{
    public class FireBaseNotification : IFireBaseNotification
    {
        private readonly PayrolLogOnlyContext _payrolLogOnlyContext;
        public FireBaseNotification(PayrolLogOnlyContext payrolLogOnlyContext)
        {
            _payrolLogOnlyContext = payrolLogOnlyContext;
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
        public async Task<List<RemiderOutputNotifications>> GetNotificationsForFireBaseAsync(int ProjectId)
        {
            Dictionary<string, object> inputParams = new Dictionary<string, object>
            {
                { "pProjectID", ProjectId },
                { "pFlag", 7 }
            };

            // Define output parameters (optional)
            Dictionary<string, object> outputParams = new Dictionary<string, object>
            {
                { "prowcount","int" }
            };

            var (NotificationResponse, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync<RemiderOutputNotifications>("[dbo].[GetReminders]", inputParams, outputParams);
            return NotificationResponse;
        }

        public async Task UpdateNotificationFireBase(List<RemiderOutputNotifications> remiderOutputs)
        {

            foreach (var item in remiderOutputs)
            {
                Dictionary<string, object> inputParams = new Dictionary<string, object>
            {
                { "pID", item.ID },
                { "pFlag",  7 },

            };

                // Define output parameters (optional)
                Dictionary<string, object> outputParams = new Dictionary<string, object>
            {
                 { "pError","int" },
            };

                // Call the ExecuteStoredProcedureAsync function
                var (result, outputValues) = await _payrolLogOnlyContext.GetProcedures().ExecuteStoredProcedureAsync("dbo.UpdateReminder", inputParams, outputParams);

            }

        }
    }
}
