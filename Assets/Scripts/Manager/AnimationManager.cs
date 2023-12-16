using System;
using System.Collections.Generic;
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

    
    public bool animationPlaying{
        get;
        private set;
    }

    List<Animator> animatorsPlaying;


    // Start is called before the first frame update
    void Start()
    {
        animatorsPlaying = new List<Animator>();
        animationPlaying = false;
    }

    // Update is called once per frame
    void Update()
    {
        for(var i = 0; i < animatorsPlaying.Count; i++){
            var animator = animatorsPlaying[i];
            if(AnimatorIsPlaying(animator)){
                if(animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")){
                    animatorsPlaying.Remove(animator);
                    i--;
                }
            }
        }

        animationPlaying = animatorsPlaying.Count > 0;
    }


    public void PlayAnimationForAction(Action action){

        switch(action){
            case EntityMoveAction entityMoveAction: 
                var entityManager = boardManager.GetEntityManagerFromEntity(entityMoveAction.entity);
                var goalTileManager = boardManager.GetTileManagerFromTile(entityMoveAction.endTile);
                if(entityManager != null){
                    var animator = entityManager.gameObject.GetComponent<Animator>();
                    animatorsPlaying.Add(animator);
                    entityManager.goalTileManager = goalTileManager;
                }
                break;

            case PlayerSpawnEntityAction playerSpawnEntityAction:
                SpawnEntity(playerSpawnEntityAction.entitySpawned);
                break;

            case TileChangeTypeAction tileChangeTypeAction:
                var tileManager = boardManager.GetTileManagerFromTile(tileChangeTypeAction.tile);
                tileManager.UpdateAccordingToTile();
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
