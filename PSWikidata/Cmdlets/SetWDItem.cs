using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using Wikibase;

namespace PSWikidata
{
    [Cmdlet(VerbsCommon.Set, "WDItem",
        SupportsShouldProcess = true,
        ConfirmImpact = ConfirmImpact.Medium)]
    public class SetWDItem : PSWDSetEntityCommonCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0, HelpMessage = "Item to be modified.")]
        [PSWDEntityArgumentTransformationAttribute]
        public PSWDItem[] Item { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Sitelink")]
        public PSWDSitelink Sitelink { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Removes the sitelink")]
        public string RemoveSitelink { get; set; }

        protected override void BeginProcessing()
        {
            CheckEntityParameters();

            Dictionary<String, object> parms = MyInvocation.BoundParameters;

            bool setAndRemoveSitelink = (parms.ContainsKey("Sitelink")
                        && parms.ContainsKey("RemoveSitelink"));

            if (setAndRemoveSitelink)
                ThrowTerminatingError(new ErrorRecord(
                    new ArgumentException("SiteLink cannot be set alongside RemoveSitelink"), "BothSitelinkAndRemoveSitelink",
                    ErrorCategory.InvalidArgument, null));

            base.BeginProcessing();
        }

        protected override void ProcessRecord()
        {
            foreach (PSWDItem i in Item)
            {
                if (ShouldProcess(i.Id, "Set label, description or sitelink"))
                {

                    bool touched = ProcessEntity(i);

                    if (Sitelink != null)
                    {
                        WriteVerbose($"Setting sitelink {Sitelink.Site} {Sitelink.Title} on {i.Id}");
                        i.SetSitelink(Sitelink);
                        touched = true;
                    }

                    if (!String.IsNullOrEmpty(RemoveSitelink))
                    {
                        WriteVerbose($"Removing sitelink {RemoveSitelink} on {i.Id}");
                        i.RemoveSitelink(RemoveSitelink);
                        touched = true;
                    }

                    if (touched && !DoNotSave)
                    {
                        string comment = i.Save();
                        WriteVerbose(comment);
                    }

                    WriteObject(i, true);
                }
            }
        }

    }
}
