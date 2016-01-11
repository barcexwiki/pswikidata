using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using Wikibase;

namespace PSWikidata
{
    [Cmdlet(VerbsCommunications.Disconnect, "WDServer")]
    public class DisconnectWDServer: PSCmdlet
    {
        protected override void BeginProcessing()
        {
            PSWDSessionState sessionState = (PSWDSessionState)this.SessionState.PSVariable.Get("__WikidataState").Value;
            sessionState.Api.logout();
            this.SessionState.PSVariable.Remove("__WikidataState");
            WriteDebug("Disconnected from Wikidata");
        }
    }
}
