using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PSWikidata.DataValues
{
    public class PSWDGlobeCoordinateValue
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Precision { get; set; }
        public Wikibase.DataValues.Globe Globe { get; set; }

        internal PSWDGlobeCoordinateValue(Wikibase.DataValues.GlobeCoordinateValue c)
        {
            Globe = c.Globe;
            Latitude = c.Latitude;
            Longitude = c.Longitude;
            Precision = c.Precision;
        }

        public override string ToString()
        {
            return $"({Latitude},{Longitude}) {Globe}";
        }
    }
}
