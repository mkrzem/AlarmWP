using AlarmAttempt.Common.Attributes;
using System;
using System.ComponentModel;

namespace AlarmAttempt.Models
{
    public class Enums
    {
        [Flags]
        public enum StartDays
        {
            //[Display(Name = "Codziennie")]
            //Everyday = 1,
            //[Display(Name = "Dni Robocze")]
            //WorkDays = 2,
            [Display(Name = "Poniedziałek")]
            Monday = 4,
            [Display(Name = "Wtorek")]
            Tuesday = 8,
            [Display(Name = "Środa")]
            Wednesday = 16,
            [Display(Name = "Czwartek")]
            Thursday = 32,
            [Display(Name = "Piątek")]
            Friday = 64,
            [Display(Name = "Sobota")]
            Saturday = 128,
            [Display(Name = "Niedziela")]
            Sunday = 256
        }
    }
}
