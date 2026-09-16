using System.Collections.Generic;
using UnityEngine;

namespace Meowra.Data
{
    [CreateAssetMenu(menuName = "Meowra/Study Definition", fileName = "StudyDefinition")]
    public sealed class StudyDefinition : ScriptableObject
    {
        [Tooltip("Six distinct scenarios. Slots 1–2, 3–4 and 5–6 form the three rotating pairs.")]
        public ScenarioData[] scenarios = new ScenarioData[6];
        public Sprite meowraPortrait;
        [TextArea(4, 12)] public string meowraIntroduction;

        public string GetValidationError(bool preview = false)
        {
            if (scenarios == null || scenarios.Length != 6) return "Assign exactly six scenarios.";
            var ids = new HashSet<string>();
            for (int i = 0; i < scenarios.Length; i++)
            {
                var scenario = scenarios[i];
                if (scenario == null) return $"Assign scenario slot {i + 1}.";
                if (string.IsNullOrWhiteSpace(scenario.scenarioId) || !ids.Add(scenario.scenarioId.Trim()))
                    return $"Scenario slot {i + 1} needs a unique, nonempty ID.";
                if (!preview)
                {
                    string error = scenario.GetValidationError();
                    if (error != null) return $"{scenario.name}: {error}";
                }
            }
            if (!preview && string.IsNullOrWhiteSpace(meowraIntroduction)) return "Enter the Meowra Introduction in the study asset.";
            if (!preview && meowraPortrait == null) return "Assign the Meowra Portrait in the study asset.";
            return null;
        }
    }
}
