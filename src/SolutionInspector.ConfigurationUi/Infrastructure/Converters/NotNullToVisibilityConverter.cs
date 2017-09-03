﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using JetBrains.Annotations;

namespace SolutionInspector.ConfigurationUi.Infrastructure.Converters
{
  [ValueConversion(typeof(object), typeof(Visibility))]
  internal class NotNullToVisibilityConverter : IValueConverter
  {
    public object Convert([CanBeNull] object value, [NotNull] Type targetType, [CanBeNull] object parameter, [NotNull] CultureInfo culture)
    {
      return value != null ? Visibility.Visible : Visibility.Collapsed;
    }

    [ExcludeFromCodeCoverage]
    public object ConvertBack([CanBeNull] object value, [NotNull] Type targetType, [CanBeNull] object parameter, [NotNull] CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}