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
    public class ConnectWDServer : PSCmdlet
    {
        [Parameter(
           Mandatory = true,
           Position = 0,
           HelpMessage = "Credentials to login to Wikidata."
        )]
        public PSCredential Credential { get; set; }

        [Parameter(
           Mandatory = false,
           Position = 1,
           HelpMessage = "API URL."
        )]
        public string ApiUrl {get; set;} =  "https://www.wikidata.org/w/api.php";

        protected override void BeginProcessing()
        {
            WikibaseApi Api = new WikibaseApi(ApiUrl, "PSWikidata 0.0.1");
            bool loggedIn = Api.Login(Credential.UserName, Credential.GetNetworkCredential().Password);
            if (loggedIn)
            {
                EntityProvider p = new EntityProvider(Api);
                PSWDSessionState state = new PSWDSessionState(Api, p);
                SessionState.PSVariable.Set(new PSVariable("Global:__WikidataState", state));
                WriteDebug("Connected to wikibase");
            }
            else
            {
                throw new Exception("Cannot login");
            }
        }
    }
}
