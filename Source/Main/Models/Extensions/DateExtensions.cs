namespace Main.Models.Extensions
{
    using System;

    public static class DateExtensions
    {
        public static Date AddDays(this Date date, int days)
        {
            var dateTime = new DateTime(date.Year, date.Month, date.Day).AddDays(days);
            return new Date(dateTime.Year, dateTime.Month, dateTime.Day);
        }
    }
}
