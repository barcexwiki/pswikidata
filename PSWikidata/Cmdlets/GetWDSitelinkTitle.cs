using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using Wikibase;

namespace PSWikidata
{
    [Cmdlet(VerbsCommon.Get, "WDSitelinkTitle",
        SupportsShouldProcess = true,
        ConfirmImpact = ConfirmImpact.None)]
    public class GetWDSitelinkTitle: PSCmdlet
    {
        [Parameter(
           Mandatory = true,
           ValueFromPipeline = true,
           Position = 0,
           HelpMessage = "Item to be read."
        )]
        [PSWDEntityArgumentTransformation]
        public PSWDItem Item { get; set; }

        [Parameter(
           Mandatory = true,
           Position = 2,
           HelpMessage = "Site to be used."
        )]
        public string Site { get; set; }

        protected override void ProcessRecord()
        {
            string title = ((Item)(Item.ExtensionData)).GetSitelink(Site);
            if (title != null) WriteObject(title);
        }
    }
}