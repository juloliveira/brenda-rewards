using FirebaseAdmin.Messaging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Brenda.Infrastructure.Impl
{
    public class FCSPushNotifications : IPushNotifications
    {
        public async Task NotifyPointsBalance(string token, double pointsBalance)
        {
            // See documentation on defining a message payload.
            var message = new Message()
            {
                Notification = new Notification
                { 
                    Title = "Seu saldo foi atualizado!",
                    Body = $"Seu saldo foi atualizado no valor de {pointsBalance:F4}"
                },
                Data = new Dictionary<string, string>()
                {
                    { "click_action", "FLUTTER_NOTIFICATION_CLICK" },
                    { "points_balance", pointsBalance.ToString() },
                },
                Token = token

            };

            // Send a message to the device corresponding to the provided
            // registration token.
            string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
            // Response is a message ID string.
            Console.WriteLine("Successfully sent message: " + response);
        }
    }
}
