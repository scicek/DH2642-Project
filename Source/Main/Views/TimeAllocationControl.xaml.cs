namespace Main.Views
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using Models;

    /// <summary>
    /// Interaction logic for TimeAllocationControl.xaml
    /// </summary>
    public partial class TimeAllocationControl : UserControl
    {
        public static readonly DependencyProperty ActivitiesProperty = DependencyProperty.Register("Activities", typeof(Dictionary<ActivityType, double>), typeof(TimeAllocationControl), new PropertyMetadata(null));
        public static readonly DependencyProperty RequiredBreakPercentageProperty = DependencyProperty.Register("RequiredBreakPercentage", typeof(double), typeof(TimeAllocationControl), new PropertyMetadata(0.0));

        public TimeAllocationControl()
        {
            InitializeComponent();
        }

        public Dictionary<ActivityType, double> Activities
        {
            get { return (Dictionary<ActivityType, double>)GetValue(ActivitiesProperty); }
            set { SetValue(ActivitiesProperty, value); }
        }

        public double RequiredBreakPercentage
        {
            get { return (double)GetValue(RequiredBreakPercentageProperty); }
            set { SetValue(RequiredBreakPercentageProperty, value); }
        }
    }
}
