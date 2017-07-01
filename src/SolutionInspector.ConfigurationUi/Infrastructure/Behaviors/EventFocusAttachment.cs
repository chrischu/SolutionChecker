﻿using System.Windows;
using System.Windows.Controls;

namespace SolutionInspector.ConfigurationUi.Infrastructure.Behaviors
{
  internal static class EventFocusAttachment
  {
    public static Control GetElementToFocus(Button button)
    {
      return (Control)button.GetValue(ElementToFocusProperty);
    }

    public static void SetElementToFocus(Button button, Control value)
    {
      button.SetValue(ElementToFocusProperty, value);
    }

    public static readonly DependencyProperty ElementToFocusProperty =
        DependencyProperty.RegisterAttached("ElementToFocus", typeof(Control),
        typeof(EventFocusAttachment), new UIPropertyMetadata(null, ElementToFocusPropertyChanged));

    public static void ElementToFocusPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
      var button = sender as Button;
      if (button != null)
        button.Click += (s, args) => GetElementToFocus(button)?.Focus();
    }
  }
}