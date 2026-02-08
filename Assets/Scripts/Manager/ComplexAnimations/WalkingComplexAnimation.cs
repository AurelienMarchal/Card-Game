using UnityEngine;

public class WalkingComplexAnimation : ComplexAnimation
{
    EntityManager entityManager;

    [SerializeField]
    public TileManager goalTileManager;

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
                    animator.SetBool("isIdle", false);
                    animator.SetInteger("walkVersion", Random.Range(0, 2));
                    animator.SetBool("isWalking", true);

                    entityManager.goalTileManager = goalTileManager;
                    currentlyAffecting.Add(entityManager);
                }
                
                break;
            case 1:
                animator.SetBool("isWalking", false);
                break;

            case 2:
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
                return entityManager.goalTileManager == null;

            case 1 : 
                return animator == null || animator.GetCurrentAnimatorStateInfo(0).IsName("ToIdle");
        }

        return true;
    }
}
