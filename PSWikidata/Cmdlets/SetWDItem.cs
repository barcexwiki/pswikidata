using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using Wikibase;

namespace PSWikidata
{


    [Cmdlet(VerbsCommon.Set, "WDItem",
        SupportsShouldProcess = true,
        ConfirmImpact = ConfirmImpact.Medium)]
    public class SetWDItem : PSWDNetCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, HelpMessage = "Item to be modified.")]
        public PSWDItem Item { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Language to be used.")]
        public string Language { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Description to be set.")]
        public string Description { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Label to be set.")]
        public string Label { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Site of the sitelink")]
        public string SitelinkSite { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Title on the specified SitelinkSite")]
        public string SitelinkTitle { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Removes the label")]
        public SwitchParameter RemoveLabel
        {
            get { return removeLabel; }
            set { removeLabel = value; }
        }
        private bool removeLabel;

        [Parameter(Mandatory = false, HelpMessage = "Removes the description")]
        public SwitchParameter RemoveDescription
        {
            get { return removeDescription; }
            set { removeDescription = value; }
        }
        private bool removeDescription;

        [Parameter(Mandatory = false, HelpMessage = "Removes the sitelink")]
        public SwitchParameter RemoveSitelink
        {
            get { return removeSitelink; }
            set { removeSitelink = value; }
        }
        private bool removeSitelink;



        protected override void BeginProcessing()
        {

            Dictionary<String,object> parms = this.MyInvocation.BoundParameters;

            bool setAndRemoveLabel = (parms.ContainsKey("Label")
                                    & parms.ContainsKey("RemoveLabel"));
            bool setAndRemoveDescription = (parms.ContainsKey("Description")
                                    & parms.ContainsKey("RemoveDescription"));
            bool setAndRemoveSitelink = (parms.ContainsKey("SitelinkTitle")
                        & parms.ContainsKey("RemoveSitelink"));
            bool labelButNoLanguage = (parms.ContainsKey("Label") & !parms.ContainsKey("Language"));
            bool descriptionButNoLanguage = (parms.ContainsKey("Description") & !parms.ContainsKey("Language"));
            bool titleButnoSite = (parms.ContainsKey("SitelinkTitle") & !parms.ContainsKey("SitelinkSite"));

            if (setAndRemoveLabel)
                this.ThrowTerminatingError(new ErrorRecord(
                    new ArgumentException("Label cannot be set alongside RemoveLabel"),"BothLabelAndRemoveLabel", 
                    ErrorCategory.InvalidArgument, null));

            if (setAndRemoveDescription)
                this.ThrowTerminatingError(new ErrorRecord(
                    new ArgumentException("Description cannot be set alongside RemoveDescription"), "BothLabelAndRemoveLabel",
                    ErrorCategory.InvalidArgument, null));                                  
                    
            if (setAndRemoveSitelink)
                this.ThrowTerminatingError(new ErrorRecord(
                    new ArgumentException("SiteLinkTitle cannot be set alongside RemoveSitelink"),"BothSitelinkAndRemoveSitelink", 
                    ErrorCategory.InvalidArgument, null));

            if (labelButNoLanguage)
                this.ThrowTerminatingError(new ErrorRecord(
                    new ArgumentException("Language is required if Label is specified"),"LabelButNoLanguage", 
                    ErrorCategory.InvalidArgument, null));
            
            if (descriptionButNoLanguage)
                this.ThrowTerminatingError(new ErrorRecord(
                    new ArgumentException("Language is required if Description is specified"),"DescriptionButNoLanguage", 
                    ErrorCategory.InvalidArgument, null));

            if (titleButnoSite)
                this.ThrowTerminatingError(new ErrorRecord(
                    new ArgumentException("SitelinkSite is required if SitelinkTitle is specified"),"DescriptionButNoLanguage", 
                    ErrorCategory.InvalidArgument, null));

            base.BeginProcessing();
        }

        protected override void ProcessRecord()
        {

            List<string> editComments;

            editComments = new List<string>();

            if (ShouldProcess(Item.QId, "Setting label, description or sitelink"))
            {
                bool touched = false;

                if (!String.IsNullOrEmpty(Description))
                {
                    Item.ExtensionData.setDescription(Language, Description);
                    editComments.Add(String.Format("Setting description {0}: {1} ", Language, Description));
                    touched = true;
                }

                if (!String.IsNullOrEmpty(Label))
                {
                    Item.ExtensionData.setLabel(Language, Label);
                    editComments.Add(String.Format("Setting label {0}: {1}", Language, Label));
                    touched = true;

                }

                if (RemoveDescription)
                {
                    Item.ExtensionData.removeDescription(Language);
                    editComments.Add(String.Format("Removing description {0}", Language));
                    touched = true;
                }

                if (RemoveLabel)
                {
                    Item.ExtensionData.removeLabel(Language);
                    editComments.Add(String.Format("Removing label {0}", Language));
                    touched = true;
                }


                if (!String.IsNullOrEmpty(SitelinkSite))
                {
                    Item.ExtensionData.setSitelink(SitelinkSite, SitelinkTitle);
                    editComments.Add(String.Format("Setting sitelink {0}: {1} ", SitelinkSite, SitelinkTitle));
                    touched = true;
                }

                if (RemoveSitelink)
                {
                    Item.ExtensionData.removeSitelink(SitelinkSite);
                    editComments.Add(String.Format("Removing sitelink {0}", SitelinkSite));
                    touched = true;
                }

                if (touched)
                {
                    string comment = String.Join(" / ", editComments);
                    Item.ExtensionData.save(comment);
                    WriteVerbose(comment);
                    Item.RefreshFromExtensionData();
                }

                WriteObject(Item, true);

            }

        }

            
        

        protected override void EndProcessing()
        {
            base.EndProcessing();
        }

    }
}
