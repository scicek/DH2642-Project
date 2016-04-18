namespace Main.Views.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using Models;
    using Models.Extensions;

    /// <summary>
    /// Interaction logic for ActivityInformationControl.xaml
    /// </summary>
    public partial class ActivityInformationControl : UserControl
    {
        public static readonly DependencyProperty ActivityProperty = DependencyProperty.Register("Activity", typeof(Activity), typeof(ActivityInformationControl), new PropertyMetadata(new Activity(), PropertyChangedCallback));
        private readonly Dictionary<ActivityType, string> _activityTypes;

        public ActivityInformationControl()
        {
            _activityTypes = new Dictionary<ActivityType, string>
            {
                {ActivityType.Presentation, "Presentation"},
                {ActivityType.Discussion, "Discussion"},
                {ActivityType.GroupWork, "Group Work"},
                {ActivityType.Break, "Break"}
            };

            InitializeComponent();

            LengthTextBox.Text = Activity.Length.ToString();
        }

        public event EventHandler Cancel;

        public event EventHandler<Activity> Save;

        public Dictionary<ActivityType, string> ActivityTypes { get { return _activityTypes; } }

        public Activity Activity
        {
            get { return (Activity)GetValue(ActivityProperty); }
            set { SetValue(ActivityProperty, value); }
        }

        private static void PropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var control = dependencyObject as ActivityInformationControl;
            if (control == null)
                return;

            control.ActivityNameTextBox.Text = control.Activity != null ? control.Activity.Name : null;
            control.LengthTextBox.Text = control.Activity != null ? control.Activity.Length.ToString() : null;
            control.ActivityTypeComboBox.SelectedValue = control.Activity != null ? control.Activity.Type : ActivityType.Presentation;
            control.ActivityDescriptionTextBox.Text = control.Activity != null ? control.Activity.Description : null;
        }

        private void OnAddActivityCancel(object sender, RoutedEventArgs e)
        {
            ErrorText.Visibility = Visibility.Hidden;
            Clear();
            Cancel.Raise(this);
        }

        private void OnAddActivitySave(object sender, RoutedEventArgs e)
        {
            TimeSpan length;
            TimeSpan.TryParse(LengthTextBox.Text, out length);
            var name = ActivityNameTextBox.Text;
            var type = (ActivityType)ActivityTypeComboBox.SelectedValue;
            var description = ActivityDescriptionTextBox.Text;

            if (string.IsNullOrWhiteSpace(name) || length.TotalSeconds <= 0)
                ErrorText.Visibility = Visibility.Visible;
            else
            {
                ErrorText.Visibility = Visibility.Hidden;
                Clear();
                Save.Raise(this, new Activity(name, length, type, description));
            }
        }

        private void LengthTextBox_OnPreviewLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            TimeSpan time;
            if (!TimeSpan.TryParse(LengthTextBox.Text, out time))
                LengthTextBox.Text = Activity.Length.ToString();
        }

        private void Clear()
        {
            ActivityNameTextBox.Text = null;
            LengthTextBox.Text = null;
            ActivityTypeComboBox.SelectedValue = ActivityType.Presentation;
            ActivityDescriptionTextBox.Text = null;
        }
    }
}
