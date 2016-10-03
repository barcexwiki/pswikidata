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
    public class PSWDSnak
    {
        public string Property { get; set; }
        public Wikibase.SnakType Type { get; set; }
        public PSWDValueTypes ValueType { get; set; }
        public object Value
        {
            get
            {
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

        private object _dataValue;

        internal Wikibase.Snak ExtensionData { get; set; }

        internal PSWDSnak(Wikibase.Snak snak)
        {
            this.ExtensionData = snak;
            RefreshFromExtensionData(snak);
        }

        internal void RefreshFromExtensionData(Wikibase.Snak snak)
        {
            Property = snak.PropertyId.PrefixedId;
            Type = snak.Type;

            if (snak.DataValue is StringValue)
            {
                ValueType = PSWDValueTypes.String;
                _dataValue = new PSWDStringValue((StringValue)snak.DataValue);
            }

            if (snak.DataValue is EntityIdValue)
            {
                ValueType = PSWDValueTypes.EntityId;
                _dataValue = new PSWDEntityIdValue((EntityIdValue)snak.DataValue);
            }

            if (snak.DataValue is GlobeCoordinateValue)
            {
                ValueType = PSWDValueTypes.GlobeCoordinate;
                _dataValue = new PSWDGlobeCoordinateValue((GlobeCoordinateValue)snak.DataValue);
            }

            if (snak.DataValue is TimeValue)
            {
                ValueType = PSWDValueTypes.Time;
                _dataValue = new PSWDTimeValue((TimeValue)snak.DataValue);
            }

            if (snak.DataValue is MonolingualTextValue)
            {
                ValueType = PSWDValueTypes.MonolingualText;
                _dataValue = new PSWDMonolingualTextValue((MonolingualTextValue)snak.DataValue);
            }

            if (snak.DataValue is QuantityValue)
            {
                ValueType = PSWDValueTypes.Quantity;
                _dataValue = new PSWDQuantityValue((QuantityValue)snak.DataValue);
            }
        }
    }
}