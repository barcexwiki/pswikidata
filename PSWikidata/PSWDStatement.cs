using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSWikidata
{
    public class PSWDStatement : PSWDClaim
    {

        public Wikibase.Rank Rank { get; set; }

        internal PSWDStatement(Wikibase.Statement statement) : base(statement)
        {
            Rank = statement.Rank;
        }
    }
}
