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
        [PSWDItemArgumentTransformation]
        public PSWDItem[] Item { get; set; }

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
            get { return _removeLabel; }
            set { _removeLabel = value; }
        }
        private bool _removeLabel;

        [Parameter(Mandatory = false, HelpMessage = "Removes the description")]
        public SwitchParameter RemoveDescription
        {
            get { return _removeDescription; }
            set { _removeDescription = value; }
        }
        private bool _removeDescription;

        [Parameter(Mandatory = false, HelpMessage = "Removes the sitelink")]
        public SwitchParameter RemoveSitelink
        {
            get { return _removeSitelink; }
            set { _removeSitelink = value; }
        }
        private bool _removeSitelink;

        [Parameter(Mandatory = false, HelpMessage = "Change the item but do not save the changes to Wikidata.")]
        public SwitchParameter DoNotSave
        {
            get { return _doNotSave; }
            set { _doNotSave = value; }
        }
        private bool _doNotSave;

        protected override void BeginProcessing()
        {
            Dictionary<String, object> parms = this.MyInvocation.BoundParameters;

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
                    new ArgumentException("Label cannot be set alongside RemoveLabel"), "BothLabelAndRemoveLabel",
                    ErrorCategory.InvalidArgument, null));

            if (setAndRemoveDescription)
                this.ThrowTerminatingError(new ErrorRecord(
                    new ArgumentException("Description cannot be set alongside RemoveDescription"), "BothLabelAndRemoveLabel",
                    ErrorCategory.InvalidArgument, null));

            if (setAndRemoveSitelink)
                this.ThrowTerminatingError(new ErrorRecord(
                    new ArgumentException("SiteLinkTitle cannot be set alongside RemoveSitelink"), "BothSitelinkAndRemoveSitelink",
                    ErrorCategory.InvalidArgument, null));

            if (labelButNoLanguage)
                this.ThrowTerminatingError(new ErrorRecord(
                    new ArgumentException("Language is required if Label is specified"), "LabelButNoLanguage",
                    ErrorCategory.InvalidArgument, null));

            if (descriptionButNoLanguage)
                this.ThrowTerminatingError(new ErrorRecord(
                    new ArgumentException("Language is required if Description is specified"), "DescriptionButNoLanguage",
                    ErrorCategory.InvalidArgument, null));

            if (titleButnoSite)
                this.ThrowTerminatingError(new ErrorRecord(
                    new ArgumentException("SitelinkSite is required if SitelinkTitle is specified"), "DescriptionButNoLanguage",
                    ErrorCategory.InvalidArgument, null));

            base.BeginProcessing();
        }

        protected override void ProcessRecord()
        {
            foreach (PSWDItem i in Item)
            {
                if (ShouldProcess(i.QId, "Set label, description or sitelink"))
                {
                    bool touched = false;

                    if (!String.IsNullOrEmpty(Description))
                    {
                        WriteVerbose(String.Format("Setting description {0}: {1} on {2}", Language, Description, i.QId));
                        i.SetDescription(Language, Description);
                        touched = true;
                    }

                    if (!String.IsNullOrEmpty(Label))
                    {
                        WriteVerbose(String.Format("Setting label {0}: {1} on {2}", Language, Label, i.QId));
                        i.SetLabel(Language, Label);
                        touched = true;
                    }

                    if (RemoveDescription)
                    {
                        WriteVerbose(String.Format("Removing description {0} on {1}", Language, i.QId));
                        i.RemoveDescription(Language);
                        touched = true;
                    }

                    if (RemoveLabel)
                    {
                        WriteVerbose(String.Format("Removing label {0} on {1}", Language, i.QId));
                        i.RemoveLabel(Language);
                        touched = true;
                    }

                    if (!String.IsNullOrEmpty(SitelinkSite))
                    {
                        WriteVerbose(String.Format("Setting sitelink {0}: {1} on {2}", SitelinkSite, SitelinkTitle, i.QId));
                        i.SetSitelink(SitelinkSite, SitelinkTitle);
                        touched = true;
                    }

                    if (RemoveSitelink)
                    {
                        WriteVerbose(String.Format("Removing sitelink {0} on {1}", SitelinkSite, i.QId));
                        i.RemoveSitelink(SitelinkSite);
                        touched = true;
                    }

                    if (touched)
                    {
                        if (!DoNotSave)
                        {
                            string comment = i.Save();
                            WriteVerbose(comment);
                        }
                    }

                    WriteObject(i, true);
                }
            }
        }




        protected override void EndProcessing()
        {
            base.EndProcessing();
        }
    }
}
