using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSWikidata
{
    public class PSWDSitelink
    {
        private string _language;
        private string _title;

        internal PSWDSitelink(string language, string title)
        {
            _language = language;
            _title = title;
        }

        public string Language
        {
            get { return _language; }
            protected set { _language = value; }
        }

        public string Title
        {
            get { return _title; }
            protected set { _title = value; }
        }

        public override string ToString()
        {
            return _language + ":" + _title;
        }
    }
}