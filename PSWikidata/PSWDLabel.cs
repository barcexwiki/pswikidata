using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSWikidata
{
    public class PSWDLabel
    {
        private string _language;
        private string _label;

        internal PSWDLabel(string language, string label)
        {
            _language = language;
            _label = label;
        }

        public string Language
        {
            get { return _language; }
            protected set { _language = value; }
        }

        public string Label
        {
            get { return _label; }
            protected set { _label = value; }
        }

        public override string ToString()
        {
            return _language + ":" + _label;
        }
    }
}