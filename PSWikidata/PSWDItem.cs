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
        private List<PSWDDescription> _descriptions = new List<PSWDDescription>();
        private List<PSWDLabel> _labels = new List<PSWDLabel>();
        private List<PSWDLabel> _aliases = new List<PSWDLabel>();
        private List<PSWDSitelink> _sitelinks = new List<PSWDSitelink>();
        private List<PSWDClaim> _claims = new List<PSWDClaim>();

        private string qId;

        private Item extensionData;

        internal PSWDItem(Item item)
        {
            ExtensionData = item;
            RefreshFromExtensionData();
        }

        public string QId
        {
            get { return qId; }
        }

        public PSWDDescription[] Descriptions
        {
            get { return _descriptions.ToArray(); }
        }

        public PSWDLabel[] Labels
        {
            get { return _labels.ToArray(); }
        }

        public PSWDLabel[] Aliases
        {
            get { return _aliases.ToArray(); }
        }

        public PSWDSitelink[] SiteLinks
        {
            get { return _sitelinks.ToArray(); }
        }

        public PSWDClaim[] Claims
        {
            get { return _claims.ToArray(); }
        }

        internal Item ExtensionData
        {
            get { return extensionData; }
            set { extensionData = value; }
        }

        internal void RefreshFromExtensionData() 
        {
            _descriptions.Clear();
            _labels.Clear();
            _aliases.Clear();
            _claims.Clear();

            qId = ExtensionData.Id != null ?  ExtensionData.Id.ToString() : null;

            Dictionary<string,string> d = ExtensionData.GetDescriptions();
            foreach (string k in d.Keys)
            {
                this._descriptions.Add(new PSWDDescription(k, d[k]));
            }

            Dictionary<string, string> l = ExtensionData.GetLabels();
            foreach (string k in l.Keys)
            {
                this._labels.Add(new PSWDLabel(k, l[k]));
            }

            Dictionary<string, List<string>> a = ExtensionData.GetAliases();
            foreach (string k in a.Keys)
            {
                foreach (string i in a[k])
                {
                    this._aliases.Add(new PSWDLabel(k, i));
                }
            }

            Dictionary<string, string> sl = ExtensionData.GetSitelinks();
            foreach (string k in sl.Keys)
            {
                this._sitelinks.Add(new PSWDSitelink(k, sl[k]));
            }

            foreach (Wikibase.Claim c in ExtensionData.Claims)
            {
                if (c is Wikibase.Statement)
                {
                    this._claims.Add(new PSWDStatement(this,(Statement)c));
                }
                else
                {
                    this._claims.Add(new PSWDClaim(this,c));
                }
            }
        
        }

        internal PSWDStatement GetStatement(string Id)
        {
            var s = from c in Claims
                    where c.ExtensionData.Id == Id
                    select c;

            if (s.Any())
            {
                return (PSWDStatement)s.First();
            } 
            else
            {
                return null;
            }
        }
    }
}
