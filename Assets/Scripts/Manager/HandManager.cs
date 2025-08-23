using UnityEngine;
using GameLogic;
using GameLogic.GameState;
using System;
using System.Collections.Generic;

public class HandManager : MonoBehaviour
{   
    [Obsolete]
    public Hand hand
    {
        get;
        set;
    }

    private List<CardManager> cardManagers = new List<CardManager>();

    private HandState handState_;

    public HandState handState
    {
        get
        {
            return handState_;
        }

        set
        {
            handState_ = value;
            UpdateAccordingToHandState();
        }
    }

    public CardManagerEvent cardSelectedEvent = new CardManagerEvent();

    public CardManagerEvent cardMouseDownEvent = new CardManagerEvent();

    public CardManagerEvent cardMouseUpEvent = new CardManagerEvent();

    public CardManagerEvent cardHoverEnterEvent = new CardManagerEvent();

    public CardManagerEvent cardHoverExitEvent = new CardManagerEvent();

    [SerializeField]
    GameObject cardPrefab;

    [SerializeField]
    float radius;

    [SerializeField]
    float degreeRange;

    [SerializeField]
    Vector3 hoveredCardOffset;

    void Start()
    {

    }

    void Update(){
        
    }

    public void UpdateCardsPosition(){

        for (var i = 0; i < cardManagers.Count; i++)
        {
            
            CardManager cardManager = cardManagers[i];
            Transform cardManagerTransform = cardManager.transform;

            if (cardManager == null)
            {
                continue;
            }

            if (cardManager.selected)
            {
                var mousePos = Input.mousePosition;
                //A calculer
                mousePos.z = 12;
                cardManager.transform.position = Camera.main.ScreenToWorldPoint(mousePos);
            }

            else
            {
                var angle = (i - cardManagers.Count / 2) * degreeRange * (Mathf.PI / 180) / cardManagers.Count;

                // Calculate the position on the circle
                var x = Mathf.Sin(angle) * radius;
                var y = 0;
                var z = Mathf.Cos(angle) * radius - radius;

                // Set the position of the child
                cardManagerTransform.localPosition = new Vector3(x, y, z) + (cardManager.hovered ? hoveredCardOffset : Vector3.zero);

                var circleCenter = transform.position + transform.forward * (-radius);


                // Orient the child's rotation
                Quaternion rotation = Quaternion.LookRotation(-Vector3.up, -(circleCenter - cardManagerTransform.position));
                cardManagerTransform.rotation = rotation;
            }
        }
    }

    private void UpdateAccordingToHandState()
    {
        if (handState == null)
        {
            return;
        }

        if (handState.cardStates == null)
        {
            return;
        }


        while (handState.cardStates.Count > cardManagers.Count)
        {
            var instance = Instantiate(cardPrefab, transform);
            var cardManager = instance.GetComponent<CardManager>();
            if (cardManager == null)
            {
                Destroy(instance);
                Debug.LogError("Could not find CardManager Script in cardPrefab");
                return;
            }
            else
            {
                cardManagers.Add(cardManager);
                if(cardManager != null){
                    cardManager.cardSelectedEvent.AddListener(OnCardSelected);
                    cardManager.cardMouseDownEvent.AddListener(OnCardMouseDown);
                    cardManager.cardMouseUpEvent.AddListener(OnCardMouseUp);
                    cardManager.cardHoverEnterEvent.AddListener(OnCardHoverEnter);
                    cardManager.cardHoverExitEvent.AddListener(OnCardHoverExit);
                }
            }

        }
        while (handState.cardStates.Count < cardManagers.Count)
        {
            var cardManager = cardManagers[^1];
            cardManagers.Remove(cardManager);
            Destroy(cardManager.gameObject);
        }

        for (int i = 0; i < handState.cardStates.Count; i++)
        {
            var cardState = handState.cardStates[i];
            cardManagers[i].cardState = cardState;
            cardManagers[i].positionInHand = i;
        }
    }

    

    public void UpdateVisuals()
    {

        UpdateCardsPosition();
        foreach (var cardManager in cardManagers)
        {
            cardManager.UpdateVisuals();
        }
    }

    private void OnCardSelected(CardManager cardManager)
    {
        cardSelectedEvent.Invoke(cardManager);
    }

    private void OnCardMouseDown(CardManager cardManager)
    {
        cardMouseDownEvent.Invoke(cardManager);
    }

    private void OnCardMouseUp(CardManager cardManager)
    {
        cardMouseUpEvent.Invoke(cardManager);
    }

    

    public void OnCardHoverEnter(CardManager cardManager)
    {
        cardHoverEnterEvent.Invoke(cardManager);
        UpdateCardsPosition();
    }

    public void OnCardHoverExit(CardManager cardManager)
    {
        cardHoverExitEvent.Invoke(cardManager);
        UpdateCardsPosition();
    }
}
