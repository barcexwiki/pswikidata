using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using Wikibase;

namespace PSWikidata
{
    [Cmdlet(VerbsCommon.Set, "WDStatement",
        SupportsShouldProcess = true,
        ConfirmImpact = ConfirmImpact.Medium)]
    public class SetWDStatement : PSWDValueNetCmdlet
    {
        [Parameter(
           Mandatory = true,
           ValueFromPipeline = true,
           Position = 1,
           HelpMessage = "Statement to be modified."
        )]
        public PSWDStatement Statement { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Output the modidied items instead of the modified statements.", ParameterSetName = "item")]
        [Parameter(Mandatory = false, HelpMessage = "Output the modidied items instead of the modified statements.", ParameterSetName = "monolingual")]
        [Parameter(Mandatory = false, HelpMessage = "Output the modidied items instead of the modified statements.", ParameterSetName = "string")]
        [Parameter(Mandatory = false, HelpMessage = "Output the modidied items instead of the modified statements.", ParameterSetName = "quantity")]
        [Parameter(Mandatory = false, HelpMessage = "Output the modidied items instead of the modified statements.", ParameterSetName = "time")]
        [Parameter(Mandatory = false, HelpMessage = "Output the modidied items instead of the modified statements.", ParameterSetName = "globecoordinate")]
        [Parameter(Mandatory = false, HelpMessage = "Output the modidied items instead of the modified statements.", ParameterSetName = "novalue")]
        [Parameter(Mandatory = false, HelpMessage = "Output the modidied items instead of the modified statements.", ParameterSetName = "somevalue")]
        public SwitchParameter OutputItem
        {
            get { return _outputItem; }
            set { _outputItem = value; }
        }
        private bool _outputItem;

        [Parameter(Mandatory = false, HelpMessage = "Change the statement but do not save the changes to Wikidata.", ParameterSetName = "item")]
        [Parameter(Mandatory = false, HelpMessage = "Change the statement but do not save the changes to Wikidata.", ParameterSetName = "monolingual")]
        [Parameter(Mandatory = false, HelpMessage = "Change the statement but do not save the changes to Wikidata.", ParameterSetName = "string")]
        [Parameter(Mandatory = false, HelpMessage = "Change the statement but do not save the changes to Wikidata.", ParameterSetName = "time")]
        [Parameter(Mandatory = false, HelpMessage = "Change the statement but do not save the changes to Wikidata.", ParameterSetName = "globecoordinate")]
        [Parameter(Mandatory = false, HelpMessage = "Change the statement but do not save the changes to Wikidata.", ParameterSetName = "novalue")]
        [Parameter(Mandatory = false, HelpMessage = "Change the statement but do not save the changes to Wikidata.", ParameterSetName = "somevalue")]
        public SwitchParameter DoNotSave
        {
            get { return _doNotSave; }
            set { _doNotSave = value; }
        }
        private bool _doNotSave;

        protected override void ProcessRecord()
        {
            string comment = String.Format("Modifying statement");

            if (ShouldProcess(Statement.ToString(), "modify statement"))
            {
                var dataValue = DataValue;

                Snak snak = new Snak(SnakType,
                   Statement.ExtensionData.MainSnak.PropertyId,
                   dataValue
                   );

                string statementId = Statement.ExtensionData.Id;

                this.Statement.ExtensionData.MainSnak = snak;


                if (!DoNotSave)
                {
                    Statement.Item.ExtensionData.Save(comment);
                    WriteVerbose(comment);
                }
                else
                {
                    WriteVerbose(comment + " [not saving]");
                }

                Statement.Item.RefreshFromExtensionData();

                if (OutputItem)
                {
                    WriteObject(Statement.Item, true);
                }
                else
                {
                    var output = Statement.Item.Claims.Where(c => c.ExtensionData.Id == statementId);
                    WriteObject(output, true);
                }
            }
        }
    }
}
