using UnityEngine;
using GameLogic;
using GameLogic.GameState;
using System;

public class HandManager : MonoBehaviour
{
    public Hand hand{
        get;
        set;
    }

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
            UpdateAccordingToPlayerState();
        }
    }

    public CardEvent cardClickedEvent = new CardEvent();

    public CardEvent cardHoverEnterEvent = new CardEvent();

    public CardEvent cardHoverExitEvent = new CardEvent();

    [SerializeField]
    GameObject cardPrefab;

    [SerializeField]
    float radius;

    [SerializeField]
    float degreeRange;

    [SerializeField]
    Vector3 hoveredCardOffset;

    void Start(){
        var childCount = transform.childCount;

        for (var i = 0; i < childCount; i++){
            Transform child = transform.GetChild(i);
            var cardManager = child.gameObject.GetComponent<CardManager>();
            if(cardManager != null){
                cardManager.cardClickedEvent.AddListener((card) => cardClickedEvent.Invoke(card));
                cardManager.cardHoverEnterEvent.AddListener((card) => cardHoverEnterEvent.Invoke(card));
                cardManager.cardHoverExitEvent.AddListener((card) => cardHoverExitEvent.Invoke(card));
            }
        }
    }

    void Update(){
        
        UpdateCardsPosition();
        // Update according to hand ?

        // TEST
        hand.cards.Clear();
        var childCount = transform.childCount;

        for (var i = 0; i < childCount; i++){
            Transform child = transform.GetChild(i);
            var cardManager = child.gameObject.GetComponent<CardManager>();
            if(cardManager != null){
                if(cardManager.card != null){
                    hand.cards.Add(cardManager.card);
                }
            }
        }

        //TEST
    }

    void UpdateCardsPosition(){
        var childCount = transform.childCount;

        for (var i = 0; i < childCount; i++)
        {
            Transform child = transform.GetChild(i);
            CardManager cardManager = child.gameObject.GetComponent<CardManager>();

            if(cardManager != null){
                var angle = (i - childCount/2) * degreeRange * (Mathf.PI/180) / childCount;

                // Calculate the position on the circle
                var x = Mathf.Sin(angle) * radius;
                var y = 0;
                var z = Mathf.Cos(angle) * radius - radius;

                // Set the position of the child
                child.localPosition = new Vector3(x, y, z) + (cardManager.hovered ? hoveredCardOffset : Vector3.zero);

                var circleCenter = transform.position + transform.forward * (-radius);
                

                // Orient the child's rotation
                Quaternion rotation = Quaternion.LookRotation(-Vector3.up, -(circleCenter - child.position));
                child.rotation = rotation;
            }
        }
    }

    private void UpdateAccordingToPlayerState(){
        //TODO
    }


    void OnDestroy(){
        var childCount = transform.childCount;

        for (var i = 0; i < childCount; i++){
            Transform child = transform.GetChild(i);
            Destroy(child.gameObject);
        }
    }
}
