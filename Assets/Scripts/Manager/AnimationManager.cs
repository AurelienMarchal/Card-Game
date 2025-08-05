using System;
using System.Collections.Generic;
using UnityEngine;

using GameLogic;
using GameLogic.GameAction;
using GameLogic.GameState;



public class AnimationManager : MonoBehaviour
{
    [SerializeField]
    GameManager gameManager;

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


    public void PlayAnimationForActionState(ActionState actionState)
    {
        if (actionState == null)
        {
            return;
        }

        switch (actionState)
        {
            case StartGameActionState startGameActionState:
                //TODO
                //PlayAnimationForActionState(startGameActionState);
                break;

            case StartTurnActionState startTurnActionState:
                //TODO
                //PlayAnimationForActionState(startTurnActionState);
                break;


            case PlayerStartTurnActionState playerStartTurnActionState:
                //TODO
                //PlayAnimationForActionState(playerStartTurnActionState);
                break;
            case PlayerEndTurnActionState playerEndTurnActionState:
                PlayAnimationForActionState(playerEndTurnActionState);
                break;

            case EntityMoveActionState entityMoveActionState:
                PlayAnimationForActionState(entityMoveActionState);
                break;
            case EntityChangeDirectionActionState entityChangeDirectionActionState:
                PlayAnimationForActionState(entityChangeDirectionActionState);
                break;
            case EntityAttackActionState entityAttackActionState:
                PlayAnimationForActionState(entityAttackActionState);
                break;
            case EntityUseMovementActionState entityUseMovementActionState:
                PlayAnimationForActionState(entityUseMovementActionState);
                break;
            case EntityTakesDamageActionState entityTakesDamageActionState:
                PlayAnimationForActionState(entityTakesDamageActionState);
                break;
            case EntityDieActionState entityDieActionState:
                PlayAnimationForActionState(entityDieActionState);
                break;
            case EntityGainHeartActionState entityGainHeartActionState:
                PlayAnimationForActionState(entityGainHeartActionState);
                break;
            case EntityHealsActionState entityHealsActionState:
                PlayAnimationForActionState(entityHealsActionState);
                break;
            case EntityIncreaseMaxMovementActionState entityIncreaseMaxMovementActionState:
                PlayAnimationForActionState(entityIncreaseMaxMovementActionState);
                break;
            case EntityPayHeartCostActionState entityPayHeartCostActionState:
                PlayAnimationForActionState(entityPayHeartCostActionState);
                break;
            case EntityResetMovementActionState entityResetMovementActionState:
                PlayAnimationForActionState(entityResetMovementActionState);
                break;
            

            case TileChangeTypeActionState tileChangeTypeActionState:
                PlayAnimationForActionState(tileChangeTypeActionState);
                break;
            default: break;
        }

    }

    public void PlayAnimationForActionState(PlayerEndTurnActionState playerEndTurnActionState)
    {
        gameManager.UpdatePlayerText();
    }

    public void PlayAnimationForActionState(EntityMoveActionState entityMoveActionState)
    {
        var entityManager = gameManager.GetEntityManagerFromPlayernumAndEntityNum(entityMoveActionState.playerNum, entityMoveActionState.entityNum);
        if (entityManager == null)
        {
            return;
        }
        var endTileManager = boardManager.GetTileManagerFromTileNum(entityMoveActionState.endTileNum);
        if (endTileManager == null)
        {
            return;
        }

        var animator = entityManager.gameObject.GetComponent<Animator>();
        animatorsPlaying.Add(animator);
        entityManager.goalTileManager = endTileManager;

        return;
    }

    public void PlayAnimationForActionState(EntityChangeDirectionActionState entityChangeDirectionActionState)
    {
        var entityManager = gameManager.GetEntityManagerFromPlayernumAndEntityNum(entityChangeDirectionActionState.playerNum, entityChangeDirectionActionState.entityNum);
        if (entityManager == null)
        {
            return;
        }

        //Temp
        entityManager.UpdateRotationAccordingToEntityState();
    }

    public void PlayAnimationForActionState(EntityAttackActionState entityAttackActionState)
    {
        var entityManager = gameManager.GetEntityManagerFromPlayernumAndEntityNum(entityAttackActionState.playerNum, entityAttackActionState.entityNum);
        if (entityManager == null)
        {
            return;
        }

        var animator = entityManager.gameObject.GetComponent<Animator>();
        animatorsPlaying.Add(animator);
        animator.SetTrigger("attackTrigger");
    }

    public void PlayAnimationForActionState(EntityDieActionState entityDieActionState)
    {
        var entityManager = gameManager.GetEntityManagerFromPlayernumAndEntityNum(entityDieActionState.playerNum, entityDieActionState.entityNum);
        if (entityManager == null)
        {
            return;
        }

        var animator = entityManager.gameObject.GetComponent<Animator>();
        animatorsPlaying.Add(animator);
        animator.SetTrigger("deathTrigger");
    }
    


    public void PlayAnimationForActionState(EntityUseMovementActionState entityUseMovementActionState)
    {
        var entityManager = gameManager.GetEntityManagerFromPlayernumAndEntityNum(entityUseMovementActionState.playerNum, entityUseMovementActionState.entityNum);
        if (entityManager == null)
        {
            return;
        }

        //temp
        entityManager.UpdateMovementUIDisplay();
    }
    
    public void PlayAnimationForActionState(EntityIncreaseMaxMovementActionState entityIncreaseMaxMovementActionState)
    {
        var entityManager = gameManager.GetEntityManagerFromPlayernumAndEntityNum(entityIncreaseMaxMovementActionState.playerNum, entityIncreaseMaxMovementActionState.entityNum);
        if (entityManager == null)
        {
            return;
        }
        
        //temp
        entityManager.UpdateMovementUIDisplay();
    }

    public void PlayAnimationForActionState(EntityResetMovementActionState entityResetMovementActionState)
    {
        var entityManager = gameManager.GetEntityManagerFromPlayernumAndEntityNum(entityResetMovementActionState.playerNum, entityResetMovementActionState.entityNum);
        if (entityManager == null)
        {
            return;
        }
        
        //temp
        entityManager.UpdateMovementUIDisplay();
    }

    public void PlayAnimationForActionState(EntityTakesDamageActionState entityTakesDamageActionState)
    {
        var entityManager = gameManager.GetEntityManagerFromPlayernumAndEntityNum(entityTakesDamageActionState.playerNum, entityTakesDamageActionState.entityNum);
        if (entityManager == null)
        {
            return;
        }

        //temp
        entityManager.UpdateHealthUIDisplay();
    }

    public void PlayAnimationForActionState(EntityGainHeartActionState entityGainHeartActionState)
    {
        var entityManager = gameManager.GetEntityManagerFromPlayernumAndEntityNum(entityGainHeartActionState.playerNum, entityGainHeartActionState.entityNum);
        if (entityManager == null)
        {
            return;
        }

        //temp
        entityManager.UpdateHealthUIDisplay();
    }

    public void PlayAnimationForActionState(EntityPayHeartCostActionState entityPayHeartCostActionState)
    {
        var entityManager = gameManager.GetEntityManagerFromPlayernumAndEntityNum(entityPayHeartCostActionState.playerNum, entityPayHeartCostActionState.entityNum);
        if (entityManager == null)
        {
            return;
        }

        //temp
        entityManager.UpdateHealthUIDisplay();
    }

    public void PlayAnimationForActionState(TileChangeTypeActionState tileChangeTypeActionState)
    {
        var tileManager = boardManager.GetTileManagerFromTileNum(tileChangeTypeActionState.tileNum);
        if (tileManager == null)
        {
            return;
        }
        //Temp
        tileManager.UpdateVisuals();
    }

    [Obsolete]
    public void PlayAnimationForAction(GameLogic.GameAction.Action action){

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

    [Obsolete]
    public void PlayAnimationForAction(EntityMoveAction entityMoveAction)
    {
        /*
        var entityManager = boardManager.GetEntityManagerFromEntity(entityMoveAction.entity);
        var goalTileManager = boardManager.GetTileManagerFromTile(entityMoveAction.endTile);
        if(entityManager != null){
            var animator = entityManager.gameObject.GetComponent<Animator>();
            animatorsPlaying.Add(animator);
            entityManager.goalTileManager = goalTileManager;
        }
        */
    }

    [Obsolete]
    public void PlayAnimationForAction(EntityTakeDamageAction entityTakeDamageAction)
    {
        var entityManager = boardManager.GetEntityManagerFromEntity(entityTakeDamageAction.entity);
        if (entityManager != null)
        {
            var animator = entityManager.gameObject.GetComponent<Animator>();
            animatorsPlaying.Add(animator);
            animator.SetTrigger("hitTrigger");
            entityManager.UpdateHealthUIDisplay();
        }
    }

    [Obsolete]
    public void PlayAnimationForAction(EntityGainHeartAction entityGainHeartAction)
    {
        var entityManager = boardManager.GetEntityManagerFromEntity(entityGainHeartAction.entity);
        entityManager.UpdateHealthUIDisplay();
    }

    [Obsolete]
    public void PlayAnimationForAction(EntityPayHeartCostAction entityPayHeartCostAction)
    {
        var entityManager = boardManager.GetEntityManagerFromEntity(entityPayHeartCostAction.entity);
        //Debug.Log($"{entityPayHeartCostAction.entity} paid hearts, current health : {entityPayHeartCostAction.entity.health}");
        entityManager.UpdateHealthUIDisplay();
    }

    [Obsolete]
    public void PlayAnimationForAction(EntityUseMovementAction entityUseMovementAction){
        var entityManager = boardManager.GetEntityManagerFromEntity(entityUseMovementAction.entity);
        entityManager.UpdateMovementUIDisplay();
    }

    [Obsolete]
    public void PlayAnimationForAction(EntityResetMovementAction entityResetMovementAction)
    {
        var entityManager = boardManager.GetEntityManagerFromEntity(entityResetMovementAction.entity);
        entityManager.UpdateMovementUIDisplay();
    }

    [Obsolete]
    public void PlayAnimationForAction(EntityAttackAction entityAttackAction)
    {
        var entityManager = boardManager.GetEntityManagerFromEntity(entityAttackAction.entity);
        if (entityManager != null)
        {
            var animator = entityManager.gameObject.GetComponent<Animator>();
            animatorsPlaying.Add(animator);
            animator.SetTrigger("attackTrigger");
        }
    }

    [Obsolete]
    public void PlayAnimationForAction(EntityChangeDirectionAction entityChangeDirectionAction)
    {
        var entityManager = boardManager.GetEntityManagerFromEntity(entityChangeDirectionAction.entity);
        entityManager.UpdateRotationAccordingToEntity();
    }

    [Obsolete]
    public void PlayAnimationForAction(PlayerSpawnEntityAction playerSpawnEntityAction)
    {
        SpawnEntity(playerSpawnEntityAction.entitySpawned);
    }

    [Obsolete]
    public void PlayAnimationForAction(TileChangeTypeAction tileChangeTypeAction)
    {
        //var tileManager = boardManager.GetTileManagerFromTile(tileChangeTypeAction.tile);
        //tileManager.UpdateAccordingToTile();
    }

    [Obsolete]
    public void PlayAnimationForAction(EntityDieAction entityDieAction)
    {
        var entityManager = boardManager.GetEntityManagerFromEntity(entityDieAction.entity);
        if (entityManager != null)
        {
            var animator = entityManager.gameObject.GetComponent<Animator>();
            animatorsPlaying.Add(animator);
            animator.SetTrigger("deathTrigger");
        }
    }

    [Obsolete]
    public void SpawnEntity(Entity entity)
    {
        foreach (PrefabCorrespondingToEntityModel prefabCorrespondingToEntityModel in prefabCorrespondingToEntityModels)
        {
            if (prefabCorrespondingToEntityModel.entityModel == entity.model)
            {
                boardManager.SpawnEntity(prefabCorrespondingToEntityModel.prefab, entity);
            }
        }
    }

    bool AnimatorIsPlaying(Animator animator){
        return animator.GetCurrentAnimatorStateInfo(0).length >
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }


}
