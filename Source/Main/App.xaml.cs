namespace Main
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using FireSharp;
    using FireSharp.Config;
    using FireSharp.Interfaces;
    using Models;
    using Newtonsoft.Json.Linq;
    using ViewModels;
    using Views;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private MainWindow _mainWindow;
        private MainViewModel _mainViewModel;
        private ActivityList _parkingLot;
        private DayContainer _dayContainer;
        private IFirebaseConfig _config;
        private IFirebaseClient _client;

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            _config = new FirebaseConfig
            {
                AuthSecret = "CspEtEGUeNNHBAx7wW5u1ovygAsnUUZeueSQHfXV",
                BasePath = "https://flickering-fire-5452.firebaseio.com/"
            };

            _client = new FirebaseClient(_config);

            // Show a splash screen
            var splashScreen = new SplashScreen("Resources/SplashScreen.png");
            splashScreen.Show(false, true);

            // Get the stored state
            var response = await _client.GetAsync("ParkedActivities");
            var activities = response.ResultAs<List<Activity>>(); 

            response = await _client.GetAsync("Days");
            var daysAsJson = response.ResultAs<JArray>();
            var days = new List<Day>();

            // Parse the JSON for the stored days and re-create the days before adding them.
            if (daysAsJson != null && daysAsJson.Any())
            {
                foreach (var dayAsJson in daysAsJson)
                {
                    // Parse the fields of the day.
                    var date = dayAsJson["Date"].ToObject<Date>();
                    var beginTime= dayAsJson["BeginTime"].ToObject<TimeSpan>();

                    // Parse the activities of the day and add them.
                    IEnumerable<Activity> allActivities = null;
                    var activitiesToken = dayAsJson["AllActivities"];
                    if (activitiesToken != null)
                        allActivities = activitiesToken.ToObject<IEnumerable<Activity>>();

                    var day = new Day(date) { BeginTime = beginTime };

                    if (allActivities != null)
                        foreach (var activity in allActivities)
                            day.AddActivity(activity);

                    days.Add(day);
                }
            }

            _dayContainer = new DayContainer(days);
            _parkingLot = new ActivityList(activities);
            _mainViewModel = new MainViewModel(_parkingLot, _dayContainer);
            _mainWindow = new MainWindow(_mainViewModel);

            splashScreen.Close(TimeSpan.FromMilliseconds(200));
            _mainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            // Store the state
            _client.Set("ParkedActivities", _parkingLot.Activities);
            _client.Set("Days", _dayContainer.Days);
        }
    }
}
