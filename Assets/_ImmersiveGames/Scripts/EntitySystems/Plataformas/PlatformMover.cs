using DG.Tweening;
using UnityEngine;

namespace _ImmersiveGames.Scripts.EntitySystems.Plataformas {
    public class PlatformMover : MonoBehaviour {
        [SerializeField] private Vector3 moveTo = Vector3.zero;
        [SerializeField] private float moveTime = 1f;
        [SerializeField] private Ease ease = Ease.InOutQuad;

        private Vector3 _startPosition;

        private void Start() {
            _startPosition = transform.position;
            Move();
        }

        private void Move() {
            transform.DOMove(_startPosition + moveTo, moveTime)
                .SetEase(ease)
                .SetLoops(-1, LoopType.Yoyo);
        }
    }
}