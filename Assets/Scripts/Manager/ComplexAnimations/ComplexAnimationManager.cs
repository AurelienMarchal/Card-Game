using System.Collections.Generic;
using UnityEngine;

public class ComplexAnimationManager : MonoBehaviour
{
    List<ComplexAnimation> complexAnimationsPlaying;

    List<ComplexAnimation> complexAnimationsQueue;
    void Start()
    {   
        complexAnimationsQueue = new List<ComplexAnimation>();
        complexAnimationsPlaying = new List<ComplexAnimation>();
    }

    // Update is called once per frame
    void Update()
    {

        if(complexAnimationsPlaying.Count > 0)
        {
            for (int i = 0; i < complexAnimationsPlaying.Count; i++)
            {
                var complexeAnimation = complexAnimationsPlaying[i];
            
                if (complexeAnimation.StepFinished())
                {
                    var finished = complexeAnimation.NextStep();

                    Debug.Log($"Going to next step {complexeAnimation.step} of {complexeAnimation}");

                    if (finished)
                    {
                        Debug.Log($"Removing {complexeAnimation}");
                        complexAnimationsPlaying.RemoveAt(i);
                        Destroy(complexeAnimation.gameObject);
                        i--;
                    }
                    else
                    {
                        Debug.Log($"Playing step {complexeAnimation.step} of {complexeAnimation}");
                        complexeAnimation.PlayStep();
                    }
                }
            }
        }
        else
        {
            if (complexAnimationsQueue.Count == 0)
            {   
                return;
            }

            var complexeAnimation = complexAnimationsQueue[0];
            complexAnimationsPlaying.Add(complexeAnimation);
            complexAnimationsQueue.RemoveAt(0);
        }
    }


    public void QueueComplexAnimation(ComplexAnimation complexAnimation)
    {
        complexAnimationsQueue.Add(complexAnimation);
    }
}
