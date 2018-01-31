using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using Wikibase;

namespace PSWikidata
{
    [Cmdlet(VerbsCommon.Set, "WDEntity",
        SupportsShouldProcess = true,
        ConfirmImpact = ConfirmImpact.Medium)]
    public class SetWDEntity : PSWDSetEntityCommonCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0, HelpMessage = "Entity to be modified.")]
        [PSWDEntityArgumentTransformationAttribute]
        public PSWDEntity[] Entity { get; set; }

        protected override void BeginProcessing()
        {
            CheckEntityParameters();
            base.BeginProcessing();
        }

        protected override void ProcessRecord()
        {
            foreach (PSWDEntity entity in Entity)
            {
                if (ShouldProcess(entity.Id, "Set label, description or sitelink"))
                {
                    bool touched = ProcessEntity(entity);

                    if (touched && !DoNotSave)
                    {
                        string comment = entity.Save();
                        WriteVerbose(comment);
                    }

                    WriteObject(entity, true);
                }
            }
        }

    }
}
