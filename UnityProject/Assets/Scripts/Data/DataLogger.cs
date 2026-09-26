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
            WriteSnapshot(Path.Combine(directory, "assignment.csv"), BuildAssignmentCsv(session));
            WriteSnapshot(Path.Combine(directory, "trials.csv"), BuildCsv(session));
            WriteSnapshot(Path.Combine(directory, "ueqs.csv"), BuildUeqsCsv(session));
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

        // Export the frozen session schedule, rather than recomputing an assignment
        // from study assets that the researcher may edit after the session starts.
        private static string ConditionOrder(ParticipantSession session)
        {
            var order = new StringBuilder();
            for (int i = 0; i < session.PlannedConditions.Count; i += 4)
            {
                if (i > 0) order.Append(" > ");
                order.Append(session.PlannedConditions[i]);
            }
            return order.ToString();
        }

        private static string TaskOrder(ParticipantSession session) => string.Join(" > ", session.PlannedScenarioIds);

        private static string BuildAssignmentCsv(ParticipantSession session)
        {
            var csv = new StringBuilder("participant_id,assignment_cell,condition_order,initial_task_set,task_order,preview,session_completed,trial_number,block_number,trial_in_block,scenario_id,condition,task_set,submitted\n");
            for (int i = 0; i < session.PlannedScenarioIds.Count; i++)
                csv.Append(session.ParticipantId).Append(',').Append(session.OrderNumber).Append(',')
                    .Append(Quote(ConditionOrder(session))).Append(',').Append((session.PlannedTaskSets.Count > 0 ? session.PlannedTaskSets[0] : "")).Append(',')
                    .Append(Quote(TaskOrder(session))).Append(',').Append(session.IsPreview ? "true" : "false").Append(',')
                    .Append(session.Completed ? "true" : "false").Append(',').Append(i + 1).Append(',')
                    .Append(i / 4 + 1).Append(',').Append(i % 4 + 1).Append(',')
                    .Append(Quote(session.PlannedScenarioIds[i])).Append(',').Append(session.PlannedConditions[i]).Append(',').Append(session.PlannedTaskSets[i]).Append(',')
                    .Append(i < session.Responses.Count ? "true" : "false").Append('\n');
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
            var csv = new StringBuilder("participant_id,assignment_cell,initial_task_set,preview,condition_order,task_order,block_number,condition,item_number,dimension,left_anchor,right_anchor,position_1_to_7\n");
            foreach (var response in session.UeqsResponses)
                for (int item = 0; item < UeqsItems.Count; item++)
                {
                    // These fields are generated IDs, enums, numbers, and fixed comma-free anchors.
                    csv.Append(session.ParticipantId).Append(',').Append(session.OrderNumber).Append(',')
                        .Append((session.PlannedTaskSets.Count > 0 ? session.PlannedTaskSets[0] : "")).Append(',').Append(session.IsPreview ? "true" : "false").Append(',')
                        .Append(Quote(ConditionOrder(session))).Append(',').Append(Quote(TaskOrder(session))).Append(',')
                        .Append(response.BlockNumber).Append(',').Append(response.Condition).Append(',')
                        .Append(item + 1).Append(',').Append(item < 4 ? "Pragmatic" : "Hedonic").Append(',')
                        .Append(UeqsItems.Left(item)).Append(',').Append(UeqsItems.Right(item)).Append(',')
                        .Append(response.Positions[item]).Append('\n');
                }
            return csv.ToString();
        }

        private static string BuildCsv(ParticipantSession session)
        {
            var csv = new StringBuilder("participant_id,assignment_cell,condition_order,initial_task_set,preview,session_completed,trial_number,scenario_id,condition,selected_answer,scored,correct,task_order,block_number,trial_in_block,response_time_seconds,task_set,error_category,correct_answer,task_response_time_ms\n");
            string order = ConditionOrder(session);
            for (int i = 0; i < session.Responses.Count; i++)
            {
                var response = session.Responses[i];
                string[] row = {
                    session.ParticipantId, session.OrderNumber.ToString(CultureInfo.InvariantCulture), order,
                    session.PlannedTaskSets[0], session.IsPreview ? "true" : "false",
                    session.Completed ? "true" : "false", (i + 1).ToString(CultureInfo.InvariantCulture),
                    response.ScenarioId, response.Condition.ToString(), response.SelectedAnswer.ToString(),
                    response.Scored ? "true" : "false", response.Scored ? (response.Correct ? "true" : "false") : "",
                    TaskOrder(session), (i / 4 + 1).ToString(CultureInfo.InvariantCulture),
                    (i % 4 + 1).ToString(CultureInfo.InvariantCulture),
                    response.ResponseTimeSeconds.ToString("R", CultureInfo.InvariantCulture),
                    response.TaskSet, response.ErrorCategory, response.CorrectAnswer.ToString(),
                    (response.ResponseTimeSeconds * 1000).ToString("R", CultureInfo.InvariantCulture)
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
