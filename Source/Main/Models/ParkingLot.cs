namespace Main.Models
{
    using System;
    using System.Collections.Generic;
    using Extensions;

    public class ParkingLot
    {
        private readonly List<Activity> _activities;
 
        public ParkingLot()
        {
            _activities = new List<Activity>();   
            _activities.Add(new Activity("mm", new TimeSpan(0,45,0), ActivityType.Discussion, ""));
        }

        public event EventHandler ActivitiesChanged;

        public IEnumerable<Activity> Activities
        {
            get
            {
                return _activities;
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
    }
}
