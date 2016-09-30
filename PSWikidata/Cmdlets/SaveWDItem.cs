﻿using System;
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
                if (ShouldProcess(i.QId, "Save item"))
                {
                    try
                    {
                        string comment = i.Save();
                        WriteVerbose(comment);
                        WriteObject(i, true);
                    }
                    catch (Exception e)
                    {
                        WriteError(new ErrorRecord(e,null, ErrorCategory.NotSpecified, i));
                        continue;
                    }
                }
            }
        }

    }
}
