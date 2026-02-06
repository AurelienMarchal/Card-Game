using UnityEngine;

public class DeathComplexAnimation : ComplexAnimation
{
    
    EntityManager entityManager;

    Animator animator;

    public DeathComplexAnimation(EntityManager entityManager) : base(1)
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
                    animator.SetTrigger("deathTrigger");
                    currentlyAffecting.Add(entityManager);
                }
                
                break;
            case 1:
                Object.Destroy(entityManager.gameObject);
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
