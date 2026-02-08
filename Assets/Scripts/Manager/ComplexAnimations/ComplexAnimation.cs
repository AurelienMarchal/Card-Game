using System.Collections.Generic;
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

    void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {
        step = -1;
        currentlyAffecting = new List<MonoBehaviour>();
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
