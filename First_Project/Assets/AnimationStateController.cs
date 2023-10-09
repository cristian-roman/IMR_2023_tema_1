using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
    private const float animationChangingPoint = 10f;
    
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
            distanceToTarget = Vector3.Distance(targetObject.transform.position, Vector3.zero);
            
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
        
        MoveObject();
    }

    void MoveObject()
    {
        float zPosition = Mathf.PingPong(Time.time * 0.1f, 1) * 30f; // Oscillate between 0 and 30 over 1 second
        Vector3 newPosition = targetObject.transform.position;
        newPosition.z = zPosition;
        targetObject.transform.position = newPosition;
    }
}