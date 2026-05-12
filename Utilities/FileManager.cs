using MovieRecommendationSystem.Interfaces;
using Newtonsoft.Json;   // يحول البيانات من C# الى JSON
using static MovieRecommendationSystem.Interfaces.IDataManager;

namespace MovieRecommendationSystem.Utilities
{
    public class FileManager<T> : IDataManager<T>
    {
        // Load data from JSON file.
        public List<T> LoadData(string filePath)
        {
            // If file does not exist, return empty list.
            if (!File.Exists(filePath))
            {
                return new List<T>();
            }

            string json = File.ReadAllText(filePath);

            // If file is empty, return empty list.
            if (string.IsNullOrWhiteSpace(json))
            {
                return new List<T>();
            }

            return JsonConvert.DeserializeObject<List<T>>(json) ?? new List<T>();
        }

        // Save data to JSON file.
        public void SaveData(string filePath, List<T> data)
        {
            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }
    }
}