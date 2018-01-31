using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wikibase;

namespace PSWikidata
{
    public class PSWDStatement : PSWDClaim
    {
        public Wikibase.Rank Rank { get; set; }

        internal PSWDStatement(PSWDEntity item, Wikibase.Statement statement) : base(item, statement)
        {
            Rank = statement.Rank;
        }

        public PSWDReference[] References { get => _references.ToArray(); }

        private readonly List<PSWDReference> _references = new List<PSWDReference>();

        internal override void RefreshFromExtensionData()
        {
            Statement statement = (Statement)ExtensionData;

            _references.Clear();
            foreach (var reference in statement.References)
            {
                _references.Add(new PSWDReference(reference));
            }
            base.RefreshFromExtensionData();
        }
    }
}
