namespace Main.Views.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using Models;
    using Models.Extensions;

    /// <summary>
    /// Interaction logic for DayControl.xaml
    /// </summary>
    public partial class DayControl : UserControl
    {
        public static readonly DependencyProperty DayProperty = DependencyProperty.Register("Day", typeof(Day), typeof(DayControl), new PropertyMetadata(null, PropertyChangedCallback));
        public static readonly DependencyProperty HolidayProperty = DependencyProperty.Register("Holiday", typeof(string), typeof(DayControl), new PropertyMetadata(null));

        public DayControl()
        {
            InitializeComponent();
        }

        public event EventHandler<Activity> ActivityAdded;
        public event EventHandler<Activity> ActivityRemoved;

        public Day Day
        {
            get { return (Day)GetValue(DayProperty); }
            set { SetValue(DayProperty, value); }
        }

        public string Holiday
        {
            get { return (string)GetValue(HolidayProperty); }
            set { SetValue(HolidayProperty, value); }
        }

        private void ActivityListControl_OnAdded(object sender, Activity e)
        {
            ActivityAdded.Raise(this, e);
        }

        private void ActivityListControl_OnRemoved(object sender, Activity e)
        {
            ActivityRemoved.Raise(this, e);
        }

        private static async void PropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var control = dependencyObject as DayControl;
            if (control == null)
                return;

            if (control.Day != null)
            {
                control.Holiday = await control.Day.Date.GetHoliday();

                control.Day.PropertyChanged += (sender, args) =>
                {
                    if (args.PropertyName == Utilities.GetPropertyName(() => control.Day.AllActivities))
                    {
                        // Force the binding to update.
                        control.ActivityListControl.Activities = null;
                        control.ActivityListControl.Activities = control.Day.AllActivities;
                    }
                };

                control.StartTime.Value = control.StartTime.DefaultValue + control.Day.BeginTime;
            }
        }

        private void OnBeginTimeChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (Day == null)
                return;

            TimeSpan time;
            if (TimeSpan.TryParse(StartTime.Text, out time))
                Day.BeginTime = time;
            else
                StartTime.Text = Day.BeginTime.ToString();
        }
    }
}
