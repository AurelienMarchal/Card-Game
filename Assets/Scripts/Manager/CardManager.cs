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
public class CardManagerEvent : UnityEvent<CardManager>
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

    public CardManagerEvent cardSelectedEvent = new CardManagerEvent();

    public CardManagerEvent cardMouseDownEvent = new CardManagerEvent();

    public CardManagerEvent cardMouseUpEvent = new CardManagerEvent();

    public CardManagerEvent cardHoverEnterEvent = new CardManagerEvent();

    public CardManagerEvent cardHoverExitEvent = new CardManagerEvent();

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

    private bool selected_;

    public bool selected
    {
        get
        {
            return selected_;
        }
        set
        {
            selected_ = value;

            if (selected)
            {
                cardSelectedEvent.Invoke(this);
            }
            else
            {
                
            }
        }
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
        selected = false;
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

    public static void UnselectEveryEntity(){
        foreach(GameObject cardGO in GameObject.FindGameObjectsWithTag("Card")){
            var cardManager = cardGO.GetComponent<CardManager>();
            if(cardManager != null){
                cardManager.selected = false;
            }
        }
    }

    void OnMouseOver()
    {
        hovered = true;
        cardHoverEnterEvent.Invoke(this);
    }

    void OnMouseExit()
    {
        hovered = false;
        cardHoverExitEvent.Invoke(this);
    }

    void OnMouseDown()
    {
        cardMouseDownEvent.Invoke(this);
    }

    void OnMouseUp()
    {
        cardMouseUpEvent.Invoke(this);
    }

    void OnDestroy()
    {
        cardMouseDownEvent.RemoveAllListeners();
        cardHoverEnterEvent.RemoveAllListeners();
        cardHoverExitEvent.RemoveAllListeners();
    }
}
