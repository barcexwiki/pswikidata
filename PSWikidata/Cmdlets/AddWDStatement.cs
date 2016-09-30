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
    public class AddWDStatement : PSWDValueNetCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, HelpMessage = "Item to be modified.", ParameterSetName = "item")]
        [Parameter(Mandatory = true, ValueFromPipeline = true, HelpMessage = "Item to be modified.", ParameterSetName = "monolingual")]
        [Parameter(Mandatory = true, ValueFromPipeline = true, HelpMessage = "Item to be modified.", ParameterSetName = "string")]
        [Parameter(Mandatory = true, ValueFromPipeline = true, HelpMessage = "Item to be modified.", ParameterSetName = "quantity")]
        [Parameter(Mandatory = true, ValueFromPipeline = true, HelpMessage = "Item to be modified.", ParameterSetName = "time")]
        [Parameter(Mandatory = true, ValueFromPipeline = true, HelpMessage = "Item to be modified.", ParameterSetName = "globecoordinate")]
        [Parameter(Mandatory = true, ValueFromPipeline = true, HelpMessage = "Item to be modified.", ParameterSetName = "novalue")]
        [Parameter(Mandatory = true, ValueFromPipeline = true, HelpMessage = "Item to be modified.", ParameterSetName = "somevalue")]
        public PSWDItem Item { get; set; }

        [Parameter(Mandatory = true, HelpMessage = "Property for the statement.", ParameterSetName = "item")]
        [Parameter(Mandatory = true, HelpMessage = "Property for the statement.", ParameterSetName = "monolingual")]
        [Parameter(Mandatory = true, HelpMessage = "Property for the statement.", ParameterSetName = "string")]
        [Parameter(Mandatory = true, HelpMessage = "Property for the statement.", ParameterSetName = "quantity")]
        [Parameter(Mandatory = true, HelpMessage = "Property for the statement.", ParameterSetName = "time")]
        [Parameter(Mandatory = true, HelpMessage = "Property for the statement.", ParameterSetName = "globecoordinate")]
        [Parameter(Mandatory = true, HelpMessage = "Property for the statement.", ParameterSetName = "novalue")]
        [Parameter(Mandatory = true, HelpMessage = "Property for the statement.", ParameterSetName = "somevalue")]
        public string Property { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Add the statement even if there is already an statement for this property.", ParameterSetName = "item")]
        [Parameter(Mandatory = false, HelpMessage = "Add the statement even if there is already an statement for this property.", ParameterSetName = "monolingual")]
        [Parameter(Mandatory = false, HelpMessage = "Add the statement even if there is already an statement for this property.", ParameterSetName = "string")]
        [Parameter(Mandatory = false, HelpMessage = "Add the statement even if there is already an statement for this property.", ParameterSetName = "time")]
        [Parameter(Mandatory = false, HelpMessage = "Add the statement even if there is already an statement for this property.", ParameterSetName = "globecoordinate")]
        [Parameter(Mandatory = false, HelpMessage = "Add the statement even if there is already an statement for this property.", ParameterSetName = "novalue")]
        [Parameter(Mandatory = false, HelpMessage = "Add the statement even if there is already an statement for this property.", ParameterSetName = "somevalue")]
        public SwitchParameter Multiple
        {
            get { return _multiple; }
            set { _multiple = value; }
        }
        private bool _multiple;

        [Parameter(Mandatory = false, HelpMessage = "Outputs the new statement instead of the modified item.", ParameterSetName = "item")]
        [Parameter(Mandatory = false, HelpMessage = "Outputs the new statement instead of the modified item.", ParameterSetName = "monolingual")]
        [Parameter(Mandatory = false, HelpMessage = "Outputs the new statement instead of the modified item.", ParameterSetName = "string")]
        [Parameter(Mandatory = false, HelpMessage = "Outputs the new statement instead of the modified item.", ParameterSetName = "time")]
        [Parameter(Mandatory = false, HelpMessage = "Outputs the new statement instead of the modified item.", ParameterSetName = "globecoordinate")]
        [Parameter(Mandatory = false, HelpMessage = "Outputs the new statement instead of the modified item.", ParameterSetName = "novalue")]
        [Parameter(Mandatory = false, HelpMessage = "Outputs the new statement instead of the modified item.", ParameterSetName = "somevalue")]
        public SwitchParameter OutputStatement
        {
            get { return _outputStatement; }
            set { _outputStatement = value; }
        }
        private bool _outputStatement;

        [Parameter(Mandatory = false, HelpMessage = "Add the statement but do not save the changes to Wikidata.", ParameterSetName = "item")]
        [Parameter(Mandatory = false, HelpMessage = "Add the statement but do not save the changes to Wikidata.", ParameterSetName = "monolingual")]
        [Parameter(Mandatory = false, HelpMessage = "Add the statement but do not save the changes to Wikidata.", ParameterSetName = "string")]
        [Parameter(Mandatory = false, HelpMessage = "Add the statement but do not save the changes to Wikidata.", ParameterSetName = "time")]
        [Parameter(Mandatory = false, HelpMessage = "Add the statement but do not save the changes to Wikidata.", ParameterSetName = "globecoordinate")]
        [Parameter(Mandatory = false, HelpMessage = "Add the statement but do not save the changes to Wikidata.", ParameterSetName = "novalue")]
        [Parameter(Mandatory = false, HelpMessage = "Add the statement but do not save the changes to Wikidata.", ParameterSetName = "somevalue")]
        public SwitchParameter DoNotSave
        {
            get { return _doNotSave; }
            set { _doNotSave = value; }
        }
        private bool _doNotSave;


        private bool IsDuplicatedStatement(Wikibase.DataValues.DataValue dataValue)
        {
            
            Claim[] claims = Item.ExtensionData.GetClaims(Property.ToUpper());

            var sameValueClaims = from c in claims
                                  where c.MainSnak.DataValue.Equals(dataValue)
                                  select c;

            return sameValueClaims.Any();

        }

        protected override void ProcessRecord()
        {

            var dataValue = DataValue;

            if (Multiple || !IsDuplicatedStatement(dataValue))
            {
                if (ShouldProcess(Item.QId, "Add statement"))
                {

                    Snak snak = new Snak(SnakType,
                                    new EntityId(Property),
                                    dataValue
                                    );

                    PSWDStatement statement = Item.AddStatement(snak, Rank.Normal);
                    WriteVerbose(String.Format("Adding statement {0} {1} on {2}", Property, dataValue != null ? dataValue.ToString() : "unknown/novalue",Item.QId));

                    if (!DoNotSave)
                    {
                        string comment = Item.Save();
                        WriteVerbose(comment);
                    }

                    if (OutputStatement)
                    {
                        WriteObject(statement, true);
                    }
                }
            }

            if (!OutputStatement)
                WriteObject(Item, true);
        }


    }


}
