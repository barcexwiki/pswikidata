using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using Wikibase;

namespace PSWikidata
{
    [Cmdlet(VerbsCommon.Add, "WDAlias",
        SupportsShouldProcess = true,
        ConfirmImpact = ConfirmImpact.Medium)]
    public class AddWDAlias : PSWDNetCmdlet
    {
        [Parameter(
           Mandatory = true,
           ValueFromPipeline = true,
           Position = 1,
           HelpMessage = "Item to be modified."
        )]
        public PSWDItem Item { get; set; }

        [Parameter(
           Mandatory = true,
           Position = 2,
           HelpMessage = "Language to be used."
        )]
        public string Language { get; set; }

        [Parameter(
           Mandatory = true,
           Position = 3,
           HelpMessage = "Alias to be added."
        )]
        public string[] Alias { get; set; }

        protected override void ProcessRecord()
        {
            foreach (string a in Alias)
            {
                string comment = String.Format("Adding alias {0}: {1}", Language, a);

                if (ShouldProcess(Item.QId, comment))
                {
                    Item.ExtensionData.AddAlias(Language,a);

                    Item.ExtensionData.Save(comment);
                    WriteVerbose(comment);
                    Item.RefreshFromExtensionData();
                    WriteObject(Item, true);
                }

            }
        }

    }
}
