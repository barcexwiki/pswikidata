using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wikibase;

namespace PSWikidata
{
    public class PSWDItem
    {
        private List<PSWDDescription> descriptions = new List<PSWDDescription>();
        private List<PSWDLabel> labels = new List<PSWDLabel>();
        private List<PSWDLabel> aliases = new List<PSWDLabel>();
        private List<PSWDSitelink> sitelinks = new List<PSWDSitelink>();
        private List<PSWDClaim> claims = new List<PSWDClaim>();

        private string qId;

        private Item extensionData;

        internal PSWDItem(Item item)
        {

            ExtensionData = item;
            RefreshFromExtensionData();
        }

        internal void RefreshFromExtensionData() 
        {
            descriptions.Clear();
            labels.Clear();
            aliases.Clear();
            claims.Clear();

            qId = ExtensionData.id.ToString();

            Dictionary<string,string> d = ExtensionData.getDescriptions();
            foreach (string k in d.Keys)
            {
                this.descriptions.Add(new PSWDDescription(k, d[k]));
            }

            Dictionary<string, string> l = ExtensionData.getLabels();
            foreach (string k in l.Keys)
            {
                this.labels.Add(new PSWDLabel(k, l[k]));
            }

            Dictionary<string, List<string>> a = ExtensionData.getAliases();
            foreach (string k in a.Keys)
            {
                foreach (string i in a[k])
                {
                    this.aliases.Add(new PSWDLabel(k, i));
                }
            }

            Dictionary<string, string> sl = ExtensionData.getSitelinks();
            foreach (string k in sl.Keys)
            {
                this.sitelinks.Add(new PSWDSitelink(k, sl[k]));
            }

            foreach (Wikibase.Claim c in ExtensionData.Claims)
            {
                this.claims.Add(new PSWDClaim(c));
            }
        
        }

        public string QId
        {
            get { return qId; }
        }

        public PSWDDescription[] Descriptions
        {
            get { return descriptions.ToArray(); }
        }

        public PSWDLabel[] Labels
        {
            get { return labels.ToArray(); }
        }

        public PSWDLabel[] Aliases
        {
            get { return aliases.ToArray(); }
        }

        public PSWDSitelink[] SiteLinks
        {
            get { return sitelinks.ToArray(); }
        }

        public PSWDClaim[] Claims
        {
            get { return claims.ToArray(); }
        }

        internal Item ExtensionData
        {
            get { return extensionData; }
            set { extensionData = value; }
        }


    }
}
