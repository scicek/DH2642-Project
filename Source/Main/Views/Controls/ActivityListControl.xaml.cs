namespace Main.Views.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using Models;
    using Models.Extensions;

    /// <summary>
    /// Interaction logic for ActivityListControl.xaml
    /// </summary>
    public partial class ActivityListControl : UserControl
    {
        public static readonly DependencyProperty ActivitiesProperty = DependencyProperty.Register("Activities", typeof(IEnumerable<Activity>), typeof(ActivityListControl), new PropertyMetadata(null));

        public ActivityListControl()
        {
            InitializeComponent();
        }

        public event EventHandler<Activity> Added;
        public event EventHandler<Activity> Removed;

        public IEnumerable<Activity> Activities
        {
            get { return (IEnumerable<Activity>)GetValue(ActivitiesProperty); }
            set { SetValue(ActivitiesProperty, value); }
        }

        private void OnDrop(object sender, DragEventArgs dragEventArgs)
        {
            var data = (Dictionary<string, object>)dragEventArgs.Data.GetData("data");
            var activity = (Activity)data["activity"];
            var source = (ActivityListControl)data["source"];

            if (activity != null && !source.Equals(this))
            {
                dragEventArgs.Effects = DragDropEffects.Move;
                Added.Raise(this, activity);                
            }
            else
            {
                dragEventArgs.Effects = DragDropEffects.None;                
            }

            dragEventArgs.Handled = true;
        }

        private void OnDragOver(object sender, DragEventArgs dragEventArgs)
        {
            dragEventArgs.Effects = DragDropEffects.Move;
            dragEventArgs.Handled = true;
        }

        private void OnActivityMoved(object sender, RoutedEventArgs e)
        {
            Removed.Raise(this, ((ActivityControl)e.OriginalSource).Activity);
        }
    }
}