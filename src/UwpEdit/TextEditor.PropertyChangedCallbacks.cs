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

namespace UwpEdit
{
    public sealed partial class TextEditor : Control
    {
        #region Private Methods

        private void OnCursorColorPropertyChanged(DependencyObject sender, DependencyProperty dp)
        {
            _needsCursorColorBrushRecreation = true;
            _canvasElement.Invalidate();
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

        private void OnReadOnlyPropertyChanged(DependencyObject sender, DependencyProperty dp)
        {
            _canvasElement.Invalidate();
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
            _selectionRanges.Clear();
            _canvasElement.Invalidate();
        }

        #endregion Private Methods
    }
}
