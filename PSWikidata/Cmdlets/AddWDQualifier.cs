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
        [Parameter(Mandatory = true, ValueFromPipeline = true, HelpMessage = "Claim to be modified.", ParameterSetName = "item")]
        [Parameter(Mandatory = true, ValueFromPipeline = true, HelpMessage = "Claim to be modified.", ParameterSetName = "monolingual")]
        [Parameter(Mandatory = true, ValueFromPipeline = true, HelpMessage = "Claim to be modified.", ParameterSetName = "string")]
        [Parameter(Mandatory = true, ValueFromPipeline = true, HelpMessage = "Claim to be modified.", ParameterSetName = "quantity")]
        [Parameter(Mandatory = true, ValueFromPipeline = true, HelpMessage = "Claim to be modified.", ParameterSetName = "time")]
        [Parameter(Mandatory = true, ValueFromPipeline = true, HelpMessage = "Claim to be modified.", ParameterSetName = "globecoordinate")]
        [Parameter(Mandatory = true, ValueFromPipeline = true, HelpMessage = "Claim to be modified.", ParameterSetName = "novalue")]
        [Parameter(Mandatory = true, ValueFromPipeline = true, HelpMessage = "Claim to be modified.", ParameterSetName = "somevalue")]
        public PSWDClaim Claim { get; set; }

        [Parameter(Mandatory = true, HelpMessage = "Property for the statement.", ParameterSetName = "item")]
        [Parameter(Mandatory = true, HelpMessage = "Property for the statement.", ParameterSetName = "monolingual")]
        [Parameter(Mandatory = true, HelpMessage = "Property for the statement.", ParameterSetName = "string")]
        [Parameter(Mandatory = true, HelpMessage = "Property for the statement.", ParameterSetName = "quantity")]
        [Parameter(Mandatory = true, HelpMessage = "Property for the statement.", ParameterSetName = "time")]
        [Parameter(Mandatory = true, HelpMessage = "Property for the statement.", ParameterSetName = "globecoordinate")]
        [Parameter(Mandatory = true, HelpMessage = "Property for the statement.", ParameterSetName = "novalue")]
        [Parameter(Mandatory = true, HelpMessage = "Property for the statement.", ParameterSetName = "somevalue")]
        [PSWDEntityArgumentTransformation]
        public PSWDProperty Property { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Add the qualifier even if there is already a qualifier for this property.", ParameterSetName = "item")]
        [Parameter(Mandatory = false, HelpMessage = "Add the qualifier even if there is already a qualifier for this property.", ParameterSetName = "monolingual")]
        [Parameter(Mandatory = false, HelpMessage = "Add the qualifier even if there is already a qualifier for this property.", ParameterSetName = "string")]
        [Parameter(Mandatory = false, HelpMessage = "Add the qualifier even if there is already a qualifier for this property.", ParameterSetName = "time")]
        [Parameter(Mandatory = false, HelpMessage = "Add the qualifier even if there is already a qualifier for this property.", ParameterSetName = "globecoordinate")]
        [Parameter(Mandatory = false, HelpMessage = "Add the qualifier even if there is already a qualifier for this property.", ParameterSetName = "novalue")]
        [Parameter(Mandatory = false, HelpMessage = "Add the qualifier even if there is already a qualifier for this property.", ParameterSetName = "somevalue")]
        public SwitchParameter Multiple {get; set;}

        [Parameter(Mandatory = false, HelpMessage = "Add the qualifier but do not save the changes to Wikidata.", ParameterSetName = "item")]
        [Parameter(Mandatory = false, HelpMessage = "Add the qualifier but do not save the changes to Wikidata.", ParameterSetName = "monolingual")]
        [Parameter(Mandatory = false, HelpMessage = "Add the qualifier but do not save the changes to Wikidata.", ParameterSetName = "string")]
        [Parameter(Mandatory = false, HelpMessage = "Add the qualifier but do not save the changes to Wikidata.", ParameterSetName = "time")]
        [Parameter(Mandatory = false, HelpMessage = "Add the qualifier but do not save the changes to Wikidata.", ParameterSetName = "globecoordinate")]
        [Parameter(Mandatory = false, HelpMessage = "Add the qualifier but do not save the changes to Wikidata.", ParameterSetName = "novalue")]
        [Parameter(Mandatory = false, HelpMessage = "Add the qualifier but do not save the changes to Wikidata.", ParameterSetName = "somevalue")]
        public SwitchParameter DoNotSave {get; set;}

         private bool IsDuplicatedQualifier(Wikibase.DataValues.DataValue dataValue)
        {
            Qualifier[] qualifiers = Claim.ExtensionData.GetQualifiers(Property.Id.ToUpper());

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
                if (ShouldProcess(Claim.ToString(), "Add qualifier"))
                {
                    Claim.AddQualifier(SnakType, Property.Id, dataValue);
                    WriteVerbose(String.Format("Adding qualifier {0} {1} on {2}", Property.Id, dataValue != null ? dataValue.ToString() : "unknown/novalue", Claim.Item.Id));

                    if (!DoNotSave)
                    {
                        string comment = Claim.Item.Save();
                        WriteVerbose(comment);
                    }
                }
            }

            WriteObject(Claim, true);
        }

    }
}
