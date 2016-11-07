using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using Wikibase;

namespace PSWikidata
{
    [Cmdlet(VerbsCommon.Get, "WDItem")]
    public class GetWDItem : PSWDNetCmdlet
    {
        private string[] _qIdCollection;
        [Parameter(
           Mandatory = true,
           ValueFromPipelineByPropertyName = true,
           ValueFromPipeline = true,
           Position = 0,
           HelpMessage = "Q identifier for the item.",
           ParameterSetName = "qid"
        )]
        public string[] QId
        {
            get { return _qIdCollection; }
            set { _qIdCollection = value; }
        }

        [Parameter(Mandatory = true, HelpMessage = "Site of the sitelink", ParameterSetName = "sitelink")]
        public string SitelinkSite { get; set; }

        [Parameter(Mandatory = true, HelpMessage = "Title on the specified SitelinkSite", ParameterSetName = "sitelink")]
        public string SitelinkTitle { get; set; }

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
        }

        protected override void ProcessRecord()
        {
            Item item;

            switch (ParameterSetName)
            {
                case "qid":
                    foreach (string q in _qIdCollection)
                    {
                        WriteVerbose("Getting item " + q);
                        item = (Item)provider.GetEntityFromId(new EntityId(q));
                        if (item != null)
                        {
                            WriteObject(new PSWDItem(item));
                        }
                    }
                    break;
                case "sitelink":
                    item = (Item)provider.GetEntityFromSitelink(SitelinkSite, SitelinkTitle);
                    if (item != null)
                    {
                        WriteObject(new PSWDItem(item));
                    }
                    break;
                default:
                    throw new Exception("Unidentified parameter set");
            }

        }

        protected override void EndProcessing()
        {
            base.EndProcessing();
        }
    }
}
