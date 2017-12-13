using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using Wikibase;

namespace PSWikidata
{
    [Cmdlet(VerbsData.Save, "WDProperty",
        SupportsShouldProcess = true,
        ConfirmImpact = ConfirmImpact.Medium)]
    public class SaveWDProperty : PSWDNetCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0, HelpMessage = "Property to be saved.")]
        [PSWDEntityArgumentTransformationAttribute]
        public PSWDProperty[] Property { get; set; }

        protected override void ProcessRecord()
        {
            foreach (PSWDProperty p in Property)
            {
                if (ShouldProcess(p.QId, "Save property"))
                {
                    try
                    {
                        string comment = p.Save();
                        WriteVerbose(comment);
                        WriteObject(p, true);
                    }
                    catch (Exception e)
                    {
                        WriteError(new ErrorRecord(e, null, ErrorCategory.NotSpecified, p));
                        continue;
                    }
                }
            }
        }
    }
}
