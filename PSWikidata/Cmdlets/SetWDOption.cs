using System.Management.Automation;

namespace PSWikidata
{
    [Cmdlet("Set", "WDOption")]
    public class SetWDOption : PSCmdlet
    {
        [Parameter(ParameterSetName = "OptionsSet")]
        public string[] PreferredDisplayLanguages { get; set; }

        protected override void EndProcessing()
        {
            PSWDOptions options;

            PSVariable optionsVariable = this.SessionState.PSVariable.Get("PSWDOptions");

            if (optionsVariable == null)
            {
                options = new PSWDOptions();
                this.SessionState.PSVariable.Set("PSWDOptions", options);
            }
            else
            {
                options = (PSWDOptions)optionsVariable.Value;
            }

            if (this.MyInvocation.BoundParameters.ContainsKey("PreferredDisplayLanguages"))
            {
                options.PreferredDisplayLanguages = PreferredDisplayLanguages;
            }

        }

    }
}