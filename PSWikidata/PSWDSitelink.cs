using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSWikidata
{
    public class PSWDSitelink
    {
        private string _site;
        private string _title;

        private HashSet<string> _badges =  new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        internal Wikibase.Sitelink ExtensionData { get; set; }

        public string Site
        {
            get { return _site; }
            protected set { _site = value; }
        }

        public string Title
        {
            get { return _title; }
            protected set { _title = value; }
        }

        public IEnumerable<string> Badges
        {
            get { return _badges.ToArray(); }
        }

        public override string ToString()
        {
            return _site + ":" + _title;
        }

        internal PSWDSitelink(string site, string title, IEnumerable<string> badges = null )
        {
            _site = site;
            _title = title;

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
            this.ExtensionData = sitelink;
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