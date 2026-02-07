using GameLogic.GameState;
using UnityEngine;


public class ComplexAnimationFactory : MonoBehaviour
{
    [SerializeField]
    GameManager gameManager;

    [SerializeField]
    BoardManager boardManager;

    [SerializeField]
    PrefabCorrespondingToEntityModel[] prefabCorrespondingToEntityModels;

    [SerializeField]
    ComplexAnimationManager complexAnimationManager;


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
                HandlePlayerActionState(playerActionState);
                break;

            case EntityActionState entityActionState:
                HandleEntityActionState(entityActionState);
                break;

            case TileActionState tileActionState:
                HandleTileActionState(tileActionState);
                break;
        }
    }


    private void HandlePlayerActionState(PlayerActionState playerActionState)
    {
        switch (playerActionState)
        {
            case PlayerStartTurnActionState playerStartTurnActionState:
                
                break;
            case PlayerEndTurnActionState playerEndTurnActionState:
                
                break;
            case PlayerIncreaseMaxManaActionState playerIncreaseMaxManaActionState:
                
                break;
            case PlayerUseManaActionState playerUseManaActionState:
                
                break;
            case PlayerResetManaActionState playerResetManaActionState:
                
                break;
            case PlayerPlayCardActionState playerPlayCardActionState:
                
                break;
            case PlayerAddCardToHandActionState playerAddCardToHandActionState:
                
                break;
            case PlayerSpawnEntityActionState playerSpawnEntityActionState:
                
                break;
            default:
                break;
        }
    }


    private void HandleEntityActionState(EntityActionState entityActionState)
    {

        var entityManager = gameManager.GetEntityManagerFromPlayernumAndEntityNum(entityActionState.playerNum, entityActionState.entityNum);

        if (!entityManager)
        {
            return;
        }

        switch (entityActionState)
        {
            case EntityMoveActionState entityMoveActionState:
                var tileManager  = boardManager.GetTileManagerFromTileNum(entityMoveActionState.endTileNum);
                if (!tileManager)
                {
                    return;
                }
                complexAnimationManager.QueueComplexAnimation(
                    new WalkingComplexAnimation(entityManager, tileManager)
                );

                break;
            case EntityChangeDirectionActionState entityChangeDirectionActionState:
                
                break;
            case EntityAttackActionState entityAttackActionState:
                break;
            case EntityUseMovementActionState entityUseMovementActionState:
                break;
            case EntityTakesDamageActionState entityTakesDamageActionState:
                complexAnimationManager.QueueComplexAnimation(
                    new EntityTakesDamageComplexAnimation(entityManager)
                );
                break;
            case EntityDieActionState entityDieActionState:
                complexAnimationManager.QueueComplexAnimation(
                    new DeathComplexAnimation(entityManager)
                );
                break;
            case EntityGainHeartActionState entityGainHeartActionState:
                break;
            case EntityHealsActionState entityHealsActionState:
                break;
            case EntityIncreaseMaxMovementActionState entityIncreaseMaxMovementActionState:
                break;
            case EntityPayHeartCostActionState entityPayHeartCostActionState:
                break;
            case EntityResetMovementActionState entityResetMovementActionState:
                break;
            case EntityIncreasesAtkDamageActionState entityIncreasesAtkDamageActionState:
                break;
            case EntityIncreasesRangeActionState entityIncreasesRangeActionState:
                break;
            case EntityIncreasesCostToAtkActionState entityIncreasesCostToAtkActionState:
                break;
            case EntityIncreasesCostToMoveActionState entityIncreasesCostToMoveActionState:
                break;
            default:
                break;
        }
    }

    private void HandleTileActionState(TileActionState tileActionState)
    {
        switch (tileActionState)
        {
            case TileChangeTypeActionState tileChangeTypeActionState:
                
                break;
            default:
                break;
        }
    }
}
