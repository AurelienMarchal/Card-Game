using GameLogic.GameState;
using UnityEngine;

public class EntityMoveComplexAnimation : ComplexAnimation
{
    EntityManager entityManager;

    [SerializeField]
    TileManager goalTileManager;

    Animator animator;

    public override bool Init(ActionState actionState) 
    {
        base.Init(actionState);
        finalStep = 2;
        var entityMoveActionState = (EntityMoveActionState)actionState;

        if (entityMoveActionState == null)
        {
            return false;
        }

        var boardManager = FindFirstObjectByType<BoardManager>();

        if (!boardManager)
        {
            return false;
        }

        goalTileManager = boardManager.GetTileManagerFromTileNum(entityMoveActionState.endTileNum);

        if (!goalTileManager)
        {
            return false;
        }

        entityManager = gameObject.GetComponentInParent<EntityManager>();
        animator = gameObject.GetComponentInParent<Animator>();

        return animator != null && entityManager != null;
    }

    public override void PlayStep()
    {
        switch (step)
        {
            case 0:
                
                animator.SetBool("isIdle", false);
                animator.SetInteger("walkVersion", Random.Range(0, 2));
                animator.SetBool("isWalking", true);

                entityManager.goalTileManager = goalTileManager;
                currentlyAffecting.Add(entityManager);
                
                
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
                return animator.GetCurrentAnimatorStateInfo(0).IsName("ToIdle");
        }

        return true;
    }
}
