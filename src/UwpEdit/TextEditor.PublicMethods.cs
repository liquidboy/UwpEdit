﻿using Microsoft.Graphics.Canvas.UI;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace UwpEdit
{
    public sealed partial class TextEditor : Control
    {
        #region Public Methods

        /// <summary>
        /// Selects a range of text in the text editor.
        /// </summary>
        /// <param name="start">The zero-based index of the first character in the selection.</param>
        /// <param name="length">The length of the selection, in characters.</param>
        public void Select(int start, int length)
        {
            _selectionRanges.Clear();
            _selectionRanges.Add(new Range() { StartIndex = start, EndIndex = start + length });
            _canvasElement.Invalidate();
        }

        /// <summary>
        /// Selects the entire contents of the text box.
        /// </summary>
        public void SelectAll()
        {
            Select(0, Text.Length);
        }

        #endregion Public Methods
    }
}
