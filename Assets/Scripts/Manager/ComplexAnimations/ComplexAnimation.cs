using System.Collections.Generic;
using GameLogic.GameState;
using UnityEngine;

public class ComplexAnimation : MonoBehaviour{

    public List<MonoBehaviour> currentlyAffecting{
        get;
        private set;
    }

    public int step{
        get;
        set;
    }

    public bool isPlaying
    {
        get
        {
            return step >= 0 && step <= finalStep;
        }
    }

    [SerializeField] int finalStep;

    public virtual bool Init(ActionState actionState)
    {
        step = -1;
        currentlyAffecting = new List<MonoBehaviour>();

        return true;
    }

    public bool NextStep()
    {
        step ++;
        return step > finalStep;   
    }

    public virtual bool StepFinished()
    {
        return true;
    }

    public virtual void PlayStep()
    {

    }

    protected static bool AnimatorIsPlaying(Animator animator){
        return animator.GetCurrentAnimatorStateInfo(0).length >
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }

}
