namespace Main.Models
{
    using System;

    public class Date
    {
        private DateTime _date;

        public Date(int year, int month, int day)
        {
            _date = new DateTime(year, month, day);
        }

        public int Year { get { return _date.Year; } }

        public int Month { get { return _date.Month; } }

        public int Day { get { return _date.Day; } }

        public override string ToString()
        {
            return Year + "-" + PaddedNumber(Month) + "-" + PaddedNumber(Day);
        }

        private string PaddedNumber(int number)
        {
            if (number >= 10)
                return "" + number;
            
            return "0" + number;
        }
    }
}
