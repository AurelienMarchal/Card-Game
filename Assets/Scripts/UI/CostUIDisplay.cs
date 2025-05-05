using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using GameLogic;

public class CostUIDisplay : MonoBehaviour
{

    Cost cost_;

    public Cost cost{
        get{
            return cost_;
        }

        set{
            cost_ = value;
            UpdateFromCost();
        }
    }

    [SerializeField]
    Image manaImage;

    [SerializeField]
    TextMeshProUGUI manaText;

    [SerializeField]
    HeartSprite[] heartSprites;

    Dictionary<HeartType, Sprite> heartSpriteDictionary;

    [SerializeField]
    Image[] heartImages;

    [SerializeField]
    TextMeshProUGUI[] heartTexts;


    void Awake(){
        cost = Cost.noCost;
        heartSpriteDictionary = new Dictionary<HeartType, Sprite>();
        foreach(HeartSprite heartSprite in heartSprites){
            heartSpriteDictionary.Add(heartSprite.heartType, heartSprite.sprite);
        }

        UpdateFromCost();
    }

    private void UpdateFromCost()
    {

        for (int i = 0; i < heartImages.Length; i++){
            heartImages[i].gameObject.SetActive(false);
        }
        
        manaImage.gameObject.SetActive(cost.mouvementCost > 0);
        manaText.text = cost.mouvementCost.ToString();

        var count = 0;

        foreach(KeyValuePair<HeartType, int> entry in cost.GetHeartTypeDict()){
            if(count >=  heartImages.Length || count >=  heartTexts.Length){
                break;
            }

            heartImages[count].gameObject.SetActive(true);
            heartImages[count].sprite = heartSpriteDictionary[entry.Key];
            heartTexts[count].text = entry.Value.ToString();

            count ++;
        }

    }
}
