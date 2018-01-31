using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using Wikibase;

namespace PSWikidata
{
    public abstract class PSWDNetCmdlet : PSCmdlet
    {
        protected EntityProvider provider;
        internal PSWDSessionState state;

        protected override void BeginProcessing()
        {
            PSVariable stateVariable = SessionState.PSVariable.Get("Global:__WikidataState");

            if (stateVariable?.Value != null)
            {
                state = (PSWDSessionState)stateVariable.Value;
                provider = state.EntityProvider;
            }
            else
            {
                throw new Exception("Not connected to wikibase server");
            }
        }
    }
}
