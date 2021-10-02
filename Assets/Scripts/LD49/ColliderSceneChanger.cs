using Deblue.SceneManagement;
using LD49.Hero;
using UnityEngine;

namespace LD49
{
    public class ColliderSceneChanger : SceneChanger
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<HeroController>(out var hero)) 
                LoadScene();
        }
    }
}
