using UnityEngine;
using TMPro;
using UnityEngine.UI;

using UnityEngine.Events;

using GameLogic;
using System;
using GameLogic.GameState;
using System.Collections.Generic;
using Unity.VisualScripting;


[System.Serializable, Obsolete]
public class CardEvent : UnityEvent<Card>
{

}

[System.Serializable]
public class IntEvent : UnityEvent<int>
{

}

public class CardManager : MonoBehaviour
{

    //Temp
    [SerializeField]
    List<Sprite> cardArtWorks;

    public int positionInHand;

    [SerializeField]
    [Obsolete]
    ScriptableActivableEffectCard scriptableActivableEffectCard;

    [Obsolete]
    ScriptableActivableEffectCard lastScriptableActivableEffectCard;

    public IntEvent cardClickedEvent = new IntEvent();

    public IntEvent cardHoverEnterEvent = new IntEvent();

    public IntEvent cardHoverExitEvent = new IntEvent();

    [SerializeField]
    CostUIDisplay costUIDisplay;

    [SerializeField]
    TextMeshProUGUI cardNameTextMeshProUGUI;

    [SerializeField]
    TextMeshProUGUI cardTextTextMeshProUGUI;

    [SerializeField]
    Image cardImage;

    public bool hovered
    {
        get;
        private set;
    }

    //[SerializeField]
    //TextMeshProUGUI cardCostTextMeshProUGUI;

    [Obsolete]
    private Card card_;

    [Obsolete]
    public Card card
    {
        get
        {
            return card_;
        }
        set
        {
            card_ = value;
            UpdateAccordingToCard();
        }
    }


    private CardState cardState_;

    public CardState cardState
    {
        get
        {
            return cardState_;
        }
        set
        {
            cardState_ = value;
            UpdateAccordingToCardState();
        }
    }

    void Start()
    {
        hovered = false;
    }

    void Update()
    {
    }


    void UpdateAccordingToCardState()
    {



    }

    public void UpdateVisuals()
    {
        if (cardState == null)
        {
            costUIDisplay.costState = null;
        }
        else
        {

            cardTextTextMeshProUGUI.text = cardState.text;
            cardNameTextMeshProUGUI.text = cardState.cardName;
            costUIDisplay.costState = cardState.costState;
            if (cardState.num < cardArtWorks.Count)
            {
                cardImage.sprite = cardArtWorks[(int)cardState.num];
            }
        }
    }

    [Obsolete]
    void UpdateAccordingToCard()
    {
        if (card != null)
        {
            cardTextTextMeshProUGUI.text = card.GetText();
            cardNameTextMeshProUGUI.text = card.GetCardName();
            costUIDisplay.cost = card.cost;
        }
    }

    [Obsolete]
    void UpdateAccordingToScriptableCard()
    {
        if (scriptableActivableEffectCard != null)
        {
            cardImage.sprite = scriptableActivableEffectCard.sprite;
        }

        //card = new Card(player, scriptableActivableEffectCard.scriptableActivableEffect.GetActivableEffect());
    }

    void OnMouseOver()
    {
        hovered = true;
        cardHoverEnterEvent.Invoke(positionInHand);
    }

    void OnMouseExit()
    {
        hovered = false;
        cardHoverExitEvent.Invoke(positionInHand);
    }

    void OnMouseDown()
    {
        cardClickedEvent.Invoke(positionInHand);
    }

    void OnDestroy()
    {
        cardClickedEvent.RemoveAllListeners();
        cardHoverEnterEvent.RemoveAllListeners();
        cardHoverExitEvent.RemoveAllListeners();
    }
}
