using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PSWikidata.DataValues
{
    internal class PSWDMonolingualTextValue
    {
        public string Text { get; set; }
        public string Language { get; set; }

        internal PSWDMonolingualTextValue(Wikibase.DataValues.MonolingualTextValue s)
        {
            this.Text = s.Text;
            this.Language = s.Language;
        }

        public override string ToString()
        {
            return String.Format("[{0}]{1}", Language, Text);
        }
    }
}
