using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using Wikibase;

namespace PSWikidata
{
    [Cmdlet(VerbsCommon.Get, "WDLabel",
        SupportsShouldProcess = true,
        ConfirmImpact = ConfirmImpact.None)]
    public class GetWDLabel : PSCmdlet
    {
        [Parameter(
           Mandatory = true,
           ValueFromPipeline = true,
           Position = 0,
           HelpMessage = "Property or item to be read."
        )]
        [PSWDEntityArgumentTransformation]
        [Alias("Item", "Property")]
        public PSWDEntity Entity { get; set; }

        [Parameter(
           Mandatory = true,
           Position = 2,
           HelpMessage = "Language to be used."
        )]
        public string Language { get; set; }

        protected override void ProcessRecord()
        {
            string label = Entity.ExtensionData.GetLabel(Language);
            if (label != null) WriteObject(label);
        }
    }
}
