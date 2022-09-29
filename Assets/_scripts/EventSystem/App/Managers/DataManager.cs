using GrandDevs.ExtremeScooling.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


namespace GrandDevs.ExtremeScooling
{
    public class DataManager : IService, IDataManager
    {
        private readonly string StaticDataPrivateKey = "PrivateKey";

        public CachedUserData CachedUserLocalData { get; set; }

        public readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings()
        {
            Converters =
                {
                    new StringEnumConverter(),
                }
        };

        private Dictionary<Enumerators.GameDataType, string> _gameDataPathes;

        public void Init()
        {
            _gameDataPathes = new Dictionary<Enumerators.GameDataType, string>()
            {
                 { Enumerators.GameDataType.UserData, $"{Application.persistentDataPath}/userData.dat" },
            };

            LoadAllData();
        }

        public void Update()
        {
        }

        public void Dispose()
        {
            SaveAllData();
        }

        public void SaveAllData()
        {
            foreach (Enumerators.GameDataType key in _gameDataPathes.Keys)
            {
                SaveData(key);
            }
        }

        public void LoadAllData()
        {
            foreach (Enumerators.GameDataType key in _gameDataPathes.Keys)
            {
                LoadData(key);
            }
        }

        public void SaveData(Enumerators.GameDataType gameDataType)
        {
            string data = string.Empty;
            string dataPath = _gameDataPathes[gameDataType];

            switch (gameDataType)
            {
                case Enumerators.GameDataType.UserData:
                    data = Serialize(CachedUserLocalData);
                    break;

                default:break;
            }

            if (data.Length > 0)
            {
                if (!File.Exists(dataPath)) File.Create(dataPath).Close();

                File.WriteAllText(dataPath, data);
            }
        }

        public void LoadData(Enumerators.GameDataType gameDataType)
        {
            string dataPath = _gameDataPathes[gameDataType];

            switch (gameDataType)
            {
                case Enumerators.GameDataType.UserData:
                    CachedUserLocalData = DeserializeFromPath<CachedUserData>(dataPath);
                    if (CachedUserLocalData == null)
                    {
                        CachedUserLocalData = new CachedUserData();
                        CachedUserLocalData.appLanguage = Enumerators.Language.EN;
                    }
                    break;
              
                default: break;
            }
        }

        public T Deserialize<T>(string data)
        {
            return JsonConvert.DeserializeObject<T>(data, JsonSerializerSettings);
        }

        public T DeserializeFromPath<T>(string path) where T : class
        {
            if (!File.Exists(path))
                return null;

            return JsonConvert.DeserializeObject<T>(Decrypt(File.ReadAllText(path)), JsonSerializerSettings);
        }

        public string Serialize(object @object, Formatting formatting = Formatting.Indented)
        {
            return Encrypt(JsonConvert.SerializeObject(@object, formatting));
        }

        private string Decrypt(string data)
        {
            return Constants.DataEncrypted ? Utilites.Decrypt(data, StaticDataPrivateKey) : data;
        }

        private string Encrypt(string data)
        {
            return Constants.DataEncrypted ? Utilites.Encrypt(data, StaticDataPrivateKey) : data;
        }
    }
}