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
           Position = 0
        )]
        public PSCredential Credential { get; set; }

        private string _apiUrl = "https://www.wikidata.org/w/api.php";

        [Parameter(
           Mandatory = false,
           Position = 1
        )]
        public string ApiUrl
        {
            get { return _apiUrl; }
            set { _apiUrl = value; }
        }

        protected override void BeginProcessing()
        {
            WikibaseApi Api = new WikibaseApi(ApiUrl, "PSWikidata 0.0.1");
            bool loggedIn = Api.Login(Credential.UserName, Credential.GetNetworkCredential().Password);
            if (loggedIn)
            {
                EntityProvider p = new EntityProvider(Api);
                PSWDSessionState state = new PSWDSessionState(Api, p);
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
