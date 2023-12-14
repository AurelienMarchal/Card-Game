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

    [SerializeField]
    ScriptableCard scriptableCard;

    ScriptableCard lastScriptableCard;

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
        lastScriptableCard = scriptableCard;
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
        if(scriptableCard != lastScriptableCard){
            UpdateAccordingToScriptableCard();
        }
        lastScriptableCard = scriptableCard;
    }

    void UpdateAccordingToCard(){
        if(card != null){
            //cardCostTextMeshProUGUI.text = $"{card.cost.movementCost}";
            cardTextTextMeshProUGUI.text = card.text;
            cardNameTextMeshProUGUI.text = card.cardName;
        }
    }

    //TEST
    void UpdateAccordingToScriptableCard(){
        if(scriptableCard != null){
            cardImage.sprite = scriptableCard.sprite;
        }

        switch (scriptableCard)
        {
            case ScriptableMinionCard scriptableMinionCard:
                card = new Card(player, scriptableMinionCard); break;

            case ScriptableThrowProjectileCard scriptableThrowProjectileCard:
                card = new ThrowProjectileSpellCard(player, scriptableThrowProjectileCard); break;

            case ScriptableSpellCard scriptableSpellCard:
                card = new SpellCard(player, scriptableSpellCard); break;

            default: 
                card = new Card(player, Cost.noCost, "No card", "No card loaded"); break;
        }
    }
    //TEST


    void OnMouseOver(){
        hovered = true;
    }

    void OnMouseExit(){
        hovered = false;
    }

    void OnMouseDown(){
        
        if(card != null){
            
            cardClickedEvent.Invoke(card);
        }
    }
}
