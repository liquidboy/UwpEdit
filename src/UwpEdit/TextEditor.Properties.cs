﻿// Copyright (c) Phill Campbell. All rights reserved. Licensed under the MIT License. See LICENSE in
// the project root for license information.

using Microsoft.Graphics.Canvas.UI;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
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
        #region Public Properties

        /// <summary>
        /// Identifies the Header dependency property.
        /// </summary>
        public static DependencyProperty HeaderProperty { get; } = DependencyProperty.Register(nameof(Header), typeof(object), typeof(TextEditor), null);

        /// <summary>
        /// Identifies the HeaderTemplate dependency property.
        /// </summary>
        public static DependencyProperty HeaderTemplateProperty { get; } = DependencyProperty.Register(nameof(HeaderTemplate), typeof(DataTemplate), typeof(TextEditor), null);

        /// <summary>
        /// Identifies the PlaceholderText dependency property.
        /// </summary>
        public static DependencyProperty PlaceholderTextProperty { get; } = DependencyProperty.Register(nameof(PlaceholderText), typeof(string), typeof(TextEditor), null);

        /// <summary>
        /// Identifies the SelectionHighlightColor dependency property.
        /// </summary>
        public static DependencyProperty SelectionHighlightColorProperty { get; } = DependencyProperty.Register(nameof(SelectionHighlightColor), typeof(SolidColorBrush), typeof(TextEditor), null);

        /// <summary>
        /// Identifies the TextAlignment dependency property.
        /// </summary>
        public static DependencyProperty TextAlignmentProperty { get; } = DependencyProperty.Register(nameof(TextAlignment), typeof(TextAlignment), typeof(TextEditor), new PropertyMetadata(TextAlignment.Left));

        /// <summary>
        /// Identifies the TextProperty dependency property.
        /// </summary>
        public static DependencyProperty TextProperty { get; } = DependencyProperty.Register(nameof(Text), typeof(string), typeof(TextEditor), null);

        /// <summary>
        /// Gets or sets the content for the control's header.
        /// </summary>
        public object Header { get { return GetValue(HeaderProperty); } set { SetValue(HeaderProperty, value); } }

        /// <summary>
        /// Gets or sets the DataTemplate used to display the content of the control's header.
        /// </summary>
        public DataTemplate HeaderTemplate { get { return (DataTemplate)GetValue(HeaderTemplateProperty); } set { SetValue(HeaderTemplateProperty, value); } }

        /// <summary>
        /// Gets or sets the text that is displayed in the control until the value is changed by a
        /// user action or some other operation.
        /// </summary>
        public string PlaceholderText { get { return (string)GetValue(PlaceholderTextProperty); } set { SetValue(PlaceholderTextProperty, value); } }

        /// <summary>
        /// Gets or sets the brush used to highlight the selected text.
        /// </summary>
        public SolidColorBrush SelectionHighlightColor { get { return (SolidColorBrush)GetValue(SelectionHighlightColorProperty); } set { SetValue(SelectionHighlightColorProperty, value); } }

        /// <summary>
        /// Gets or sets the text content of a TextEditor.
        /// </summary>
        public string Text { get { return (string)GetValue(TextProperty); } set { SetValue(TextProperty, value); } }

        /// <summary>
        /// Gets or sets how the text should be aligned in the text editor.
        /// </summary>
        public TextAlignment TextAlignment { get { return (TextAlignment)GetValue(TextAlignmentProperty); } set { SetValue(TextAlignmentProperty, value); } }

        #endregion Public Properties
    }
}