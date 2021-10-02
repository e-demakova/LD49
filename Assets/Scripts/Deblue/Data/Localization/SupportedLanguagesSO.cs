using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Deblue.Data.Localization
{
    [CreateAssetMenu(fileName = "SupportedLanguages", menuName = "Configs/Localization")]
    public class SupportedLanguagesSO : ScriptableObject
    {
        public Language[] Languages;
        
        public Language this[SystemLanguage type] => Languages.First(x => x.LanguageType == type);
    }
}