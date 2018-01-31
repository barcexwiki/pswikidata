using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PSWikidata.DataValues
{
    public class PSWDEntityIdValue
    {
        public string Id { get; set; }

        internal PSWDEntityIdValue(Wikibase.DataValues.EntityIdValue e)
        {
            switch (e.EntityType)
            {
                case Wikibase.EntityType.Item:
                    Id = "q" + e.NumericId;
                    break;

                case Wikibase.EntityType.Property:
                    Id = "p" + e.NumericId;
                    break;
            }
        }

        public override string ToString() => Id;
    }
}
