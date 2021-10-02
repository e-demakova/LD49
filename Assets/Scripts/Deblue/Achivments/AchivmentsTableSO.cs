using System.Collections.Generic;
using UnityEngine;

namespace Deblue.Achivments
{
    public class AchivmentsTable
    {
        public class AchivmentData
        {
            public string Id;
            public string Label;
            public string Description;
            public string IconPath;
            public Sprite Icon;
            public bool   IsUnlocked;
        }

        public AchivmentsTable(string path)
        {
            LoadAchivments(path);
        }

        public AchivmentData this[string id]
        {
            get => _achivments[id];
        }

        private Dictionary<string, AchivmentData> _achivments;
        private AchivmentData[] _achivmentsArray;

        private void LoadAchivments(string path)
        {
            var jsonString = Resources.Load<TextAsset>(path).ToString();

            _achivmentsArray = JsonUtility.FromJson<AchivmentData[]>(jsonString);

            for (int i = 0; i < _achivmentsArray.Length; i++)
            {
                var achivment = _achivmentsArray[i];
                _achivments.Add(achivment.Id, achivment);
                achivment.Icon = Resources.Load<Sprite>(achivment.IconPath);
            }
        }
    }
}
