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
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0, HelpMessage = "Item to be modified.")]
        [PSWDEntityArgumentTransformationAttribute]
        public PSWDItem[] Item { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Language to be used.")]
        public string Language { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Description to be set.")]
        public string Description { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Label to be set.")]
        public string Label { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Sitelink")]
        public PSWDSitelink Sitelink { get; set; }

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
        public string RemoveSitelink {get; set;}

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
                                    && parms.ContainsKey("RemoveLabel"));
            bool setAndRemoveDescription = (parms.ContainsKey("Description")
                                    && parms.ContainsKey("RemoveDescription"));
            bool setAndRemoveSitelink = (parms.ContainsKey("Sitelink")
                        && parms.ContainsKey("RemoveSitelink"));
            bool labelButNoLanguage = (parms.ContainsKey("Label") & !parms.ContainsKey("Language"));
            bool descriptionButNoLanguage = (parms.ContainsKey("Description") & !parms.ContainsKey("Language"));

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
                    new ArgumentException("SiteLink cannot be set alongside RemoveSitelink"), "BothSitelinkAndRemoveSitelink",
                    ErrorCategory.InvalidArgument, null));

            if (labelButNoLanguage)
                this.ThrowTerminatingError(new ErrorRecord(
                    new ArgumentException("Language is required if Label is specified"), "LabelButNoLanguage",
                    ErrorCategory.InvalidArgument, null));

            if (descriptionButNoLanguage)
                this.ThrowTerminatingError(new ErrorRecord(
                    new ArgumentException("Language is required if Description is specified"), "DescriptionButNoLanguage",
                    ErrorCategory.InvalidArgument, null));

            base.BeginProcessing();
        }

        protected override void ProcessRecord()
        {
            foreach (PSWDItem i in Item)
            {
                if (ShouldProcess(i.Id, "Set label, description or sitelink"))
                {
                    bool touched = false;

                    if (!String.IsNullOrEmpty(Description))
                    {
                        WriteVerbose($"Setting description {Language}: {Description} on {i.Id}");
                        i.SetDescription(Language, Description);
                        touched = true;
                    }

                    if (!String.IsNullOrEmpty(Label))
                    {
                        WriteVerbose($"Setting label {Language}: {Label} on {i.Id}");
                        i.SetLabel(Language, Label);
                        touched = true;
                    }

                    if (RemoveDescription)
                    {
                        WriteVerbose($"Removing description {Language} on {i.Id}");
                        i.RemoveDescription(Language);
                        touched = true;
                    }

                    if (RemoveLabel)
                    {
                        WriteVerbose($"Removing label {Language} on {i.Id}");
                        i.RemoveLabel(Language);
                        touched = true;
                    }

                    if (Sitelink != null)
                    {
                        WriteVerbose($"Setting sitelink {Sitelink.Site} {Sitelink.Title} on {i.Id}");
                        i.SetSitelink(Sitelink);
                        touched = true;
                    }

                    if (!String.IsNullOrEmpty(RemoveSitelink))
                    {
                        WriteVerbose($"Removing sitelink {RemoveSitelink} on {i.Id}");
                        i.RemoveSitelink(RemoveSitelink);
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
