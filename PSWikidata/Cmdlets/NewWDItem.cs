using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using Wikibase;

namespace PSWikidata
{
    [Cmdlet(VerbsCommon.New, "WDItem",
        SupportsShouldProcess = true,
        ConfirmImpact = ConfirmImpact.High)]
    public class NewWDItem : PSWDNetCmdlet
    {

        [Parameter(Mandatory = false, HelpMessage = "Create the item but do not save the changes to Wikidata.")]
        public SwitchParameter DoNotSave {get; set;}

        protected override void ProcessRecord()
        {
            if (ShouldProcess("new item", "Create"))
            {
                PSWDItem i = new PSWDItem(state.Api);

                if (!DoNotSave)
                {
                    string comment = i.Save();
                    WriteVerbose(comment);
                }

                WriteObject(i, true);
            }
        }

    }
}
