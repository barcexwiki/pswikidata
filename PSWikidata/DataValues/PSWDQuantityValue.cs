using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Globalization;

namespace PSWikidata.DataValues
{
    class PSWDQuantityValue
    {
        public decimal Amount { get; set; }
        public decimal LowerBound { get; set; }
        public decimal UpperBound { get; set; }
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
            Match m = Regex.Match(Unit, "^http://www.wikidata.org/entity/(?<quid>Q[0-9]+)$", RegexOptions.IgnoreCase);

            string unitSuffix = (Unit == "1") ? "" : " / " + (m.Success ? m.Groups["quid"].Value : Unit);



            decimal minus = Math.Abs(Amount - LowerBound);
            decimal plus = Math.Abs(Amount - UpperBound);

            if (minus == plus && plus == 0)
            {
                return Amount + unitSuffix;
            }
            if (minus == plus)
            {
                return Amount + "±" + plus + unitSuffix;
            }
            return Amount + " +" + plus + " -" + minus + unitSuffix;
        }
    }
}
