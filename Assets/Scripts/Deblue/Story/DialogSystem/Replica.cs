using UnityEngine;

namespace Deblue.Story.DialogSystem
{
    [System.Serializable]
    public class Replica
    {
        [SerializeField] private string _textId;
        [SerializeField] private string characterId;
        [SerializeField] private EmotionId _emotion;

        public string TextId => _textId;
        public string CharacterId => characterId;
        public EmotionId Emotion => _emotion;

        public Replica(string charactersId, string textId)
        {
            characterId = charactersId;
            _textId = textId;
        }
    }

    public enum EmotionId
    {
        Normal,
        Happy,
        Sad,
        Angry,
        Sceared
    }
}