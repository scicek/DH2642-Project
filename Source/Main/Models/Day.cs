namespace Main.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Extensions;

    public class Day 
    {
        private readonly List<Activity> _activities;

        public Day()
        {
            _activities = new List<Activity>();    
        }

        public event EventHandler ActivitiesChanged;

        public Date Date { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime
        {
            get
            {
                return StartTime + TotalLength; 
            }
        }

        public TimeSpan TotalLength
        {
            get
            {
                var length = new TimeSpan();
                return Enum.GetValues(typeof (ActivityType)).Cast<ActivityType>().Aggregate(length, (current, activity) => current.Add(TotalActivityLength(activity)));
            }
        }

        public IEnumerable<Activity> Activities
        {
            get
            {
                return _activities;
            }
        }

        public Dictionary<ActivityType, double> TimeAllocations
        {
            get
            {
                var allocations = new Dictionary<ActivityType, double>();

                foreach (var activityType in Enum.GetValues(typeof(ActivityType)).Cast<ActivityType>())
                {
                    var allocation = TotalActivityTimeAllocation(activityType);
                    if (allocation > 0)
                        allocations.Add(activityType, allocation);
                }

                return allocations;
            }
        }

        public void AddActivity(Activity activity)
        {
            _activities.Add(activity);
            ActivitiesChanged.Raise(this);
        }

        public void RemoveActivity(Activity activity)
        {
            _activities.Remove(activity);
            ActivitiesChanged.Raise(this);
        }

        private TimeSpan TotalActivityLength(ActivityType type)
        {
            return TimeSpan.FromSeconds(Activities.Where(activity => activity.Type == type).Sum(activity => activity.Length.TotalSeconds));
        }

        private double TotalActivityTimeAllocation(ActivityType type)
        {
            return TotalActivityLength(type).TotalSeconds / TotalLength.TotalSeconds;
        }
    }
}
