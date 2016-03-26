namespace Main.Views
{
    using System;
    using System.Windows;
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

        private void AddNewActivity(object sender, RoutedEventArgs e)
        {
            AddActivityPopup.Visibility = Visibility.Visible;
        }

        private void OnAddActivityCancel(object sender, RoutedEventArgs e)
        {
            _mainViewModel.CancelAddingActivity();
            AddActivityPopup.Visibility = Visibility.Hidden;
            ErrorText.Visibility = Visibility.Hidden;
        }

        private void OnAddActivitySave(object sender, RoutedEventArgs e)
        {
            if (_mainViewModel.SaveActivity())
            {
                AddActivityPopup.Visibility = Visibility.Hidden;
                ErrorText.Visibility = Visibility.Hidden;
            }
            else
            {
                ErrorText.Visibility = Visibility.Visible;
            }
        }

        private void OnAddDay(object sender, RoutedEventArgs e)
        {
            _mainViewModel.AddDay();
        }

        private void OnActivityAdded(object sender, Activity e)
        {
            _mainViewModel.AddActivity(e);
        }

        private void OnActivityRemoved(object sender, Activity e)
        {
            _mainViewModel.RemoveActivity(e);
        }

        private void OnDragOver(object sender, DragEventArgs args)
        {
            args.Effects = DragDropEffects.None;
            args.Handled = true;
        }

        private void OnDayChanged(object sender, Day e)
        {
            _mainViewModel.UpdateDay(e);
        }
    }
}
