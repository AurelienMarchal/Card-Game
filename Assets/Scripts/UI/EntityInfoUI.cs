using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class EntityInfoUI : MonoBehaviour
{   

    [SerializeField]
    CameraFollowingSelectedEntity cameraFollowingSelectedEntity;

    [SerializeField]
    HealthUIDisplay healthUIDisplay;

    [SerializeField]
    MovementUIDisplay movementUIDisplay;

    [SerializeField]
    TextMeshProUGUI entityAtkTextMeshProUGUI;

    [SerializeField]
    TextMeshProUGUI entityNameTextMeshProUGUI;

    [SerializeField]
    Scrollbar angleScrollbar;

    [SerializeField]
    GameObject effectScrollViewContent;

    [SerializeField]
    GameObject effectCanvasPrefab;

    private EntityManager entityManager_;
    public EntityManager entityManager{
        get{
            return entityManager_;
        }
        set{
            entityManager_ = value;
            ClearScrollView();
            if(entityManager != null){
                foreach (var effect in entityManager.entity.effects){
                    AddEffectCanvasFromEffect(effect);
                }
            }
            
        }
    }


    void Start(){

    }

    void Update(){
        cameraFollowingSelectedEntity.angle = angleScrollbar.value * 2 * Mathf.PI;
        if(entityManager == null){
            cameraFollowingSelectedEntity.entityGameobject = null;
            entityNameTextMeshProUGUI.text = "";
        }
        else{
            cameraFollowingSelectedEntity.entityGameobject = entityManager.gameObject;
            healthUIDisplay.health = entityManager.entity.health;
            movementUIDisplay.entity = entityManager.entity;
            entityAtkTextMeshProUGUI.text = entityManager.entity.atkDamage.amount.ToString();
            entityNameTextMeshProUGUI.text = entityManager.entity.name;
        }
    }

    void ClearScrollView(){
        foreach (Transform child in effectScrollViewContent.transform){
            Destroy(child.gameObject);
        }
    }

    void AddEffectCanvasFromEffect(Effect effect){
        var effectCanvas = Instantiate(effectCanvasPrefab, effectScrollViewContent.transform);
        var effectUIDisplay = effectCanvas.GetComponent<EffectUIDisplay>();

        if(effectUIDisplay != null){
            effectUIDisplay.effect = effect;
        }
    }
}
