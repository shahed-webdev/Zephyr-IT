using System;

namespace InventoryManagement.Data
{
    public static class TimeZoneExtensions
    {
        public static DateTime BdTime(this DateTime date)
        {
            var est = TimeZoneInfo.FindSystemTimeZoneById("Bangladesh Standard Time");
            return TimeZoneInfo.ConvertTime(date, est);
        }
    }
}