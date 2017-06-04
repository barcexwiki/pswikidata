using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wikibase;

namespace PSWikidata
{
    public class PSWDOptions
    {
        public PSWDOptions()
        {
            
        }
        public string[] PreferredDisplayLanguages { get; set; }
        public string[] PreferredDisplaySites { get; set; }
    }
}
