using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using Wikibase;

namespace PSWikidata
{
    [Cmdlet(VerbsCommon.New, "WDReference",
        SupportsShouldProcess = true,
        ConfirmImpact = ConfirmImpact.Low)]
    public class NewWDReference : PSCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "Snaks for the reference.")]
        public PSWDSnak[] Snak { get; set; }

        private readonly List<PSWDSnak> _snaks = new List<PSWDSnak>();

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
        }

        protected override void ProcessRecord()
        {
            foreach (PSWDSnak s in Snak)
            {
                if (ShouldProcess("new reference", "create reference"))
                {
                    _snaks.Add(s);                    
                }
            }
        }

        protected override void EndProcessing()
        {
            base.EndProcessing();
            if (_snaks.Any())
            {
                WriteObject(new PSWDReference(_snaks));
            }
        }
    }
}
