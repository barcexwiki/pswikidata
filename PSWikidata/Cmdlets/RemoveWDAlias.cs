﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using Wikibase;

namespace PSWikidata
{
    [Cmdlet(VerbsCommon.Remove, "WDAlias",
        SupportsShouldProcess = true,
        ConfirmImpact = ConfirmImpact.Medium)]
    public class RemoveWDAlias : PSWDNetCmdlet
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
                string comment = String.Format("Removing alias {0}: {1}", Language, a);

                if (ShouldProcess(Item.QId, comment))
                {
                    Item.ExtensionData.removeAlias(Language,a);
                    
                    Item.ExtensionData.save(comment);
                    WriteVerbose(comment);
                    Item.RefreshFromExtensionData();
                    WriteObject(Item, true);
                }

            }
        }

    }
}
