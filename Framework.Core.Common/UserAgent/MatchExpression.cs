using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Framework.Core.Common
{
    /// <summary>
    ///
    /// </summary>
    public class MatchExpression
    {
        /// <summary>
        ///
        /// </summary>
        public List<Regex> Regexes { get; set; }

        /// <summary>
        ///
        /// </summary>
        public Action<System.Text.RegularExpressions.Match, object> Action { get; set; }
    }

}