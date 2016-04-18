namespace Main.Views
{
    using System;
    using System.Windows;
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
        private void AddNewActivity(object sender, RoutedEventArgs e)
        {
            _mainViewModel.DisplayEditActivityDialog();
        }

        private void OnAddDay(object sender, RoutedEventArgs e)
        {
            _mainViewModel.AddDay();
        }

        private void OnParkedActivityAdded(object sender, Activity e)
        {
            _mainViewModel.AddParkedActivity(e);
        }

        private void OnParkedActivityRemoved(object sender, Activity e)
        {
            _mainViewModel.RemoveParkedActivity(e);
        }
        
        private void OnSaveActivity(object sender, Activity activity)
        {
            _mainViewModel.SaveActivity(activity);
        }

        private void OnCancelActivity(object sender, EventArgs e)
        {
            _mainViewModel.HideEditActivityDialog();
        }

        private void OnEditActivity(object sender, RoutedEventArgs e)
        {
            var activityControl = e.OriginalSource as ActivityControl;
            if (activityControl == null)
                return;

            _mainViewModel.DisplayEditActivityDialog(activityControl.Activity, false);
        }
        #endregion

        private void OnDragOver(object sender, DragEventArgs args)
        {
            args.Effects = DragDropEffects.None;
            args.Handled = true;
        }
    }
}
