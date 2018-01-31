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
    public class DisconnectWDServer : PSCmdlet
    {
        protected override void BeginProcessing()
        {
            PSWDSessionState sessionState = (PSWDSessionState)SessionState.PSVariable.Get("Global:__WikidataState").Value;
            sessionState.Api.Logout();
            SessionState.PSVariable.Set("Global:__WikidataState", null);
            WriteDebug("Disconnected from server");
        }
    }
}
