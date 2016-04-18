namespace Main.Views.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using Models;

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
            Day.AddActivity(e);
        }

        private void ActivityListControl_OnRemoved(object sender, Activity e)
        {
            Day.RemoveActivity(e);
        }

        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                TimeSpan time;
                if (TimeSpan.TryParse(StartTimeTextBox.Text, out time))
                {
                    Day.BeginTime = time;
                }
                else
                    StartTimeTextBox.Text = Day.BeginTime.ToString();
            }
        }

        private static async void PropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var control = dependencyObject as DayControl;
            if (control == null)
                return;

            if (control.Day != null)
            {
                control.Holiday = await control.Day.GetHoliday();

                control.Day.PropertyChanged += (sender, args) =>
                {
                    if (args.PropertyName == Utilities.GetPropertyName(() => control.Day.AllActivities))
                    {
                        control.ActivityListControl.Activities = null;
                        control.ActivityListControl.Activities = control.Day.AllActivities;
                    }
                };
            }
        }
    }
}
