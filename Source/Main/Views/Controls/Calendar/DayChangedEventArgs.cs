namespace Main.Views.Controls.Calendar
{
    using System;

    public class DayChangedEventArgs : EventArgs
    {
        public CalendarDay CalendarDay { get; private set; }

        public DayChangedEventArgs(CalendarDay calendarDay)
        {
            this.CalendarDay = calendarDay;
        }
    }
}