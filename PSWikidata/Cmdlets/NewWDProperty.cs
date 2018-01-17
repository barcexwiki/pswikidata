using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using Wikibase;

namespace PSWikidata
{
    [Cmdlet(VerbsCommon.New, "WDProperty",
        SupportsShouldProcess = true,
        ConfirmImpact = ConfirmImpact.High)]
    public class NewWDProperty : PSWDNetCmdlet
    {

        [Parameter(Mandatory = true, HelpMessage = "Data type to be assigned to the property.")]
        public PSWDDataType DataType { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Create the property but do not save the changes to Wikidata.")]
        public SwitchParameter DoNotSave {get; set;}

        protected override void ProcessRecord()
        {
            if (ShouldProcess("new propery", "Create"))
            {
                PSWDProperty p = new PSWDProperty(state.Api, DataType);

                if (!DoNotSave)
                {
                    string comment = p.Save();
                    WriteVerbose(comment);
                }

                WriteObject(p, true);
            }
        }

    }
}
