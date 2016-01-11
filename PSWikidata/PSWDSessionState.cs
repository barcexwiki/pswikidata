using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wikibase;

namespace PSWikidata
{
    class PSWDSessionState
    {
        internal PSWDSessionState(WikibaseApi api, EntityProvider entityProvider)
        {
            Api = api;
            EntityProvider = entityProvider;
        }

        internal WikibaseApi Api { get; set; }
        internal EntityProvider EntityProvider { get; set; }
    }
}
