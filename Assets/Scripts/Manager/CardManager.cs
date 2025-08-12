using UnityEngine;
using TMPro;
using UnityEngine.UI;

using UnityEngine.Events;

using GameLogic;
using System;
using GameLogic.GameState;


[System.Serializable, Obsolete]
public class CardEvent : UnityEvent<Card>
{

}

[System.Serializable]
public class CardStateEvent : UnityEvent<CardState>
{

}

public class CardManager : MonoBehaviour
{   
    [Obsolete]
    public CardEvent cardClickedEvent = new CardEvent();

    [Obsolete]
    public CardEvent cardHoverEnterEvent = new CardEvent();

    [Obsolete]
    public CardEvent cardHoverExitEvent = new CardEvent();

    [SerializeField][Obsolete]
    ScriptableActivableEffectCard scriptableActivableEffectCard;

    [Obsolete]
    ScriptableActivableEffectCard lastScriptableActivableEffectCard;

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


    [Obsolete]
    void OnMouseOver()
    {
        hovered = true;
        cardHoverEnterEvent.Invoke(card);
    }

    [Obsolete]
    void OnMouseExit()
    {
        hovered = false;
        cardHoverExitEvent.Invoke(card);
    }

    [Obsolete]
    void OnMouseDown()
    {
        if (card != null)
        {
            cardClickedEvent.Invoke(card);
        }
    }
}
