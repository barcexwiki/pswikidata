using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using Wikibase;

namespace PSWikidata
{
    [Cmdlet(VerbsCommon.New, "WDSitelink",
        SupportsShouldProcess = true,
        ConfirmImpact = ConfirmImpact.Medium)]
    public class NewWDSitelink : PSCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "Site for the sitelink.", ParameterSetName = "sitelink")]
        public string Site { get; set; }

        [Parameter(Mandatory = true, HelpMessage = "Title for the sitelink page in the site.", ParameterSetName = "sitelink")]
        public string Title { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Badges for the sitelink.", ParameterSetName = "sitelink")]
        public string[] Badges { get; set; }


        protected override void ProcessRecord()
        {
            PSWDSitelink sitelink;

            if (ShouldProcess("new sitelink", "Creating sitelink"))
            {
                sitelink = new PSWDSitelink(Site, Title, Badges);
                WriteObject(sitelink, true);
            }
        }


    }
}
