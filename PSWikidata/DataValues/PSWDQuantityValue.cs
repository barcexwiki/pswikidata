using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PSWikidata.DataValues
{
    class PSWDQuantityValue
    {
        public string Amount { get; set; }
        public string LowerBound { get; set; }
        public string UpperBound { get; set; }
        public string Unit { get; set; }

        internal PSWDQuantityValue(Wikibase.DataValues.QuantityValue q)
        {
            Amount = q.Amount;
            LowerBound = q.LowerBound;
            UpperBound = q.UpperBound;
            Unit = q.Unit;
        }

        public override string ToString()
        {
            return Amount + " / " + LowerBound + " / " + UpperBound + " / " + Unit;
        }
    }
}
