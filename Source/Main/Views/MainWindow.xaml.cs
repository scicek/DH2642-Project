namespace Main.Views
{
    using System.Windows;
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
        }
        
        private void AddActivity(object sender, RoutedEventArgs e)
        {
            _mainViewModel.IsAddingActivity = true;
        }

        private void OnAddActivityCancel(object sender, RoutedEventArgs e)
        {
            _mainViewModel.IsAddingActivity = false;
        }

        private void OnAddActivitySave(object sender, RoutedEventArgs e)
        {
            _mainViewModel.IsAddingActivity = false;
        }
    }
}
