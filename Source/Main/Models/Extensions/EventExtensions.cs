namespace Main.Models.Extensions
{
    using System;

    public static class EventExtensions
    {
        public static void Raise(this EventHandler @event, object sender)
        {
            var handler = @event;
            if (handler != null)
                handler.Invoke(sender, EventArgs.Empty);
        }

        public static void Raise<T>(this EventHandler<T> @event, object sender, T args)
        {
            var handler = @event;
            if (handler != null)
                handler.Invoke(sender, args);
        }
    }
}
