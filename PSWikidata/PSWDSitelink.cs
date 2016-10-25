using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSWikidata
{
    public class PSWDSitelink
    {
        private string _site;
        private string _title;

        internal PSWDSitelink(string language, string title)
        {
            _site = language;
            _title = title;
        }

        public string Site
        {
            get { return _site; }
            protected set { _site = value; }
        }

        public string Title
        {
            get { return _title; }
            protected set { _title = value; }
        }

        public override string ToString()
        {
            return _site + ":" + _title;
        }
    }
}