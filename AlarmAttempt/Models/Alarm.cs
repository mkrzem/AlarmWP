using GalaSoft.MvvmLight;
using System;
using System.ComponentModel;
using Windows.Storage;

namespace AlarmAttempt.Models
{
    public class Alarm : ObservableObject
    {
        public int Id { get; set; }
        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                RaisePropertyChanged(nameof(Name));
            }
        }
        private TimeSpan time;
        public TimeSpan Time
        {
            get { return time; }
            set
            {
                time = value;
                RaisePropertyChanged(nameof(Time));
            }
        }
        public bool IsOn { get; set; }

        [DefaultValue(5)]
        public int Volume { get; set; }
        public bool Nap { get; set; }
        public int NapDuration { get; set; }
        public Enums.StartDays Repetition { get; set; }
        public Uri Sound { get; set; }
        public Uri Background { get; set; }
    }
}
