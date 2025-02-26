using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace _ImmersiveGames.Scripts.Utils.SerializedSystems {
    public class FileDataService : IDataService {
        private readonly ISerializer serializer;
        private readonly string dataPath;
        private readonly string fileExtension;

        public FileDataService(ISerializer serializer) {
            dataPath = Application.persistentDataPath;
            fileExtension = "json";
            this.serializer = serializer;
        }

        private string GetPathToFile(string fileName) {
            return Path.Combine(dataPath, string.Concat(fileName, ".", fileExtension));
        }
        
        public void Save(GameData data, bool overwrite = true) {
            var fileLocation = GetPathToFile(data.name);

            if (!overwrite && File.Exists(fileLocation)) {
                throw new IOException($"The file '{data.name}.{fileExtension}' already exists and cannot be overwritten.");
            }

            File.WriteAllText(fileLocation, serializer.Serialize(data));
        }

        public GameData Load(string name) {
            var fileLocation = GetPathToFile(name);

            if (!File.Exists(fileLocation)) {
                throw new ArgumentException($"No persisted GameData with name '{name}'");
            }

            return serializer.Deserialize<GameData>(File.ReadAllText(fileLocation));
        }

        public void Delete(string name) {
            var fileLocation = GetPathToFile(name);

            if (File.Exists(fileLocation)) {
                File.Delete(fileLocation);
            }
        }

        public void DeleteAll() {
            foreach (var filePath in Directory.GetFiles(dataPath)) {
                File.Delete(filePath);
            }
        }

        public IEnumerable<string> ListSaves() {
            foreach (var path in Directory.EnumerateFiles(dataPath)) {
                if (Path.GetExtension(path) == fileExtension) {
                    yield return Path.GetFileNameWithoutExtension(path);
                }
            }
        }
    }
}