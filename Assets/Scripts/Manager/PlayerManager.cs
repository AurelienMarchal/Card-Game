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
    HandManager handManager;

    List<EntityManager> entityManagers = new List<EntityManager>();

    public CardEvent cardHoverEnterEvent = new CardEvent();

    public CardEvent cardHoverExitEvent = new CardEvent();

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

    [Obsolete]
    void Start()
    {
        handManager.cardClickedEvent.AddListener(OnCardClicked);
        handManager.cardHoverEnterEvent.AddListener(
            (card) =>
            {
                card.activableEffect.associatedEntity = player.hero;
                cardHoverEnterEvent.Invoke(card);
                card.activableEffect.associatedEntity = Entity.noEntity;
            });
        handManager.cardHoverExitEvent.AddListener(
            (card) =>
            {
                cardHoverExitEvent.Invoke(card);

            });
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
                var entityManager = boardManager.SpawnEntity(entityState);
                if (entityManager != null)
                {
                    entityManagers.Add(entityManager);
                }
            }
        }

        handManager.handState = playerState.handState;


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
