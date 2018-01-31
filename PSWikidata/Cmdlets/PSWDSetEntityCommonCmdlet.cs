using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using Wikibase;

namespace PSWikidata
{
    public abstract class PSWDSetEntityCommonCmdlet : PSWDNetCmdlet
    {

        [Parameter(Mandatory = false, HelpMessage = "Language to be used.")]
        public string Language { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Description to be set.")]
        public string Description { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Label to be set.")]
        public string Label { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Removes the label")]
        public SwitchParameter RemoveLabel { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Removes the description")]
        public SwitchParameter RemoveDescription { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Change the item but do not save the changes to Wikidata.")]
        public SwitchParameter DoNotSave { get; set; }

        protected void CheckEntityParameters()
        {
            Dictionary<String, object> parms = MyInvocation.BoundParameters;

            bool setAndRemoveLabel = (parms.ContainsKey("Label")
                                    && parms.ContainsKey("RemoveLabel"));
            bool setAndRemoveDescription = (parms.ContainsKey("Description")
                                    && parms.ContainsKey("RemoveDescription"));
            bool setAndRemoveSitelink = (parms.ContainsKey("Sitelink")
                        && parms.ContainsKey("RemoveSitelink"));
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
        }

        protected bool ProcessEntity(PSWDEntity entity)
        {
                    bool touched = false; 
                    
                    if (!String.IsNullOrEmpty(Description))
                    {
                        WriteVerbose($"Setting description {Language}: {Description} on {entity.Id}");
                        entity.SetDescription(Language, Description);
                        touched = true;
                    }

                    if (!String.IsNullOrEmpty(Label))
                    {
                        WriteVerbose($"Setting label {Language}: {Label} on {entity.Id}");
                        entity.SetLabel(Language, Label);
                        touched = true;
                    }

                    if (RemoveDescription)
                    {
                        WriteVerbose($"Removing description {Language} on {entity.Id}");
                        entity.RemoveDescription(Language);
                        touched = true;
                    }

                    if (RemoveLabel)
                    {
                        WriteVerbose($"Removing label {Language} on {entity.Id}");
                        entity.RemoveLabel(Language);
                        touched = true;
                    }

                    return touched;
        }



    }
}
