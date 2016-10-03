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
           HelpMessage = "Q identifier for the item."
        )]
        public string[] QId
        {
            get { return _qIdCollection; }
            set { _qIdCollection = value; }
        }

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
        }

        protected override void ProcessRecord()
        {
            foreach (string q in _qIdCollection)
            {
                WriteVerbose("Getting item " + q);
                Item item = (Item)provider.GetEntityFromId(new EntityId(q));
                if (item != null)
                {
                    WriteObject(new PSWDItem(item));
                }
            }
        }

        protected override void EndProcessing()
        {
            base.EndProcessing();
        }
    }
}
