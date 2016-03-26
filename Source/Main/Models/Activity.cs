namespace Main.Models
{
    using System;

    public class Activity
    {
        public Activity(string name, TimeSpan length, ActivityType type, string description)
        {
            Name = name;
            Length = length;
            Type = type;
            Description = description;
        }

        public string Name { get; set; }
        public TimeSpan Length { get; set; }
        public ActivityType Type { get; set; }
        public string Description { get; set; }
    }
}
