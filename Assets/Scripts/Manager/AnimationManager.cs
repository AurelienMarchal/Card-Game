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
                PlayAnimationForAction(entityMoveAction);
                break;

            case EntityTakeDamageAction entityTakeDamageAction: 
                PlayAnimationForAction(entityTakeDamageAction);
                break;

            case EntityGainHeartAction entityGainHeartAction: 
                PlayAnimationForAction(entityGainHeartAction);
                break;

            case EntityPayHeartCostAction entityPayHeartCostAction: 
                PlayAnimationForAction(entityPayHeartCostAction);
                break;

            case EntityUseMovementAction entityUseMovementAction: 
                PlayAnimationForAction(entityUseMovementAction);
                break;

            case EntityResetMovementAction entityResetMovementAction: 
                PlayAnimationForAction(entityResetMovementAction);
                break;

            case EntityAttackAction entityAttackAction: 
                PlayAnimationForAction(entityAttackAction);
                break;

            case EntityChangeDirectionAction entityChangeDirectionAction:
                PlayAnimationForAction(entityChangeDirectionAction);
                break;

            case PlayerSpawnEntityAction playerSpawnEntityAction:
                PlayAnimationForAction(playerSpawnEntityAction);
                break;

            case TileChangeTypeAction tileChangeTypeAction:
                PlayAnimationForAction(tileChangeTypeAction);
                break;

            case EntityDieAction entityDieAction:
                PlayAnimationForAction(entityDieAction);
                break;
        }

    }

    public void PlayAnimationForAction(EntityMoveAction entityMoveAction){
        var entityManager = boardManager.GetEntityManagerFromEntity(entityMoveAction.entity);
        var goalTileManager = boardManager.GetTileManagerFromTile(entityMoveAction.endTile);
        if(entityManager != null){
            var animator = entityManager.gameObject.GetComponent<Animator>();
            animatorsPlaying.Add(animator);
            entityManager.goalTileManager = goalTileManager;
        }
    }

    public void PlayAnimationForAction(EntityTakeDamageAction entityTakeDamageAction){
        var entityManager = boardManager.GetEntityManagerFromEntity(entityTakeDamageAction.entity);
        if(entityManager != null){
            var animator = entityManager.gameObject.GetComponent<Animator>();
            animatorsPlaying.Add(animator);
            animator.SetTrigger("hitTrigger");
            entityManager.UpdateHealthUIDisplay();
        }
    }

    public void PlayAnimationForAction(EntityGainHeartAction entityGainHeartAction){
        var entityManager = boardManager.GetEntityManagerFromEntity(entityGainHeartAction.entity);
        entityManager.UpdateHealthUIDisplay();
    }

    public void PlayAnimationForAction(EntityPayHeartCostAction entityPayHeartCostAction){
        var entityManager = boardManager.GetEntityManagerFromEntity(entityPayHeartCostAction.entity);
        //Debug.Log($"{entityPayHeartCostAction.entity} paid hearts, current health : {entityPayHeartCostAction.entity.health}");
        entityManager.UpdateHealthUIDisplay();
    }

    public void PlayAnimationForAction(EntityUseMovementAction entityUseMovementAction){
        var entityManager = boardManager.GetEntityManagerFromEntity(entityUseMovementAction.entity);
        entityManager.UpdateMovementUIDisplay();
    }

    public void PlayAnimationForAction(EntityResetMovementAction entityResetMovementAction){
        var entityManager = boardManager.GetEntityManagerFromEntity(entityResetMovementAction.entity);
        entityManager.UpdateMovementUIDisplay();
    }

    public void PlayAnimationForAction(EntityAttackAction entityAttackAction){
        var entityManager = boardManager.GetEntityManagerFromEntity(entityAttackAction.entity);
        if(entityManager != null){
            var animator = entityManager.gameObject.GetComponent<Animator>();
            animatorsPlaying.Add(animator);
            animator.SetTrigger("attackTrigger");
        }
    }

    public void PlayAnimationForAction(EntityChangeDirectionAction entityChangeDirectionAction){
        var entityManager = boardManager.GetEntityManagerFromEntity(entityChangeDirectionAction.entity);
        entityManager.UpdateRotationAccordingToEntity();
    }

    public void PlayAnimationForAction(PlayerSpawnEntityAction playerSpawnEntityAction){
        SpawnEntity(playerSpawnEntityAction.entitySpawned);
    }

    public void PlayAnimationForAction(TileChangeTypeAction tileChangeTypeAction){
        var tileManager = boardManager.GetTileManagerFromTile(tileChangeTypeAction.tile);
        tileManager.UpdateAccordingToTile();
    }

    public void PlayAnimationForAction(EntityDieAction entityDieAction){
        var entityManager = boardManager.GetEntityManagerFromEntity(entityDieAction.entity);
        if(entityManager != null){
            var animator = entityManager.gameObject.GetComponent<Animator>();
            animatorsPlaying.Add(animator);
            animator.SetTrigger("deathTrigger");
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
