using UnityEngine;

public class EntityTakesDamageComplexAnimation : ComplexAnimation
{
    EntityManager entityManager;


    Animator animator;

    public EntityTakesDamageComplexAnimation(EntityManager entityManager) : base(1)
    {
        this.entityManager = entityManager;
        
        animator = entityManager.gameObject.GetComponent<Animator>();
    }

    public override void PlayStep()
    {
        switch (step)
        {
            case 0:
                if (animator)
                {   
                    animator.SetBool("isIdle", false);
                    animator.SetInteger("hitVersion", Random.Range(0, 1));
                    animator.SetTrigger("hitTrigger");
                    currentlyAffecting.Add(entityManager);
                }
                
                break;
            case 1:
                animator.SetBool("isIdle", true);
                currentlyAffecting.Remove(entityManager);
                break;
        }
        
    }

    public override bool StepFinished()
    {
        switch (step)
        {
            case 0 :
                return animator == null || !AnimatorIsPlaying(animator);
        }

        return true;
    }
}
