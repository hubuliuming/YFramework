using UnityEngine;
using YFramework;

interface IAnimatorUti :IUtility
{
    
    /// <summary>
    /// 动态替换Animation,替换前的动画名字为Default
    /// </summary>
    /// <param name="animator"></param>
    /// <param name="clip"></param>
    void RePlaceAnimation(Animator animator,AnimationClip clip);
}

public class AnimatorUti : IAnimatorUti
{
    public void RePlaceAnimation(Animator animator,AnimationClip clip)
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