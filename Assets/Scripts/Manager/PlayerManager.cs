using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GameLogic;
using GameLogic.GameAction;
using GameLogic.GameState;

public class PlayerManager : MonoBehaviour
{

    [SerializeField]
    public HandManager handManager;

    List<EntityManager> entityManagers = new List<EntityManager>();

    public IntEvent cardClickedEvent = new IntEvent();

    public IntEvent cardHoverEnterEvent = new IntEvent();

    public IntEvent cardHoverExitEvent = new IntEvent();

    private PlayerState playerState_;

    public PlayerState playerState
    {
        get
        {
            return playerState_;
        }

        set
        {
            playerState_ = value;
            UpdateAccordingToPlayerState();
        }
    }

    [Obsolete]
    private Player player_;

    [Obsolete]
    public Player player
    {
        get
        {
            return player_;
        }

        set
        {
            player_ = value;
            UpdateAccordingToPlayer();
        }
    }

    //Maybe awake
    void Start()
    {
        handManager.cardClickedEvent.AddListener((cardPositionInHand) => cardClickedEvent.Invoke(cardPositionInHand));
        handManager.cardHoverEnterEvent.AddListener((cardPositionInHand) => cardHoverEnterEvent.Invoke(cardPositionInHand));
        handManager.cardHoverExitEvent.AddListener((cardPositionInHand) => cardHoverExitEvent.Invoke(cardPositionInHand));
    }

    [Obsolete]
    private void OnCardClicked(Card card)
    {
        Debug.Log($"Clicked on {card}");
        if (!player.hero.CanPlayCard(card))
        {
            return;
        }

        player.hero.TryToCreateEntityUseMovementAction(card.activableEffect.cost.mouvementCost, out EntityUseMovementAction entityUseMovementAction);
        player.hero.TryToCreateEntityPayHeartCostAction(card.activableEffect.cost.heartCost, out EntityPayHeartCostAction entityPayHeartCostAction);

        if (!entityPayHeartCostAction.wasPerformed || !entityUseMovementAction.wasPerformed)
        {
            return;
        }

        player.hero.TryToCreateEntityPlayCardAction(card, out EntityPlayCardAction entityPlayCardAction, entityPayHeartCostAction);
    }

    [Obsolete]
    void UpdateAccordingToPlayer()
    {
        if (player != null)
        {
            handManager.hand = player.hand;
        }
    }

    void UpdateAccordingToPlayerState()
    {
        if (playerState == null)
        {
            return;
        }

        var boardManager = FindFirstObjectByType<BoardManager>();

        if (boardManager == null)
        {
            return;
        }

        //TODO: remove entitymanagers that are not in entitystates anymore

        foreach (var entityState in playerState.entityStates)
        {
            var found = false;
            foreach (var entityManager in entityManagers)
            {
                if (entityState.num == entityManager.entityState.num)
                {
                    entityManager.entityState = entityState;
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                //Create entity with SetActive(False) And Then SetActive(True) with UpdateVisuals
                var entityManager = boardManager.SpawnEntity(entityState);
                if (entityManager != null)
                {
                    entityManagers.Add(entityManager);
                }
            }
        }

        handManager.handState = playerState.handState;
    }

    public void UpdateVisuals()
    {
        foreach (var entityManager in entityManagers)
        {
            entityManager.UpdateVisuals();
        }

        handManager.UpdateVisuals();
    }

    public EntityManager GetEntityManagerFromEntityNum(uint entityNum)
    {
        if (entityManagers == null)
        {
            return null;
        }

        //TODO: sort entityManagersByNum to avoid loop

        foreach (var entityManager in entityManagers)
        {
            if (entityManager.entityState.num == entityNum)
            {
                return entityManager;
            }
        }

        return null;
    }
}
