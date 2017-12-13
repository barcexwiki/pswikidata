using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wikibase;

namespace PSWikidata
{

    public class PSWDProperty : PSWDEntity
    {

        static Dictionary<PSWDDataType, string> dataTypeStringIdentifiers = new Dictionary<PSWDDataType, string>()
        {
            {PSWDDataType.CommonsMedia,"commonsMedia"},
            {PSWDDataType.GeoShape,"geo-shape"},
            {PSWDDataType.GlobeCoordinate,"globe-coordinate"},
            {PSWDDataType.MonolingualText,"monolingualtext"},
            {PSWDDataType.Quantity,"quantity"},
            {PSWDDataType.String,"string"},
            {PSWDDataType.TabularData,"tabular-data"},
            {PSWDDataType.Time,"time"},
            {PSWDDataType.Url,"url"},
            {PSWDDataType.ExternalId,"external-id"},
            {PSWDDataType.Item,"wikibase-item"},
            {PSWDDataType.Property,"wikibase-property"},
            {PSWDDataType.Math,"math"}
        };

        static Dictionary<string, PSWDDataType> dataTypeEnumIdentifiers = new Dictionary<string, PSWDDataType>()
        {
            {"commonsMedia", PSWDDataType.CommonsMedia},
            {"geo-shape", PSWDDataType.GeoShape},
            {"globe-coordinate", PSWDDataType.GlobeCoordinate},
            {"monolingualtext", PSWDDataType.MonolingualText},
            {"quantity", PSWDDataType.Quantity},
            {"string", PSWDDataType.String},
            {"tabular-data", PSWDDataType.TabularData},
            {"time", PSWDDataType.Time},
            {"url", PSWDDataType.Url},
            {"external-id", PSWDDataType.ExternalId},
            {"wikibase-item", PSWDDataType.Item},
            {"wikibase-property", PSWDDataType.Property},
            {"math", PSWDDataType.Math}
        };

        private PSWDDataType _dataType;

        internal PSWDProperty(Property item) : base(item)
        {
        }

        internal PSWDProperty(WikibaseApi api, PSWDDataType dataType) : this(new Property(api, dataTypeStringIdentifiers[dataType]))
        {
            _dataType = dataType;
        }

        public PSWDDataType DataType
        {
            get { return _dataType; }
        }


        internal override void RefreshFromExtensionData()
        {
            base.RefreshFromExtensionData();

            try
            {
                _dataType = dataTypeEnumIdentifiers[((Property)ExtensionData).DataType];
            }
            catch (KeyNotFoundException)
            {
                throw new ApplicationException($"Unknown property value type: {((Property)ExtensionData).DataType}");
            }
        }

        internal override string Save()
        {
            return base.Save("property");
        }
    }
}
