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

                foreach (PSWDSnak snak in Snaks)
                {
                    ((Statement)Statement.ExtensionData).AddReference(snak.ExtensionData);
                }

                string comment = String.Format("Adding references");

                if (!DoNotSave)
                {
                    Statement.ExtensionData.Entity.Save(comment);
                    WriteVerbose(comment);
                }
                else
                {
                    WriteVerbose(comment + " [not saving]");
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
