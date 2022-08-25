using GrandDevs.ExtremeScooling.Common;

namespace GrandDevs.ExtremeScooling
{
    public interface IDataManager
    {
        CachedUserData CachedUserLocalData { get; set; }

        void SaveAllData();

        void SaveData(Enumerators.GameDataType gameDataType);

        T Deserialize<T>(string data);
    }
}
