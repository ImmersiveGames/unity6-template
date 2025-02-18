using DG.Tweening;
using UnityEngine;

namespace _ImmersiveGames.Scripts.EntitySystems {
    [RequireComponent(typeof(AudioSource))]
    public class SpawnEffects : MonoBehaviour {
        [SerializeField] private GameObject spawnVFX;
        [SerializeField] private float animationDuration = 1f;

        private void Start() {
            transform.localScale = Vector3.zero;
            transform.DOScale(Vector3.one, animationDuration).SetEase(Ease.OutBack);
            
            if (spawnVFX != null) {
                Instantiate(spawnVFX, transform.position, Quaternion.identity);
            }
            
            GetComponent<AudioSource>().pitch = Random.Range(0.9f, 1.1f);
            GetComponent<AudioSource>().Play();
        }
    }
}