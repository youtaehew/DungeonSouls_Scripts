using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    public Animator ani;
    public bool canRotate;

    public void PlayTargetAnimation(string targetAni, bool isInteracting)
    {
        ani.applyRootMotion = isInteracting;
        //ani.SetBool("canRotate", canRotate);
        ani.SetBool("isInteracting", isInteracting);
        ani.CrossFade(targetAni, 0.2f);
        
    }

    // public void PlayTargetAnimationWithRoorRotation(string targetAni, bool isInteracting)
    // {
    //     ani.applyRootMotion = isInteracting;
    //     ani.SetBool("isRotatingWithRoot", canRotate);
    //     ani.SetBool("isInteracting", isInteracting);
    //     ani.CrossFade(targetAni, 0.2f);
    // }
}
