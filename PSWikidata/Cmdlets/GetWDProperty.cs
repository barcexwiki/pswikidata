using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using Wikibase;

namespace PSWikidata
{
    [Cmdlet(VerbsCommon.Get, "WDProperty")]
    public class GetWDProperty : PSWDNetCmdlet
    {

        [Parameter(
           Mandatory = true,
           ValueFromPipelineByPropertyName = true,
           ValueFromPipeline = true,
           Position = 0,
           HelpMessage = "P identifier for the item.",
           ParameterSetName = "pid"
        )]
        public string[] PId {get; set;}

        protected override void ProcessRecord()
        {
            Property property;

            switch (ParameterSetName)
            {
                case "pid":
                    foreach (string p in PId)
                    {
                        WriteVerbose("Getting property " + p);
                        property = (Property)provider.GetEntityFromId(new EntityId(p));
                        if (property != null)
                        {
                            WriteObject(new PSWDProperty(property));
                        }
                    }
                    break;
                default:
                    throw new Exception("Unidentified parameter set");
            }

        }

    }
}
