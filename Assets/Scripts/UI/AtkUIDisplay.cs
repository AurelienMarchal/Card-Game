using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using GameLogic;
using System;
using GameLogic.GameState;

public class AtkUIDisplay : MonoBehaviour
{

    [SerializeField]
    public Button atkButton;

    [SerializeField]
    TextMeshProUGUI weaponNameTextMeshProUGUI;

    [SerializeField]
    TextMeshProUGUI weaponEffectTextMeshProUGUI;

    [SerializeField]
    Image weaponImage;

    [SerializeField]
    CostUIDisplay costUIDisplay;

    [SerializeField]
    Image atkImage;

    [SerializeField]
    TextMeshProUGUI atkTextMeshProUGUI;

    [SerializeField]
    Image rangeImage;

    [SerializeField]
    TextMeshProUGUI rangeTextMeshProUGUI;

    [Obsolete]
    private Entity entity_;
    
    [Obsolete]
    public Entity entity
    {
        get
        {
            return entity_;
        }
        set
        {
            entity_ = value;
            UpdateFromEntity();
        }
    }

    private EntityState entityState_;

    public EntityState entityState{
        get{
            return entityState_;
        }

        set{
            entityState_ = value;
            UpdateAccordingToEntityState();
        }
    }

    private void UpdateAccordingToEntityState()
    {
        if(entityState == null){
            return;
        }
        
        atkTextMeshProUGUI.text = $"{entityState.atkDamageState.amount}";
        rangeTextMeshProUGUI.text = $"{entityState.range}";
        costUIDisplay.costState = entityState.costToAtkState;
        costUIDisplay.UpdateVisuals();
        //TODO : rajouter entityState.canPayAtkCost boolean
        //atkButton.enabled = entityState.CanPayAtkCost() && Game.currentGame.currentPlayer == entity.player;

        if (entityState is HeroState heroState)
        {
            weaponImage.gameObject.SetActive(heroState.weaponState != null);
            atkImage.gameObject.SetActive(heroState.weaponState != null);
            atkTextMeshProUGUI.gameObject.SetActive(heroState.weaponState != null);
            rangeImage.gameObject.SetActive(heroState.weaponState != null);
            rangeTextMeshProUGUI.gameObject.SetActive(heroState.weaponState != null);

            if (heroState.weaponState != null)
            {
                weaponNameTextMeshProUGUI.text = heroState.weaponState.name;
                weaponEffectTextMeshProUGUI.text = "No effect";

            }
            else
            {
                costUIDisplay.costState = null;
                weaponNameTextMeshProUGUI.text = "";
                weaponEffectTextMeshProUGUI.text = "No weapon equipped";
            }
        }
        else
        {
            rangeImage.gameObject.SetActive(true);
            atkImage.gameObject.SetActive(true);
            weaponNameTextMeshProUGUI.text = "";
            weaponImage.gameObject.SetActive(false);
            weaponEffectTextMeshProUGUI.text = "Attack";
        }
    }

    [Obsolete]
    void UpdateFromEntity(){
        if(entity == null || entity == Entity.noEntity){
            return;
        }
        
        atkTextMeshProUGUI.text = $"{entity.atkDamage.amount}";
        rangeTextMeshProUGUI.text = $"{entity.range}";
        costUIDisplay.cost = entity.costToAtk;
        //atkButton.enabled = entity.CanPayAtkCost() && Game.currentGame.currentPlayer == entity.player;

        if(entity is Hero hero){
            weaponImage.gameObject.SetActive(hero.weapon != null);
            atkImage.gameObject.SetActive(hero.weapon != null);
            atkTextMeshProUGUI.gameObject.SetActive(hero.weapon != null);
            rangeImage.gameObject.SetActive(hero.weapon != null);
            rangeTextMeshProUGUI.gameObject.SetActive(hero.weapon != null);
            
            if(hero.weapon != null){
                weaponNameTextMeshProUGUI.text = hero.weapon.name;
                weaponEffectTextMeshProUGUI.text = "No effect";
                
            }
            else{
                costUIDisplay.cost = Cost.noCost;
                weaponNameTextMeshProUGUI.text = "";
                weaponEffectTextMeshProUGUI.text = "No weapon equipped";
            }
        }
        else{
            rangeImage.gameObject.SetActive(true);
            atkImage.gameObject.SetActive(true);
            weaponNameTextMeshProUGUI.text = "";
            weaponImage.gameObject.SetActive(false);
            weaponEffectTextMeshProUGUI.text = "Attack";
        }
    }
}
