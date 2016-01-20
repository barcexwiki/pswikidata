using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using Wikibase;

namespace PSWikidata
{

    [Cmdlet(VerbsCommon.Add, "WDStatement",
        SupportsShouldProcess = true,
        ConfirmImpact = ConfirmImpact.Medium)]
    public class AddWDStatement : PSWDNetCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, HelpMessage = "Item to be modified.", ParameterSetName = "item")]
        [Parameter(Mandatory = true, ValueFromPipeline = true, HelpMessage = "Item to be modified.", ParameterSetName = "monolingual")]
        [Parameter(Mandatory = true, ValueFromPipeline = true, HelpMessage = "Item to be modified.", ParameterSetName = "string")]
        [Parameter(Mandatory = true, ValueFromPipeline = true, HelpMessage = "Item to be modified.", ParameterSetName = "quantity")]
        [Parameter(Mandatory = true, ValueFromPipeline = true, HelpMessage = "Item to be modified.", ParameterSetName = "time")]
        [Parameter(Mandatory = true, ValueFromPipeline = true, HelpMessage = "Item to be modified.", ParameterSetName = "globecoordinate")]
        public PSWDItem Item { get; set; }

        [Parameter(Mandatory = true, HelpMessage = "Property for the statement.", ParameterSetName = "item")]
        [Parameter(Mandatory = true, HelpMessage = "Property for the statement.", ParameterSetName = "monolingual")]
        [Parameter(Mandatory = true, HelpMessage = "Property for the statement.", ParameterSetName = "string")]
        [Parameter(Mandatory = true, HelpMessage = "Property for the statement.", ParameterSetName = "quantity")]
        [Parameter(Mandatory = true, HelpMessage = "Property for the statement.", ParameterSetName = "time")]
        [Parameter(Mandatory = true, HelpMessage = "Property for the statement.", ParameterSetName = "globecoordinate")]
        public string Property { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Add the statement even if there is already an statement for this property.", ParameterSetName = "item")]
        [Parameter(Mandatory = false, HelpMessage = "Add the statement even if there is already an statement for this property.", ParameterSetName = "monolingual")]
        [Parameter(Mandatory = false, HelpMessage = "Add the statement even if there is already an statement for this property.", ParameterSetName = "string")]
        [Parameter(Mandatory = false, HelpMessage = "Add the statement even if there is already an statement for this property.", ParameterSetName = "time")]
        [Parameter(Mandatory = false, HelpMessage = "Add the statement even if there is already an statement for this property.", ParameterSetName = "globecoordinate")]
        public SwitchParameter Multiple
        {
            get { return multiple; }
            set { multiple = value; }
        }
        private bool multiple;


        [Parameter(Mandatory = true, HelpMessage = "Item that will be the value of the property.", ParameterSetName = "item")]
        public string ValueItem { get; set; }

        [Parameter(Mandatory = true, HelpMessage = "Text that will be the value of the property.", ParameterSetName = "monolingual")]
        public string ValueText { get; set; }

        [Parameter(Mandatory = true, HelpMessage = "Language code for the text that will be the value of the property.", ParameterSetName = "monolingual")]
        public string ValueLanguage { get; set; }

        [Parameter(Mandatory = true, HelpMessage = "Text that will be the value of the property.", ParameterSetName = "string")]
        public string ValueString { get; set; }

        [Parameter(Mandatory = true, HelpMessage = "Quantity to be assigned to the property.", ParameterSetName = "quantity")]
        public decimal ValueAmount { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Range around the value.", ParameterSetName = "quantity")]
        public decimal ValuePlusMinus
        {
            get { return valuePlusMinus; }
            set { valuePlusMinus = value; }
        }
        private decimal valuePlusMinus = 0;

        [Parameter(Mandatory = false, HelpMessage = "Unit of the quantity value.", ParameterSetName = "quantity")]
        public string ValueUnit
        {
            get { return valueUnit; }
            set { valueUnit = value; }
        }
        private string valueUnit = "1";

        [Parameter(Mandatory = true, HelpMessage = "Time in the forma [+|-]yyyyyyyyyyyy-mm-ddThh:mm:ssZ.", ParameterSetName = "time")]
        public string ValueTime { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Offset in minutes from UTC.", ParameterSetName = "time")]
        public int ValueTimeZoneOffset 
        {
            get { return valueTimeZoneOffset; }
            set { valueTimeZoneOffset = value; } 
        }
        private int valueTimeZoneOffset = 0;

        [Parameter(Mandatory = false, HelpMessage = "How many units before the given time could it be. The unit is given by the precision.", ParameterSetName = "time")]
        public int ValueBefore
        {
            get { return valueBefore; }
            set { valueBefore = value; }
        }
        private int valueBefore = 0;

        [Parameter(Mandatory = false, HelpMessage = "How many units after the given time could it be. The unit is given by the precision.", ParameterSetName = "time")]
        public int ValueAfter
        {
            get { return valueAfter; }
            set { valueAfter = value; }
        }
        private int valueAfter = 0;

        [Parameter(Mandatory = false, HelpMessage = "Calendar Model.", ParameterSetName = "time")]
        public Wikibase.DataValues.CalendarModel ValueCalendarModel 
        {
            get { return valueCalendarModel; }
            set { valueCalendarModel = value; }
        }
        private Wikibase.DataValues.CalendarModel valueCalendarModel = Wikibase.DataValues.CalendarModel.GregorianCalendar;

        [Parameter(Mandatory = false, HelpMessage = "Time precision.", ParameterSetName = "time")]
        public Wikibase.DataValues.TimeValuePrecision ValueTimePrecision
        {
            get { return valueTimePrecision; }
            set { valueTimePrecision = value; }
        }
        private Wikibase.DataValues.TimeValuePrecision valueTimePrecision = Wikibase.DataValues.TimeValuePrecision.Day;


        [Parameter(Mandatory = true, HelpMessage = "Latitude of the coordinate", ParameterSetName = "globecoordinate")]
        public double ValueLatitude { get; set; }

        [Parameter(Mandatory = true, HelpMessage = "Longitude of the coordinate", ParameterSetName = "globecoordinate")]
        public decimal ValueLongitude { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Precision of the coordinate", ParameterSetName = "globecoordinate")]
        public decimal ValueCoordinatePrecision
        {
            get { return valueCoordinatePrecision; }
            set { valueCoordinatePrecision = value; }
        }
        private decimal valueCoordinatePrecision = 0.00027777777777778M;

        [Parameter(Mandatory = false, HelpMessage = "Globe of the coordinate", ParameterSetName = "globecoordinate")]
        public Wikibase.DataValues.Globe ValueGlobe 
        {
            get { return valueGlobe; }
            set { valueGlobe = value; }
        }
        private Wikibase.DataValues.Globe valueGlobe = Wikibase.DataValues.Globe.Earth;

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
        }

        private bool IsDuplicatedStatement(Wikibase.DataValues.DataValue dataValue)
        {
            
            Dictionary<string,Claim> claims = Item.ExtensionData.getClaimsForProperty(Property.ToUpper());

            bool duplicate = false;

            if (claims != null)
            {
                foreach (Claim c in claims.Values)
                {
                    if (c.mainSnak.DataValue.Equals(dataValue))
                    {
                        duplicate = true;
                        break;
                    }
                }
            }

            return duplicate;
        }

        protected override void ProcessRecord()
        {

            Wikibase.DataValues.DataValue dataValue;

            switch (this.ParameterSetName)
            {
                case "item":
                    dataValue = new Wikibase.DataValues.EntityIdValue(new EntityId(ValueItem));
                    break;
                case "monolingual":
                    dataValue = new Wikibase.DataValues.MonolingualTextValue(ValueText,ValueLanguage);
                    break;
                case "string":
                    dataValue = new Wikibase.DataValues.StringValue(ValueString);
                    break;
                case "quantity":
                    dataValue = new Wikibase.DataValues.QuantityValue(ValueAmount, ValueAmount - ValuePlusMinus, ValueAmount + ValuePlusMinus, ValueUnit);
                    break;
                case "time":
                    dataValue = new Wikibase.DataValues.TimeValue(ValueTime,valueTimeZoneOffset,ValueBefore,ValueAfter,ValueTimePrecision,ValueCalendarModel);
                    break;
                case "globecoordinate":
                    dataValue = new Wikibase.DataValues.GlobeCoordinateValue((double)ValueLatitude, (double)ValueLongitude, (double)ValueCoordinatePrecision, ValueGlobe);
                    break;
                default:
                    throw new Exception("Unidentified parameter set");
            }


            if (Multiple || !IsDuplicatedStatement(dataValue))
            {

                if (ShouldProcess(Item.QId, "Adding claim"))
                {

                    Snak snak = new Snak(SnakType.Value,
                                    new EntityId(Property),
                                    dataValue
                                    );

                    Item.ExtensionData.createStatementForSnak(snak);

                    string comment = String.Format("Adding claim {0} {1}", Property, dataValue.ToString());
                    Item.ExtensionData.save(comment);

                    Item.RefreshFromExtensionData();

                    WriteVerbose(comment);

                }
            }

            WriteObject(Item, true);

        }


        protected override void EndProcessing()
        {
            base.EndProcessing();
        }


    }


}
