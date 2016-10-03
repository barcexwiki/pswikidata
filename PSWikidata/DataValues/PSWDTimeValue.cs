using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Wikibase.DataValues;

namespace PSWikidata.DataValues
{
    internal class PSWDTimeValue
    {
        public string Time { get; set; }


        public Int32 Before
        {
            get;
            set;
        }

        public Int32 After
        {
            get;
            set;
        }

        public Int32 TimeZoneOffset
        {
            get;
            set;
        }

        public TimeValuePrecision Precision
        {
            get;
            set;
        }

        public CalendarModel DisplayCalendarModel
        {
            get;
            set;
        }

        internal PSWDTimeValue(Wikibase.DataValues.TimeValue t)
        {
            Time = t.Time;
            After = t.After;
            Before = t.Before;
            Precision = t.Precision;
            TimeZoneOffset = t.TimeZoneOffset;
            DisplayCalendarModel = t.DisplayCalendarModel;
        }

        public override string ToString()
        {
            return Time;
        }
    }
}
