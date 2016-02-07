﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using Wikibase;

namespace PSWikidata
{


    [Cmdlet(VerbsCommon.Get, "WDDescription",
        SupportsShouldProcess = true,
        ConfirmImpact = ConfirmImpact.None)]
    public class GetWDDescription : PSCmdlet
    {
        [Parameter(
           Mandatory = true,
           ValueFromPipeline = true,
           Position = 1,
           HelpMessage = "Item to be read."
        )]
        public PSWDItem Item { get; set; }

        [Parameter(
           Mandatory = true,
           Position = 2,
           HelpMessage = "Language to be used."
        )]
        public string Language { get; set; }

        protected override void ProcessRecord()
        {
            string description = Item.ExtensionData.GetDescription(Language); 
            if (description != null)  WriteObject(description);
        }

    }
}
