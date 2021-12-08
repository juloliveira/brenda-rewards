using System;
using System.Collections.Generic;
using System.Text;

namespace Brenda.Utils
{
    public static class DateTimeExtentions
    {
        public static DateTime AtDaysAgo(this DateTime datetime, int days)
        {
            return datetime.AddDays(days * (-1)).Date;
        }

        public static DateTime AtMonthAgo(this DateTime datetime)
        {
            return datetime.AtDaysAgo(30);
        }

        public static DateTime FromTimestamp(long from)
        {
            return new DateTime(1970, 1, 1).AddSeconds(from);
        }

        public static long ToTimestamp(this DateTime datetime)
        {
            return (long)(datetime.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }

        public static string Friendly2(this DateTime dateTime)
        {
            return dateTime.ToString("dd MMMM yy", System.Globalization.CultureInfo.GetCultureInfo("pt-BR")).ToUpper();
        }

        public static string Friendly2(this DateTime? dateTime)
        {
            if (!dateTime.HasValue) return "ND";
            return Friendly2(dateTime.Value);
        }

        public static string Friendly(this DateTime dateTime)
        {
            return dateTime.ToString("dd MMMM yy 'às' HH'h'", System.Globalization.CultureInfo.GetCultureInfo("pt-BR")).ToLower();
        }

        public static string TimeAgo(this DateTime dateTime)
        {
            var timeSpan = DateTime.Now.Subtract(dateTime);
            if (timeSpan <= TimeSpan.FromSeconds(60))
                return $"{timeSpan.Seconds} segundos atrás";
            else if (timeSpan <= TimeSpan.FromMinutes(60))
                return timeSpan.Minutes > 1 ? $"{timeSpan.Minutes} minutos atrás" : "a um minuto atrás";
            else if (timeSpan <= TimeSpan.FromHours(24))
                return timeSpan.Hours > 1 ? $"{timeSpan.Hours} horas atrás" : "a uma hora atrás";
            else if (timeSpan <= TimeSpan.FromDays(30))
                return timeSpan.Days > 1 ? $"{timeSpan.Days} dias atrás" : "ontem";
            else if (timeSpan <= TimeSpan.FromDays(365))
                return timeSpan.Days > 30 ? $"{timeSpan.Days / 30} meses atrás" : "a um mês atrás";
            else
                return timeSpan.Days > 365 ? $"{timeSpan.Days / 365} anos atrás" : "a um ano atrás";
        }
    }
}
