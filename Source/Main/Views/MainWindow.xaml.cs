namespace Main.Views
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;
    using Controls;
    using Models;
    using ViewModels;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _mainViewModel;

        public MainWindow(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            DataContext = _mainViewModel;
            InitializeComponent();
            DragOver += OnDragOver;
        }

        #region The following methods are purely piping, binding events from the view to the viewmodel could be done through commands but becomes tedious and is quite ugly.
        // Add a new activity.
        private void AddNewActivity(object sender, RoutedEventArgs e)
        {
            _mainViewModel.DisplayEditActivityDialog();
        }

        // Add a parked activity.
        private void OnParkedActivityAdded(object sender, Activity e)
        {
            _mainViewModel.AddParkedActivity(e);
        }

        // Remove a parked activity.
        private void OnParkedActivityRemoved(object sender, Activity e)
        {
            _mainViewModel.RemoveParkedActivity(e);
        }

        // Save changes to an activity.
        private void OnSaveActivity(object sender, Activity activity)
        {
            _mainViewModel.SaveActivity(activity);
        }

        // Cancel any changes to an activity.
        private void OnCancelActivity(object sender, EventArgs e)
        {
            _mainViewModel.HideEditActivityDialog();
        }

        // Start editing an activity.
        private void OnEditActivity(object sender, RoutedEventArgs e)
        {
            var activityControl = e.OriginalSource as ActivityControl;
            if (activityControl == null)
                return;

            _mainViewModel.DisplayEditActivityDialog(activityControl.Activity, false);
        }

        // An activity was added to a day.
        private void OnActivityAddedToDay(object sender, Activity e)
        {
            var dayControl = sender as DayControl;
            if (dayControl == null)
                return;

            var day = dayControl.Day;
            if (day == null)
                return;

            _mainViewModel.AddActivityToDay(day, e);
            Calendar.Days = null;
            Calendar.Days = _mainViewModel.Days;
        }

        // An activity was removed from a day.
        private void OnActivityRemovedFromDay(object sender, Activity e)
        {
            var dayControl = sender as DayControl;
            if (dayControl == null)
                return;

            var day = dayControl.Day;
            if (day == null)
                return;

            _mainViewModel.RemoveActivityToDay(day, e);
            Calendar.Days = null;
            Calendar.Days = _mainViewModel.Days;
        }

        // Show the days of the given dates.
        private void OnShowDays(object sender, IEnumerable<Date> args)
        {
            WeekGrid.Visibility = Visibility.Visible;
            Calendar.Visibility = Visibility.Hidden;
            _mainViewModel.DisplayDays(args);
        }

        // Show the previous week.
        private void PreviousMonthOnClick(object sender, MouseButtonEventArgs e)
        {
            _mainViewModel.PreviousWeek();
        }

        // Show the next week.
        private void NextMonthOnClick(object sender, MouseButtonEventArgs e)
        {
            _mainViewModel.NextWeek();
        }

        // Display the calendar.
        private void OnDateClicked(object sender, MouseButtonEventArgs e)
        {
            WeekGrid.Visibility = Visibility.Hidden;
            Calendar.Visibility = Visibility.Visible;

            // A little bit of code to make sure that the calendar displays the month containing the current week.
            var lastDay = _mainViewModel.Days.LastOrDefault();
            if (lastDay != null)
                Calendar.SelectedMonth = lastDay.Date.Month;
        }

        // hide/Show the parking lot.
        private void ParkingLotExpanderOnClick(object sender, MouseButtonEventArgs e)
        {
            if (ParkingLot.IsVisible)
            {
                ParkingLot.Visibility = Visibility.Collapsed;
                ParkingLotExpander.Text = ">";
            }
            else
            {
                ParkingLot.Visibility = Visibility.Visible;
                ParkingLotExpander.Text = "<";
            }
        }
        #endregion

        // Something was dragged over the window.
        private void OnDragOver(object sender, DragEventArgs args)
        {
            args.Effects = DragDropEffects.None;
            args.Handled = true;
        }
    }
}
