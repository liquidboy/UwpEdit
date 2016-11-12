// Copyright (c) Phill Campbell. All rights reserved. Licensed under the MIT License. See LICENSE in
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
        #region Private Fields

        private CanvasControl _canvasElement;

        private ScrollViewer _contentElement;

        private ContentPresenter _headerContentPresenter;

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
            Unloaded += Control_Unloaded;
            _headerContentPresenter = GetTemplateChild("HeaderContentPresenter") as ContentPresenter;
            _contentElement = GetTemplateChild("ContentElement") as ScrollViewer;
            _canvasElement = new CanvasControl();
            _canvasElement.Draw += CanvasElement_Draw;
            _contentElement.Content = _canvasElement;

            UpdateHeader();

            RegisterPropertyChangedCallbacks();
            RegisterEventHandlers();

            base.OnApplyTemplate();
        }

        #endregion Protected Methods

        #region Private Methods

        private void CanvasElement_CreateResources(CanvasControl sender, CanvasCreateResourcesEventArgs args)
        {
        }

        private void CanvasElement_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        {
        }

        private void Control_Unloaded(object sender, RoutedEventArgs e)
        {
            _canvasElement.RemoveFromVisualTree();
            _canvasElement = null;
        }

        private void OnHeaderPropertyChanged(DependencyObject sender, DependencyProperty dp)
        {
            UpdateHeader();
        }

        private void RegisterEventHandlers()
        {
            PointerEntered += TextEditor_PointerEntered;
            PointerExited += TextEditor_PointerExited;
            GotFocus += TextEditor_GotFocus;
            LostFocus += TextEditor_LostFocus;
        }

        private void RegisterPropertyChangedCallbacks()
        {
            RegisterPropertyChangedCallback(HeaderProperty, OnHeaderPropertyChanged);
        }

        private void TextEditor_GotFocus(object sender, RoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "Focused", true);
        }

        private void TextEditor_LostFocus(object sender, RoutedEventArgs e)
        {
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

        private void UpdateHeader()
        {
            if (_headerContentPresenter != null)
            {
                _headerContentPresenter.Visibility = _headerContentPresenter.Content == null ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        #endregion Private Methods
    }
}
