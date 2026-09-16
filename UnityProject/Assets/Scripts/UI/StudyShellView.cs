using System.Collections.Generic;
using Meowra.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Meowra.UI
{
    public sealed class StudyShellView : MonoBehaviour
    {
        [SerializeField] private Dropdown orderDropdown;
        [SerializeField] private Dropdown stimulusSetDropdown;
        [SerializeField] private Text setupStatus;
        [SerializeField] private Button startButton;
        [SerializeField] private Button previewButton;
        [SerializeField] private GameObject previewBanner;
        [SerializeField] private Text introduction;
        [SerializeField] private Image introductionPortrait;
        [SerializeField] private Text completion;

        public int OrderNumber => orderDropdown.value + 1;
        public int StimulusSet => stimulusSetDropdown.value + 1;

        public void Configure(List<string> orders)
        {
            orderDropdown.ClearOptions();
            orderDropdown.AddOptions(orders);
            stimulusSetDropdown.ClearOptions();
            stimulusSetDropdown.AddOptions(new List<string> { "1: Original pairs", "2: Rotate pairs once", "3: Rotate pairs twice" });
        }

        public void ShowSetup(StudyDefinition study)
        {
            string error = study == null ? "Assign a Study Definition on ExperimentManager." : study.GetValidationError();
            startButton.interactable = error == null;
            previewButton.interactable = study != null && study.GetValidationError(true) == null;
            setupStatus.text = error == null
                ? "Trial content is configured. This skeleton does not yet save responses to disk."
                : $"Content needs attention: {error}\nUse Preview Layout to inspect the blank templates.";
            previewBanner.SetActive(false);
        }

        public void ShowSession(bool preview) => previewBanner.SetActive(preview);

        public void ShowIntroduction(StudyDefinition study)
        {
            introduction.text = string.IsNullOrWhiteSpace(study.meowraIntroduction)
                ? "[Your Dr. Meowra introduction text]" : study.meowraIntroduction;
            introductionPortrait.sprite = study.meowraPortrait;
            introductionPortrait.gameObject.SetActive(study.meowraPortrait != null);
        }

        public void ShowCompletion(bool preview)
        {
            completion.text = preview
                ? "Layout preview complete. Preview answers are not scored."
                : "Trial section complete. Please notify the researcher.";
        }
    }
}
