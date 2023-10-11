using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeetAnimation : MonoBehaviour
{
    public float moveSpeed = 1.0f;  // Speed at which the birds move towards each other
    public float proximityThreshold = 0.1f;  // Adjust this value to set how close the birds need to be to trigger the interaction

	private Animator bird1Animator;
	private Animator bird2Animator;

    private GameObject bird1;  // Reference to the first bird
    private GameObject bird2;  // Reference to the second bird

    void Update()
    {
        // Check if both birds are instantiated and close enough
        if (bird1 != null && bird2 != null)
        {
            MoveShapesTowardsEachOther();
			PlayWalkingAnimation();

			if (AreShapesClose())
            {
                PlayAttackAnimation();
            }
        }
    }

    bool AreShapesClose()
    {
        float distance = Vector3.Distance(bird1.transform.position, bird2.transform.position);
        return distance < proximityThreshold;
    }

    void MoveShapesTowardsEachOther()
    {
        Vector3 direction = (bird2.transform.position - bird1.transform.position).normalized;
        bird1.transform.Translate(direction * moveSpeed * Time.deltaTime);
        bird2.transform.Translate(-direction * moveSpeed * Time.deltaTime);
    }

    void PlayAttackAnimation()
    {
      	bird1Animator.SetTrigger("Attacking");
		bird2Animator.SetTrigger("Attacking");
    }

	void PlayWalkingAnimation()
	{
		bird1Animator.SetTrigger("Walking");
        bird2Animator.SetTrigger("Walking");
	}

    public void SetShape(GameObject bird)
    {
        if (bird1 == null)
        {
            bird1 = bird;
        }
        else if (bird2 == null)
        {
            bird2 = bird;
        }
    }

	public void SetAnimator(Animator animator)
    {
       if(bird1Animator == null)
        {
            bird1Animator = animator;
        }
        else if (bird2Animator == null)
        {
            bird2Animator = animator;
        }
    }
}
