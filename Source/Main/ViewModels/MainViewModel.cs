namespace Main.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using Annotations;
    using Views;

    public class MainViewModel : INotifyPropertyChanged
    {
        private Dictionary<ActivityType, string> _activityTypes;
        private ActivityType _selectedActivityType;
        private bool _isAddingActivity;

        public MainViewModel()
        {
            _activityTypes = new Dictionary<ActivityType, string>();
            _activityTypes.Add(ActivityType.Presentation, "Presentation");
            _activityTypes.Add(ActivityType.Discussion, "Discussion");
            _activityTypes.Add(ActivityType.GroupWork, "Group Work");
            _activityTypes.Add(ActivityType.Break, "Break");
        }

        public Dictionary<ActivityType, string> ActivityTypes
        {
            get
            {
                return _activityTypes;
            }
        }

        public ActivityType SelectedActivityType
        {
            get { return _selectedActivityType; }
            set { _selectedActivityType = value; NotifyPropertyChanged();}
        }

        public bool IsAddingActivity
        {
            get { return _isAddingActivity; }
            set { _isAddingActivity = value; NotifyPropertyChanged();}
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
