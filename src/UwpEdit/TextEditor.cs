﻿using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Text;
using Microsoft.Graphics.Canvas.UI;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.Foundation;
using Windows.System;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

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

        private CanvasSolidColorBrush _backgroundBrush;
        private CanvasControl _canvasElement;
        private ScrollViewer _contentElement;
        private CanvasSolidColorBrush _cursorColorBrush;
        private int _cursorIndex;
        private CanvasSolidColorBrush _foregroundBrush;
        private ContentPresenter _headerContentPresenter;
        private CanvasHorizontalAlignment _horizontalAlignment;
        private CanvasTextFormat _lineNumberTextFormat;
        private CanvasTextLayout _lineNumberTextLayout;
        private int _marginPadding = 3;
        private int _marginWidth = 50;
        private bool _needsBackgroundBrushRecreation;
        private bool _needsCursorColorBrushRecreation;
        private bool _needsForegroundBrushRecreation;
        private bool _needsLineNumberTextFormatRecreation;
        private bool _needsLineNumberTextLayoutRecreation;
        private bool _needsSelectionForegroundBrushRecreation;
        private bool _needsSelectionHighlightColorBrushRecreation;
        private bool _needsTextFormatRecreation;
        private bool _needsTextLayoutRecreation;
        private ContentControl _placeholderTextContentPresenter;

        private CanvasSolidColorBrush _selectionForegroundBrush;
        private CanvasSolidColorBrush _selectionHighlightColorBrush;
        private List<Range> _selectionRanges = new List<Range>();
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
            _contentElement.Padding = new Thickness(0);
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

        private static Rect InflateRect(Rect r)
        {
            return new Rect(
                new Point(Math.Floor(r.Left), Math.Floor(r.Top)),
                new Point(Math.Ceiling(r.Right), Math.Ceiling(r.Bottom)));
        }

        private void CanvasElement_CreateResources(CanvasControl sender, CanvasCreateResourcesEventArgs args)
        {
            _needsTextFormatRecreation = true;
            _needsTextLayoutRecreation = true;
            _needsForegroundBrushRecreation = true;
            _needsBackgroundBrushRecreation = true;
            _needsSelectionHighlightColorBrushRecreation = true;
            _needsCursorColorBrushRecreation = true;
            _needsSelectionForegroundBrushRecreation = true;
            _needsLineNumberTextFormatRecreation = true;
            _needsLineNumberTextLayoutRecreation = true;
        }

        private void CanvasElement_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        {
#if DEBUG
            var s = new System.Diagnostics.Stopwatch();
            s.Start();
#endif

            EnsureResources(sender, sender.Size);

            // Draw selections
            bool drawnSelections = false;
            if (!IsReadOnly && (FocusState != FocusState.Unfocused) && _selectionRanges.Any())
            {
                foreach (var range in _selectionRanges)
                {
                    var start = Math.Min(range.StartIndex, range.EndIndex);
                    var length = (range.Length < 0) ? range.Length * -1 : range.Length;
                    if (length > 0)
                    {
                        length++;
                        var descriptions = _textLayout.GetCharacterRegions(start, length);
                        foreach (CanvasTextLayoutRegion description in descriptions)
                        {
                            var rect = description.LayoutBounds;
                            rect.X += _marginWidth + _marginPadding;
                            args.DrawingSession.FillRectangle(InflateRect(rect), _selectionHighlightColorBrush);
                        }
                        _textLayout.SetBrush(start, length, _selectionForegroundBrush);
                        drawnSelections = true;
                    }
                }
            }

            // Draw cursor
            if (!IsReadOnly && FocusState != FocusState.Unfocused && !drawnSelections)
            {
                var description = _textLayout.GetCharacterRegions(_cursorIndex, 1).Single();
                var left = (float)description.LayoutBounds.Left + _marginWidth + _marginPadding;
                args.DrawingSession.DrawLine(left, (float)description.LayoutBounds.Top, left, (float)description.LayoutBounds.Bottom, _cursorColorBrush);
            }

            // Draw margin background
            args.DrawingSession.FillRectangle(0, 0, _marginWidth, (float)_lineNumberTextLayout.RequestedSize.Height, _foregroundBrush);

            // Draw line numbers
            args.DrawingSession.DrawTextLayout(_lineNumberTextLayout, 0, 0, _backgroundBrush);

            // Draw text
            args.DrawingSession.DrawTextLayout(_textLayout, _marginWidth + _marginPadding, 0, _foregroundBrush);

            // Update canvas height
            UpdateCanvasHeight();

#if DEBUG
            s.Stop();
            System.Diagnostics.Debug.WriteLine($"Completed TextEditor.CanvasElement_Draw in {s.Elapsed}");
#endif
        }

        private void CanvasElement_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (_selectionRanges.Any())
            {
                foreach (var point in e.GetIntermediatePoints(_canvasElement))
                {
                    if (point.IsInContact)
                    {
                        var position = point.Position;
                        position.X -= _marginWidth + _marginPadding;
                        _selectionRanges.Last().EndIndex = GetHitIndex(position);
                        _canvasElement.Invalidate();
                        e.Handled = true;
                    }
                }
            }
        }

        private void CanvasElement_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Focus(FocusState.Pointer);
            _selectionRanges.Clear();
            bool trailing;
            var position = e.GetCurrentPoint(_canvasElement).Position;
            position.X -= _marginWidth + _marginPadding;
            var index = GetHitIndex(position, out trailing);
            _selectionRanges.Add(new Range() { StartIndex = index, EndIndex = index });

            _cursorIndex = trailing ? index + 1 : index;

            _canvasElement.Invalidate();
            e.Handled = true;
        }

        private void CanvasElement_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            _needsTextLayoutRecreation = true;
        }

        private int CharacterIndexToLineIndex(int characterIndex)
        {
            int characterCount = 0;
            for (int i = 0; i < _textLayout.LineMetrics.Count(); i++)
            {
                characterCount += _textLayout.LineMetrics[i].CharacterCount;
                if (characterCount > characterIndex)
                {
                    return i;
                }
            }
            throw new ArgumentOutOfRangeException();
        }

        private int CharacterOffsetInLine(int characterIndex)
        {
            int characterCount = 0;
            for (int i = 0; i < _textLayout.LineMetrics.Count(); i++)
            {
                characterCount += _textLayout.LineMetrics[i].CharacterCount;
                if (characterCount > characterIndex)
                {
                    return characterIndex - (characterCount - _textLayout.LineMetrics[i].CharacterCount);
                }
            }
            throw new ArgumentOutOfRangeException();
        }

        private Range CharacterRangeForLine(int lineIndex)
        {
            int characterCount = 0;
            for (int i = 0; i < _textLayout.LineMetrics.Count(); i++)
            {
                if (i == lineIndex)
                {
                    return new Range() { StartIndex = characterCount, EndIndex = characterCount + _textLayout.LineMetrics[i].CharacterCount };
                }
                characterCount += _textLayout.LineMetrics[i].CharacterCount;
            }
            throw new ArgumentOutOfRangeException();
        }

        private void CoreWindow_CharacterReceived(CoreWindow sender, CharacterReceivedEventArgs args)
        {
            if (IsReadOnly)
            {
                return;
            }

            var character = (char)args.KeyCode;

            // skip certain control keys that are still passed as characters
            if (character == '\b')
            {
                return;
            }

            Text = Text.Insert(_cursorIndex, character.ToString());
            MoveCursor(1);

            args.Handled = true;
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
                _textLayout = new CanvasTextLayout(resourceCreator, Text, _textFormat, (float)targetSize.Width - _marginWidth - _marginPadding, (float)targetSize.Height);
            }

            if (_needsForegroundBrushRecreation)
            {
                _foregroundBrush?.Dispose();
                _foregroundBrush = new CanvasSolidColorBrush(resourceCreator, (Foreground as SolidColorBrush).Color);
            }

            if (_needsBackgroundBrushRecreation)
            {
                _backgroundBrush?.Dispose();
                _backgroundBrush = new CanvasSolidColorBrush(resourceCreator, (Background as SolidColorBrush).Color);
            }

            if (_needsSelectionHighlightColorBrushRecreation)
            {
                _selectionHighlightColorBrush?.Dispose();
                _selectionHighlightColorBrush = new CanvasSolidColorBrush(resourceCreator, SelectionHighlightColor.Color);
            }

            if (_needsCursorColorBrushRecreation)
            {
                _cursorColorBrush?.Dispose();
                _cursorColorBrush = new CanvasSolidColorBrush(resourceCreator, CursorColor.Color);
            }

            if (_needsSelectionForegroundBrushRecreation)
            {
                _selectionForegroundBrush?.Dispose();
                _selectionForegroundBrush = new CanvasSolidColorBrush(resourceCreator, Colors.White);
            }

            if (_needsLineNumberTextFormatRecreation)
            {
                _lineNumberTextFormat?.Dispose();
                _lineNumberTextFormat = new CanvasTextFormat()
                {
                    HorizontalAlignment = CanvasHorizontalAlignment.Right,
                    FontFamily = FontFamily.Source,
                    FontSize = (float)FontSize,
                };
            }

            if (_needsLineNumberTextLayoutRecreation)
            {
                _lineNumberTextLayout?.Dispose();
                var lineNumberSb = new StringBuilder();
                var characterCount = 0;
                var lineNumber = 1;
                lineNumberSb.AppendLine(lineNumber++.ToString());
                foreach (var lineMetric in _textLayout.LineMetrics)
                {
                    characterCount += lineMetric.CharacterCount;
                    if (Text[characterCount - 1] == '\n' || Text[characterCount - 1] == '\r')
                    {
                        lineNumberSb.AppendLine(lineNumber++.ToString());
                    }
                    else
                    {
                        lineNumberSb.AppendLine();
                    }
                }
                _lineNumberTextLayout = new CanvasTextLayout(resourceCreator, lineNumberSb.ToString(), _lineNumberTextFormat, _marginWidth - _marginPadding, (float)targetSize.Height);
            }
        }

        private int GetHitIndex(Point mouseOverPoint)
        {
            bool trailing;
            return GetHitIndex(mouseOverPoint, out trailing);
        }

        private int GetHitIndex(Point mouseOverPoint, out bool trailing)
        {
            CanvasTextLayoutRegion textLayoutRegion;
            _textLayout.HitTest((float)mouseOverPoint.X, (float)mouseOverPoint.Y, out textLayoutRegion, out trailing);
            return textLayoutRegion.CharacterIndex;
        }

        private void MoveCursor(int moveBy)
        {
            if (moveBy > 0)
            {
                _cursorIndex = Math.Min(_cursorIndex + moveBy, Text.Length);
            }
            else
            {
                _cursorIndex = Math.Max(_cursorIndex + moveBy, 0);
            }
            _canvasElement.Invalidate();
        }

        private void MoveCursorByLine(int moveBy)
        {
            var currentLine = CharacterIndexToLineIndex(_cursorIndex);
            var offsetInLine = CharacterOffsetInLine(_cursorIndex);
            int nextLine;
            if (moveBy > 0)
            {
                nextLine = Math.Min(currentLine + moveBy, _textLayout.LineCount - 1);
            }
            else
            {
                nextLine = Math.Max(currentLine + moveBy, 0);
            }
            var lineRange = CharacterRangeForLine(nextLine);
            _cursorIndex = Math.Min(lineRange.StartIndex + offsetInLine, lineRange.EndIndex);
            if (moveBy > 0)
            {
                _cursorIndex = Math.Min(_cursorIndex, Text.Length);
            }
            else
            {
                _cursorIndex = Math.Max(_cursorIndex, 0);
            }
            _canvasElement.Invalidate();
        }

        private void RegisterEventHandlers()
        {
            Unloaded += TextEditor_Unloaded;
            PointerEntered += TextEditor_PointerEntered;
            PointerExited += TextEditor_PointerExited;
            GotFocus += TextEditor_GotFocus;
            LostFocus += TextEditor_LostFocus;
            Window.Current.CoreWindow.CharacterReceived += CoreWindow_CharacterReceived;
            KeyDown += TextEditor_KeyDown;

            _canvasElement.CreateResources += CanvasElement_CreateResources;
            _canvasElement.SizeChanged += CanvasElement_SizeChanged;
            _canvasElement.Draw += CanvasElement_Draw;
            _canvasElement.PointerPressed += CanvasElement_PointerPressed;
            _canvasElement.PointerMoved += CanvasElement_PointerMoved;
        }

        private void RegisterPropertyChangedCallbacks()
        {
            RegisterPropertyChangedCallback(HeaderProperty, OnHeaderPropertyChanged);
            RegisterPropertyChangedCallback(ForegroundProperty, OnForegroundPropertyChanged);
            RegisterPropertyChangedCallback(BackgroundProperty, OnBackgroundPropertyChanged);
            RegisterPropertyChangedCallback(TextProperty, OnTextPropertyChanged);
            RegisterPropertyChangedCallback(FontSizeProperty, OnFontSizePropertyChanged);
            RegisterPropertyChangedCallback(FontFamilyProperty, OnFontFamilyPropertyChanged);
            RegisterPropertyChangedCallback(TextAlignmentProperty, OnTextAlignmentPropertyChanged);
            RegisterPropertyChangedCallback(FontStyleProperty, OnFontStylePropertyChanged);
            RegisterPropertyChangedCallback(FontWeightProperty, OnFontWeightPropertyChanged);
            RegisterPropertyChangedCallback(FontStretchProperty, OnFontStretchPropertyChanged);
            RegisterPropertyChangedCallback(SelectionHighlightColorProperty, OnSelectionHighlightColorPropertyChanged);
            RegisterPropertyChangedCallback(CursorColorProperty, OnCursorColorPropertyChanged);
            RegisterPropertyChangedCallback(IsReadOnlyProperty, OnIsReadOnlyPropertyChanged);
        }

        private void TextEditor_GotFocus(object sender, RoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "Focused", true);
        }

        private void TextEditor_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (IsReadOnly || e.Handled)
            {
                return;
            }

            switch (e.Key)
            {
                case VirtualKey.Back:
                    if (_cursorIndex > 0)
                    {
                        Text = Text.Substring(0, _cursorIndex - 1) + Text.Substring(_cursorIndex);
                        _cursorIndex--;
                        _canvasElement.Invalidate();
                    }
                    e.Handled = true;
                    break;

                case VirtualKey.Left:
                    MoveCursor(-1);
                    e.Handled = true;
                    break;

                case VirtualKey.Up:
                    MoveCursorByLine(-1);
                    e.Handled = true;
                    break;

                case VirtualKey.Right:
                    MoveCursor(1);
                    e.Handled = true;
                    break;

                case VirtualKey.Down:
                    MoveCursorByLine(1);
                    e.Handled = true;
                    break;

                case VirtualKey.Delete:
                    if (_cursorIndex < Text.Length)
                    {
                        Text = Text.Substring(0, _cursorIndex) + Text.Substring(_cursorIndex + 1);
                        _canvasElement.Invalidate();
                    }
                    break;
            }
        }

        private void TextEditor_LostFocus(object sender, RoutedEventArgs e)
        {
            _selectionRanges.Clear();
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

            _cursorColorBrush?.Dispose();
            _selectionHighlightColorBrush?.Dispose();
            _selectionForegroundBrush?.Dispose();
            _foregroundBrush?.Dispose();
            _backgroundBrush?.Dispose();
            _lineNumberTextFormat?.Dispose();
            _lineNumberTextLayout?.Dispose();
            _textLayout?.Dispose();
            _textFormat?.Dispose();
        }

        private void UpdateCanvasHeight()
        {
            var dips = _textLayout.LineMetrics.Sum(x => x.Height);
            _canvasElement.Height = dips * (Windows.Graphics.Display.DisplayInformation.GetForCurrentView().LogicalDpi / 96);
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
