using UnityEngine;
using TMPro;
using UnityEngine.UI;

using UnityEngine.Events;


[System.Serializable]
public class CardEvent : UnityEvent<Card>
{

}

public class CardManager : MonoBehaviour
{
    public CardEvent cardClickedEvent = new CardEvent();

    public CardEvent cardHoverEnterEvent = new CardEvent();

    public CardEvent cardHoverExitEvent = new CardEvent();

    [SerializeField]
    ScriptableActivableEffectCard scriptableActivableEffectCard;

    ScriptableActivableEffectCard lastScriptableActivableEffectCard;

    [SerializeField]
    CostUIDisplay costUIDisplay;

    [SerializeField]
    TextMeshProUGUI cardNameTextMeshProUGUI;

    [SerializeField]
    TextMeshProUGUI cardTextTextMeshProUGUI;

    [SerializeField]
    Image cardImage;

    public bool hovered{
        get;
        private set;
    }

    //[SerializeField]
    //TextMeshProUGUI cardCostTextMeshProUGUI;

    Player player;

    private Card card_;

    public Card card{
        get{
            return card_;
        }
        set{
            card_ = value;
            UpdateAccordingToCard();
        }
    }

    void Start(){
        hovered = false;
        
        if(player == null){
            var handManager = transform.parent.gameObject.GetComponent<HandManager>();
            if(handManager != null){
                player = handManager.hand.player;
            }
        }
        
        UpdateAccordingToScriptableCard();
        lastScriptableActivableEffectCard = scriptableActivableEffectCard;
    }

    void Update(){
        
        //TEST
        if(player == null){
            var handManager = transform.parent.gameObject.GetComponent<HandManager>();
            if(handManager != null){
                player = handManager.hand.player;
            }
        }
        //TEST
        if(scriptableActivableEffectCard != lastScriptableActivableEffectCard){
            UpdateAccordingToScriptableCard();
        }
        lastScriptableActivableEffectCard = scriptableActivableEffectCard;
    }

    void UpdateAccordingToCard(){
        if(card != null){
            cardTextTextMeshProUGUI.text = card.GetText();
            cardNameTextMeshProUGUI.text = card.GetCardName();
            costUIDisplay.cost = card.cost;
        }
    }

    //TEST
    void UpdateAccordingToScriptableCard(){
        if(scriptableActivableEffectCard != null){
            cardImage.sprite = scriptableActivableEffectCard.sprite;
        }

        card = new Card(player, scriptableActivableEffectCard.scriptableActivableEffect.GetActivableEffect());
    }
    //TEST


    void OnMouseOver(){
        hovered = true;
        cardHoverEnterEvent.Invoke(card);
    }

    void OnMouseExit(){
        hovered = false;
        cardHoverExitEvent.Invoke(card);
    }

    void OnMouseDown(){
        if(card != null){
            cardClickedEvent.Invoke(card);
        }
    }
}
