using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UwpEdit
{
    internal class Range
    {
        #region Public Properties

        public int EndIndex { get; set; } = 0;

        public int Length => EndIndex - StartIndex;

        public int StartIndex { get; set; } = 0;

        #endregion Public Properties
    }
}
