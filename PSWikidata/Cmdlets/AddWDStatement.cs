using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using Wikibase;

namespace PSWikidata
{

    [Cmdlet(VerbsCommon.Add, "WDStatement",
        SupportsShouldProcess = true,
        ConfirmImpact = ConfirmImpact.Medium)]
    public class AddWDStatement : PSWDNetCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, HelpMessage = "Item to be modified.", ParameterSetName = "item")]
        [Parameter(Mandatory = true, ValueFromPipeline = true, HelpMessage = "Item to be modified.", ParameterSetName = "monolingual")]
        [Parameter(Mandatory = true, ValueFromPipeline = true, HelpMessage = "Item to be modified.", ParameterSetName = "string")]
        public PSWDItem Item { get; set; }

        [Parameter(Mandatory = true, HelpMessage = "Property for the statement.", ParameterSetName = "item")]
        [Parameter(Mandatory = true, HelpMessage = "Property for the statement.", ParameterSetName = "monolingual")]
        [Parameter(Mandatory = true, HelpMessage = "Property for the statement.", ParameterSetName = "string")]
        public string Property { get; set; }

        [Parameter(Mandatory = true, HelpMessage = "Item that will be the value of the property.", ParameterSetName = "item")]
        public string ValueItem { get; set; }

        [Parameter(Mandatory = true, HelpMessage = "Text that will be the value of the property.", ParameterSetName = "monolingual")]
        public string ValueText { get; set; }

        [Parameter(Mandatory = true, HelpMessage = "Language code for the text that will be the value of the property.", ParameterSetName = "monolingual")]
        public string ValueLanguage { get; set; }

        [Parameter(Mandatory = true, HelpMessage = "Text that will be the value of the property.", ParameterSetName = "string")]
        public string ValueString { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Add the statement even if there is already an statement for this property.", ParameterSetName = "item")]
        [Parameter(Mandatory = false, HelpMessage = "Add the statement even if there is already an statement for this property.", ParameterSetName = "monolingual")]
        [Parameter(Mandatory = false, HelpMessage = "Add the statement even if there is already an statement for this property.", ParameterSetName = "string")]
        public SwitchParameter Multiple
        {
            get { return multiple; }
            set { multiple = value; }
        }
        private bool multiple;


        protected override void BeginProcessing()
        {
            base.BeginProcessing();
        }

        private bool IsDuplicatedStatement(Wikibase.DataValues.DataValue dataValue)
        {
            
            Dictionary<string,Claim> claims = Item.ExtensionData.getClaimsForProperty(Property.ToUpper());

            bool duplicate = false;

            if (claims != null)
            {
                foreach (Claim c in claims.Values)
                {
                    if (c.mainSnak.DataValue.Equals(dataValue))
                    {
                        duplicate = true;
                        break;
                    }
                }
            }

            return duplicate;
        }

        protected override void ProcessRecord()
        {

            Wikibase.DataValues.DataValue dataValue;

            switch (this.ParameterSetName)
            {
                case "item":
                    dataValue = new Wikibase.DataValues.EntityIdValue(new EntityId(ValueItem));
                    break;
                case "monolingual":
                    dataValue = new Wikibase.DataValues.MonolingualTextValue(ValueText,ValueLanguage);
                    break;
                case "string":
                    dataValue = new Wikibase.DataValues.StringValue(ValueString);
                    break;
                default:
                    throw new Exception("Unidentified parameter set");
            }


            if (Multiple || !IsDuplicatedStatement(dataValue))
            {

                if (ShouldProcess(Item.QId, "Adding claim"))
                {

                    Snak snak = new Snak(SnakType.Value,
                                    new EntityId(Property),
                                    dataValue
                                    );

                    Item.ExtensionData.createStatementForSnak(snak);

                    string comment = String.Format("Adding claim {0} {1}", Property, dataValue.ToString());
                    Item.ExtensionData.save(comment);

                    Item.RefreshFromExtensionData();

                    WriteVerbose(comment);

                }
            }

            WriteObject(Item, true);

        }


        protected override void EndProcessing()
        {
            base.EndProcessing();
        }


    }


}
