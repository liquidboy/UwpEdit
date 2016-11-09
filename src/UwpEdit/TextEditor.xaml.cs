// Copyright (c) Phill Campbell. All rights reserved. Licensed under the MIT License. See LICENSE in
// the project root for license information.

using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace UwpEdit
{
    /// <summary>
    /// Primary UwpEdit control. Provides full editor capabilities.
    /// </summary>
    public sealed partial class TextEditor : UserControl
    {
        #region Public Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public TextEditor()
        {
            this.InitializeComponent();
        }

        #endregion Public Constructors
    }
}
