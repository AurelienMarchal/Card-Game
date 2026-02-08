using UnityEngine;
using UnityEngine.Events;

public class AnimationEventTransmitter : MonoBehaviour
{
    public UnityEvent<AnimationEvent> transmittedAnimationEvent = new UnityEvent<AnimationEvent>();
    
    public void ReceiveAnimationEvent(AnimationEvent animationEvent)
    {
        //Debug.Log( $"Received Animation Event with {animationEvent}");
        transmittedAnimationEvent.Invoke(animationEvent);
    }
}
