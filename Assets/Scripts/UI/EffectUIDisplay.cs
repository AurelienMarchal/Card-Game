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
    HealthUIDisplay healthUIDisplay;

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

        healthUIDisplay.gameObject.SetActive(effect is ActivableEffect);

        switch(effect){
            case ActivableEffect activableEffect:
                
                healthUIDisplay.health = new Health(activableEffect.cost.heartCost);
                button.onClick.AddListener(OnButtonClick);
                
                break;
            default:

            break;
        }
    }

    private void OnButtonClick()
    {
        Debug.Log($"Effect : {effect}");
        var activableEffect = effect as ActivableEffect;
        if(activableEffect != null){
            activableEffect.associatedEntity.player.TryToActivateActivableEffect(activableEffect);
        }
        
    }

    void UpdateFromEffect(){
        if(effect == null){
            return;
        }
        
        effectTextMeshProUGUI.text = effect.GetEffectText();
        button.interactable = effect is ActivableEffect;

    }
}
