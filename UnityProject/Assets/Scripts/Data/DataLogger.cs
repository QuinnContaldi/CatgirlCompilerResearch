using System;
using System.Globalization;
using System.IO;
using System.Text;
using UnityEngine;

namespace Meowra.Data
{
    // A plain C# service: no scene object or Unity lifecycle is needed for file writing.
    public sealed class DataLogger
    {
        public string RootDirectory { get; }

        public DataLogger(string rootDirectory)
        {
            RootDirectory = rootDirectory ?? throw new ArgumentNullException(nameof(rootDirectory));
        }

        public string GetSessionDirectory(ParticipantSession session) => Path.Combine(
            RootDirectory, session.IsPreview ? "Previews" : "Participants", session.ParticipantId);

        public void Save(ParticipantSession session)
        {
            string directory = GetSessionDirectory(session);
            Directory.CreateDirectory(directory);
            // JSON is authoritative. CSV can be regenerated from it if the second write fails.
            WriteSnapshot(Path.Combine(directory, "session.json"), JsonUtility.ToJson(session, true));
            WriteSnapshot(Path.Combine(directory, "trials.csv"), BuildCsv(session));
            WriteSnapshot(Path.Combine(directory, "ueqs.csv"), BuildUeqsCsv(session));
            WriteSnapshot(Path.Combine(directory, "api.csv"), BuildApiCsv(session));
            WriteSnapshot(Path.Combine(directory, "preference.csv"), BuildPreferenceCsv(session));
        }

        private static void WriteSnapshot(string path, string content)
        {
            string temporary = path + ".tmp";
            using (var stream = new FileStream(temporary, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                byte[] bytes = new UTF8Encoding(false).GetBytes(content);
                stream.Write(bytes, 0, bytes.Length);
                stream.Flush(true);
            }
            // Replace only after the complete snapshot has been flushed. Keep the previous version.
            if (File.Exists(path)) File.Replace(temporary, path, path + ".bak");
            else File.Move(temporary, path);
        }

        private static string Quote(string value) => "\"" + (value ?? "").Replace("\"", "\"\"") + "\"";

        private static string BuildApiCsv(ParticipantSession session)
        {
            var csv = new StringBuilder("participant_id,item_number,dimension,item,rating_1_to_5\n");
            if (session.ApiSubmitted)
                for (int item = 0; item < ApiItems.Count; item++)
                    csv.Append(session.ParticipantId).Append(',').Append(item + 1).Append(',')
                        .Append(ApiItems.Dimension(item)).Append(',').Append(Quote(ApiItems.Wording(item))).Append(',')
                        .Append(session.ApiResponse.Ratings[item]).Append('\n');
            return csv.ToString();
        }

        private static string BuildPreferenceCsv(ParticipantSession session)
        {
            var csv = new StringBuilder("participant_id,preferred_condition,reason_submitted,reason\n");
            if (session.PreferenceSubmitted)
                csv.Append(session.ParticipantId).Append(',').Append(session.PreferredCondition).Append(',')
                    .Append(session.ReasonSubmitted ? "true" : "false").Append(',')
                    .Append(Quote(session.PreferenceReason)).Append('\n');
            return csv.ToString();
        }

        private static string BuildUeqsCsv(ParticipantSession session)
        {
            var csv = new StringBuilder("participant_id,order_number,stimulus_set,preview,block_number,condition,item_number,dimension,left_anchor,right_anchor,position_1_to_7\n");
            foreach (var response in session.UeqsResponses)
                for (int item = 0; item < UeqsItems.Count; item++)
                {
                    // These fields are generated IDs, enums, numbers, and fixed comma-free anchors.
                    csv.Append(session.ParticipantId).Append(',').Append(session.OrderNumber).Append(',')
                        .Append(session.StimulusSet).Append(',').Append(session.IsPreview ? "true" : "false").Append(',')
                        .Append(response.BlockNumber).Append(',').Append(response.Condition).Append(',')
                        .Append(item + 1).Append(',').Append(item < 4 ? "Pragmatic" : "Hedonic").Append(',')
                        .Append(UeqsItems.Left(item)).Append(',').Append(UeqsItems.Right(item)).Append(',')
                        .Append(response.Positions[item]).Append('\n');
                }
            return csv.ToString();
        }

        private static string BuildCsv(ParticipantSession session)
        {
            var csv = new StringBuilder("participant_id,order_number,condition_order,stimulus_set,preview,trial_section_completed,trial_number,scenario_id,condition,selected_answer,scored,correct,response_time_seconds\n");
            string order = string.Join(" > ", Experiment.Counterbalancing.GetOrder(session.OrderNumber));
            for (int i = 0; i < session.Responses.Count; i++)
            {
                var response = session.Responses[i];
                string[] row = {
                    session.ParticipantId, session.OrderNumber.ToString(CultureInfo.InvariantCulture), order,
                    session.StimulusSet.ToString(CultureInfo.InvariantCulture), session.IsPreview ? "true" : "false",
                    session.Completed ? "true" : "false", (i + 1).ToString(CultureInfo.InvariantCulture),
                    response.ScenarioId, response.Condition.ToString(), response.SelectedAnswer.ToString(),
                    response.Scored ? "true" : "false", response.Scored ? (response.Correct ? "true" : "false") : "",
                    response.ResponseTimeSeconds.ToString("R", CultureInfo.InvariantCulture)
                };
                for (int column = 0; column < row.Length; column++)
                {
                    if (column > 0) csv.Append(',');
                    csv.Append('"').Append(row[column].Replace("\"", "\"\"")).Append('"');
                }
                csv.Append('\n');
            }
            return csv.ToString();
        }
    }
}
