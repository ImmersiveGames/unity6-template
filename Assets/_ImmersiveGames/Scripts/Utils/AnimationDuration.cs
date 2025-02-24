using UnityEngine;

namespace _ImmersiveGames.Scripts.Utils {
    public static class AnimationDuration
    {
        public static float GetAnimationDuration(Animator animator, string triggerName)
        {
            if (animator == null) return 0f;
            var controller = animator.runtimeAnimatorController;
            foreach (var clip in controller.animationClips)
            {
                if (clip.name == triggerName) return clip.length;
            }
            return 0f; // Default se não encontrar
        }
    }
}