namespace Main.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using Models;
    using Models.Extensions;

    public class MainViewModel : NotifyBase
    {
        private readonly ActivityList _parkingLot;
        private readonly DayContainer _dayContainer;
        
        private string _months;
        private string _years;

        public MainViewModel(ActivityList parkingLot, DayContainer dayContainer)
        {
            _parkingLot = parkingLot;
            _dayContainer = dayContainer;
            
            ParkedActivities = new ObservableCollection<Activity>();
            foreach (var activity in _parkingLot.Activities)
                ParkedActivities.Add(activity);

            Days = new ObservableCollection<Day>();
            foreach (var day in _dayContainer.Days)
                Days.Add(day);
            
            _parkingLot.Added += (sender, activity) => ParkedActivities.Add(activity);
            _parkingLot.Removed += (sender, activity) => ParkedActivities.Remove(activity);

            EditingState = new ActivityEditingState();
        }

        /// <summary>
        /// The parked activities.
        /// </summary>
        public ObservableCollection<Activity> ParkedActivities { get; set; }

        /// <summary>
        /// The days.
        /// </summary>
        public ObservableCollection<Day> Days { get; set; }

        /// <summary>
        /// The editing state (used to keep track of the edit dialog).
        /// </summary>
        public ActivityEditingState EditingState { get; private set; }
        
        /// <summary>
        /// The months of the days being displayed.
        /// </summary>
        public string Months
        {
            get { return _months; }
            set { _months = value; NotifyPropertyChanged(); }
        }

        /// <summary>
        /// The years of the days being displayed.
        /// </summary>
        public string Years
        {
            get { return _years; }
            set { _years = value; NotifyPropertyChanged(); }
        }

        /// <summary>
        /// Adds the given activity to the parking lot.
        /// </summary>
        /// <param name="activity">The activity.</param>
        public void AddParkedActivity(Activity activity)
        {
            activity.StartTime = null;
            _parkingLot.AddActivity(activity);
        }
        
        /// <summary>
        /// Removes a parked activity.
        /// </summary>
        /// <param name="activity">The activity to remove.</param>
        public void RemoveParkedActivity(Activity activity)
        {
            _parkingLot.RemoveActivity(activity);
        }

        /// <summary>
        /// Displays the edit dialog.
        /// </summary>
        /// <param name="activity">The activity to edit (leave as null to create a new one).</param>
        /// <param name="isNew">Whether or not the activity is new.</param>
        /// <param name="isParked">Whether or not the activity is parked.</param>
        /// <param name="day">The day of the activity to edit.</param>
        public void DisplayEditActivityDialog(Activity activity = null, bool isNew = true, bool isParked = false, Day day = null)
        {
            EditingState.IsEditing = true;
            EditingState.IsNew = isNew;
            EditingState.IsParked = isParked;
            EditingState.Day = day;
            EditingState.Activity = activity ?? new Activity();
        }

        /// <summary>
        /// Hides the edit dialog.
        /// </summary>
        public void HideEditActivityDialog()
        {
            EditingState.IsEditing = false;
            EditingState.IsNew = true;
            EditingState.Day = null;
            EditingState.Activity = new Activity();
        }

        /// <summary>
        /// Saves the given activity.
        /// </summary>
        /// <param name="activity">The activity to save.</param>
        public void SaveActivity(Activity activity)
        {
            // The view doesn't change the actual activity (it creates a new one with updated values) so we edit it here.
            EditingState.Activity.Name = activity.Name;
            EditingState.Activity.Length = activity.Length;
            EditingState.Activity.Type = activity.Type;
            EditingState.Activity.Description = activity.Description;

            // If it's new, add it to the parking lot.
            if (EditingState.IsNew)
                _parkingLot.AddActivity(EditingState.Activity);

            HideEditActivityDialog();
        }

        /// <summary>
        /// Deletes the given activity.
        /// </summary>
        /// <param name="activity">The activity to save.</param>
        public void DeleteActivity(Activity activity)
        {
            if (EditingState.Day != null)
                EditingState.Day.RemoveActivity(activity);
            else if (EditingState.IsParked)
                _parkingLot.RemoveActivity(activity);

            HideEditActivityDialog();
        }

        /// <summary>
        /// Adds the given activity to the given day.
        /// </summary>
        /// <param name="day">The day to add the activity to.</param>
        /// <param name="activity">The activity to add.</param>
        public void AddActivityToDay(Day day, Activity activity)
        {
            day.AddActivity(activity);
            
            // Store the day so that it is persisted.
            if (!_dayContainer.Days.Contains(day))
                _dayContainer.AddDay(day);
        }

        /// <summary>
        /// Removes the given activity from the given day.
        /// </summary>
        /// <param name="day">The day to remove the activity from.</param>
        /// <param name="activity">The activity to remove.</param>
        public void RemoveActivityToDay(Day day, Activity activity)
        {
            day.RemoveActivity(activity);
            
            // Empty day, remove it so that it won't be persisted.
            if(!day.AllActivities.Any())
                _dayContainer.RemoveDay(day);
        }

        /// <summary>
        /// Displays the days of the given dates.
        /// </summary>
        /// <param name="dates">A collection of dates.</param>
        public void DisplayDays(IEnumerable<Date> dates)
        {
            Days.Clear();

            // For every date, try to get the persisted one or just create an empty one.
            foreach (var day in dates.Select(date => _dayContainer.Days.FirstOrDefault(d => d.Date == date) ?? new Day(date) {BeginTime = new TimeSpan(8, 0, 0)}))
                Days.Add(day);

            var months = new HashSet<Month>();
            var years = new HashSet<int>();

            // Seperate the months and years.
            foreach (var day in Days)
            {
                months.Add((Month)day.Date.Month);
                years.Add(day.Date.Year);
            }

            // Build a string of the months being displayed.
            var monthsStringBuilder = new StringBuilder();
            foreach (var month in months)
                monthsStringBuilder.Append(month).Append('/');

            // Build a string of the years being dusplayed.
            var yearsStringBuilder = new StringBuilder();
            foreach (var year in years)
                yearsStringBuilder.Append(year).Append('/');

            // Remove trailing '/'.
            Months = monthsStringBuilder.ToString().TrimEnd('/');
            Years = yearsStringBuilder.ToString().TrimEnd('/');
        }

        /// <summary>
        /// Moves to the previous week.
        /// </summary>
        public void PreviousWeek()
        {
            var firstDay = Days.FirstOrDefault();
            if (firstDay == null)
                return;

            var firstDate = firstDay.Date;

            var dates = new List<Date>();
            for (var i = 7; i > 0; i--)
            {
                dates.Add(firstDate.AddDays(-i));
            }

            DisplayDays(dates);
        }

        /// <summary>
        /// Moves to the next week.
        /// </summary>
        public void NextWeek()
        {
            var lastDay = Days.LastOrDefault();
            if (lastDay == null)
                return;

            var lastDate = lastDay.Date;
            var dates = new List<Date>();
            for (var i = 1; i <= 7; i++)
            {
                dates.Add(lastDate.AddDays(i));
            }

            DisplayDays(dates);
        }

        /// <summary>
        /// Defines an editing state (for editing activities).
        /// </summary>
        public class ActivityEditingState : NotifyBase
        {
            private bool _isEditing;
            private Activity _activity;
            private bool _isNew;
            private Day _day;
            private bool _isParked;

            public ActivityEditingState()
            {
                _activity = new Activity();
            }

            /// <summary>
            /// Whether or not the user is currently editing an activity.
            /// </summary>
            public bool IsEditing
            {
                get { return _isEditing; }
                set
                {
                    _isEditing = value; 
                    NotifyPropertyChanged();
                }
            }

            /// <summary>
            /// Whether or not the activity is new.
            /// </summary>
            public bool IsNew
            {
                get { return _isNew; }
                set
                {
                    _isNew = value; 
                    NotifyPropertyChanged();
                }
            }

            /// <summary>
            /// Whether or not the activity is parked.
            /// </summary>
            public bool IsParked
            {
                get { return _isParked; }
                set
                {
                    _isParked = value;
                    
                    if (_isParked)
                        Day = null;
                    
                    NotifyPropertyChanged();
                }
            }

            /// <summary>
            /// The activity being edited.
            /// </summary>
            public Activity Activity
            {
                get { return _activity; }
                set
                {
                    _activity = value;
                    NotifyPropertyChanged();
                }
            }

            /// <summary>
            /// The day of the activity being edited.
            /// </summary>
            public Day Day
            {
                get { return _day; }
                set
                {
                    _day = value;
                    
                    if (_day != null)
                        IsParked = false;

                    NotifyPropertyChanged();
                }
            }
        }
    }
}
