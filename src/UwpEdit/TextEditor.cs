// Copyright (c) Phill Campbell. All rights reserved. Licensed under the MIT License. See LICENSE in
// the project root for license information.

using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Text;
using Microsoft.Graphics.Canvas.UI;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace UwpEdit
{
    /// <summary>
    /// Primary UwpEdit control. Provides full editor capabilities.
    /// </summary>
    [TemplatePart(Name = "BackgroundElement", Type = typeof(Border))]
    [TemplatePart(Name = "BorderElement", Type = typeof(Border))]
    [TemplatePart(Name = "HeaderContentPresenter", Type = typeof(ContentPresenter))]
    [TemplatePart(Name = "ContentElement", Type = typeof(ScrollViewer))]
    [TemplatePart(Name = "PlaceholderTextContentPresenter", Type = typeof(ContentControl))]
    public sealed partial class TextEditor : Control
    {
        #region Private Fields

        private CanvasControl _canvasElement;
        private ScrollViewer _contentElement;
        private CanvasSolidColorBrush _foregroundBrush;
        private ContentPresenter _headerContentPresenter;

        private CanvasHorizontalAlignment _horizontalAlignment;

        private bool _needsForegroundBrushRecreation;

        private bool _needsSelectionForegroundBrushRecreation;
        private bool _needsSelectionHighlightColorBrushRecreation;

        private bool _needsTextFormatRecreation;

        private bool _needsTextLayoutRecreation;
        private ContentControl _placeholderTextContentPresenter;

        private CanvasSolidColorBrush _selectionForegroundBrush;
        private CanvasSolidColorBrush _selectionHighlightColorBrush;
        private List<Range> _selectionRanges;
        private CanvasTextFormat _textFormat;
        private CanvasTextLayout _textLayout;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public TextEditor()
        {
            DefaultStyleKey = typeof(TextEditor);
        }

        #endregion Public Constructors

        #region Protected Methods

        /// <summary>
        /// Invoked whenever application code or internal processes (such as a rebuilding layout
        /// pass) call ApplyTemplate.
        /// </summary>
        protected override void OnApplyTemplate()
        {
            _headerContentPresenter = GetTemplateChild("HeaderContentPresenter") as ContentPresenter;
            _placeholderTextContentPresenter = GetTemplateChild("PlaceholderTextContentPresenter") as ContentControl;
            _contentElement = GetTemplateChild("ContentElement") as ScrollViewer;
            _canvasElement = new CanvasControl();
            _contentElement.Content = _canvasElement;

            UpdateHeaderVisibility();
            UpdatePlaceholderTextVisibility();
            UpdateTextAlignment();

            RegisterPropertyChangedCallbacks();
            RegisterEventHandlers();

            base.OnApplyTemplate();
        }

        #endregion Protected Methods

        #region Private Methods

        private void CanvasElement_CreateResources(CanvasControl sender, CanvasCreateResourcesEventArgs args)
        {
            _needsTextFormatRecreation = true;
            _needsTextLayoutRecreation = true;
            _needsForegroundBrushRecreation = true;
            _needsSelectionHighlightColorBrushRecreation = true;
            _needsSelectionForegroundBrushRecreation = true;
        }

        private void CanvasElement_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            EnsureResources(sender, sender.Size);

            if ((FocusState != FocusState.Unfocused) && (_selectionRanges?.Any() ?? false))
            {
                foreach (var range in _selectionRanges)
                {
                    var descriptions = _textLayout.GetCharacterRegions(range.StartIndex, range.Length);
                    foreach (CanvasTextLayoutRegion description in descriptions)
                    {
                        args.DrawingSession.FillRectangle(InflateRect(description.LayoutBounds), _selectionHighlightColorBrush);
                    }
                    _textLayout.SetBrush(range.StartIndex, range.Length, _selectionForegroundBrush);
                }
            }

            args.DrawingSession.DrawTextLayout(_textLayout, 0, 0, _foregroundBrush);
        }

        private void CanvasElement_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            _needsTextLayoutRecreation = true;
        }

        private void EnsureResources(ICanvasResourceCreatorWithDpi resourceCreator, Size targetSize)
        {
            if (_needsTextFormatRecreation)
            {
                _textFormat?.Dispose();
                _textFormat = new CanvasTextFormat()
                {
                    HorizontalAlignment = _horizontalAlignment,
                    FontFamily = FontFamily.Source,
                    FontSize = (float)FontSize,
                    FontStretch = FontStretch,
                    FontStyle = FontStyle,
                    FontWeight = FontWeight,
                };
            }

            if (_needsTextLayoutRecreation)
            {
                _textLayout?.Dispose();
                _textLayout = new CanvasTextLayout(resourceCreator, Text, _textFormat, (float)targetSize.Width, (float)targetSize.Height);
            }

            if (_needsForegroundBrushRecreation)
            {
                _foregroundBrush?.Dispose();
                _foregroundBrush = new CanvasSolidColorBrush(resourceCreator, (Foreground as SolidColorBrush).Color);
            }

            if (_needsSelectionHighlightColorBrushRecreation)
            {
                _selectionHighlightColorBrush?.Dispose();
                _selectionHighlightColorBrush = new CanvasSolidColorBrush(resourceCreator, SelectionHighlightColor.Color);
            }

            if (_needsSelectionForegroundBrushRecreation)
            {
                _selectionForegroundBrush?.Dispose();
                _selectionForegroundBrush = new CanvasSolidColorBrush(resourceCreator, Colors.White);
            }
        }

        private Rect InflateRect(Rect r)
        {
            return new Rect(
                new Point(Math.Floor(r.Left), Math.Floor(r.Top)),
                new Point(Math.Ceiling(r.Right), Math.Ceiling(r.Bottom)));
        }

        private void OnFontFamilyPropertyChanged(DependencyObject sender, DependencyProperty dp)
        {
            _needsTextFormatRecreation = true;
            _canvasElement.Invalidate();
        }

        private void OnFontSizePropertyChanged(DependencyObject sender, DependencyProperty dp)
        {
            _needsTextFormatRecreation = true;
            _canvasElement.Invalidate();
        }

        private void OnFontStretchPropertyChanged(DependencyObject sender, DependencyProperty dp)
        {
            _needsTextFormatRecreation = true;
            _canvasElement.Invalidate();
        }

        private void OnFontStylePropertyChanged(DependencyObject sender, DependencyProperty dp)
        {
            _needsTextFormatRecreation = true;
            _canvasElement.Invalidate();
        }

        private void OnFontWeightPropertyChanged(DependencyObject sender, DependencyProperty dp)
        {
            _needsTextFormatRecreation = true;
            _canvasElement.Invalidate();
        }

        private void OnForegroundPropertyChanged(DependencyObject sender, DependencyProperty dp)
        {
            _needsForegroundBrushRecreation = true;
            _canvasElement.Invalidate();
        }

        private void OnHeaderPropertyChanged(DependencyObject sender, DependencyProperty dp)
        {
            UpdateHeaderVisibility();
        }

        private void OnSelectionHighlightColorPropertyChanged(DependencyObject sender, DependencyProperty dp)
        {
            _needsSelectionHighlightColorBrushRecreation = true;
            _canvasElement.Invalidate();
        }

        private void OnTextAlignmentPropertyChanged(DependencyObject sender, DependencyProperty dp)
        {
            UpdateTextAlignment();
            _needsTextFormatRecreation = true;
            _canvasElement.Invalidate();
        }

        private void OnTextPropertyChanged(DependencyObject sender, DependencyProperty dp)
        {
            UpdatePlaceholderTextVisibility();
            _selectionRanges?.Clear();
            _canvasElement.Invalidate();
        }

        private void RegisterEventHandlers()
        {
            Unloaded += TextEditor_Unloaded;
            PointerEntered += TextEditor_PointerEntered;
            PointerExited += TextEditor_PointerExited;
            GotFocus += TextEditor_GotFocus;
            LostFocus += TextEditor_LostFocus;

            _canvasElement.CreateResources += CanvasElement_CreateResources;
            _canvasElement.SizeChanged += CanvasElement_SizeChanged;
            _canvasElement.Draw += CanvasElement_Draw;
        }

        private void RegisterPropertyChangedCallbacks()
        {
            RegisterPropertyChangedCallback(HeaderProperty, OnHeaderPropertyChanged);
            RegisterPropertyChangedCallback(ForegroundProperty, OnForegroundPropertyChanged);
            RegisterPropertyChangedCallback(TextProperty, OnTextPropertyChanged);
            RegisterPropertyChangedCallback(FontSizeProperty, OnFontSizePropertyChanged);
            RegisterPropertyChangedCallback(FontFamilyProperty, OnFontFamilyPropertyChanged);
            RegisterPropertyChangedCallback(TextAlignmentProperty, OnTextAlignmentPropertyChanged);
            RegisterPropertyChangedCallback(FontStyleProperty, OnFontStylePropertyChanged);
            RegisterPropertyChangedCallback(FontWeightProperty, OnFontWeightPropertyChanged);
            RegisterPropertyChangedCallback(FontStretchProperty, OnFontStretchPropertyChanged);
            RegisterPropertyChangedCallback(SelectionHighlightColorProperty, OnSelectionHighlightColorPropertyChanged);
        }

        private void TextEditor_GotFocus(object sender, RoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "Focused", true);
        }

        private void TextEditor_LostFocus(object sender, RoutedEventArgs e)
        {
            _selectionRanges?.Clear();
            _canvasElement?.Invalidate();
            VisualStateManager.GoToState(this, "Normal", true);
        }

        private void TextEditor_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (FocusState == FocusState.Unfocused)
            {
                VisualStateManager.GoToState(this, "PointerOver", true);
            }
        }

        private void TextEditor_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (FocusState == FocusState.Unfocused)
            {
                VisualStateManager.GoToState(this, "Normal", true);
            }
        }

        private void TextEditor_Unloaded(object sender, RoutedEventArgs e)
        {
            _canvasElement.RemoveFromVisualTree();
            _canvasElement = null;

            _selectionHighlightColorBrush?.Dispose();
            _selectionForegroundBrush?.Dispose();
            _foregroundBrush?.Dispose();
            _textLayout?.Dispose();
            _textFormat?.Dispose();
        }

        private void UpdateHeaderVisibility()
        {
            if (_headerContentPresenter != null)
            {
                _headerContentPresenter.Visibility = _headerContentPresenter.Content == null ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        private void UpdatePlaceholderTextVisibility()
        {
            if (string.IsNullOrWhiteSpace(Text))
            {
                _placeholderTextContentPresenter.Visibility = Visibility.Visible;
            }
            else
            {
                _placeholderTextContentPresenter.Visibility = Visibility.Collapsed;
            }
        }

        private void UpdateTextAlignment()
        {
            switch (TextAlignment)
            {
                case TextAlignment.Left:
                    _horizontalAlignment = CanvasHorizontalAlignment.Left;
                    break;

                case TextAlignment.Center:
                    _horizontalAlignment = CanvasHorizontalAlignment.Center;
                    break;

                case TextAlignment.Right:
                    _horizontalAlignment = CanvasHorizontalAlignment.Right;
                    break;

                case TextAlignment.Justify:
                    _horizontalAlignment = CanvasHorizontalAlignment.Justified;
                    break;

                case TextAlignment.DetectFromContent:
                    _horizontalAlignment = CanvasHorizontalAlignment.Left;
                    break;
            }
        }

        #endregion Private Methods
    }
}
