namespace Main.Models
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Represents a date.
    /// </summary>
    public class Date
    {
        private DateTime _date;

        public Date(DateTime dateTime)
        {
            _date = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day);
        }

        [JsonConstructor]
        public Date(int year, int month, int day)
        {
            _date = new DateTime(year, month, day);
        }

        public int Year { get { return _date.Year; } }

        public int Month { get { return _date.Month; } }

        public int Day { get { return _date.Day; } }

        public bool IsToday { get { return _date == DateTime.Today; } }

        public int DayOfWeek
        {
            get
            {
                // DayOfWeek starts with sunday, we want to start the week on a monday.
                switch (_date.DayOfWeek)
                {
                    case System.DayOfWeek.Monday:
                        return 0;
                    case System.DayOfWeek.Tuesday:
                        return 1;
                    case System.DayOfWeek.Wednesday:
                        return 2;
                    case System.DayOfWeek.Thursday:
                        return 3;
                    case System.DayOfWeek.Friday:
                        return 4;
                    case System.DayOfWeek.Saturday:
                        return 5;
                    case System.DayOfWeek.Sunday:
                        return 6;
                    default:
                        return -1;
                }
            }
        }

        public int WeekOfYear
        {
            get
            {
                // Little bit of magic to get the correct week number (ISO 8601). 
                var day = (int)CultureInfo.CurrentCulture.Calendar.GetDayOfWeek(_date);
                return CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(_date.AddDays(4 - (day == 0 ? 7 : day)), CalendarWeekRule.FirstFourDayWeek, System.DayOfWeek.Monday);
            }
        }

        public static Date Today { get { return new Date(DateTime.Today); } }

        /// <summary>
        /// Uses the holiday.com API to get the name of the holiday (if the date is a holiday). 
        /// </summary>
        /// <returns>If the date is a holiday, returns the name of the holiday.</returns>
        public async Task<string> GetHoliday()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://holidayapi.com/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = await client.GetAsync("v1/holidays?country=US&year=" + _date.Year + "&month=" + _date.Month + "&day=" + _date.Day).ConfigureAwait(false);

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
                return null;
            }
        }

        public static bool operator < (Date c1, Date c2)
        {
            return c1._date < c2._date;
        }

        public static bool operator > (Date c1, Date c2)
        {
            return c1._date > c2._date;
        }

        public static bool operator == (Date c1, Date c2)
        {
            return c1._date == c2._date;
        }

        public static bool operator != (Date c1, Date c2)
        {
            return c1._date != c2._date;
        }

        public override string ToString()
        {
            return Year + "-" + PaddedNumber(Month) + "-" + PaddedNumber(Day);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Date)obj);
        }

        public override int GetHashCode()
        {
            return _date.GetHashCode();
        }

        protected bool Equals(Date other)
        {
            return _date.Equals(other._date);
        }

        private string PaddedNumber(int number)
        {
            if (number >= 10)
                return "" + number;
            
            return "0" + number;
        }
    }
}
