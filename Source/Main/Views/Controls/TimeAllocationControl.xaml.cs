namespace Main.Views.Controls
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using Models;

    /// <summary>
    /// Interaction logic for TimeAllocationControl.xaml
    /// </summary>
    public partial class TimeAllocationControl : UserControl
    {
        public static readonly DependencyProperty ActivityAllocationsProperty = DependencyProperty.Register("ActivityAllocations", typeof(Dictionary<ActivityType, double>), typeof(TimeAllocationControl), new FrameworkPropertyMetadata(null, CoerceValueCallback));
        public static readonly DependencyProperty RequiredBreakPercentageProperty = DependencyProperty.Register("RequiredBreakPercentage", typeof(double), typeof(TimeAllocationControl), new PropertyMetadata(0.0));

        public TimeAllocationControl()
        {
            InitializeComponent();
        }

        public Dictionary<ActivityType, double> ActivityAllocations
        {
            get { return (Dictionary<ActivityType, double>)GetValue(ActivityAllocationsProperty); }
            set { SetValue(ActivityAllocationsProperty, value); }
        }

        public double RequiredBreakPercentage
        {
            get { return (double)GetValue(RequiredBreakPercentageProperty); }
            set { SetValue(RequiredBreakPercentageProperty, value); }
        }

        // Coorce the order of the allocations.
        private static object CoerceValueCallback(DependencyObject dependencyObject, object baseValue)
        {
            var allocations = baseValue as Dictionary<ActivityType, double>;
            if (allocations == null)
                return null;

            var coercedValue = new Dictionary<ActivityType, double>();

            new List<ActivityType> { ActivityType.Break, ActivityType.GroupWork, ActivityType.Discussion, ActivityType.Presentation }.ForEach(activityType =>
            {
                if (allocations.ContainsKey(activityType))
                    coercedValue.Add(activityType, allocations[activityType]);
            });

            return coercedValue;
        }
    }
}
