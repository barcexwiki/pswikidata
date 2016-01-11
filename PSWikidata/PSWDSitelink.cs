using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSWikidata
{
    public class PSWDSitelink
    {
        private string language;
        private string title;

        internal PSWDSitelink(string language, string title)
        {
            this.language = language;
            this.title = title;               
        }

        public string Language
        {
            get { return language; }
            protected set { language = value; }
        }

        public string Title
        {
            get { return title; }
            protected set { title = value; }
        }

        public override string ToString()
        {
            return language + ":" + title;
        }

    }
}