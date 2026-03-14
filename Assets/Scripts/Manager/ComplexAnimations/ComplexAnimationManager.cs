using System.Collections.Generic;
using GameLogic.GameState;
using UnityEngine;

public class ComplexAnimationManager : MonoBehaviour


{

    [SerializeField]
    GameManager gameManager;

    [SerializeField]
    BoardManager boardManager;
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


    private void QueueComplexAnimation(ComplexAnimation complexAnimation)
    {
        complexAnimationsQueue.Add(complexAnimation);
    }

    public void HandleActionState(ActionState actionState)
    {
        if (actionState == null)
        {
            return;
        }

        switch (actionState)
        {
            case StartGameActionState startGameActionState:
                
                
                break;

            case StartTurnActionState startTurnActionState:
                
                break;


            case PlayerActionState playerActionState:
                //HandlePlayerActionState(playerActionState);
                break;

            case EntityActionState entityActionState:
                var entityManager = gameManager.GetEntityManagerFromPlayernumAndEntityNum(entityActionState.playerNum, entityActionState.entityNum);
                if (!entityManager)
                {
                    return;
                }

                var complexAnimationPrefabRegistry = entityManager.GetComponent<ComplexAnimationPrefabRegistry>();

                if (!complexAnimationPrefabRegistry)
                {
                    return;
                }

                var complexAnimationPrefab = complexAnimationPrefabRegistry.GetComplexAnimationPrefab(entityActionState.GetType());

                if (!complexAnimationPrefab)
                {
                    Debug.LogWarning($"No complexAnimationPrefab in EntityManager {entityManager.entityState.name} for entityActionState {entityActionState}");
                    return;
                }

                var complexAnimationInstance = Instantiate(complexAnimationPrefab, entityManager.transform);

                var complexAnimation = complexAnimationInstance.GetComponent<ComplexAnimation>();

                if (!complexAnimation)
                {
                    Destroy(complexAnimationInstance);
                    return;
                }

                var didiInit = complexAnimation.Init(actionState);

                if (!didiInit)
                {
                    Debug.LogWarning($"Something wen wrong when initing {complexAnimation} for actionState {entityActionState}");
                }

                QueueComplexAnimation(complexAnimation);

                break;

            case TileActionState tileActionState:
                //HandleTileActionState(tileActionState);
                break;
        }
    }
}
