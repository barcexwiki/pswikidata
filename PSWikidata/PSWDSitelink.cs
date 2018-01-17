using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSWikidata
{
    public class PSWDSitelink
    {
        private HashSet<string> _badges = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        internal Wikibase.Sitelink ExtensionData { get; set; }

        public string Site { get; protected set; }

        public string Title { get; protected set; }

        public IEnumerable<string> Badges { get => _badges.ToArray(); }

        public override string ToString() => $"{Site}:{Title}";

        internal PSWDSitelink(string site, string title, IEnumerable<string> badges = null)
        {
            Site = site;
            Title = title;

            if (badges != null)
            {
                foreach (string b in badges)
                {
                    _badges.Add(b);
                }
            }
        }

        internal PSWDSitelink(Wikibase.Sitelink sitelink)
        {
            ExtensionData = sitelink;
            RefreshFromExtensionData(sitelink);
        }

        internal void RefreshFromExtensionData(Wikibase.Sitelink sitelink)
        {
            Site = sitelink.Site;
            Title = sitelink.Title;

            _badges.Clear();
            foreach (Wikibase.EntityId badge in sitelink.Badges)
            {
                _badges.Add(badge.PrefixedId);
            }
        }

    }
}