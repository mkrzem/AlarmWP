using System.Collections.ObjectModel;
using System;
using Windows.Storage;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace AlarmAttempt.DAL
{
    public static class StorageManager 
    {
        public static async Task<T> ReadFromFile<T>(string fileName, StorageFolder location) where T : class
        {            
            string serializedData = "";
            StorageFile dataFile = await location.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);

            if (dataFile != null)
            {
                serializedData = await FileIO.ReadTextAsync(dataFile);                
            }

            var result = JsonConvert.DeserializeObject<T>(serializedData);
            return result;
        }

        public static async Task WriteToFile<T>(T data, string fileName, StorageFolder location) where T : class
        {
            string serializedData = JsonConvert.SerializeObject(data);
            StorageFile dataFile = await location.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);

            if (dataFile != null)
            {
                await FileIO.WriteTextAsync(dataFile, serializedData);
            }
        }

        public static async Task<IReadOnlyList<StorageFile>> GetFiles(StorageFolder location)
        {
            if (location == null)
            {
                return new List<StorageFile>();                
            }

            return await location.GetFilesAsync();
            
        }
    }
}
