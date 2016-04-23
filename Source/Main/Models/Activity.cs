namespace Main.Models
{
    using System;

    /// <summary>
    /// Represents an activity.
    /// </summary>
    public class Activity : NotifyBase
    {
        private string _name;
        private TimeSpan? _startTime;
        private TimeSpan _length;
        private ActivityType _type;
        private string _description;

        public Activity() { }

        public Activity(string name, TimeSpan length, ActivityType type, string description)
        {
            Name = name;
            Length = length;
            Type = type;
            Description = description;
        }

        /// <summary>
        /// The name of the activity.
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; NotifyPropertyChanged(); }
        }

        /// <summary>
        /// The start time of the activity.
        /// </summary>
        public TimeSpan? StartTime
        {
            get { return _startTime; }
            set
            {
                _startTime = value; 
                NotifyPropertyChanged();
                NotifyPropertyChanged("EndTime");
            }
        }

        /// <summary>
        /// The length of the activity.
        /// </summary>
        public TimeSpan Length
        {
            get { return _length; }
            set 
            { 
                _length = value; 
                NotifyPropertyChanged();
                NotifyPropertyChanged("EndTime");
            }
        }

        /// <summary>
        /// The end time of the activity.
        /// </summary>
        public TimeSpan? EndTime
        {
            get
            {
                if (_startTime == null)
                    return null;

                return _startTime.Value + Length;
            }
        }

        /// <summary>
        /// The type of the activity.
        /// </summary>
        public ActivityType Type
        {
            get { return _type; }
            set { _type = value; NotifyPropertyChanged(); }
        }

        /// <summary>
        /// The description of the activity.
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { _description = value; NotifyPropertyChanged(); }
        }
    }
}
