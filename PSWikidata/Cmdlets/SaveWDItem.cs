using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using Wikibase;

namespace PSWikidata
{
    [Cmdlet(VerbsData.Save, "WDItem",
        SupportsShouldProcess = true,
        ConfirmImpact = ConfirmImpact.Medium)]
    public class SaveWDItem : PSWDNetCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, HelpMessage = "Item to be saved.")]
        [PSWDItemArgumentTransformation]
        public PSWDItem[] Item { get; set; }


        protected override void ProcessRecord()
        {
            foreach (PSWDItem i in Item)
            {
                string comment = String.Format("Updating item");

                if (ShouldProcess(i.QId, comment))
                {
                     i.ExtensionData.Save(comment);
                     WriteVerbose(comment);
                     i.RefreshFromExtensionData();
                     WriteObject(Item, true);
                }
            }
        }

    }
}
