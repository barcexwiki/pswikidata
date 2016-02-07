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
        private List<PSWDSnak> _qualifiers = new List<PSWDSnak>();

        public string Property { get; set; }
        public Wikibase.SnakType Type { get; set; }
        public PSWDValueTypes ValueType { get; set; }
        public object Value {
            get {
                if (_dataValue != null)
                {
                    return _dataValue;
                }
                else
                {
                    return null;
                }

            }
        }
        public PSWDSnak[] Qualifiers
        {
            get { return _qualifiers.ToArray(); }
        }

        private object _dataValue;

        internal Wikibase.Claim ExtensionData {get; set;}

        internal PSWDClaim(Wikibase.Claim claim)
        {
            this.ExtensionData = claim;
            RefreshFromClaim(claim);
        }

        internal void RefreshFromClaim(Wikibase.Claim claim)
        {
            Property = claim.MainSnak.PropertyId.PrefixedId;
            Type = claim.MainSnak.Type;

            if (claim.MainSnak.DataValue is StringValue)
            {
                ValueType = PSWDValueTypes.String;
                _dataValue = new PSWDStringValue((StringValue)claim.MainSnak.DataValue);
            }

            if (claim.MainSnak.DataValue is EntityIdValue)
            {
                ValueType = PSWDValueTypes.EntityId;
                _dataValue = new PSWDEntityIdValue((EntityIdValue)claim.MainSnak.DataValue);
            }

            if (claim.MainSnak.DataValue is GlobeCoordinateValue)
            {
                ValueType = PSWDValueTypes.GlobeCoordinate;
                _dataValue = new PSWDGlobeCoordinateValue((GlobeCoordinateValue)claim.MainSnak.DataValue);
            }

            if (claim.MainSnak.DataValue is TimeValue)
            {
                ValueType = PSWDValueTypes.Time;
                _dataValue = new PSWDTimeValue((TimeValue)claim.MainSnak.DataValue);
            }

            if (claim.MainSnak.DataValue is MonolingualTextValue)
            {
                ValueType = PSWDValueTypes.MonolingualText;
                _dataValue = new PSWDMonolingualTextValue((MonolingualTextValue)claim.MainSnak.DataValue);
            }

            if (claim.MainSnak.DataValue is QuantityValue)
            {
                ValueType = PSWDValueTypes.Quantity;
                _dataValue = new PSWDQuantityValue((QuantityValue)claim.MainSnak.DataValue);
            }

            _qualifiers.Clear();
            foreach ( Qualifier q in claim.Qualifiers)
            {
                _qualifiers.Add(new PSWDSnak(q));
            }

        }
    }
}
