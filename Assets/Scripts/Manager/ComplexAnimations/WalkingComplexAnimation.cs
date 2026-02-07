using UnityEngine;

public class WalkingComplexAnimation : ComplexAnimation
{
    EntityManager entityManager;

    TileManager goalTileManager;

    Animator animator;

    public WalkingComplexAnimation(EntityManager entityManager, TileManager goalTileManager) : base(1)
    {
        this.entityManager = entityManager;
        this.goalTileManager = goalTileManager;
        
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
                    animator.SetInteger("walkVersion", Random.Range(0, 2));
                    animator.SetBool("isWalking", true);

                    entityManager.goalTileManager = goalTileManager;
                    currentlyAffecting.Add(entityManager);
                }
                
                break;
            case 1:
                animator.SetBool("isIdle", true);
                animator.SetBool("isWalking", false);
                currentlyAffecting.Remove(entityManager);
                break;
        }
        
    }

    public override bool StepFinished()
    {
        switch (step)
        {
            case 0 : 
                return entityManager.goalTileManager == null;
        }

        return true;
    }
}
