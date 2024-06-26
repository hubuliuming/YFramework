using UnityEngine;

namespace YFramework.Extension
{
    public static class AnimatorExtension
    {
        /// <summary>
        /// 动态替换Animation,替换前的动画名字为Default
        /// </summary>
        /// <param name="animator"></param>
        /// <param name="clip"></param>
        public static void RePlaceAnimation(this Animator animator,AnimationClip clip)
        {
            AnimatorOverrideController overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
            var clips = overrideController.animationClips;
            for (int i = 0; i < clips.Length; i++)
            {
                if (clips[i].name.Contains("Default"))
                {
                    overrideController["Default"] = clip;
                    break;
                }
            }
            animator.runtimeAnimatorController = overrideController;
        }
    }
}