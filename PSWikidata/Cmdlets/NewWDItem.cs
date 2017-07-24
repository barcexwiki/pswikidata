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
    [OutputType(typeof(PSWDItem))]
    public class NewWDItem : PSWDNetCmdlet
    {
        protected override void BeginProcessing()
        {
            base.BeginProcessing();
        }

        [Parameter(Mandatory = false)]
        public SwitchParameter DoNotSave
        {
            get { return _doNotSave; }
            set { _doNotSave = value; }
        }
        private bool _doNotSave;

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

        protected override void EndProcessing()
        {
            base.EndProcessing();
        }
    }
}
