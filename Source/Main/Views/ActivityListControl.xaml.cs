namespace Main.Views
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
        public static readonly DependencyProperty ActivitiesProperty = DependencyProperty.Register("Activities", typeof(ICollection<Activity>), typeof(ActivityListControl), new PropertyMetadata(null));

        public ActivityListControl()
        {
            InitializeComponent();
            AllowDrop = true;
            Drop += OnDrop;
            DragOver += OnDragOver;
        }

        public event EventHandler<Activity> Added;
        public event EventHandler<Activity> Removed;

        public ICollection<Activity> Activities
        {
            get { return (ICollection<Activity>)GetValue(ActivitiesProperty); }
            set { SetValue(ActivitiesProperty, value); }
        }

        private void OnDrop(object sender, DragEventArgs dragEventArgs)
        {
            var data = (Dictionary<string, object>)dragEventArgs.Data.GetData("data");
            var activity = (Activity)data["activity"];
            var source = (ActivityListControl)data["source"];

            if (activity != null && !source.Equals(this))
            {
                Added.Raise(this, activity);
                dragEventArgs.Effects = DragDropEffects.Move;                
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

        private void ActivityControl_OnMoved(object sender, RoutedEventArgs e)
        {
            Removed.Raise(this, ((ActivityControl)e.OriginalSource).Activity);
        }
    }
}