using System.Collections.Generic;
using UnityEngine;

public abstract class ComplexAnimation {

    public List<MonoBehaviour> currentlyAffecting{
        get;
        private set;
    }

    public int step{
        get;
        private set;
    }

    public int finalStep{
        get;
        private set;
    }

    public bool isPlaying
    {
        get
        {
            return step > 0 && step <= finalStep;
        }
    }

    //for each animation set a startup frame number an active frame number and a recovry frame numberm
    public ComplexAnimation(int finalStep){
        this.finalStep = finalStep;
        step = -1;
        currentlyAffecting = new List<MonoBehaviour>();
    }

    public bool NextStep()
    {
        step ++;
        return step > finalStep;   
    }

    public abstract bool StepFinished();

    public abstract void PlayStep();

    protected static bool AnimatorIsPlaying(Animator animator){
        return animator.GetCurrentAnimatorStateInfo(0).length >
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }

}
