using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Tengu.Converters
{
    public class EnumToCheckedConverter : IValueConverter
    {
        public Type Type { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value.GetType() == Type)
            {
                try
                {
                    var parameterFlag = Enum.Parse(Type, parameter as string);

                    if (Equals(parameterFlag, value))
                    {
                        return true;
                    }
                }
                catch (ArgumentNullException)
                {
                    return false;
                }
                catch (ArgumentException)
                {
                    throw new NotSupportedException();
                }

                return false;
            }
            else if (value == null)
            {
                return false;
            }

            throw new NotSupportedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value is bool check)
            {
                if (check)
                {
                    try
                    {
                        return Enum.Parse(Type, parameter as string);
                    }
                    catch (ArgumentNullException)
                    {
                        return Binding.DoNothing;
                    }
                    catch (ArgumentException)
                    {
                        return Binding.DoNothing;
                    }
                }

                return Binding.DoNothing;
            }

            throw new NotSupportedException();
        }
    }
}
