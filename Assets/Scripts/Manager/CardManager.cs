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

    [HideInInspector]
    public CardManagerEvent cardSelectedEvent = new CardManagerEvent();

    [HideInInspector]
    public CardManagerEvent cardMouseDownEvent = new CardManagerEvent();

    [HideInInspector]
    public CardManagerEvent cardMouseUpEvent = new CardManagerEvent();

    [HideInInspector]
    public CardManagerEvent cardHoverEnterEvent = new CardManagerEvent();

    [HideInInspector]
    public CardManagerEvent cardHoverExitEvent = new CardManagerEvent();

    [SerializeField]
    CostUIDisplay costUIDisplay;

    [SerializeField]
    TextMeshProUGUI cardNameTextMeshProUGUI;

    [SerializeField]
    TextMeshProUGUI cardTextTextMeshProUGUI;

    [SerializeField]
    Image cardImage;

    [SerializeField]
    CanvasGroup canvasGroup;

    [SerializeField, Range(0, 1f)]
    float alphaWhenHoveringOver;

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
                hoveringOver = false;
            }
        }
    }

    private bool hoveringOver_;

    public bool hoveringOver
    {
        get
        {
            return hoveringOver_;
        }
        set
        {
            hoveringOver_ = value;
            canvasGroup.alpha = value ? alphaWhenHoveringOver : 1f;
        }
    }

    //[SerializeField]
    //TextMeshProUGUI cardCostTextMeshProUGUI;


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

    void Awake()
    {
        hovered = false;
        selected = false;
        hoveringOver = false;
    }

    void Update()
    {
        
    }


    void UpdateAccordingToCardState()
    {
        if (cardState == null)
        {
            costUIDisplay.costState = null;
            return;
        }
        costUIDisplay.costState = cardState.costState;

    }

    public void UpdateVisuals()
    {
        if (cardState == null)
        {
            return;
        }
        else
        {
            cardTextTextMeshProUGUI.text = cardState.text;
            cardNameTextMeshProUGUI.text = cardState.cardName;
            
            costUIDisplay.UpdateVisuals();
            if (cardState.num < cardArtWorks.Count)
            {
                cardImage.sprite = cardArtWorks[(int)cardState.num];
            }
        }
    }


    

    public static void UnselectEveryCard(){
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
