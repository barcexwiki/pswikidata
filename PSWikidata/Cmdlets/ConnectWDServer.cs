using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using Wikibase;

namespace PSWikidata
{
    [Cmdlet(VerbsCommunications.Connect, "WDServer")]
    public class ConnectWDServer: PSCmdlet
    {

        [Parameter(
           Mandatory = true,
           Position = 0,
           HelpMessage = "Credentials to login to Wikidata."
        )]
        public PSCredential Credential { get; set; }

        protected override void BeginProcessing()
        {
            WikibaseApi Api = new WikibaseApi("https://www.wikidata.org");
            bool loggedIn = Api.login(Credential.UserName, Credential.GetNetworkCredential().Password);
            if (loggedIn)
            {
                EntityProvider p = new EntityProvider(Api);
                PSWDSessionState state = new PSWDSessionState(Api,p);
                this.SessionState.PSVariable.Set(new PSVariable("__WikidataState", state));
                WriteDebug("Connected to Wikidata");
            }
            else
            {
                throw new Exception("Cannot login");
            }
        }
    }
}
