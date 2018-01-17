using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PSWikidata.DataValues;
using Wikibase;
using Wikibase.DataValues;

namespace PSWikidata
{
    public enum PSWDValueTypes
    {
        String,
        EntityId,
        Time,
        Quantity,
        MonolingualText,
        GlobeCoordinate
    }

    public class PSWDClaim
    {
        public string Property { get; set; }
        public Wikibase.SnakType Type { get; set; }
        public PSWDValueTypes ValueType { get; set; }
        public object Value { get; private set;}

        public PSWDSnak[] Qualifiers { get => _qualifiers.ToArray(); }

        internal PSWDEntity Item { get; set; }

        private List<PSWDSnak> _qualifiers = new List<PSWDSnak>();

        internal Wikibase.Claim ExtensionData { get; set; }

        internal PSWDClaim(PSWDEntity item, Wikibase.Claim claim)
        {
            ExtensionData = claim;
            Item = item;
            RefreshFromExtensionData();
        }

        internal virtual void RefreshFromExtensionData()
        {
            Claim claim = ExtensionData;
            Property = claim.MainSnak.PropertyId.PrefixedId;
            Type = claim.MainSnak.Type;

            if (claim.MainSnak.DataValue is StringValue)
            {
                ValueType = PSWDValueTypes.String;
                Value = new PSWDStringValue((StringValue)claim.MainSnak.DataValue);
            }

            if (claim.MainSnak.DataValue is EntityIdValue)
            {
                ValueType = PSWDValueTypes.EntityId;
                Value = new PSWDEntityIdValue((EntityIdValue)claim.MainSnak.DataValue);
            }

            if (claim.MainSnak.DataValue is GlobeCoordinateValue)
            {
                ValueType = PSWDValueTypes.GlobeCoordinate;
                Value = new PSWDGlobeCoordinateValue((GlobeCoordinateValue)claim.MainSnak.DataValue);
            }

            if (claim.MainSnak.DataValue is TimeValue)
            {
                ValueType = PSWDValueTypes.Time;
                Value = new PSWDTimeValue((TimeValue)claim.MainSnak.DataValue);
            }

            if (claim.MainSnak.DataValue is MonolingualTextValue)
            {
                ValueType = PSWDValueTypes.MonolingualText;
                Value = new PSWDMonolingualTextValue((MonolingualTextValue)claim.MainSnak.DataValue);
            }

            if (claim.MainSnak.DataValue is QuantityValue)
            {
                ValueType = PSWDValueTypes.Quantity;
                Value = new PSWDQuantityValue((QuantityValue)claim.MainSnak.DataValue);
            }

            _qualifiers.Clear();
            foreach (Qualifier q in claim.Qualifiers)
            {
                _qualifiers.Add(new PSWDSnak(q));
            }
        }

        internal void AddQualifier(SnakType type, string propertyId, DataValue dataValue)
        {
            ExtensionData.AddQualifier(type, new EntityId(propertyId), dataValue);
            RefreshFromExtensionData();
        }
    }
}
