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
using Windows.UI.Text;
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

        private Dictionary<int, FontWeight> _fontWeights = new Dictionary<int, FontWeight>()
        {
            {0, FontWeights.Black },
            {1, FontWeights.Bold },
            {2, FontWeights.ExtraBlack },
            {3, FontWeights.ExtraBold },
            {4, FontWeights.ExtraLight },
            {5, FontWeights.Light },
            {6, FontWeights.Medium },
            {7, FontWeights.Normal },
            {8, FontWeights.SemiBold },
            {9, FontWeights.SemiLight },
            {10, FontWeights.Thin },
        };

        private int _selectedFontWeight = 7;

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
            textBox.FontSize = textEditor.FontSize;
        }

        private void IncreaseFontSizeButton_Click(object sender, RoutedEventArgs e)
        {
            textEditor.FontSize++;
            textBox.FontSize = textEditor.FontSize;
        }

        private void ToggleFontFamilyButton_Click(object sender, RoutedEventArgs e)
        {
            if (textEditor.FontFamily == Application.Current.Resources["ContentControlThemeFontFamily"])
            {
                textEditor.FontFamily = new FontFamily("Consolas");
            }
            else if (textEditor.FontFamily.Source == "Consolas")
            {
                textEditor.FontFamily = new FontFamily("Arial");
            }
            else
            {
                textEditor.FontFamily = (FontFamily)Application.Current.Resources["ContentControlThemeFontFamily"];
            }
            textBox.FontFamily = textEditor.FontFamily;
        }

        private void ToggleFontStretchButton_Click(object sender, RoutedEventArgs e)
        {
            if ((int)textEditor.FontStretch < 9)
            {
                textEditor.FontStretch = (FontStretch)((int)textEditor.FontStretch + 1);
            }
            else
            {
                textEditor.FontStretch = (FontStretch)1;
            }
            textBox.FontStretch = textEditor.FontStretch;
        }

        private void ToggleFontStyleButton_Click(object sender, RoutedEventArgs e)
        {
            if ((int)textEditor.FontStyle < 2)
            {
                textEditor.FontStyle = (FontStyle)((int)textEditor.FontStyle + 1);
            }
            else
            {
                textEditor.FontStyle = (FontStyle)0;
            }
            textBox.FontStyle = textEditor.FontStyle;
        }

        private void ToggleFontWeightButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedFontWeight < 9)
            {
                _selectedFontWeight++;
            }
            else
            {
                _selectedFontWeight = 0;
            }
            textEditor.FontWeight = _fontWeights[_selectedFontWeight];
            textBox.FontWeight = textEditor.FontWeight;
        }

        private void ToggleForegroundButton_Click(object sender, RoutedEventArgs e)
        {
            if (textEditor.Foreground == Application.Current.Resources["SystemControlForegroundBaseHighBrush"])
            {
                textEditor.Foreground = new SolidColorBrush(Colors.Red);
            }
            else
            {
                textEditor.Foreground = (SolidColorBrush)Application.Current.Resources["SystemControlForegroundBaseHighBrush"];
            }
            textBox.Foreground = textEditor.Foreground;
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
                textBox.Header = textEditor.Header;
            }
        }

        private void ToggleHeaderTemplateButton_Click(object sender, RoutedEventArgs e)
        {
            if (textEditor.HeaderTemplate == null)
            {
                var dataTemplateXaml = "<DataTemplate xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"><Grid><TextBlock Text=\"{Binding}\" Foreground=\"Red\" FontSize=\"15\"/></Grid></DataTemplate>";
                var dataTemplate = XamlReader.Load(dataTemplateXaml) as DataTemplate;
                textEditor.HeaderTemplate = dataTemplate;
            }
            else
            {
                textEditor.HeaderTemplate = null;
            }
            textBox.HeaderTemplate = textEditor.HeaderTemplate;
        }

        private void TogglePlaceholderTextButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(textEditor.PlaceholderText))
            {
                textEditor.PlaceholderText = "This is some placeholder text.";
            }
            else
            {
                textEditor.PlaceholderText = string.Empty;
            }
            textBox.PlaceholderText = textEditor.PlaceholderText;
        }

        private void ToggleSelectionColorButton_Click(object sender, RoutedEventArgs e)
        {
            if (textEditor.SelectionHighlightColor == Application.Current.Resources["SystemControlHighlightAccentBrush"])
            {
                textEditor.SelectionHighlightColor = new SolidColorBrush(Colors.Green);
            }
            else
            {
                textEditor.SelectionHighlightColor = (SolidColorBrush)Application.Current.Resources["SystemControlHighlightAccentBrush"];
            }
            textBox.SelectionHighlightColor = textEditor.SelectionHighlightColor;
        }

        private void ToggleTextAlignmentButton_Click(object sender, RoutedEventArgs e)
        {
            if ((int)textEditor.TextAlignment < 4)
            {
                textEditor.TextAlignment = (TextAlignment)((int)textEditor.TextAlignment + 1);
            }
            else
            {
                textEditor.TextAlignment = (TextAlignment)0;
            }
            textBox.TextAlignment = textEditor.TextAlignment;
        }

        #endregion Private Methods
    }
}
