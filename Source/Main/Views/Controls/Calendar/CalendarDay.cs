namespace Main.Views.Controls.Calendar
{
    using Models;

    public class CalendarDay : NotifyBase
    {
        private Date _date;
        private bool _isTargetMonth;
        private bool _isToday;
        
        public bool IsToday
        {
            get { return _isToday; }
            set
            {
                _isToday = value;
                NotifyPropertyChanged();
            }
        }

        public bool IsTargetMonth
        {
            get { return _isTargetMonth; }
            set
            {
                _isTargetMonth = value;
                NotifyPropertyChanged();
            }
        }

        public Date Date
        {
            get { return _date; }
            set
            {
                _date = value;
                NotifyPropertyChanged();
            }
        }
    }
}