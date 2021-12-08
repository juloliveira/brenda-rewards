using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Brenda.Infrastructure
{
    public interface IPushNotifications
    {
        Task NotifyPointsBalance(string token, double pointsBalance);
    }
}
