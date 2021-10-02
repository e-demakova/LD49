using System;
using System.Collections.Generic;
using Deblue.ObservingSystem;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Deblue.Data.Localization
{
    [Serializable]
    public struct Language
    {
        public SystemLanguage LanguageType;
        public AssetReference FileAssetRef;
    }

    public readonly struct LanguageChange
    {
        public readonly Language NewLanguage;

        public LanguageChange(Language language)
        {
            NewLanguage = language;
        }
    }

    public class LocalizationService
    {
        public readonly Handler<LanguageChange> LanguageChange = new Handler<LanguageChange>();

        public SystemLanguage DefaultLanguage = SystemLanguage.Russian;
        public Language Language;

        private readonly Dictionary<string, string> _locale = new Dictionary<string, string>(50);
        private readonly LoadService _loader;
        private readonly SupportedLanguagesSO _languages;

        public LocalizationService(LoadService loader, SupportedLanguagesSO languages)
        {
            _loader = loader;
            _languages = languages;
            
            LoadLanguage();
            LoadTextData();
        }

        public void ChangeLanguage(SystemLanguage language)
        {
            Language = _languages[language];
            LoadTextData();
            SaveSelectedLanguage();
            RaiseLanguageChanged();
        }

        private void LoadLanguage()
        {
            var languageString = PlayerPrefs.GetString("Language");
            var language = DefaultLanguage;

            if (!string.IsNullOrEmpty(languageString)) 
                Enum.TryParse(languageString, out language);

            Language = _languages[language];
        }
        
        private void SaveSelectedLanguage()
        {
            PlayerPrefs.SetString("Language", Enum.GetName(typeof(SystemLanguage), Language.LanguageType));
            PlayerPrefs.Save();
        }

        private void LoadTextData()
        {
            var assetRef = Language.FileAssetRef;
            _locale.Clear();
            _loader.LoadAddressableJSON<TextItems>(AddLocalizationToDictionary, assetRef);

            void AddLocalizationToDictionary(TextItems data)
            {
                var items = data.Items;
                for (int i = 0; i < items.Length; i++)
                {
                    _locale.Add(items[i].Id, items[i].Text);
                }
            }
        }

        public string GetText(string id)
        {
            _locale.TryGetValue(id, out var text);
            if (string.IsNullOrEmpty(text))
                throw new Exception($"Text id {id} didn't register.");

            return text;
        }

        private void RaiseLanguageChanged()
        {
            LoadTextData();
            LanguageChange.Raise(new LanguageChange(Language));
        }
    }
}