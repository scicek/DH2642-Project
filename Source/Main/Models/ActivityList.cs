namespace Main.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Extensions;

    /// <summary>
    /// Represents a list of activities.
    /// </summary>
    public class ActivityList : NotifyBase
    {
        private readonly List<Activity> _activities;

        public ActivityList()
        {
            _activities = new List<Activity>();
        }

        public ActivityList(List<Activity> activities) : this()
        {
            if (activities == null || !activities.Any()) 
                return;
            
            foreach (var activity in activities)
                AddActivity(activity);
        }

        public event EventHandler<Activity> Added;
        public event EventHandler<Activity> Removed;

        public IEnumerable<Activity> Activities
        {
            get
            {
                return _activities;
            }
        }

        public void AddActivity(Activity activity)
        {
            if (_activities.Contains(activity))
                return;

            _activities.Add(activity);

            Added.Raise(this, activity);
            NotifyPropertyChanged(() => Activities);
        }

        public void RemoveActivity(Activity activity)
        {
            if (!_activities.Contains(activity))
                return;

            _activities.Remove(activity);
            Removed.Raise(this, activity); 
            NotifyPropertyChanged(() => Activities);
        }
    }
}
