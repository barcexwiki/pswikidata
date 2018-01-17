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

        public PSWDSitelink[] SiteLinks { get => _sitelinks.ToArray(); }

        internal void SetSitelink(PSWDSitelink sitelink)
        {
            var badges = sitelink.Badges.Select( x => new EntityId(x) );
            ((Item)ExtensionData).SetSitelink(sitelink.Site, sitelink.Title, badges);
            RefreshFromExtensionData();
            _log.Add(new LogEntry("SetSitelink", sitelink.Site, sitelink.Title));
        }

        internal void RemoveSitelink(string site)
        {
            ((Item)ExtensionData).RemoveSitelink(site);
            RefreshFromExtensionData();
            _log.Add(new LogEntry("RemoveSitelink", site, null));
        }

        internal PSWDSitelink GetSitelink(string site)
        {
            Sitelink sitelink = ((Item)(ExtensionData)).GetSitelink(site);
            return new PSWDSitelink(sitelink);
        }

        internal override void RefreshFromExtensionData()
        {
            base.RefreshFromExtensionData();
            _sitelinks.Clear();

            Dictionary<string, Sitelink> sl = ((Item)ExtensionData).GetSitelinks();
            foreach (string k in sl.Keys)
            {
                var badges = sl[k].Badges.Select( x => x.PrefixedId );
                _sitelinks.Add(new PSWDSitelink(k, sl[k].Title, badges));
            }
        }

        internal override string Save() => base.Save("item");
    }
}
