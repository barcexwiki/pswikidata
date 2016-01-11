using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSWikidata
{
    public class PSWDLabel
    {
        private string language;
        private string label;

        internal PSWDLabel(string language, string label)
        {
            this.language = language;
            this.label = label;               
        }

        public string Language
        {
            get { return language; }
            protected set { language = value; }
        }

        public string Label
        {
            get { return label; }
            protected set { label = value; }
        }

        public override string ToString()
        {
            return language + ":" + label;
        }

    }
}