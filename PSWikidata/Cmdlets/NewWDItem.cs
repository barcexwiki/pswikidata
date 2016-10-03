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
        protected override void BeginProcessing()
        {
            base.BeginProcessing();
        }

        protected override void ProcessRecord()
        {
            if (ShouldProcess("new item", "Create"))
            {
                Item item = new Item(state.Api);
                PSWDItem i = new PSWDItem(item);
                string comment = "Creating new item";
                i.ExtensionData.Save(comment);
                WriteVerbose(comment);
                i.RefreshFromExtensionData();
                WriteObject(i, true);
            }
        }




        protected override void EndProcessing()
        {
            base.EndProcessing();
        }
    }
}
