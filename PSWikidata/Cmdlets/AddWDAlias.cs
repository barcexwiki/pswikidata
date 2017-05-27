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
           Position = 0,
           HelpMessage = "Item to be modified."
        )]
        [PSWDItemArgumentTransformation]
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

        [Parameter(Mandatory = false, HelpMessage = "Add the alias but do not save the changes to Wikidata.")]
        public SwitchParameter DoNotSave
        {
            get { return _doNotSave; }
            set { _doNotSave = value; }
        }
        private bool _doNotSave;

        protected override void ProcessRecord()
        {
            foreach (string a in Alias)
            {
                if (ShouldProcess(Item.QId, String.Format("Add alias {0}: {1}", Language, a)))
                {
                    Item.AddAlias(Language, a);
                    WriteVerbose(String.Format("Add alias {0}: {1} on {2}", Language, a, Item.QId));

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
}
