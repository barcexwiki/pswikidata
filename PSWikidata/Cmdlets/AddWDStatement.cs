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
        [Parameter(Mandatory = true, ValueFromPipeline = true, HelpMessage = "Item to be modified.")]
        public PSWDItem Item { get; set; }

        [Parameter(Mandatory = true, HelpMessage = "Property for the statement.")]
        public string Property { get; set; }

        [Parameter(Mandatory = true, HelpMessage = "Item that will be the value of the property.")]
        public string ValueItem { get; set; }


        protected override void BeginProcessing()
        {
            base.BeginProcessing();
        }

        protected override void ProcessRecord()
        {

            Snak snak = new Snak(SnakType.Value, 
                            new EntityId(Property),
                            new Wikibase.DataValues.EntityIdValue(new EntityId(ValueItem))
                            );

            Item.ExtensionData.createStatementForSnak(snak);

            string comment = String.Format("Adding claim {0} {1}", Property, ValueItem);
            Item.ExtensionData.save(comment);

            Item.RefreshFromExtensionData();

            WriteVerbose(comment);
            WriteObject(Item, true);

        }


        protected override void EndProcessing()
        {
            base.EndProcessing();
        }

    }


}
