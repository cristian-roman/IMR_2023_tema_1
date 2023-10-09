using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
    private const float animationChangingPoint = 0.1f;
    
    private Animator animator;
    public GameObject targetObject;
    private float distanceToTarget;

    private int isAttackingHash;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        isAttackingHash = Animator.StringToHash("isAttacking");
    }

    // Update is called once per frame
    void Update()
    {
        bool isAttacking = animator.GetBool(isAttackingHash);
        if (targetObject != null)
        {
            distanceToTarget = Vector3.Distance(transform.position, targetObject.transform.position);
        
            if (distanceToTarget < animationChangingPoint)
            {
                // Play the "Attacking" animation
                animator.SetBool(isAttackingHash, true);
            }
            else if (distanceToTarget >= animationChangingPoint)
            {
                // Play the "Bounce" animation
                animator.SetBool(isAttackingHash, false);
            }
        }
    }
}