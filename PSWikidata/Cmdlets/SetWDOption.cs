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

            PSVariable optionsVariable = SessionState.PSVariable.Get("PSWDOptions");

            if (optionsVariable == null)
            {
                options = new PSWDOptions();
                SessionState.PSVariable.Set("PSWDOptions", options);
            }
            else
            {
                options = (PSWDOptions)optionsVariable.Value;
            }

            if (MyInvocation.BoundParameters.ContainsKey("PreferredDisplayLanguages"))
            {
                options.PreferredDisplayLanguages = PreferredDisplayLanguages;
            }

        }

    }
}