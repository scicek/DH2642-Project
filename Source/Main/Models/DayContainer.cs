namespace Main.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Extensions;

    /// <summary>
    /// Represents a container of days.
    /// </summary>
    public class DayContainer
    {
        private readonly TimeSpan _defaultStartTime = new TimeSpan(8, 0, 0);
        private List<Day> _days;

        public DayContainer()
        {
            _days = new List<Day>();
        }

        public DayContainer(List<Day> days) : this()
        {
            if (days == null || !days.Any())
                return;

            foreach (var day in days)
                AddDay(day);
        }

        public event EventHandler<Day> Added;
        public event EventHandler<Day> Removed;

        public IEnumerable<Day> Days
        {
            get
            {
                return _days.ToList();
            }
        }

        public void AddDay(Day day)
        {
            if (_days.Contains(day))
                return;

            _days.Add(day);
            _days = _days.OrderBy(d => d).ToList();
            Added.Raise(this, day);
        }

        public void AddNewDay()
        {
            var lastDay = Days.LastOrDefault();
            Date lastDate;

            if (lastDay == null || lastDay.Date < Date.Today)
            {
                var today = DateTime.Now;
                lastDate = new Date(today.Year, today.Month, today.Day);
            }
            else
                lastDate = lastDay.Date.AddDays(1);

            var day = new Day(lastDate) { BeginTime = _defaultStartTime };
            
            _days.Add(day);
            Added.Raise(this, day);
        }

        public void RemoveDay(Day day)
        {
            _days.Remove(day);
            Removed.Raise(this, day);
        }
    }
}
