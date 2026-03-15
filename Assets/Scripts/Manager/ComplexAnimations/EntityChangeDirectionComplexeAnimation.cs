using GameLogic;
using GameLogic.GameState;
using UnityEngine;

public class EntityChangeDirectionComplexeAnimation : ComplexAnimation
{
    EntityManager entityManager;

    //Animator animator;

    Direction direction;

    public override bool Init(ActionState actionState) 
    {
        base.Init(actionState);

        finalStep = 1;

        var entityChangeDirectionActionState = (EntityChangeDirectionActionState)actionState;

        if (entityChangeDirectionActionState == null)
        {
            return false;
        }

        direction = entityChangeDirectionActionState.newDirection;

        entityManager = gameObject.GetComponentInParent<EntityManager>();
        //animator = gameObject.GetComponentInParent<Animator>();

        return entityManager != null;
    }

    public override void PlayStep()
    {
        switch (step)
        {
            case 0:
                
                //animator.SetBool("isIdle", false);
                //animator.SetInteger("walkVersion", Random.Range(0, 2));
                //animator.SetBool("isWalking", true);

                currentlyAffecting.Add(entityManager);
                entityManager.entityState.direction = direction;
                entityManager.UpdateRotationAccordingToEntityState();
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
                return true;
        }

        return true;
    }
}
