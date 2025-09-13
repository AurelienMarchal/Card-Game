using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

using GameLogic;
using GameLogic.GameAction;
using GameLogic.GameEffect;
using GameLogic.GameState;
[System.Serializable]
[Obsolete]
public class EffectEvent : UnityEvent<Effect>
{
}

public class EffectStateEvent : UnityEvent<EffectState>
{
}

public class EffectUIDisplay : MonoBehaviour
{

    [SerializeField]
    Button button;

    [SerializeField]
    TextMeshProUGUI effectTextMeshProUGUI;

    [SerializeField]
    CostUIDisplay costUIDisplay;


    private EffectState effectState_;
    public EffectState effectState
    {
        get
        {
            return effectState_;
        }
        set
        {
            effectState_ = value;
            UpdateFromEffectState();
        }
    }

    public EffectStateEvent effectHoverEnterEvent = new EffectStateEvent();


    public EffectStateEvent effectHoverExitEvent = new EffectStateEvent();


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //UpdateFromEffect();
    }

    private void UpdateFromEffectState()
    {
        if (effectState == null)
        {
            return;
        }


        effectTextMeshProUGUI.text = effectState.effectText;

        //Peut etre a changer
        //button.interactable = effectState.canBeActivated;

        /*
        costUIDisplay.gameObject.SetActive(effectState.costState != null);

        if (effectState.costState != null)
        {
            costUIDisplay.costState = effectState.costState;
            costUIDisplay.UpdateVisuals();
        }
        */

        /*
        switch (effect)
        {
            case EntityEffect activableEffect:

                costUIDisplay.cost = activableEffect.cost;
                button.onClick.AddListener(OnButtonClick);

                break;
            default:

                break;
        }
        */
    }

    

    private void OnButtonClick()
    {
        /*
        Debug.Log($"Effect : {effect}");
        if (effect is EntityEffect activableEffect){
            activableEffect.associatedEntity.TryToCreateEntityUseMovementAction(activableEffect.cost.mouvementCost, out EntityUseMovementAction entityUseMovementAction);
            activableEffect.associatedEntity.TryToCreateEntityPayHeartCostAction(activableEffect.cost.heartCost, out EntityPayHeartCostAction entityPayHeartCostAction, entityUseMovementAction);
            if(!entityPayHeartCostAction.wasPerformed || !entityUseMovementAction.wasPerformed){
                return;
            }
            activableEffect.TryToCreateEffectActivatedAction(entityPayHeartCostAction, out _);
        }
        */

    }

    public void OnHoverEnter()
    {
        effectHoverEnterEvent.Invoke(effectState);
    }

    public void OnHoverExit()
    {
        effectHoverExitEvent.Invoke(effectState);
    }


    void OnDestroy()
    {
        effectHoverEnterEvent.RemoveAllListeners();
        effectHoverExitEvent.RemoveAllListeners();
    }
}
