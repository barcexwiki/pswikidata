using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using Wikibase;

namespace PSWikidata
{
    public abstract class PSWDValueNetCmdlet : PSWDNetCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "The property has no value.", ParameterSetName = "novalue")]
        public SwitchParameter NoValue { get; set; }

        [Parameter(Mandatory = true, HelpMessage = "The property has some unknown value.", ParameterSetName = "somevalue")]
        public SwitchParameter SomeValue { get; set; }

        [Parameter(Mandatory = true, HelpMessage = "Item that will be the value of the property.", ParameterSetName = "item")]
        [PSWDEntityArgumentTransformationAttribute]
        public PSWDItem ValueItem { get; set; }

        [Parameter(Mandatory = true, HelpMessage = "Text that will be the value of the property.", ParameterSetName = "monolingual")]
        public string ValueText { get; set; }

        [Parameter(Mandatory = true, HelpMessage = "Language code for the text that will be the value of the property.", ParameterSetName = "monolingual")]
        public string ValueLanguage { get; set; }

        [Parameter(Mandatory = true, HelpMessage = "Text that will be the value of the property.", ParameterSetName = "string")]
        public string ValueString { get; set; }

        [Parameter(Mandatory = true, HelpMessage = "Quantity to be assigned to the property.", ParameterSetName = "quantity")]
        public decimal ValueAmount { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Range around the value.", ParameterSetName = "quantity")]
        public decimal ValuePlusMinus { get; set; } = 0;

        [Parameter(Mandatory = false, HelpMessage = "Unit of the quantity value.", ParameterSetName = "quantity")]
        public string ValueUnit { get; set; } = "1";

        [Parameter(Mandatory = true, HelpMessage = "Time in the format [+|-]yyyyyyyyyyyy-mm-ddThh:mm:ssZ.", ParameterSetName = "time")]
        public string ValueTime { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Offset in minutes from UTC.", ParameterSetName = "time")]
        public int ValueTimeZoneOffset { get; set; } = 0;

        [Parameter(Mandatory = false, HelpMessage = "How many units before the given time could it be. The unit is given by the precision.", ParameterSetName = "time")]
        public int ValueBefore { get; set; } = 0;

        [Parameter(Mandatory = false, HelpMessage = "How many units after the given time could it be. The unit is given by the precision.", ParameterSetName = "time")]
        public int ValueAfter { get; set; } = 0;

        [Parameter(Mandatory = false, HelpMessage = "Calendar Model.", ParameterSetName = "time")]
        public Wikibase.DataValues.CalendarModel ValueCalendarModel { get; set; } = Wikibase.DataValues.CalendarModel.GregorianCalendar;

        [Parameter(Mandatory = false, HelpMessage = "Time precision.", ParameterSetName = "time")]
        public Wikibase.DataValues.TimeValuePrecision ValueTimePrecision { get; set; } = Wikibase.DataValues.TimeValuePrecision.Day;

        [Parameter(Mandatory = true, HelpMessage = "Latitude of the coordinate", ParameterSetName = "globecoordinate")]
        public double ValueLatitude { get; set; }

        [Parameter(Mandatory = true, HelpMessage = "Longitude of the coordinate", ParameterSetName = "globecoordinate")]
        public decimal ValueLongitude { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Precision of the coordinate", ParameterSetName = "globecoordinate")]
        public decimal ValueCoordinatePrecision { get; set; } = 0.00027777777777778M;

        [Parameter(Mandatory = false, HelpMessage = "Globe of the coordinate", ParameterSetName = "globecoordinate")]
        public Wikibase.DataValues.Globe ValueGlobe { get; set; } = Wikibase.DataValues.Globe.Earth;

        protected Wikibase.DataValues.DataValue DataValue
        {
            get
            {
                Wikibase.DataValues.DataValue dataValue;

                switch (ParameterSetName)
                {
                    case "novalue":
                    case "somevalue":
                        dataValue = null;
                        break;
                    case "item":
                        dataValue = new Wikibase.DataValues.EntityIdValue(new EntityId(ValueItem.Id));
                        break;
                    case "monolingual":
                        dataValue = new Wikibase.DataValues.MonolingualTextValue(ValueText, ValueLanguage);
                        break;
                    case "string":
                        dataValue = new Wikibase.DataValues.StringValue(ValueString);
                        break;
                    case "quantity":
                        dataValue = new Wikibase.DataValues.QuantityValue(ValueAmount, ValueAmount - ValuePlusMinus, ValueAmount + ValuePlusMinus, ValueUnit);
                        break;
                    case "time":
                        dataValue = new Wikibase.DataValues.TimeValue(ValueTime, ValueTimeZoneOffset, ValueBefore, ValueAfter, ValueTimePrecision, ValueCalendarModel);
                        break;
                    case "globecoordinate":
                        dataValue = new Wikibase.DataValues.GlobeCoordinateValue((double)ValueLatitude, (double)ValueLongitude, (double)ValueCoordinatePrecision, ValueGlobe);
                        break;
                    default:
                        throw new Exception("Unidentified parameter set");
                }

                return dataValue;
            }
        }

        protected SnakType SnakType
        {
            get
            {
                if (NoValue)
                {
                    return SnakType.None;
                }
                else if (SomeValue)
                {
                    return SnakType.SomeValue;
                }
                else
                {
                    return SnakType.Value;
                }
            }
        }
    }
}