using System;
using System.IO;
using UnityEngine;

namespace Meowra.Data
{
    // Disk-based discovery also works after restarting the app or leaving Play mode.
    public static class StudyResults
    {
        public static string RootDirectory => Path.Combine(Application.persistentDataPath, "StudySessions");

        public static string FindLatestParticipantDirectory(string rootDirectory)
        {
            string participants = Path.Combine(rootDirectory, "Participants");
            if (!Directory.Exists(participants)) return null;
            string latest = null;
            DateTime latestWrite = DateTime.MinValue;
            foreach (string directory in Directory.EnumerateDirectories(participants))
            {
                string snapshot = Path.Combine(directory, "session.json");
                if (!File.Exists(snapshot)) continue;
                DateTime written = File.GetLastWriteTimeUtc(snapshot);
                if (latest == null || written > latestWrite ||
                    (written == latestWrite && string.CompareOrdinal(directory, latest) > 0))
                {
                    latest = directory;
                    latestWrite = written;
                }
            }
            return latest;
        }

        public static string OpenSavedData(string rootDirectory, bool latestParticipant)
        {
            try
            {
                string directory = latestParticipant ? FindLatestParticipantDirectory(rootDirectory) : rootDirectory;
                if (directory == null) return "No live sessions saved yet. Use Open saved data to view previews.";
                Directory.CreateDirectory(directory);
                // Only locally generated paths are used here, never participant input.
#if UNITY_EDITOR
                UnityEditor.EditorUtility.RevealInFinder(directory + Path.DirectorySeparatorChar);
#else
                Application.OpenURL(new Uri(Path.GetFullPath(directory) + Path.DirectorySeparatorChar).AbsoluteUri);
#endif
                return "Saved data: " + directory;
            }
            catch (Exception error) when (error is IOException || error is UnauthorizedAccessException ||
                error is System.Security.SecurityException)
            {
                Debug.LogWarning("Could not open saved data: " + error.Message);
                return "Could not open the folder. Saved data location: " + rootDirectory;
            }
        }
    }
}
