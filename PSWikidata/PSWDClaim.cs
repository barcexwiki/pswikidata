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
        public Wikibase.Rank Rank { get; set; }
        public Wikibase.SnakType Type { get; set; }
        public PSWDValueTypes ValueType { get; set; }
        public object Value {
            get {
                if (dataValue != null)
                {
                    return dataValue;
                }
                else
                {
                    return null;
                }

            }
        }

        private object dataValue;

        internal Wikibase.Claim ExtensionData {get; set;}

        internal PSWDClaim(Wikibase.Claim claim)
        {
            this.ExtensionData = claim;
            RefreshFromClaim(claim);
        }

        internal void RefreshFromClaim(Wikibase.Claim claim)
        {
            Property = claim.mainSnak.PropertyId.PrefixedId;
            Rank = ((Statement)claim).Rank;
            Type = claim.mainSnak.Type;

            if (claim.mainSnak.DataValue is StringValue)
            {
                ValueType = PSWDValueTypes.String;
                dataValue = new PSWDStringValue((StringValue)claim.mainSnak.DataValue);
            }

            if (claim.mainSnak.DataValue is EntityIdValue)
            {
                ValueType = PSWDValueTypes.EntityId;
                dataValue = new PSWDEntityIdValue((EntityIdValue)claim.mainSnak.DataValue);
            }

            if (claim.mainSnak.DataValue is GlobeCoordinateValue)
            {
                ValueType = PSWDValueTypes.GlobeCoordinate;
                dataValue = new PSWDGlobeCoordinateValue((GlobeCoordinateValue)claim.mainSnak.DataValue);
            }

            if (claim.mainSnak.DataValue is TimeValue)
            {
                ValueType = PSWDValueTypes.Time;
                dataValue = new PSWDTimeValue((TimeValue)claim.mainSnak.DataValue);
            }

            if (claim.mainSnak.DataValue is MonolingualTextValue)
            {
                ValueType = PSWDValueTypes.MonolingualText;
                dataValue = new PSWDMonolingualTextValue((MonolingualTextValue)claim.mainSnak.DataValue);
            }

            if (claim.mainSnak.DataValue is QuantityValue)
            {
                ValueType = PSWDValueTypes.Quantity;
                dataValue = new PSWDQuantityValue((QuantityValue)claim.mainSnak.DataValue);
            } 

        }
    }
}
