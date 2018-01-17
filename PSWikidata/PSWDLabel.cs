using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSWikidata
{
    public class PSWDLabel
    {

        internal PSWDLabel(string language, string label)
        {
            Language = language;
            Label = label;
        }

        public string Language { get; protected set; }

        public string Label { get; protected set; }

        public override string ToString() => $"{Language}:{Label}";
        
    }
}