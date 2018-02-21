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

        internal static PSWDEntity CreateStubPSWDEntity(EntityProvider provider, string Id)
        {
            // This factory method creates a PSWDEntity in stub mode
            // Stub mode only contains the entity ID with the data not 
            // loaded from the server. The data is loaded from the server
            // just in time when it is actually read. Some parameters in 
            // cmdlets only need the entity Id and contacting the server 
            // every time slows them.
            var entityId = new EntityId(Id);

            switch (entityId.Type)
            {
                case EntityType.Item:
                    return new PSWDItem(provider, entityId);
                case EntityType.Property:
                    return new PSWDProperty(provider, entityId);
                default:
                    throw new ApplicationException("Unknown entity type");
            }

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

        private EntityProvider _stubProvider;
        private EntityId _stubEntityId;
        private List<PSWDDescription> _descriptions = new List<PSWDDescription>();
        private List<PSWDLabel> _labels = new List<PSWDLabel>();
        private List<PSWDLabel> _aliases = new List<PSWDLabel>();
        private List<PSWDClaim> _claims = new List<PSWDClaim>();
        protected List<LogEntry> _log = new List<LogEntry>();

        public PSWDEntity(EntityProvider provider, EntityId entityId)
        {
            // this constructor creates a stub entity
            // the provider and entityId are stored in a private field
            // until they are needed to load the whole item from
            // the server
            ExtensionData = null;
            Status = "Stub";
            _stubProvider = provider;
            _stubEntityId = entityId;
            Id = entityId.PrefixedId;
        }

        internal PSWDEntity(Entity entity)
        {
            ExtensionData = entity;
            RefreshFromExtensionData();
        }

        public string Status
        {
            get
            {
                LoadIfStub();
                return _status;
            }
            private set => _status = value;
        }
        private string _status;

        public string Id { get; private set; }

        public PSWDDescription[] Descriptions { get { LoadIfStub(); return _descriptions.ToArray(); } }

        public PSWDLabel[] Labels { get { LoadIfStub(); return _labels.ToArray(); } }

        public PSWDLabel[] Aliases { get { LoadIfStub(); return _aliases.ToArray(); } }

        public PSWDClaim[] Claims { get { LoadIfStub(); return _claims.ToArray(); } }

        private Entity _extensionData;
        internal Entity ExtensionData 
        { 
            get 
            {
                LoadIfStub();
                return _extensionData;
            }
        
            set => _extensionData = value;        
        }

        protected void LoadIfStub()
        {
            // If the entity is in the stub status the data is loaded from the server            
            if (_status == "Stub" && _extensionData == null)
            {
                ExtensionData = _stubProvider.GetEntityFromId(_stubEntityId);
                if (_extensionData != null)
                {
                    RefreshFromExtensionData();
                }
                else
                {
                    throw new ApplicationException($"Could not load {_stubEntityId.PrefixedId} from server");
                }
            }
        }

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
            LoadIfStub();
            var s = from c in Claims
                    where c.ExtensionData.Id == Id
                    select c;

            return s.Any() ? (PSWDStatement)s.First() : null;
        }

        internal void SetDescription(string language, string description)
        {
            LoadIfStub();
            ExtensionData.SetDescription(language, description);
            RefreshFromExtensionData();
            _log.Add(new LogEntry("SetDescription", language, description));
        }

        internal void RemoveDescription(string language)
        {
            LoadIfStub();
            ExtensionData.RemoveDescription(language);
            RefreshFromExtensionData();
            _log.Add(new LogEntry("RemoveDescription", language, null));
        }

        internal void SetLabel(string language, string description)
        {
            LoadIfStub();
            ExtensionData.SetLabel(language, description);
            RefreshFromExtensionData();
            _log.Add(new LogEntry("SetLabel", language, description));
        }

        internal void RemoveLabel(string language)
        {
            LoadIfStub();
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
                output += $" {operationGroup.Key}: ";
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
            LoadIfStub();
            ExtensionData.AddAlias(language, alias);
            RefreshFromExtensionData();
            _log.Add(new LogEntry("AddAlias", language, alias));
        }

        internal void RemoveAlias(string language, string alias)
        {
            LoadIfStub();
            ExtensionData.RemoveAlias(language, alias);
            RefreshFromExtensionData();
            _log.Add(new LogEntry("RemoveAlias", language, alias));
        }

        public PSWDStatement AddStatement(Snak snak, Rank rank)
        {
            LoadIfStub();
            Statement s = ExtensionData.AddStatement(snak, rank);
            RefreshFromExtensionData();
            _log.Add(new LogEntry("AddStatement", snak.PropertyId.ToString(), null));
            return GetStatement(s.Id);
        }

        public void Delete()
        {
            LoadIfStub();
            ExtensionData.Delete();
            RefreshFromExtensionData();
            _log.Add(new LogEntry("Delete", Id, null));
        }

        internal virtual string Save(string entityType)
        {
            LoadIfStub();
            string comment = GetSaveComment();
            ExtensionData.Save(comment);
            RefreshFromExtensionData();
            _log.Clear();
            return $"Saved {entityType} {Id}: {comment} ";
        }

        internal virtual string Save() => throw new InvalidOperationException("PSWDEntity Save() cannot be called.");
    }
}
