using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PSWikidata.DataValues
{
    class PSWDGlobeCoordinateValue
    {
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public decimal Precision { get; set; }
        public Wikibase.DataValues.Globe Globe { get; set; }

        internal PSWDGlobeCoordinateValue(Wikibase.DataValues.GlobeCoordinateValue c)
        {
            this.Globe = c.Globe; 
            this.Latitude = c.Latitude; 
            this.Longitude = c.Longitude; 
            this.Precision = c.Precision;
        }

        public override string ToString()
        {
            return String.Format("({0},{1}) {2}",Latitude,Longitude,Globe);
        }
    }
}
