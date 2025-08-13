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

    [Obsolete]
    public CardEvent cardClickedEvent = new CardEvent();

    [Obsolete]
    public CardEvent cardHoverEnterEvent = new CardEvent();

    [Obsolete]
    public CardEvent cardHoverExitEvent = new CardEvent();

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
        /*
        var childCount = transform.childCount;
        //DO When addind cards
        for (var i = 0; i < childCount; i++){
            Transform child = transform.GetChild(i);
            var cardManager = child.gameObject.GetComponent<CardManager>();
            if(cardManager != null){
                cardManager.cardClickedEvent.AddListener((card) => cardClickedEvent.Invoke(card));
                cardManager.cardHoverEnterEvent.AddListener((card) => cardHoverEnterEvent.Invoke(card));
                cardManager.cardHoverExitEvent.AddListener((card) => cardHoverExitEvent.Invoke(card));
            }
        }
        */
    }

    void Update(){
        
        
        // Update according to hand ?
    }

    void UpdateCardsPosition(){

        for (var i = 0; i < cardManagers.Count; i++)
        {
            
            CardManager cardManager = cardManagers[i];
            Transform cardManagerTransform = cardManager.transform;

            if (cardManager != null)
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
        }
    }

    public void UpdateVisuals()
    {
        //TODO
        UpdateCardsPosition();
    }


    void OnDestroy(){
        var childCount = transform.childCount;

        for (var i = 0; i < childCount; i++){
            Transform child = transform.GetChild(i);
            Destroy(child.gameObject);
        }
    }
}
