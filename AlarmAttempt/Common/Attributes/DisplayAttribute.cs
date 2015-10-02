using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlarmAttempt.Common.Attributes
{
    [AttributeUsage(AttributeTargets.All)]
    public class DisplayAttribute : Attribute
    {
        public string Name { get; set; }

        public DisplayAttribute()
        {
        }
    }
}
