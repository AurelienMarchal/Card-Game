using System;
using GameLogic.GameState;
using UnityEngine;

public class EntityDieComplexAnimation : ComplexAnimation
{
    EntityManager entityManager;

    Animator animator;

    public override bool Init(ActionState actionState) 
    {
        base.Init(actionState);
        entityManager = gameObject.GetComponentInParent<EntityManager>();
        animator = gameObject.GetComponentInParent<Animator>();

        return animator != null && entityManager != null;
    }

    public override void PlayStep()
    {
        switch (step)
        {
            case 0:
                
                    
                animator.SetTrigger("deathTrigger");
                animator.SetBool("isIdle", false);
                currentlyAffecting.Add(entityManager);
                
                
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
                return !AnimatorIsPlaying(animator) ;
        }

        return true;
    }
}
