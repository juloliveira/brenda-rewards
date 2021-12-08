using FirebaseAdmin.Messaging;
using Sara.Contracts.Commands;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sara.Api.Infrasctructure.Impl
{
    public class PushMessage : IPushMessage
    {
        public async Task<string> AwardMessage(AwardPushMessage push, string token)
        {
            var message = new Message
            {
                Notification = new Notification()
                {
                    Title = $"Chegou suas Brendas",
                    Body = $"Você recebeu BRE$ {push.Reward:0.00} ao " +
                    $"assistir a campanha {push.CampaignTitle} do {push.CampaignCustomer}. " +
                    $"Seu saldo agora é de BRE$ {push.Balance:0.00}. " +
                    $"Continue participando para ganhar mais Brendas.",
                },
                Data = new Dictionary<string, string>() {
                    { "updated_balance", $"{push.Balance:0.00}" },
                },
                Token = token
            };

            return await FirebaseMessaging.DefaultInstance.SendAsync(message);
        }

        public async Task<string> TransferReceivedMessage(TransferReceivedPushMessage push, string token)
        {
            try
            {
                var message = new Message
                {
                    Notification = new Notification
                    {
                        Title = "Você recebeu uma transferência",
                        Body = $"Você recebeu BRE$ {push.Value:0.00} do usuário {push.FromUserEmail} " +
                        $"Seu saldo total agora é de BRE$ {push.Balance:0.00}"
                    },
                    Data = new Dictionary<string, string>() {
                    { "updated_balance", $"{push.Balance:0.00}" },
                },
                    Token = token
                };

                return await FirebaseMessaging.DefaultInstance.SendAsync(message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> UpdateBalance(double balance, string token)
        {
            var message = new Message
            {
                Notification = new Notification 
                { 
                    Title = "Seu saldo foi atualizado.",
                    Body = $"Seu saldo agora é de BRE$ {balance:0.00}"
                },
                Data = new Dictionary<string, string>() {
                    { "updated_balance", $"{balance:0.00#}" },
                },
                Token = token
            };

            var result = await FirebaseMessaging.DefaultInstance.SendAsync(message);
            return result;
        }
    }
}
