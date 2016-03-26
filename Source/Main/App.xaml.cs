namespace Main
{
    using System.Windows;
    using Models;
    using ViewModels;
    using Views;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private MainWindow _mainWindow;
        private MainViewModel _mainViewModel;
        private ParkingLot _parkingLot;
        private MainModel _mainModel;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            _mainModel = new MainModel();
            _parkingLot = new ParkingLot();
            _mainViewModel = new MainViewModel(_parkingLot, _mainModel);
            _mainWindow = new MainWindow(_mainViewModel);

            _mainWindow.Show();
        }
    }
}
