namespace Main.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Represents a day.
    /// </summary>
    public class Day : NotifyBase, IComparable<Day>
    {
        private TimeSpan _beginTime;
        private readonly ActivityList _activityList;

        public Day(Date date)
        {
            Date = date;
            _activityList = new ActivityList();
            _activityList.PropertyChanged += (sender, args) =>
            {
                NotifyPropertyChanged(() => TimeAllocations);
                NotifyPropertyChanged(() => AllActivities);
                NotifyPropertyChanged(() => EndTime);
                NotifyPropertyChanged(() => TotalLength);
            };
        }

        public Date Date { get; private set; }

        public TimeSpan BeginTime
        {
            get { return _beginTime; }
            set
            {
                _beginTime = value;
                RecalculateActivityStartTimes();
                NotifyPropertyChanged();
                NotifyPropertyChanged(() => EndTime);
            }
        }

        public TimeSpan EndTime
        {
            get
            {
                return BeginTime + TotalLength; 
            }
        }

        public TimeSpan TotalLength
        {
            get
            {
                var length = new TimeSpan();
                return Enum.GetValues(typeof (ActivityType)).Cast<ActivityType>().Aggregate(length, (current, activity) => current.Add(TotalActivityLength(activity)));
            }
        }

        public IEnumerable<Activity> AllActivities
        {
            get { return _activityList.Activities; }
        }

        [JsonIgnore]
        public Dictionary<ActivityType, double> TimeAllocations
        {
            get
            {
                var allocations = new Dictionary<ActivityType, double>();

                foreach (var activityType in Enum.GetValues(typeof(ActivityType)).Cast<ActivityType>())
                {
                    var allocation = TotalActivityTimeAllocation(activityType);
                    if (allocation > 0)
                        allocations.Add(activityType, allocation);
                }

                return allocations;
            }
        }
        
        public void AddActivity(Activity activity)
        {
            if (!AllActivities.Any())
                activity.StartTime = BeginTime;
            else
            {
                var last = AllActivities.Last();
                activity.StartTime = last.StartTime + last.Length;
            }

            activity.PropertyChanged += OnActivityChanged;
            _activityList.AddActivity(activity);
        }

        public void RemoveActivity(Activity activity)
        {
            activity.PropertyChanged -= OnActivityChanged;
            _activityList.RemoveActivity(activity);
            RecalculateActivityStartTimes();
        }

        public async Task<string> GetHoliday()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://holidayapi.com/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = await client.GetAsync("v1/holidays?country=US&year=" + Date.Year + "&month=" + Date.Month + "&day=" + Date.Day).ConfigureAwait(false);

                    if (response.IsSuccessStatusCode)
                    {
                        var results = await response.Content.ReadAsStringAsync();
                        dynamic parsedResults = JObject.Parse(results);

                        var holidays = parsedResults.holidays as JArray;
                        if (holidays == null || !holidays.Any())
                            return string.Empty;

                        return holidays.First["name"].ToString();
                    }

                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public int CompareTo(Day other)
        {
            if (Date < other.Date)
                return -1;

            if (Date > other.Date)
                return 1;

            return 0;
        }

        private TimeSpan TotalActivityLength(ActivityType type)
        {
            return TimeSpan.FromSeconds(_activityList.Activities.Where(activity => activity.Type == type).Sum(activity => activity.Length.TotalSeconds));
        }

        private double TotalActivityTimeAllocation(ActivityType type)
        {
            return TotalActivityLength(type).TotalSeconds / TotalLength.TotalSeconds;
        }

        // Goes through the list of activities and updates their start time (makes sure that an activity always starts when the previous one ends).
        private void RecalculateActivityStartTimes()
        {
            if (AllActivities.Any())
            {
                var start = BeginTime;
                var length = TimeSpan.Zero;

                foreach (var activity in AllActivities)
                {
                    activity.StartTime = start + length;

                    start = activity.StartTime.Value;
                    length = activity.Length;
                }
            }
        }
        
        private void OnActivityChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            var activity = sender as Activity;

            if (activity == null)
                return;

            // The length of the activity changed, we need to recalculate the start times of the activities and update the end time and total length of the day.
            if (propertyChangedEventArgs.PropertyName == Utilities.GetPropertyName(() => activity.Length))
            {
                NotifyPropertyChanged(() => EndTime);
                NotifyPropertyChanged(() => TotalLength);
                RecalculateActivityStartTimes();
            }
            // The type changed, update the time allocations.
            else if (propertyChangedEventArgs.PropertyName == Utilities.GetPropertyName(() => activity.Type)) 
                NotifyPropertyChanged(() => TimeAllocations);
        }
    }
}
