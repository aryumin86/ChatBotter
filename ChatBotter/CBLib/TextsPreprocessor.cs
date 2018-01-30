using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CBLib
{
    /// <summary>
    /// Preprocessing of user's message methods:
    /// removing of some chars, trimming, extra spaces removing,
    /// tokenizing.
    /// </summary>
    public class TextsPreprocessor
    {
        public Func<string, string[]> SimpleUserMessagePrepocessing = (raw) =>
        {
            return raw.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                      .Select(x => x.Trim('?', '!', '%', '.', ',', ':', ')', '(', ';', '$', '*', '[', ']'))
                      .ToArray();
        };
    }
}
