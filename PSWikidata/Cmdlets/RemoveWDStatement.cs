using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using Wikibase;

namespace PSWikidata
{
    [Cmdlet(VerbsCommon.Remove, "WDStatement",
        SupportsShouldProcess = true,
        ConfirmImpact = ConfirmImpact.Medium)]
    public class RemoveWDStatement : PSWDNetCmdlet
    {
        [Parameter(
           Mandatory = true,
           ValueFromPipeline = true,
           Position = 0,
           HelpMessage = "Statement to be removed."
        )]
        public PSWDStatement Statement { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Remove the statement but do not save the changes to Wikidata.")]
        public SwitchParameter DoNotSave {get; set;}

        protected override void ProcessRecord()
        {
            string comment = "Removing statement";

            if (ShouldProcess(Statement.ToString(), comment))
            {
                Statement.Item.ExtensionData.RemoveClaim(Statement.ExtensionData);

                if (!DoNotSave)
                {
                    Statement.Item.ExtensionData.Save(comment);
                    WriteVerbose(comment);
                }
                else
                {
                    WriteVerbose(comment + " [not saving]");
                }

                Statement.Item.RefreshFromExtensionData();
                WriteObject(Statement.Item, true);
            }
        }
    }
}
