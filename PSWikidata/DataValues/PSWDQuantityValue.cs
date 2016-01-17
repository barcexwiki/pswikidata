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
            Amount = decimal.Parse(q.Amount,CultureInfo.InvariantCulture);
            LowerBound = decimal.Parse(q.LowerBound,CultureInfo.InvariantCulture);
            UpperBound = decimal.Parse(q.UpperBound, CultureInfo.InvariantCulture);
            Unit = q.Unit;
        }

        public override string ToString()
        {
            string unitSuffix = (Unit == "1") ? "" : " / " + Unit;



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
