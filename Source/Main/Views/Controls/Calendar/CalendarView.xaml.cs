namespace Main.Views.Controls.Calendar
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using Annotations;
    using Models;
    using Models.Extensions;

    /// <summary>
    /// Interaction logic for CalendarView.xaml
    /// </summary>
    public partial class CalendarView : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty DaysProperty = DependencyProperty.Register("Days", typeof(IEnumerable<Day>), typeof(CalendarView), new PropertyMetadata(null, OnDaysPropertyChanged));
        public static readonly DependencyProperty CurrentDateProperty = DependencyProperty.Register("CurrentDate", typeof (DateTime), typeof (CalendarControl));

        private const int MaxYear = 2100;
        private const int MinYear = 1900;

        private string _selectedMonthName;
        private IDictionary<Date, Day> _dateTimeToDaysMapping;
        private int _selectedMonth;
        private int _selectedYear;
        private CalendarState _currentState;

        public CalendarView()
        {
            CalendarDays = new ObservableCollection<CalendarDay>();
            CalendarDayNames = new ObservableCollection<string> { "Monday",  "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
            Months = new Dictionary<int, string>
            {
                {1, "January"},
                {2, "February"},
                {3, "March"},
                {4, "April"},
                {5, "May"},
                {6, "June"},
                {7, "July"},
                {8, "August"},
                {9, "September"},
                {10, "October"},
                {11, "November"},
                {12, "December"},
            };

            InitializeCalendar(DateTime.Today);
            SelectedYear = Date.Today.Year;
            SelectedMonth = Date.Today.Month;

            InitializeComponent();

            CurrentState = CalendarState.Days;
        }

        public event EventHandler<IEnumerable<Date>> ShowDates;
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<CalendarDay> CalendarDays { get; set; }
        
        public ObservableCollection<string> CalendarDayNames { get; set; }

        public IEnumerable<Day> Days 
        {
            get { return (IEnumerable<Day>)GetValue(DaysProperty); }
            set { SetValue(DaysProperty, value); }
        }

        public IDictionary<Date, Day> DateTimeToDaysMapping
        {
            get { return _dateTimeToDaysMapping; }
            set { _dateTimeToDaysMapping = value; OnPropertyChanged(); }
        }

        public string SelectedMonthName
        {
            get { return _selectedMonthName; }
            private set { _selectedMonthName = value; OnPropertyChanged(); }
        }

        public int SelectedMonth
        {
            get { return _selectedMonth; }
            set
            {
                _selectedMonth = value; 
                OnPropertyChanged();

                SelectedMonthName = ((Month) _selectedMonth).ToString();

                if (CurrentState == CalendarState.Days)
                {
                    Next.Visibility = SelectedMonth < 12 ? Visibility.Visible : Visibility.Hidden;
                    Previous.Visibility = SelectedMonth > 1 ? Visibility.Visible : Visibility.Hidden;
                }

                InitializeCalendar(new DateTime(SelectedYear, SelectedMonth, 1));                
            }
        }

        public int SelectedYear
        {
            get { return _selectedYear; }
            private set
            {
                _selectedYear = value;
                OnPropertyChanged();
            }
        }

        public Dictionary<int, string> Months { get; private set; }

        private CalendarState CurrentState
        {
            get { return _currentState; }
            set
            {
                _currentState = value;

                SelectedMonthText.Visibility = CalendarDaysGrid.Visibility = _currentState == CalendarState.Days ? Visibility.Visible : Visibility.Hidden;
                SelectedYearText.Visibility = CalendarMonthGrid.Visibility = _currentState == CalendarState.Months ? Visibility.Visible : Visibility.Hidden;

                if (_currentState == CalendarState.Months)
                    Previous.Visibility = Next.Visibility = Visibility.Visible;
                else
                {
                    Previous.Visibility = SelectedMonth > 1 ? Visibility.Visible : Visibility.Hidden;
                    Next.Visibility = SelectedMonth < 12 ? Visibility.Visible : Visibility.Hidden;
                }
            }
        }

        private void CalendarDayOnClicked(object sender, MouseButtonEventArgs e)
        {
            var calendarDay = ((FrameworkElement)sender).DataContext as CalendarDay;
            if (calendarDay == null)
                return;

            var date = calendarDay.Date;
            var week = new List<Date>();
            var dayOfTheWeek = date.DayOfWeek;
            var firstDate = date.AddDays(-dayOfTheWeek);

            for (var i = 0; i < 7; i++)
            {
                week.Add(firstDate);
                firstDate = firstDate.AddDays(1);
            }

            ShowDates.Raise(this, week);
        }


        public void InitializeCalendar(DateTime targetDate)
        {
            CalendarDays.Clear();

            // First day of the targeted month.
            var firstDay = new Date(targetDate.Year, targetDate.Month, 1);
            
            // Make sure that each calendar page starts with a monday. 
            var dayOfTheWeek = firstDay.DayOfWeek;
            firstDay = firstDay.AddDays(-dayOfTheWeek);

            // Each calendar page has six weeks (rows). That means that we will show 42 days in total. 
            for (var i = 0; i < 42; i++)
            {
                var calendarDay = new CalendarDay
                {
                    Date = firstDay,
                    IsTargetMonth = targetDate.Month == firstDay.Month,
                    IsToday = firstDay == Date.Today
                };

                CalendarDays.Add(calendarDay);

                // Move to the next day.
                firstDay = firstDay.AddDays(1);
            }
        }

        private void PreviousMonthOrYearOnClick(object sender, MouseButtonEventArgs e)
        {
            if (CurrentState == CalendarState.Days)
                SelectedMonth = Math.Max(Months.First().Key, SelectedMonth - 1);
            else if (CurrentState == CalendarState.Months)
                SelectedYear = Math.Max(MinYear, SelectedYear - 1);
        }

        private void NextMonthOrYearOnClick(object sender, MouseButtonEventArgs e)
        {
            if (CurrentState == CalendarState.Days)
                SelectedMonth = Math.Min(Months.Last().Key, SelectedMonth + 1);
            else if (CurrentState == CalendarState.Months)
                SelectedYear = Math.Min(MaxYear, SelectedYear + 1);
        }

        private void OnSelectMonthOrYearClicked(object sender, MouseButtonEventArgs e)
        {
            CurrentState = CalendarState.Months;
        }

        private void OnMonthClicked(object sender, MouseButtonEventArgs e)
        {
            SelectedMonth = (int)((FrameworkElement) sender).Tag;
            CurrentState = CalendarState.Days;
        }

        private static void OnDaysPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var control = dependencyObject as CalendarView;
            if (control == null)
                return;

            if (control.Days == null)
            {
                control.DateTimeToDaysMapping = null;
                return;
            }

            // Create a mapping between the date and the day.
            control.DateTimeToDaysMapping = control.Days.ToDictionary(day => day.Date, day => day);
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        private enum CalendarState
        {
            Months,
            Days
        }
    }
}
