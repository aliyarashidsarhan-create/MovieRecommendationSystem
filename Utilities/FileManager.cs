using MovieRecommendationSystem.Interfaces;
using Newtonsoft.Json;
using static MovieRecommendationSystem.Interfaces.IDataManager;

namespace MovieRecommendationSystem.Utilities
{
    // Generic class for loading and saving JSON data.
    public class FileManager<T> : IDataManager<T>
    {
        // Load data from JSON file.
        public List<T> LoadData(string filePath)
        {
            // Get folder path.
            string? directory = Path.GetDirectoryName(filePath);

            // Create folder if it does not exist.
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Create file if it does not exist.
            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath, "[]");

                return new List<T>();
            }

            // Read JSON content.
            string json = File.ReadAllText(filePath);

            // Return empty list if file is empty.
            if (string.IsNullOrWhiteSpace(json))
            {
                return new List<T>();
            }

            // Convert JSON into list.
            return JsonConvert.DeserializeObject<List<T>>(json) ?? new List<T>();
        }

        // Save data to JSON file.
        public void SaveData(string filePath, List<T> data)
        {
            // Get folder path.
            string? directory = Path.GetDirectoryName(filePath);

            // Create folder if it does not exist.
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Convert object list into JSON.
            string json = JsonConvert.SerializeObject(data, Formatting.Indented);

            // Save JSON into file.
            File.WriteAllText(filePath, json);
        }
    }
}
