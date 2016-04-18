namespace Main.Models
{
    using System;
    using Newtonsoft.Json;

    /// <summary>
    /// Represents a date.
    /// </summary>
    public class Date
    {
        private DateTime _date;

        public Date(DateTime dateTime)
        {
            _date = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day);
        }

        [JsonConstructor]
        public Date(int year, int month, int day)
        {
            _date = new DateTime(year, month, day);
        }

        public int Year { get { return _date.Year; } }

        public int Month { get { return _date.Month; } }

        public int Day { get { return _date.Day; } }

        public static Date Today()
        {
            return new Date(DateTime.Now);
        }

        public static bool operator < (Date c1, Date c2)
        {
            return c1._date < c2._date;
        }

        public static bool operator > (Date c1, Date c2)
        {
            return c1._date > c2._date;
        }

        public static bool operator == (Date c1, Date c2)
        {
            return c1._date == c2._date;
        }

        public static bool operator != (Date c1, Date c2)
        {
            return c1._date != c2._date;
        }

        public override string ToString()
        {
            return Year + "-" + PaddedNumber(Month) + "-" + PaddedNumber(Day);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Date)obj);
        }

        public override int GetHashCode()
        {
            return _date.GetHashCode();
        }

        protected bool Equals(Date other)
        {
            return _date.Equals(other._date);
        }

        private string PaddedNumber(int number)
        {
            if (number >= 10)
                return "" + number;
            
            return "0" + number;
        }
    }
}
