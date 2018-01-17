using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSWikidata
{
    public class PSWDDescription
    {

        internal PSWDDescription(string language, string description)
        {
            Language = language;
            Description = description;
        }

        public string Language { get; protected set; }

        public string Description { get; protected set; }

        public override string ToString() => $"{Language}:{Description}";

    }
}
