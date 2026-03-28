using System;
using System.IO;
using UnityEngine;

namespace Frezerka.Experiment
{
    public static class ExperimentFileWriter
    {
        private const string FolderName = "ExperimentData";

        public static string GetSavePath()
        {
            return Path.Combine(Application.persistentDataPath, FolderName);
        }

        public static void Save(ExperimentSessionData data, string suffix = "")
        {
            try
            {
                string directory = GetSavePath();
                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);

                string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                string fileName = $"{data.participantId}_{data.machineType}_{data.sessionMode}_{timestamp}{suffix}.json";
                string filePath = Path.Combine(directory, fileName);

                string json = ExperimentJsonSerializer.Serialize(data);
                File.WriteAllText(filePath, json);

                Debug.Log($"[ExperimentFileWriter] Saved: {filePath} ({json.Length} bytes)");
            }
            catch (Exception ex)
            {
                Debug.LogError($"[ExperimentFileWriter] Failed to save: {ex.Message}");
            }
        }

        public static ExperimentSessionData Load(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    Debug.LogError($"[ExperimentFileWriter] File not found: {filePath}");
                    return null;
                }

                string json = File.ReadAllText(filePath);
                return ExperimentJsonSerializer.Deserialize(json);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[ExperimentFileWriter] Failed to load: {ex.Message}");
                return null;
            }
        }
    }
}
