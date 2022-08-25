using GrandDevs.ExtremeScooling.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using GrandDevs.ExtremeScooling.Helpers;

namespace GrandDevs.ExtremeScooling
{
    public class LocalizationManager : IService, ILocalizationManager
    {
        public event Action<Enumerators.Language> LanguageWasChangedEvent;

        private IDataManager _dataManager;

        private Enumerators.Language _defaultLanguage = Enumerators.Language.EN,
                                     _currentLanguage = Enumerators.Language.EN;


        private Dictionary<SystemLanguage, Enumerators.Language> _languages;

        public Dictionary<SystemLanguage, Enumerators.Language> SupportedLanguages { get { return _languages; } }

        public Enumerators.Language CurrentLanguage { get { return _currentLanguage; } }
        public Enumerators.Language DefaultLanguage { get { return _defaultLanguage; } }

        public void Dispose()
        {
        }

        public void Init()
        {
            _dataManager = GameClient.Get<IDataManager>();
            FillLanguages();
            ApplyLocalization();
        }

        public void ApplyLocalization()
        {
            if (!_languages.ContainsKey(Application.systemLanguage))
            {
                if (_dataManager.CachedUserLocalData.appLanguage == Enumerators.Language.NONE)
                    SetLanguage(_defaultLanguage, true);
                else
                {
                    SetLanguage(_dataManager.CachedUserLocalData.appLanguage, true);
                }
            }
            else
            {
                if (_dataManager.CachedUserLocalData.appLanguage == Enumerators.Language.NONE)
                    SetLanguage(_languages[Application.systemLanguage], true);
                else
                    SetLanguage(_dataManager.CachedUserLocalData.appLanguage, true);
            }
        }


        public void Update()
        {
        }

        public void SetLanguage(Enumerators.Language language, bool forceUpdate = false)
        {
            if (language == CurrentLanguage && !forceUpdate)
                return;

            string languageCode = language.ToString().ToLower();


            _currentLanguage = language;


            if (LanguageWasChangedEvent != null)
                LanguageWasChangedEvent(_currentLanguage);
        }

        public string GetUITranslation(string key)
        {
            return InternalTools.ReplaceLineBreaks(key);
        }


        private void FillLanguages()
        {
            _languages = new Dictionary<SystemLanguage, Enumerators.Language>();

            _languages.Add(SystemLanguage.English, Enumerators.Language.EN);
        }
    }
}