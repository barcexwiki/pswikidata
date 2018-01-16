using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using Wikibase;

namespace PSWikidata
{
    [Cmdlet(VerbsCommon.New, "WDSnak",
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
        [Parameter(Mandatory = true, HelpMessage = "Property for the statement.", ParameterSetName = "novalue")]
        [Parameter(Mandatory = true, HelpMessage = "Property for the statement.", ParameterSetName = "somevalue")]
        [PSWDEntityArgumentTransformation]
        public PSWDProperty Property { get; set; }

        protected override void ProcessRecord()
        {
            var dataValue = DataValue;
            PSWDSnak snak;

            if (ShouldProcess("new snak", "Creating Snak"))
            {
                snak = new PSWDSnak(new Snak(SnakType, new EntityId(Property.Id), DataValue));
                WriteObject(snak, true);
            }
        }

    }
}
