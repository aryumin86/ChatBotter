using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CBLib
{
    /// <summary>
    /// Preprocessing of user's message methods:
    /// removing of some chars, trimming, extra spaces removing,
    /// tokenizing.
    /// </summary>
    public class TextsPreProcessor
    {
        public Func<string, string[]> SimpleUserMessagePrepocessing = (raw) =>
        {
            return 
                Regex.Replace(raw, "\\s+", "\\s")
                .ToLower()
                .Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim('?', '!', '%', '.', ',', ':', ')', '(', ';', '$', '*', '[', ']'))
                .ToArray();
        };
    }
}
