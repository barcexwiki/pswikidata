using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wikibase;

namespace PSWikidata
{
    public class PSWDItem : PSWDEntity
    {

        private List<PSWDSitelink> _sitelinks = new List<PSWDSitelink>();

        internal PSWDItem(Item item) : base(item) 
        {
        }

        internal PSWDItem(WikibaseApi api) : this(new Item(api))
        {
        }

        public PSWDSitelink[] SiteLinks
        {
            get { return _sitelinks.ToArray(); }
        }

        internal void SetSitelink(string site, string title)
        {
            ((Item)ExtensionData).SetSitelink(site, title);
            RefreshFromExtensionData();
            _log.Add(new LogEntry("SetSitelink", site, title));
        }

        internal void RemoveSitelink(string site)
        {
            ((Item)ExtensionData).RemoveSitelink(site);
            RefreshFromExtensionData();
            _log.Add(new LogEntry("RemoveSitelink", site, null));
        }

        internal override void RefreshFromExtensionData()
        {
            base.RefreshFromExtensionData();
            _sitelinks.Clear();

            Dictionary<string, string> sl = ((Item)ExtensionData).GetSitelinks();
            foreach (string k in sl.Keys)
            {
                _sitelinks.Add(new PSWDSitelink(k, sl[k]));
            }
        }

        internal override string Save()
        {
            return base.Save("item");            
        }
    }
}
