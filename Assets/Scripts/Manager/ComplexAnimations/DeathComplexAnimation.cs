using UnityEngine;

public class DeathComplexAnimation : ComplexAnimation
{
    EntityManager entityManager;

    Animator animator;

    protected override void Init() 
    {
        base.Init();
        entityManager = gameObject.GetComponentInParent<EntityManager>();
        animator = gameObject.GetComponentInParent<Animator>();
    }

    public override void PlayStep()
    {
        switch (step)
        {
            case 0:
                if (animator)
                {   
                    
                    animator.SetTrigger("deathTrigger");
                    animator.SetBool("isIdle", false);
                    currentlyAffecting.Add(entityManager);
                }
                
                break;
            case 1:
                currentlyAffecting.Remove(entityManager);
                break;
        }
        
    }

    public override bool StepFinished()
    {
        switch (step)
        {
            case 0 : 
                return animator == null || !AnimatorIsPlaying(animator) ;
        }

        return true;
    }
}
