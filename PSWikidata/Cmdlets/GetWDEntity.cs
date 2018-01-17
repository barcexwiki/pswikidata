using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using Wikibase;

namespace PSWikidata
{
    [Cmdlet(VerbsCommon.Get, "WDEntity")]
    public class GetWDEntity : PSWDNetCmdlet
    {

        [Parameter(
           Mandatory = true,
           ValueFromPipelineByPropertyName = true,
           ValueFromPipeline = true,
           Position = 0,
           HelpMessage = "Q/P identifier for the item.",
           ParameterSetName = "id"
        )]
        public string[] Id {get; set;}

        [Parameter(Mandatory = true, HelpMessage = "Site of the sitelink", ParameterSetName = "sitelink")]
        public string SitelinkSite { get; set; }

        [Parameter(Mandatory = true, HelpMessage = "Title on the specified SitelinkSite", ParameterSetName = "sitelink")]
        public string SitelinkTitle { get; set; }

        protected override void ProcessRecord()
        {
            Entity entity;

            switch (ParameterSetName)
            {
                case "id":
                    foreach (string id in Id)
                    {
                        WriteVerbose("Getting entity " + id);
                        entity = provider.GetEntityFromId(new EntityId(id));
                        if (entity != null)
                        {
                            WriteObject(PSWDEntity.GetPSWDEntity(entity));
                        }
                    }
                    break;
                case "sitelink":
                    entity = provider.GetEntityFromSitelink(SitelinkSite, SitelinkTitle);
                    if (entity != null)
                    {
                        WriteObject(PSWDEntity.GetPSWDEntity(entity));
                    }
                    break;
                default:
                    throw new Exception("Unidentified parameter set");
            }

        }

    }
}
