using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using UnityEngine.Events;


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
    WeaponUIDisplay weaponUIDisplay;

    public UnityEvent weaponUsedUnityEvent = new UnityEvent();

    [SerializeField]
    Scrollbar angleScrollbar;

    [SerializeField]
    GameObject effectScrollViewContent;

    [SerializeField]
    GameObject effectCanvasPrefab;

    [SerializeField]
    GameObject buffCanvasPrefab;

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
            weaponUIDisplay.weapon = entityManager.entity.weapon;
            weaponUIDisplay.weaponButton.enabled = entityManager.entity.CanPayWeaponCost() && Game.currentGame.currentPlayer == entityManager.entity.player;
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
            
            weaponUIDisplay.weapon = entityManager.entity.weapon;
            weaponUIDisplay.weaponButton.enabled = entityManager.entity.CanPayWeaponCost() && Game.currentGame.currentPlayer == entityManager.entity.player;
            weaponUIDisplay.weaponButton.onClick.RemoveAllListeners();
            weaponUIDisplay.weaponButton.onClick.AddListener(OnWeaponButtonClick);
            
            entityNameTextMeshProUGUI.text = entityManager.entity.name;
            foreach (var effect in entityManager.entity.effects){
                AddEffectCanvasFromEffect(effect);
            }

            foreach (var buff in entityManager.entity.buffs){
                AddBuffCanvasFromBuff(buff);
            }
        }
    }
}
