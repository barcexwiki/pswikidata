using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PSWikidata.DataValues
{
    class PSWDStringValue
    {
        public string Value { get; set; }

        internal PSWDStringValue(Wikibase.DataValues.StringValue s)
        {
            this.Value = s.Value;          
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
