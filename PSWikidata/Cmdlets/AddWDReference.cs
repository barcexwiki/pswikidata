using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using Wikibase;

namespace PSWikidata
{
    [Cmdlet(VerbsCommon.Add, "WDReference",
        SupportsShouldProcess = true,
        ConfirmImpact = ConfirmImpact.Medium)]
    public class AddWDReference : PSWDNetCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, HelpMessage = "Statement to be modified.")]
        public PSWDStatement Statement { get; set; }

        [Parameter(Mandatory = true, ValueFromPipeline = false, HelpMessage = "Snaks that make the reference.")]
        [ValidateNotNull()]
        public PSWDSnak[] Snaks { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Add the reference but do not save the changes to Wikidata.")]
        public SwitchParameter DoNotSave
        {
            get { return _doNotSave; }
            set { _doNotSave = value; }
        }
        private bool _doNotSave;

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
        }

        protected override void ProcessRecord()
        {
            if (ShouldProcess(Statement.ToString(), "Adding reference"))
            {
                List<Snak> referenceSnaks = new List<Snak>();

                foreach (PSWDSnak snak in Snaks)
                {
                    referenceSnaks.Add(snak.ExtensionData);
                }

                if (referenceSnaks.Any())
                { 
                    ((Statement)Statement.ExtensionData).AddReference(referenceSnaks);
                }

                if (!DoNotSave)
                {
                    string comment = Statement.Item.Save();
                    WriteVerbose(comment);
                }

                Statement.RefreshFromExtensionData();
            }

            WriteObject(Statement, true);
        }


        protected override void EndProcessing()
        {
            base.EndProcessing();
        }
    }
}
