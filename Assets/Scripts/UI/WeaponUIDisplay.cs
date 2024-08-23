using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUIDisplay : MonoBehaviour
{

    [SerializeField]
    public Button weaponButton;

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

    private Weapon weapon_;
    public Weapon weapon{
        get{
            return weapon_;
        }
        set{
            weapon_ = value;
            UpdateFromWeapon();
        }
    }

    void UpdateFromWeapon(){
        weaponImage.gameObject.SetActive(weapon != null);
        atkImage.gameObject.SetActive(weapon != null);
        atkTextMeshProUGUI.gameObject.SetActive(weapon != null);
        rangeImage.gameObject.SetActive(weapon != null);
        rangeTextMeshProUGUI.gameObject.SetActive(weapon != null);
        weaponButton.enabled = weapon != null;
        
        if(weapon != null){
            weaponNameTextMeshProUGUI.text = weapon.name;
            atkTextMeshProUGUI.text = $"{weapon.atkDamage.amount}";
            rangeTextMeshProUGUI.text = $"{weapon.range}";
            weaponEffectTextMeshProUGUI.text = "No effect";
            costUIDisplay.cost = weapon.costToUse;
            
        }
        else{
            costUIDisplay.cost = Cost.noCost;
            weaponNameTextMeshProUGUI.text = "";
            weaponEffectTextMeshProUGUI.text = "No weapon equipped";
        }
    }
}
