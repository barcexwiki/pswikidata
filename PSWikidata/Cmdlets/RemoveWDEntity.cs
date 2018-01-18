using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using Wikibase;

namespace PSWikidata
{
    [Cmdlet(VerbsCommon.Remove, "WDEntity",
        SupportsShouldProcess = true,
        ConfirmImpact = ConfirmImpact.High)]
    public class RemoveWDEntity : PSWDNetCmdlet
    {
        [Parameter(
           Mandatory = true,
           ValueFromPipeline = true,
           Position = 0,
           HelpMessage = "Entity to be deleted."
        )]
        [PSWDEntityArgumentTransformationAttribute]
        public PSWDEntity Entity { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Mark the entity for deletion but do not save the changes to Wikidata.")]
        public SwitchParameter DoNotSave { get; set; }

        protected override void ProcessRecord()
        {
            if (ShouldProcess(Entity.Id, "remove entity"))
            {
                Entity.Delete();

                if (!DoNotSave)
                {
                    string comment = Entity.Save();
                    WriteVerbose(comment);
                }
                
                WriteObject(Entity, true);
            }
        }
    }
}
