using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSWikidata
{
    public class PSWDDescription
    {
        private string _language;
        private string _description;

        internal PSWDDescription(string language, string description)
        {
            _language = language;
            _description = description;
        }

        public string Language
        {
            get { return _language; }
            protected set { _language = value; }
        }

        public string Description
        {
            get { return _description; }
            protected set { _description = value; }
        }

        public override string ToString()
        {
            return _language + ":" + _description;
        }
    }
}
