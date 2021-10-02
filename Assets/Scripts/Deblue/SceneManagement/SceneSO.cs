using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;

namespace Deblue.SceneManagement
{
    [CreateAssetMenu(fileName = "Scene", menuName = "Scenes/Scene")]
    public class SceneSO : ScriptableObject
    {
        [FormerlySerializedAs("Reference")] public AssetReference AssetRef;
    }
}