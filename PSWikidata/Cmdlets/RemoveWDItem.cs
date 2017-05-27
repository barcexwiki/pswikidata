using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using Wikibase;

namespace PSWikidata
{
    [Cmdlet(VerbsCommon.Remove, "WDItem",
        SupportsShouldProcess = true,
        ConfirmImpact = ConfirmImpact.High)]
    public class RemoveWDItem : PSWDNetCmdlet
    {
        [Parameter(
           Mandatory = true,
           ValueFromPipeline = true,
           Position = 0,
           HelpMessage = "Item to be modified."
        )]
        [PSWDItemArgumentTransformation]
        public PSWDItem Item { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Mark the item for deletion but do not save the changes to Wikidata.")]
        public SwitchParameter DoNotSave
        {
            get { return _doNotSave; }
            set { _doNotSave = value; }
        }
        private bool _doNotSave;

        protected override void ProcessRecord()
        {
            if (ShouldProcess(Item.QId, "remove item"))
            {
                Item.Delete();

                if (!DoNotSave)
                {
                    string comment = Item.Save();
                    WriteVerbose(comment);
                }
                
                WriteObject(Item, true);
            }
        }
    }
}
