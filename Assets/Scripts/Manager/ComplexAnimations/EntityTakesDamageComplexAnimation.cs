using UnityEngine;

public class EntityTakesDamageComplexAnimation : ComplexAnimation
{
    EntityManager entityManager;

    Animator animator;
    
    [SerializeField]
    ParticleSystem bloodParticuleSystem; 

    protected override void Init() 
    {
        base.Init();
        entityManager = gameObject.GetComponentInParent<EntityManager>();
        animator = gameObject.GetComponentInParent<Animator>();
        var animationEventTransmitter = entityManager.gameObject.GetComponentInParent<AnimationEventTransmitter>();
        if (animationEventTransmitter)
        {
            animationEventTransmitter.transmittedAnimationEvent.AddListener(OnAnimationEvent);
        }
    }

    private void OnAnimationEvent(AnimationEvent animationEvent)
    {
        Debug.Log($"Received animationEvent {animationEvent}");
        bloodParticuleSystem.Play();
    }

    public override void PlayStep()
    {
        switch (step)
        {
            case 0:
                if (animator)
                {   
                    animator.SetBool("isIdle", false);
                    animator.SetInteger("hitVersion", Random.Range(0, 1));
                    animator.SetTrigger("hitTrigger");
                    currentlyAffecting.Add(entityManager);
                }
                
                break;

            case 1:
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
                return animator == null || animator.GetCurrentAnimatorStateInfo(0).IsName("ToIdle");
        }

        return true;
    }
}
