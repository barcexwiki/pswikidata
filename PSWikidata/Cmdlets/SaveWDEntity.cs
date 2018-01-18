using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using Wikibase;

namespace PSWikidata
{
    [Cmdlet(VerbsData.Save, "WDEntity",
        SupportsShouldProcess = true,
        ConfirmImpact = ConfirmImpact.Medium)]
    public class SaveWDEntity : PSWDNetCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0, HelpMessage = "Entity to be saved.")]
        [PSWDEntityArgumentTransformationAttribute]
        public PSWDEntity[] Entity { get; set; }

        protected override void ProcessRecord()
        {
            foreach (PSWDEntity entity in Entity)
            {
                if (ShouldProcess(entity.Id, "Save entity"))
                {
                    try
                    {
                        string comment = entity.Save();
                        WriteVerbose(comment);
                        WriteObject(entity, true);
                    }
                    catch (Exception e)
                    {
                        WriteError(new ErrorRecord(e, null, ErrorCategory.NotSpecified, entity));
                        continue;
                    }
                }
            }
        }
    }
}
