using UnityEngine;

#if UNITY_EDITOR
#endif

namespace GellosGames
{
    public abstract class Profile : ScriptableObject, IService
    {
        static string FolderPathAudio = "profiles/sound/";
        static string FolderPathColor = "profiles/color/";
        static string FolderPathAnimation = "profiles/animation/";

        public static Tprofil LoadService<Tprofil>(string fullPathName) where Tprofil : Profile
        {
            var CachedProfile = Resources.Load<Tprofil>(fullPathName);
            if (CachedProfile is initializable)
                ((initializable)CachedProfile).Initialization();
#if UNITY_EDITOR
            Locator.SetCurrentProfile(fullPathName, CachedProfile);
#endif
            return CachedProfile;
        }
        public static void LoadRessorces()
        {
            // Load default Profiles
            Locator.AddService(LoadService<ColorProfile>(FolderPathColor + "default"));
            Locator.AddService(LoadService<AnimationProfiles>(FolderPathAnimation + "default"));
            //Locator.AddService(LoadService<SoundProfiles>(FolderPathAudio + "default"));
            //Locator.AddService(LoadService<TutorialTexts>("TutorialTexts"));
            //Locator.AddService(LoadService<InAppTextDescriptions>("AllInAppTextDescriptions"));
        }
    }

    public interface initializable
    {
        void Initialization();
    }
}
