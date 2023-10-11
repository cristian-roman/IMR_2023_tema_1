using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeetAnimation : MonoBehaviour
{
    private Animator animator;
    private GameObject bird;

    public void SetAnimator(Animator anim)
    {
        animator = anim;
    }

    public void SetShape(GameObject obj)
    {
        bird = obj;
    }

    public void PlayBounceAnimation()
    {
        if (animator != null)
        {
            animator.Play("Bounce"); 
        }
    }

    public void PlayAttackAnimation()
    {
        if (animator != null)
        {
            animator.Play("Attack"); 
        }
    }
}
