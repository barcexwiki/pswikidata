﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PSWikidata.DataValues
{
    public class PSWDMonolingualTextValue
    {
        public string Text { get; set; }
        public string Language { get; set; }

        internal PSWDMonolingualTextValue(Wikibase.DataValues.MonolingualTextValue s)
        {
            Text = s.Text;
            Language = s.Language;
        }

        public override string ToString() => $"[{Language}]{Text}";
    }
}
