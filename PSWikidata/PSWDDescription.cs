using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSWikidata
{
    public class PSWDDescription
    {
        private string language;
        private string description;

        internal PSWDDescription(string language, string description)
        {
            this.language = language;
            this.description = description;               
        }

        public string Language
        {
            get { return language; }
            protected set { language = value; }
        }

        public string Description
        {
            get { return description; }
            protected set { description = value; }
        }

        public override string ToString()
        {
            return language + ":" + description;
        }

    }
}
