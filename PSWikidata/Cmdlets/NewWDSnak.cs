using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using Wikibase;

namespace PSWikidata
{

    [Cmdlet(VerbsCommon.Add, "WDSnak",
        SupportsShouldProcess = true,
        ConfirmImpact = ConfirmImpact.Medium)]
    public class NewWDSnak : PSWDValueNetCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "Property for the statement.", ParameterSetName = "item")]
        [Parameter(Mandatory = true, HelpMessage = "Property for the statement.", ParameterSetName = "monolingual")]
        [Parameter(Mandatory = true, HelpMessage = "Property for the statement.", ParameterSetName = "string")]
        [Parameter(Mandatory = true, HelpMessage = "Property for the statement.", ParameterSetName = "quantity")]
        [Parameter(Mandatory = true, HelpMessage = "Property for the statement.", ParameterSetName = "time")]
        [Parameter(Mandatory = true, HelpMessage = "Property for the statement.", ParameterSetName = "globecoordinate")]
        public string Property { get; set; }

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
        }

        protected override void ProcessRecord()
        {

            var dataValue = DataValue;
            PSWDSnak snak;

            if (ShouldProcess("new snak", "Creating Snak"))
            {

                snak = new PSWDSnak(new Snak(SnakType.Value, new EntityId(Property), DataValue));
                WriteObject(snak, true);
            }
        }


        protected override void EndProcessing()
        {
            base.EndProcessing();
        }


    }


}
