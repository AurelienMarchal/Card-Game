using System;
using UnityEditor.Animations;
using UnityEngine;


[Serializable]
public struct PrefabCorrespondingToEntityModel {
    public EntityModel entityModel;
    public GameObject prefab;
}


public class AnimationManager : MonoBehaviour
{

    [SerializeField]
    BoardManager boardManager;

    [SerializeField]
    PrefabCorrespondingToEntityModel[] prefabCorrespondingToEntityModels;

    [SerializeField]
    float walkingSpeed;

    bool animationPlaying;

    Animator animatorPlaying;



    // Start is called before the first frame update
    void Start()
    {
        animatorPlaying = null;
        animationPlaying = false;
    }

    // Update is called once per frame
    void Update()
    {
        animationPlaying = false;
        if(animatorPlaying != null){
            if(AnimatorIsPlaying(animatorPlaying)){
                if(animatorPlaying.GetCurrentAnimatorStateInfo(0).IsName("Idle")){
                    animatorPlaying = null;
                }
                else{
                    if(animatorPlaying.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.0){
                        Debug.Log("Animation Finished");
                    }
                    else{
                        animationPlaying = true;
                    }
                }
            }
        }
        
    }


    public void PlayAnimationForAction(Action action){

        switch(action){
            case EntityMoveAction entityMoveAction: 
                var entityManager = boardManager.GetEntityManagerFromEntity(entityMoveAction.entity);
                var goalTileManager = boardManager.GetTileManagerFromTile(entityMoveAction.endTile);
                if(entityManager != null){
                    var animator = entityManager.gameObject.GetComponent<Animator>();
                    animationPlaying = animator;
                    entityManager.goalTileManager = goalTileManager;
                }
                break;

            case PlayerSpawnEntityAction playerSpawnEntityAction:
                SpawnEntity(playerSpawnEntityAction.entitySpawned);
                break;
        }

    }

    public void SpawnEntity(Entity entity){
        foreach(PrefabCorrespondingToEntityModel prefabCorrespondingToEntityModel in prefabCorrespondingToEntityModels){
            if(prefabCorrespondingToEntityModel.entityModel == entity.model){
                boardManager.SpawnEntity(prefabCorrespondingToEntityModel.prefab, entity);
            }
        }
    }

    bool AnimatorIsPlaying(Animator animator){
        return animator.GetCurrentAnimatorStateInfo(0).length >
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }


}
