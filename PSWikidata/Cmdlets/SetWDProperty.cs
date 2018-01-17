using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using Wikibase;

namespace PSWikidata
{
    [Cmdlet(VerbsCommon.Set, "WDProperty",
        SupportsShouldProcess = true,
        ConfirmImpact = ConfirmImpact.Medium)]
    public class SetWDProperty : PSWDNetCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0, HelpMessage = "Property to be modified.")]
        [PSWDEntityArgumentTransformationAttribute]
        public PSWDProperty[] Property { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Language to be used.")]
        public string Language { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Description to be set.")]
        public string Description { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Label to be set.")]
        public string Label { get; set; }

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

        [Parameter(Mandatory = false, HelpMessage = "Change the item but do not save the changes to Wikidata.")]
        public SwitchParameter DoNotSave { get; set; }

        protected override void BeginProcessing()
        {
            Dictionary<String, object> parms = MyInvocation.BoundParameters;

            bool setAndRemoveLabel = (parms.ContainsKey("Label")
                                    && parms.ContainsKey("RemoveLabel"));
            bool setAndRemoveDescription = (parms.ContainsKey("Description")
                                    && parms.ContainsKey("RemoveDescription"));
            bool labelButNoLanguage = (parms.ContainsKey("Label") & !parms.ContainsKey("Language"));
            bool descriptionButNoLanguage = (parms.ContainsKey("Description") & !parms.ContainsKey("Language"));

            if (setAndRemoveLabel)
                ThrowTerminatingError(new ErrorRecord(
                    new ArgumentException("Label cannot be set alongside RemoveLabel"), "BothLabelAndRemoveLabel",
                    ErrorCategory.InvalidArgument, null));

            if (setAndRemoveDescription)
                ThrowTerminatingError(new ErrorRecord(
                    new ArgumentException("Description cannot be set alongside RemoveDescription"), "BothLabelAndRemoveLabel",
                    ErrorCategory.InvalidArgument, null));

            if (labelButNoLanguage)
                ThrowTerminatingError(new ErrorRecord(
                    new ArgumentException("Language is required if Label is specified"), "LabelButNoLanguage",
                    ErrorCategory.InvalidArgument, null));

            if (descriptionButNoLanguage)
                ThrowTerminatingError(new ErrorRecord(
                    new ArgumentException("Language is required if Description is specified"), "DescriptionButNoLanguage",
                    ErrorCategory.InvalidArgument, null));

            base.BeginProcessing();
        }

        protected override void ProcessRecord()
        {
            foreach (PSWDProperty p in Property)
            {
                if (ShouldProcess(p.Id, "Set label, description or sitelink"))
                {
                    bool touched = false;

                    if (!String.IsNullOrEmpty(Description))
                    {
                        WriteVerbose($"Setting description {Language}: {Description} on {p.Id}");
                        p.SetDescription(Language, Description);
                        touched = true;
                    }

                    if (!String.IsNullOrEmpty(Label))
                    {
                        WriteVerbose($"Setting label {Language}: {Label} on {p.Id}");
                        p.SetLabel(Language, Label);
                        touched = true;
                    }

                    if (RemoveDescription)
                    {
                        WriteVerbose($"Removing description {Language} on {p.Id}");
                        p.RemoveDescription(Language);
                        touched = true;
                    }

                    if (RemoveLabel)
                    {
                        WriteVerbose($"Removing label {Language} on {p.Id}");
                        p.RemoveLabel(Language);
                        touched = true;
                    }

                    if (touched)
                    {
                        if (!DoNotSave)
                        {
                            string comment = p.Save();
                            WriteVerbose(comment);
                        }
                    }

                    WriteObject(p, true);
                }
            }
        }

    }
}
