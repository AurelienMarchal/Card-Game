using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EffectUIDisplay : MonoBehaviour
{
    
    [SerializeField]
    Button button;

    [SerializeField]
    TextMeshProUGUI effectTextMeshProUGUI;

    [SerializeField]
    CostUIDisplay costUIDisplay;

    private Effect effect_;
    public Effect effect{
        get{
            return effect_;
        }
        set{
            effect_ = value;
            UpdateFromNewEffect();
        }
    }
    
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update(){
        UpdateFromEffect();
    }

    void UpdateFromNewEffect(){
        if(effect == null){
            return;
        }
        

        effectTextMeshProUGUI.text = effect.GetEffectText();

        button.interactable = effect is ActivableEffect;

        costUIDisplay.gameObject.SetActive(effect is ActivableEffect);

        switch(effect){
            case ActivableEffect activableEffect:
                
                costUIDisplay.cost = activableEffect.cost;
                button.onClick.AddListener(OnButtonClick);
                
                break;
            default:

            break;
        }
    }

    private void OnButtonClick()
    {
        Debug.Log($"Effect : {effect}");
        if (effect is ActivableEffect activableEffect){
            activableEffect.associatedEntity.TryToCreateEntityUseMovementAction(activableEffect.cost.mouvementCost, out EntityUseMovementAction entityUseMovementAction);
            activableEffect.associatedEntity.TryToCreateEntityPayHeartCostAction(activableEffect.cost.heartCost, out EntityPayHeartCostAction entityPayHeartCostAction, entityUseMovementAction);
            if(!entityPayHeartCostAction.wasPerformed || !entityUseMovementAction.wasPerformed){
                return;
            }
            activableEffect.TryToCreateEffectActivatedAction(entityPayHeartCostAction, out _);
        }

    }

    void UpdateFromEffect(){
        if(effect == null){
            return;
        }
        
        effectTextMeshProUGUI.text = effect.GetEffectText();
        if(effect is ActivableEffect activableEffect)
        {
            button.interactable = activableEffect.CanBeActivated() && Game.currentGame.currentPlayer == activableEffect.associatedEntity.player;
        }
        
        

    }
}
