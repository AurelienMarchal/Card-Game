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

    public EffectStateEvent effectPointerDownEvent = new EffectStateEvent();

    public EffectStateEvent effectClickedEvent = new EffectStateEvent();

    void Awake()
    {
        button.onClick.AddListener(OnClick);
    }

    private void UpdateFromEffectState()
    {
        if (effectState == null)
        {
            return;
        }


        effectTextMeshProUGUI.text = effectState.effectText;


        //Temp
        button.interactable = effectState.isActivableEffect && effectState.canBeActivated && ((effectState.hasCost && effectState.costCanBePaid) || !effectState.hasCost);
        
        costUIDisplay.gameObject.SetActive(effectState.cost != null);

        if (effectState.cost != null)
        {
            costUIDisplay.costState = effectState.cost;
            costUIDisplay.UpdateVisuals();
        }
        
    }

    public void OnHoverEnter()
    {
        effectHoverEnterEvent.Invoke(effectState);
    }

    public void OnHoverExit()
    {
        effectHoverExitEvent.Invoke(effectState);
    }

    public void OnPointerDown()
    {
        effectPointerDownEvent.Invoke(effectState);
    }

    public void OnClick()
    {
        effectClickedEvent.Invoke(effectState);
    }



    void OnDestroy()
    {
        effectHoverEnterEvent.RemoveAllListeners();
        effectHoverExitEvent.RemoveAllListeners();
        effectClickedEvent.RemoveAllListeners();
        effectPointerDownEvent.RemoveAllListeners();
        button.onClick.RemoveListener(OnClick);
    }
}
