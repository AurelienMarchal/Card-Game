using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

using GameLogic.GameEffect;
using GameLogic.GameBuff;
using System;
using GameLogic.GameState;

public class EntityInfoUI : MonoBehaviour
{   

    [SerializeField]
    CameraFollowingSelectedEntity cameraFollowingSelectedEntity;

    [SerializeField]
    HealthUIDisplay healthUIDisplay;

    [SerializeField]
    MovementUIDisplay movementUIDisplay;

    [SerializeField]
    TextMeshProUGUI entityNameTextMeshProUGUI;

    [SerializeField]
    AtkUIDisplay atkUiDisplay;

    public UnityEvent weaponUsedUnityEvent = new UnityEvent();

    [SerializeField]
    Scrollbar angleScrollbar;

    [SerializeField]
    GameObject effectScrollViewContent;

    [SerializeField]
    GameObject effectCanvasPrefab;

    [SerializeField]
    GameObject buffCanvasPrefab;

    
    public EffectStateEvent effectHoverEnterEvent = new EffectStateEvent();

    public EffectStateEvent effectHoverExitEvent = new EffectStateEvent();

    public EffectStateEvent effectPointerDownEvent = new EffectStateEvent();

    public EffectStateEvent effectClickedEvent = new EffectStateEvent();

    public UnityEvent weaponHoverEnterEvent = new UnityEvent();

    public UnityEvent weaponHoverExitEvent = new UnityEvent();

    private EntityManager entityManager_;
    public EntityManager entityManager{
        get{
            return entityManager_;
        }
        set{
            entityManager_ = value;
            UpdateAccordingToEntityManager();
        }
    }

    void Start(){

    }

    void Update(){
        cameraFollowingSelectedEntity.angle = angleScrollbar.value * 2 * Mathf.PI;
        if(entityManager != null){
            //Temp
            healthUIDisplay.healthState = entityManager.entityState.healthState;
            movementUIDisplay.entityState = entityManager.entityState;
            //atkUiDisplay.entity = entityManager.entity;
        }
    }

    private void OnWeaponButtonClick()
    {
        weaponUsedUnityEvent.Invoke();
    }

    void ClearScrollView(){
        foreach (Transform child in effectScrollViewContent.transform){
            Destroy(child.gameObject);
        }
    }

    void AddEffectCanvasFromEffectState(EffectState effectState){
        
        if (!effectState.displayOnUI)
        {
            return;
        }
        

        var effectCanvas = Instantiate(effectCanvasPrefab, effectScrollViewContent.transform);
        var effectUIDisplay = effectCanvas.GetComponent<EffectUIDisplay>();

        if(effectUIDisplay != null){
            effectUIDisplay.effectState = effectState;
            effectUIDisplay.effectHoverEnterEvent.AddListener((effect) => effectHoverEnterEvent.Invoke(effect));
            effectUIDisplay.effectHoverExitEvent.AddListener((effect) => effectHoverExitEvent.Invoke(effect));
            effectUIDisplay.effectPointerDownEvent.AddListener((effect) => effectPointerDownEvent.Invoke(effect));
            effectUIDisplay.effectClickedEvent.AddListener((effect) => effectClickedEvent.Invoke(effect));
        }
    }

    void AddBuffCanvasFromBuffState(BuffState buffState){
        

        var buffCanvas = Instantiate(buffCanvasPrefab, effectScrollViewContent.transform);
        var buffUIDisplay = buffCanvas.GetComponent<BuffUIDisplay>();

        if(buffUIDisplay != null){
            buffUIDisplay.buffState = buffState;
        }
    }

    [Obsolete]
    void AddEffectCanvasFromEffect(Effect effect){
        if(!effect.displayOnUI){
            return;
        }

        var effectCanvas = Instantiate(effectCanvasPrefab, effectScrollViewContent.transform);
        var effectUIDisplay = effectCanvas.GetComponent<EffectUIDisplay>();

        if(effectUIDisplay != null){
            //effectUIDisplay.effect = effect;
            effectUIDisplay.effectHoverEnterEvent.AddListener((effect) => effectHoverEnterEvent.Invoke(effect));
            effectUIDisplay.effectHoverExitEvent.AddListener((effect) => effectHoverExitEvent.Invoke(effect));
        }
    }

    [Obsolete]
    void AddBuffCanvasFromBuff(Buff buff){
        /*
        if(!effect.displayOnUI){
            return;
        }
        */

        var buffCanvas = Instantiate(buffCanvasPrefab, effectScrollViewContent.transform);
        var buffUIDisplay = buffCanvas.GetComponent<BuffUIDisplay>();

        if(buffUIDisplay != null){
            buffUIDisplay.buff = buff;
        }
    }

    void UpdateAccordingToEntityManager(){
        ClearScrollView();
        if(entityManager == null){
            cameraFollowingSelectedEntity.entityGameobject = null;
            entityNameTextMeshProUGUI.text = "";
        }
        else{

            cameraFollowingSelectedEntity.entityGameobject = entityManager.gameObject;
            healthUIDisplay.healthState = entityManager.entityState.healthState;
            movementUIDisplay.entityState = entityManager.entityState;
            
            atkUiDisplay.entityState = entityManager.entityState;
            atkUiDisplay.atkButton.onClick.RemoveAllListeners();
            atkUiDisplay.atkButton.onClick.AddListener(OnWeaponButtonClick);
            
            entityNameTextMeshProUGUI.text = entityManager.entityState.name;
            foreach (var effectState in entityManager.entityState.effectStates){
                AddEffectCanvasFromEffectState(effectState);
            }

            foreach (var buffState in entityManager.entityState.buffStates){
                AddBuffCanvasFromBuffState(buffState);
            }
        }
    }

    public void OnWeaponHoverEnter(){
        weaponHoverEnterEvent.Invoke();
    }

    public void OnWeaponHoverExit(){
        weaponHoverExitEvent.Invoke();
    }

    void OnDestroy(){
        effectHoverEnterEvent.RemoveAllListeners();
        effectHoverExitEvent.RemoveAllListeners();
        effectPointerDownEvent.RemoveAllListeners();
        effectClickedEvent.RemoveAllListeners();
        weaponUsedUnityEvent.RemoveAllListeners();
        weaponHoverEnterEvent.RemoveAllListeners();
        weaponHoverExitEvent.RemoveAllListeners();
    }
}
