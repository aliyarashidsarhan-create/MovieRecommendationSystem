using System;
using System.Collections.Generic;
using System.Text;

namespace MovieRecommendationSystem.Interfaces
{
    internal interface IDataManager
    {
        // Generic interface for saving and loading data.
        public interface IDataManager<T>
        {
            List<T> LoadData(string filePath);
            void SaveData(string filePath, List<T> data);
        }
    }
}
//