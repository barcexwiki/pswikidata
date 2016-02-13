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
    public class AddWDStatement : PSWDValueNetCmdlet
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
            get { return _multiple; }
            set { _multiple = value; }
        }
        private bool _multiple;



        protected override void BeginProcessing()
        {
            base.BeginProcessing();
        }

        private bool IsDuplicatedStatement(Wikibase.DataValues.DataValue dataValue)
        {
            
            Claim[] claims = Item.ExtensionData.GetClaims(Property.ToUpper());

            var sameValueClaims = from c in claims
                                  where c.MainSnak.DataValue.Equals(dataValue)
                                  select c;

            return sameValueClaims.Any();

        }

        protected override void ProcessRecord()
        {

            var dataValue = DataValue;

            if (Multiple || !IsDuplicatedStatement(dataValue))
            {

                if (ShouldProcess(Item.QId, "Adding claim"))
                {

                    Snak snak = new Snak(SnakType.Value,
                                    new EntityId(Property),
                                    dataValue
                                    );

                    Item.ExtensionData.AddStatement(snak, Rank.Normal);

                    string comment = String.Format("Adding claim {0} {1}", Property, dataValue.ToString());
                    Item.ExtensionData.Save(comment);

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
