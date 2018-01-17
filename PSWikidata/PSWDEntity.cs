using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wikibase;

namespace PSWikidata
{
    public abstract class PSWDEntity
    {
        protected struct LogEntry
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

        internal static PSWDEntity GetPSWDEntity(Entity entity)
        {
            switch (entity)
            {
                case Item c: 
                    return new PSWDItem(c);
                case Property p: 
                    return new PSWDProperty(p);
                default:
                    throw new ArgumentException("Unknown object type", nameof(entity));
                case null:
                    throw new ArgumentNullException(nameof(entity));
            }
            
        }

        private List<PSWDDescription> _descriptions = new List<PSWDDescription>();
        private List<PSWDLabel> _labels = new List<PSWDLabel>();
        private List<PSWDLabel> _aliases = new List<PSWDLabel>();
        private List<PSWDClaim> _claims = new List<PSWDClaim>();
        protected List<LogEntry> _log = new List<LogEntry>();

        internal PSWDEntity(Entity entity)
        {
            ExtensionData = entity;
            RefreshFromExtensionData();
        }

        public string Status { get; private set; }

        public string Id { get; private set; }

        public PSWDDescription[] Descriptions { get => _descriptions.ToArray(); }

        public PSWDLabel[] Labels { get => _labels.ToArray(); }

        public PSWDLabel[] Aliases { get => _aliases.ToArray(); }

        public PSWDClaim[] Claims { get => _claims.ToArray(); }

        internal Entity ExtensionData { get; set; }

        internal virtual void RefreshFromExtensionData()
        {
            _descriptions.Clear();
            _labels.Clear();
            _aliases.Clear();
            _claims.Clear();

            Id = ExtensionData.Id != null ? ExtensionData.Id.ToString() : null;

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

            Status = ExtensionData.Status.ToString();
        }

        internal PSWDStatement GetStatement(string Id)
        {
            var s = from c in Claims
                    where c.ExtensionData.Id == Id
                    select c;

            return s.Any() ? (PSWDStatement)s.First() : null;
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
                    comments.Add(entry.language);
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

        public void Delete()
        {
            ExtensionData.Delete();
            RefreshFromExtensionData();
            _log.Add(new LogEntry("Delete", Id, null));
        }

        internal virtual string Save(string entityType)
        {
            string comment = GetSaveComment();
            ExtensionData.Save(comment);
            RefreshFromExtensionData();
            _log.Clear();
            return $"Saved {entityType} {Id}: {comment} ";
        }

        internal virtual string Save() => throw new InvalidOperationException("PSWDEntity Save() cannot be called.");       
    }
}
