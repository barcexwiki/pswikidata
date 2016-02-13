using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using Wikibase;

namespace PSWikidata
{

    [Cmdlet(VerbsCommon.Add, "WDQualifier",
        SupportsShouldProcess = true,
        ConfirmImpact = ConfirmImpact.Medium)]
    public class AddWDQualifier : PSWDValueNetCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, HelpMessage = "Item to be modified.", ParameterSetName = "item")]
        [Parameter(Mandatory = true, ValueFromPipeline = true, HelpMessage = "Item to be modified.", ParameterSetName = "monolingual")]
        [Parameter(Mandatory = true, ValueFromPipeline = true, HelpMessage = "Item to be modified.", ParameterSetName = "string")]
        [Parameter(Mandatory = true, ValueFromPipeline = true, HelpMessage = "Item to be modified.", ParameterSetName = "quantity")]
        [Parameter(Mandatory = true, ValueFromPipeline = true, HelpMessage = "Item to be modified.", ParameterSetName = "time")]
        [Parameter(Mandatory = true, ValueFromPipeline = true, HelpMessage = "Item to be modified.", ParameterSetName = "globecoordinate")]
        public PSWDClaim Claim { get; set; }

        [Parameter(Mandatory = true, HelpMessage = "Property for the statement.", ParameterSetName = "item")]
        [Parameter(Mandatory = true, HelpMessage = "Property for the statement.", ParameterSetName = "monolingual")]
        [Parameter(Mandatory = true, HelpMessage = "Property for the statement.", ParameterSetName = "string")]
        [Parameter(Mandatory = true, HelpMessage = "Property for the statement.", ParameterSetName = "quantity")]
        [Parameter(Mandatory = true, HelpMessage = "Property for the statement.", ParameterSetName = "time")]
        [Parameter(Mandatory = true, HelpMessage = "Property for the statement.", ParameterSetName = "globecoordinate")]
        public string Property { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Add the statement even if there is already a qualifier for this property.", ParameterSetName = "item")]
        [Parameter(Mandatory = false, HelpMessage = "Add the statement even if there is already a qualifier for this property.", ParameterSetName = "monolingual")]
        [Parameter(Mandatory = false, HelpMessage = "Add the statement even if there is already a qualifier for this property.", ParameterSetName = "string")]
        [Parameter(Mandatory = false, HelpMessage = "Add the statement even if there is already a qualifier for this property.", ParameterSetName = "time")]
        [Parameter(Mandatory = false, HelpMessage = "Add the statement even if there is already a qualifier for this property.", ParameterSetName = "globecoordinate")]
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

        private bool IsDuplicatedQualifier(Wikibase.DataValues.DataValue dataValue)
        {
            
            Qualifier[] qualifiers = Claim.ExtensionData.GetQualifiers(Property.ToUpper());

            var sameValueQualifiers = from q in qualifiers
                                  where q.DataValue.Equals(dataValue)
                                  select q;

            return sameValueQualifiers.Any();

        }

        protected override void ProcessRecord()
        {

            var dataValue = DataValue;

            if (Multiple || !IsDuplicatedQualifier(dataValue))
            {

                if (ShouldProcess(Claim.ToString(), "Adding qualifier"))
                {

                    Claim.ExtensionData.AddQualifier(SnakType.Value, new EntityId(Property), dataValue);

                    string comment = String.Format("Adding qualifier {0} {1}", Property, dataValue.ToString());
                    Claim.ExtensionData.Entity.Save(comment);

                    Claim.RefreshFromExtensionData();

                    WriteVerbose(comment);

                }
            }

            WriteObject(Claim, true);

        }


        protected override void EndProcessing()
        {
            base.EndProcessing();
        }


    }


}
