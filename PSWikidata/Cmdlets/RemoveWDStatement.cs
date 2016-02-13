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
           Position = 1,
           HelpMessage = "Statement to be removed."
        )]
        public PSWDStatement Statement { get; set; }

        protected override void ProcessRecord()
        {
            string comment = String.Format("Removing statement");

            if (ShouldProcess(Statement.ToString(), comment))
            {
                Statement.Item.ExtensionData.RemoveClaim(Statement.ExtensionData);
                Statement.Item.ExtensionData.Save(comment);
                WriteVerbose(comment);
                Statement.Item.RefreshFromExtensionData();
                WriteObject(Statement.Item, true);
            }
        }

    }
}
