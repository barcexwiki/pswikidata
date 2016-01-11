using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PSWikidata.DataValues
{
    class PSWDTimeValue
    {
        public DateTime Date { get; set; }

        internal PSWDTimeValue(Wikibase.DataValues.TimeValue t)
        {
            Date = t.DateTime;
        }

        public override string ToString()
        {
            return Date.ToUniversalTime().ToString();
        }
    }
}
