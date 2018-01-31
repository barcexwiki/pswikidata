using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PSWikidata.DataValues;
using Wikibase;
using Wikibase.DataValues;

namespace PSWikidata
{
    public class PSWDReference
    {
        public PSWDSnak[] Snaks { get => _snaks.ToArray(); }

        private readonly List<PSWDSnak> _snaks = new List<PSWDSnak>();

        internal PSWDReference(Reference reference)
        {
            foreach (Snak snak in reference.Snaks)
            {
                _snaks.Add(new PSWDSnak(snak));
            }
        }

        internal PSWDReference(IEnumerable<PSWDSnak> snaks)
        {
            foreach (PSWDSnak snak in snaks)
            {
                _snaks.Add(snak);
            }
        }
    }
}