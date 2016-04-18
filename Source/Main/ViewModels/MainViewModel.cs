namespace Main.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using Models;

    public class MainViewModel : NotifyBase
    {
        private readonly ActivityList _parkingLot;
        private readonly DayContainer _dayContainer;

        public MainViewModel(ActivityList parkingLot, DayContainer dayContainer)
        {
            _parkingLot = parkingLot;
            _dayContainer = dayContainer;
            
            ParkedActivities = new ObservableCollection<Activity>();
            foreach (var activity in _parkingLot.Activities)
                ParkedActivities.Add(activity);

            Days = new ObservableCollection<Day>();
            foreach (var day in _dayContainer.Days)
            {
                Days.Add(day);
            }

            _parkingLot.Added += (sender, activity) => ParkedActivities.Add(activity);
            _parkingLot.Removed += (sender, activity) => ParkedActivities.Remove(activity);

            _dayContainer.Added += (sender, day) => Days.Add(day);
            _dayContainer.Removed += (sender, day) => Days.Remove(day);

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
        /// Adds a oarked activity.
        /// </summary>
        /// <param name="activity">The activity to add.</param>
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
        /// Adds a new day (the date will be bumped by one day from the previous/last day).
        /// </summary>
        public void AddDay()
        {
            _dayContainer.AddNewDay();
        }
        
        /// <summary>
        /// Displays the edit dialog.
        /// </summary>
        /// <param name="activity">The activity to edit (leave as null to create a new one).</param>
        /// <param name="isNew">Whether or not the activity is new.</param>
        public void DisplayEditActivityDialog(Activity activity = null, bool isNew = true)
        {
            EditingState.IsEditing = true;
            EditingState.IsNew = isNew;
            EditingState.Activity = activity ?? new Activity();
        }

        /// <summary>
        /// Clears the edit dialog.
        /// </summary>
        public void HideEditActivityDialog()
        {
            EditingState.IsEditing = false;
            EditingState.IsNew = true;
            EditingState.Activity = new Activity();
        }

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
        /// Defines an editing state (for editing activities).
        /// </summary>
        public class ActivityEditingState : NotifyBase
        {
            private bool _isEditing;
            private Activity _activity;

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
            public bool IsNew { get; set; }

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
        }
    }
}
