namespace Main.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Extensions;

    public class MainModel
    {
        private readonly Dictionary<Date, Day> _days;
 
        public MainModel()
        {
            _days = new Dictionary<Date, Day>();

            var day = new Day
            {
                Date = new Date(2016, 03, 26),
                StartTime = new TimeSpan(13, 37, 00)
            };
            day.AddActivity(new Activity("Lol", new TimeSpan(0, 25, 0), ActivityType.Presentation, ""));
            day.AddActivity(new Activity("Kol", new TimeSpan(0, 10, 0), ActivityType.GroupWork, ""));
            day.AddActivity(new Activity("Kol", new TimeSpan(0, 45, 0), ActivityType.Break, ""));
            day.AddActivity(new Activity("Kol", new TimeSpan(0, 10, 0), ActivityType.GroupWork, ""));
            day.AddActivity(new Activity("Kol", new TimeSpan(1, 10, 0), ActivityType.Discussion, ""));
            _days.Add(day.Date, day);
        }

        public event EventHandler DaysChanged;

        public IEnumerable<Day> Days
        {
            get
            {
                return _days.Values.ToList();
            }
        }

        public void AddNewDay()
        {
            var lastDay = Days.LastOrDefault();
            Date lastDate;
            if (lastDay == null)
            {
                var today = DateTime.UtcNow;
                lastDate = new Date(today.Year, today.Month, today.Day);
            }
            else
                lastDate = lastDay.Date.AddDays(1);

            var day = new Day { Date = lastDate, StartTime = new TimeSpan(8, 0, 0) };
            
            _days.Add(day.Date, day);
            DaysChanged.Raise(this);
        }

        public void UpdateDay(Date date, Day day)
        {
            _days[date] = day;
            DaysChanged.Raise(this);
        }

        public void RemoveDay(Day day)
        {
            _days.Remove(day.Date);
            DaysChanged.Raise(this);
        }
    }
}
