namespace Caitlyn.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using Catel.Windows.Data.Converters;
    using Models;

    public class ProjectItemTypeToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ProjectItemType)
            {
                var projectItemType = (ProjectItemType) value;
                switch (projectItemType)
                {
                    case ProjectItemType.Folder:
                        return "/Caitlyn;component/resources/images/folder.png";

                    case ProjectItemType.File:
                        return "/Caitlyn;component/resources/images/file.png";

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ConverterHelper.DoNothingBindingValue;
        }
    }
}