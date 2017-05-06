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


        public long Year
        {
            get;
            set;
        }

        public int Month
        {
            get;
            set;
        }

        public int Day
        {
            get;
            set;
        }

        public int Hour
        {
            get;
            set;
        }

        public int Minute
        {
            get;
            set;
        }

        public int Second
        {
            get;
            set;
        }


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
            Year = t.Year;
            Month = t.Month;
            Day = t.Day;
            Hour = t.Hour;
            Hour = t.Minute;
            Hour = t.Second;
        }

        public override string ToString()
        {
            return Time;
        }
    }
}
