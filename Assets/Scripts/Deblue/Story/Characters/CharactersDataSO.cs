using System.Collections.Generic;
using UnityEngine;

namespace Deblue.Story.Characters
{
    [CreateAssetMenu(fileName = "CharactersData", menuName = "Story/Characters")]
    public class CharactersDataSO : ScriptableObject
    {
        [System.Serializable]
        public struct Character
        {
            public string CharacterID;
            public Sprite Sprite;
            public bool IsPlayer;
        }

        [SerializeField] private Character[] _charactersData;

        private readonly Dictionary<string, Character> _characters = new Dictionary<string, Character>(5);

        public void SetCharactersToDictionary()
        {
            _characters.Clear();
            for (int i = 0; i < _charactersData.Length; i++)
            {
                _characters.Add(_charactersData[i].CharacterID, _charactersData[i]);
            }
        }

        public Character GetCharacter(string id)
        {
            if (_characters.TryGetValue(id, out var character))
            {
                return character;
            }

            throw new System.Exception($"Character id {id} didn't register.");
        }
    }
}