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
        public object Value { get; private set; }

        internal Wikibase.Snak ExtensionData { get; set; }

        internal PSWDSnak(Wikibase.Snak snak)
        {
            ExtensionData = snak;
            RefreshFromExtensionData(snak);
        }

        internal void RefreshFromExtensionData(Wikibase.Snak snak)
        {
            Property = snak.PropertyId.PrefixedId;
            Type = snak.Type;

            if (snak.DataValue is StringValue)
            {
                ValueType = PSWDValueTypes.String;
                Value = new PSWDStringValue((StringValue)snak.DataValue);
            }

            if (snak.DataValue is EntityIdValue)
            {
                ValueType = PSWDValueTypes.EntityId;
                Value = new PSWDEntityIdValue((EntityIdValue)snak.DataValue);
            }

            if (snak.DataValue is GlobeCoordinateValue)
            {
                ValueType = PSWDValueTypes.GlobeCoordinate;
                Value = new PSWDGlobeCoordinateValue((GlobeCoordinateValue)snak.DataValue);
            }

            if (snak.DataValue is TimeValue)
            {
                ValueType = PSWDValueTypes.Time;
                Value = new PSWDTimeValue((TimeValue)snak.DataValue);
            }

            if (snak.DataValue is MonolingualTextValue)
            {
                ValueType = PSWDValueTypes.MonolingualText;
                Value = new PSWDMonolingualTextValue((MonolingualTextValue)snak.DataValue);
            }

            if (snak.DataValue is QuantityValue)
            {
                ValueType = PSWDValueTypes.Quantity;
                Value = new PSWDQuantityValue((QuantityValue)snak.DataValue);
            }
        }
    }
}