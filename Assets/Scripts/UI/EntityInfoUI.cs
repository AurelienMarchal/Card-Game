using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

using GameLogic.GameEffect;
using GameLogic.GameBuff;

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

    public EffectEvent effectHoverEnterEvent = new EffectEvent();

    public EffectEvent effectHoverExitEvent = new EffectEvent();

    public UnityEvent weaponHoverEnterEvent = new UnityEvent();

    public UnityEvent weaponHoverExitEvent = new UnityEvent();

    private EntityManager entityManager_;
    public EntityManager entityManager{
        get{
            return entityManager_;
        }
        set{
            entityManager_ = value;
            UpdateAccordingToEntity();
        }
    }

    void Start(){

    }

    void Update(){
        cameraFollowingSelectedEntity.angle = angleScrollbar.value * 2 * Mathf.PI;
        if(entityManager != null){
            healthUIDisplay.health = entityManager.entity.health;
            movementUIDisplay.entity = entityManager.entity;
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

    void AddEffectCanvasFromEffect(Effect effect){
        if(!effect.displayOnUI){
            return;
        }

        var effectCanvas = Instantiate(effectCanvasPrefab, effectScrollViewContent.transform);
        var effectUIDisplay = effectCanvas.GetComponent<EffectUIDisplay>();

        if(effectUIDisplay != null){
            effectUIDisplay.effect = effect;
            effectUIDisplay.effectHoverEnterEvent.AddListener((effect) => effectHoverEnterEvent.Invoke(effect));
            effectUIDisplay.effectHoverExitEvent.AddListener((effect) => effectHoverExitEvent.Invoke(effect));
        }
    }

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

    void UpdateAccordingToEntity(){
        ClearScrollView();
        if(entityManager == null){
            cameraFollowingSelectedEntity.entityGameobject = null;
            entityNameTextMeshProUGUI.text = "";
        }
        else{

            cameraFollowingSelectedEntity.entityGameobject = entityManager.gameObject;
            healthUIDisplay.health = entityManager.entity.health;
            movementUIDisplay.entity = entityManager.entity;
            
            atkUiDisplay.entity = entityManager.entity;
            atkUiDisplay.atkButton.onClick.RemoveAllListeners();
            atkUiDisplay.atkButton.onClick.AddListener(OnWeaponButtonClick);
            
            entityNameTextMeshProUGUI.text = entityManager.entity.name;
            foreach (var effect in entityManager.entity.effects){
                AddEffectCanvasFromEffect(effect);
            }

            foreach (var buff in entityManager.entity.buffs){
                AddBuffCanvasFromBuff(buff);
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
        weaponUsedUnityEvent.RemoveAllListeners();
        weaponHoverEnterEvent.RemoveAllListeners();
        weaponHoverExitEvent.RemoveAllListeners();
    }
}
