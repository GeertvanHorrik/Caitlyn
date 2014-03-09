// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProjectItemTypeToImageSourceConverter.cs" company="Caitlyn development team">
//   Copyright (c) 2008 - 2013 Caitlyn development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Caitlyn.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    using Caitlyn.Models;

    using Catel.Windows.Data.Converters;

    public class ProjectItemTypeToImageSourceConverter : ValueConverterBase
    {
        #region IValueConverter Members
        protected override object Convert(object value, Type targetType, object parameter)
        {
            if (value is ProjectItemType)
            {
                var projectItemType = (ProjectItemType)value;
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
        #endregion
    }
}