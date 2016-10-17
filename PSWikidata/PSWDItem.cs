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
        private struct LogEntry
        {
            public LogEntry(string operation, string language, string value)
            {
                this.operation = operation;
                this.language = language;
                this.value = value;
            }
            public string operation;
            public string language;
            public string value;
        }

        private List<PSWDDescription> _descriptions = new List<PSWDDescription>();
        private List<PSWDLabel> _labels = new List<PSWDLabel>();
        private List<PSWDLabel> _aliases = new List<PSWDLabel>();
        private List<PSWDSitelink> _sitelinks = new List<PSWDSitelink>();
        private List<PSWDClaim> _claims = new List<PSWDClaim>();
        private List<LogEntry> _log = new List<LogEntry>();
        private string _qId;

        private Item _extensionData;

        internal PSWDItem(Item item)
        {
            ExtensionData = item;
            RefreshFromExtensionData();
        }

        public string QId
        {
            get { return _qId; }
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
            get { return _extensionData; }
            set { _extensionData = value; }
        }

        internal void RefreshFromExtensionData()
        {
            _descriptions.Clear();
            _labels.Clear();
            _aliases.Clear();
            _claims.Clear();
            _sitelinks.Clear();

            _qId = ExtensionData.Id != null ? ExtensionData.Id.ToString() : null;

            Dictionary<string, string> d = ExtensionData.GetDescriptions();
            foreach (string k in d.Keys)
            {
                _descriptions.Add(new PSWDDescription(k, d[k]));
            }

            Dictionary<string, string> l = ExtensionData.GetLabels();
            foreach (string k in l.Keys)
            {
                _labels.Add(new PSWDLabel(k, l[k]));
            }

            Dictionary<string, List<string>> a = ExtensionData.GetAliases();
            foreach (string k in a.Keys)
            {
                foreach (string i in a[k])
                {
                    _aliases.Add(new PSWDLabel(k, i));
                }
            }

            Dictionary<string, string> sl = ExtensionData.GetSitelinks();
            foreach (string k in sl.Keys)
            {
                _sitelinks.Add(new PSWDSitelink(k, sl[k]));
            }

            foreach (Wikibase.Claim c in ExtensionData.Claims)
            {
                if (c is Wikibase.Statement)
                {
                    _claims.Add(new PSWDStatement(this, (Statement)c));
                }
                else
                {
                    _claims.Add(new PSWDClaim(this, c));
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

        internal void SetDescription(string language, string description)
        {
            ExtensionData.SetDescription(language, description);
            RefreshFromExtensionData();
            _log.Add(new LogEntry("SetDescription", language, description));
        }

        internal void RemoveDescription(string language)
        {
            ExtensionData.RemoveDescription(language);
            RefreshFromExtensionData();
            _log.Add(new LogEntry("RemoveDescription", language, null));
        }

        internal void SetLabel(string language, string description)
        {
            ExtensionData.SetLabel(language, description);
            RefreshFromExtensionData();
            _log.Add(new LogEntry("SetLabel", language, description));
        }

        internal void RemoveLabel(string language)
        {
            ExtensionData.RemoveLabel(language);
            RefreshFromExtensionData();
            _log.Add(new LogEntry("RemoveLabel", language, null));
        }

        internal void SetSitelink(string site, string title)
        {
            ExtensionData.SetSitelink(site, title);
            RefreshFromExtensionData();
            _log.Add(new LogEntry("SetSitelink", site, title));
        }

        internal void RemoveSitelink(string site)
        {
            ExtensionData.RemoveSitelink(site);
            RefreshFromExtensionData();
            _log.Add(new LogEntry("RemoveSitelink", site, null));
        }

        private string GetSaveComment()
        {
            string output = null;

            var queryOperation =
                from logEntry in _log
                group logEntry by logEntry.operation into operationGroup
                orderby operationGroup.Key
                select operationGroup;

            foreach (var operationGroup in queryOperation)
            {
                output += " " + operationGroup.Key + ": ";
                List<string> comments = new List<string>();
                foreach (var entry in operationGroup)
                {
                    comments.Add(String.Format("{0}", entry.language));
                }
                output += String.Join(" ", comments);
            }


            return output;
        }

        internal void AddAlias(string language, string alias)
        {
            ExtensionData.AddAlias(language, alias);
            RefreshFromExtensionData();
            _log.Add(new LogEntry("AddAlias", language, alias));
        }

        internal void RemoveAlias(string language, string alias)
        {
            ExtensionData.RemoveAlias(language, alias);
            RefreshFromExtensionData();
            _log.Add(new LogEntry("RemoveAlias", language, alias));
        }

        public PSWDStatement AddStatement(Snak snak, Rank rank)
        {
            Statement s = ExtensionData.AddStatement(snak, rank);
            RefreshFromExtensionData();
            _log.Add(new LogEntry("AddStatement", snak.PropertyId.ToString(), null));
            return GetStatement(s.Id);
        }

        internal string Save()
        {
            string comment = GetSaveComment();
            ExtensionData.Save(comment);
            RefreshFromExtensionData();
            _log.Clear();
            return String.Format("Saved item {0}: {1}", QId, comment);
        }
    }
}
