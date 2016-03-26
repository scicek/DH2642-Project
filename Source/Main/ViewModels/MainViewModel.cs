namespace Main.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Models;

    public class MainViewModel : NotifyBase
    {
        private readonly ParkingLot _parkingLot;
        private readonly MainModel _mainModel;
        private Dictionary<ActivityType, string> _activityTypes;
        private ActivityType _selectedActivityType;
        private bool _isAddingActivity;
        private IEnumerable<Activity> _activities;
        private string _activityName;
        private string _activityLength;
        private ActivityType _activityType;
        private string _activityDescription;

        public MainViewModel(ParkingLot parkingLot, MainModel mainModel)
        {
            _parkingLot = parkingLot;
            _mainModel = mainModel;
            ClearActivity();
            _activityTypes = new Dictionary<ActivityType, string>
            {
                {ActivityType.Presentation, "Presentation"},
                {ActivityType.Discussion, "Discussion"},
                {ActivityType.GroupWork, "Group Work"},
                {ActivityType.Break, "Break"}
            };

            Activities = new ObservableCollection<Activity>();
            foreach (var activity in _parkingLot.Activities)
                Activities.Add(activity);

            Days = new ObservableCollection<Day>();
            foreach (var day in _mainModel.Days)
                Days.Add(day);

            _parkingLot.ActivitiesChanged += (sender, args) =>
            {
                Activities.Clear();
                foreach (var activity in _parkingLot.Activities)
                    Activities.Add(activity);
            };

            _mainModel.DaysChanged += (sender, args) =>
            {
                Days.Clear();
                foreach (var day in _mainModel.Days)
                    Days.Add(day);
            };
        }

        public ObservableCollection<Activity> Activities { get; set; }

        public ObservableCollection<Day> Days { get; set; }

        public Dictionary<ActivityType, string> ActivityTypes { get { return _activityTypes; } }

        public string ActivityName
        {
            get { return _activityName; }
            set { _activityName = value; NotifyPropertyChanged(); }
        }

        public string ActivityLength
        {
            get { return _activityLength; }
            set { _activityLength = value; NotifyPropertyChanged(); }
        }

        public ActivityType ActivityType
        {
            get { return _activityType; }
            set { _activityType = value; NotifyPropertyChanged(); }
        }

        public string ActivityDescription
        {
            get { return _activityDescription; }
            set { _activityDescription = value; NotifyPropertyChanged(); }
        }
        
        public bool SaveActivity()
        {
            TimeSpan time;
            TimeSpan.TryParse(ActivityLength, out time);

            if (!string.IsNullOrWhiteSpace(ActivityName) && time.TotalSeconds > 0)
            {
                _parkingLot.AddActivity(new Activity(ActivityName, time, ActivityType, ActivityDescription));
                ClearActivity();

                return true;
            }

            return false;
        }

        public void CancelAddingActivity()
        {
            ClearActivity();
        }

        public void AddDay()
        {
            _mainModel.AddNewDay();
        }

        public void AddActivity(Activity activity)
        {
            _parkingLot.AddActivity(activity);
        }

        public void RemoveActivity(Activity activity)
        {
            _parkingLot.RemoveActivity(activity);
        }

        public void UpdateDay(Day day)
        {
            _mainModel.UpdateDay(day.Date, day);
        }

        private void ClearActivity()
        {
            ActivityName = string.Empty;
            ActivityLength = new TimeSpan(0, 0, 0).ToString();
            ActivityType = ActivityType.Presentation;
            ActivityDescription = string.Empty;
        }
    }
}
