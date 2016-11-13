// Copyright (c) Phill Campbell. All rights reserved. Licensed under the MIT License. See LICENSE in
// the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UwpEdit.Sample
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        #region Private Fields

        private const string defaultText = "Welcome to UWP Edit. The text editor for UWP apps.";

        #endregion Private Fields

        #region Public Constructors

        public MainPage()
        {
            this.InitializeComponent();
        }

        #endregion Public Constructors

        #region Public Properties

        public static DependencyProperty TextProperty { get; } = DependencyProperty.Register(nameof(Text), typeof(string), typeof(MainPage), new PropertyMetadata(defaultText));
        public string Text { get { return (string)GetValue(TextProperty); } set { SetValue(TextProperty, value); } }

        #endregion Public Properties

        #region Private Methods

        private void DecreaseFontSizeButton_Click(object sender, RoutedEventArgs e)
        {
            textEditor.FontSize--;
            textBox.FontSize--;
        }

        private void IncreaseFontSizeButton_Click(object sender, RoutedEventArgs e)
        {
            textEditor.FontSize++;
            textBox.FontSize++;
        }

        private void ToggleFontFamilyButton_Click(object sender, RoutedEventArgs e)
        {
            if (textEditor.FontFamily == Application.Current.Resources["ContentControlThemeFontFamily"])
            {
                textEditor.FontFamily = new FontFamily("Consolas");
                textBox.FontFamily = new FontFamily("Consolas");
            }
            else
            {
                textEditor.FontFamily = (FontFamily)Application.Current.Resources["ContentControlThemeFontFamily"];
                textBox.FontFamily = (FontFamily)Application.Current.Resources["ContentControlThemeFontFamily"];
            }
        }

        private void ToggleForegroundButton_Click(object sender, RoutedEventArgs e)
        {
            if (textEditor.Foreground == Application.Current.Resources["SystemControlForegroundBaseHighBrush"])
            {
                textEditor.Foreground = new SolidColorBrush(Colors.Red);
                textBox.Foreground = new SolidColorBrush(Colors.Red);
            }
            else
            {
                textEditor.Foreground = (SolidColorBrush)Application.Current.Resources["SystemControlForegroundBaseHighBrush"];
                textBox.Foreground = (SolidColorBrush)Application.Current.Resources["SystemControlForegroundBaseHighBrush"];
            }
        }

        private void ToggleHeaderButton_Click(object sender, RoutedEventArgs e)
        {
            if (textEditor.Header == null)
            {
                textEditor.Header = "UwpEdit.TextEditor";
                textBox.Header = "Windows.UI.Xaml.TextBox";
            }
            else
            {
                textEditor.Header = null;
                textBox.Header = null;
            }
        }

        private void ToggleHeaderTemplateButton_Click(object sender, RoutedEventArgs e)
        {
            if (textEditor.HeaderTemplate == null)
            {
                var dataTemplateXaml = "<DataTemplate xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"><Grid><TextBlock Text=\"{Binding}\" Foreground=\"Red\" FontSize=\"15\"/></Grid></DataTemplate>";
                var dataTemplate = XamlReader.Load(dataTemplateXaml) as DataTemplate;
                textEditor.HeaderTemplate = dataTemplate;
                textBox.HeaderTemplate = dataTemplate;
            }
            else
            {
                textEditor.HeaderTemplate = null;
                textBox.HeaderTemplate = null;
            }
        }

        private void TogglePlaceholderTextButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(textEditor.PlaceholderText))
            {
                textEditor.PlaceholderText = "This is some placeholder text.";
                textBox.PlaceholderText = "This is some placeholder text.";
            }
            else
            {
                textEditor.PlaceholderText = string.Empty;
                textBox.PlaceholderText = string.Empty;
            }
        }

        #endregion Private Methods
    }
}
