namespace Main.Views
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using Models;
    using Models.Extensions;

    /// <summary>
    /// Interaction logic for DayControl.xaml
    /// </summary>
    public partial class DayControl : UserControl
    {
        public static readonly DependencyProperty DayProperty = DependencyProperty.Register("Day", typeof(Day), typeof(DayControl), new PropertyMetadata(null));

        public DayControl()
        {
            InitializeComponent();
            
        }

        public event EventHandler<Day> DayChanged; 
        
        public Day Day
        {
            get { return (Day)GetValue(DayProperty); }
            set { SetValue(DayProperty, value); }
        }

        private void ActivityListControl_OnAdded(object sender, Activity e)
        {
            Day.AddActivity(e);
            DayChanged.Raise(this, Day);
        }

        private void ActivityListControl_OnRemoved(object sender, Activity e)
        {
            Day.RemoveActivity(e);
            DayChanged.Raise(this, Day);
        }

        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                TimeSpan time;
                if (TimeSpan.TryParse(StartTimeTextBox.Text, out time))
                {
                    Day.StartTime = time;

                    DayChanged.Raise(this, Day);
                }
                else
                    StartTimeTextBox.Text = Day.StartTime.ToString();
            }
        }
    }
}
