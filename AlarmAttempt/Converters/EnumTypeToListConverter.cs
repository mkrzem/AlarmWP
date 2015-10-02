using AlarmAttempt.Common.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Windows.UI.Xaml.Data;

namespace AlarmAttempt.Converters
{
    public class EnumTypeToListConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
            {
                return null;
            }

            Type enumType = value as Type;
            if(enumType == null || !enumType.GetTypeInfo().IsEnum)
            {
                return null;
            }
                        
            var sourceItems = (from field in enumType.GetRuntimeFields()
                               let displayAttr = field.GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute
                               let name = displayAttr != null ? displayAttr.Name : field.ToString()
                               where displayAttr != null
                               select name).ToList();

            return sourceItems;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
