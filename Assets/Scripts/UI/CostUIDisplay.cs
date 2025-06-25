using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using GameLogic;
using GameLogic.GameState;

public class CostUIDisplay : MonoBehaviour
{

    [Obsolete]
    Cost cost_;

    [Obsolete]
    public Cost cost
    {
        get
        {
            return cost_;
        }

        set
        {
            cost_ = value;
            UpdateFromCost();
        }
    }

    
    CostState costState_;

    
    public CostState costState
    {
        get
        {
            return costState_;
        }

        set
        {
            costState_ = value;
            UpdateFromCostState();
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


    void Awake()
    {
        heartSpriteDictionary = new Dictionary<HeartType, Sprite>();
        foreach (HeartSprite heartSprite in heartSprites)
        {
            heartSpriteDictionary.Add(heartSprite.heartType, heartSprite.sprite);
        }

        //UpdateFromCost();
        UpdateFromCostState();
    }

    private void UpdateFromCostState()
    {
        for (int i = 0; i < heartImages.Length; i++)
        {
            heartImages[i].gameObject.SetActive(false);
        }

        if (costState == null)
        {
            manaImage.gameObject.SetActive(false);
            return;
        }

        manaImage.gameObject.SetActive(costState.mouvementCost > 0);
        manaText.text = costState.mouvementCost.ToString();

        var count = 0;

        foreach (KeyValuePair<HeartType, int> entry in costState.GetHeartTypeDict())
        {
            if (count >= heartImages.Length || count >= heartTexts.Length)
            {
                break;
            }

            heartImages[count].gameObject.SetActive(true);
            heartImages[count].sprite = heartSpriteDictionary[entry.Key];
            heartTexts[count].text = entry.Value.ToString();

            count++;
        }
    }

    [Obsolete]
    private void UpdateFromCost()
    {

        for (int i = 0; i < heartImages.Length; i++)
        {
            heartImages[i].gameObject.SetActive(false);
        }

        manaImage.gameObject.SetActive(cost.mouvementCost > 0);
        manaText.text = cost.mouvementCost.ToString();

        var count = 0;

        foreach (KeyValuePair<HeartType, int> entry in cost.GetHeartTypeDict())
        {
            if (count >= heartImages.Length || count >= heartTexts.Length)
            {
                break;
            }

            heartImages[count].gameObject.SetActive(true);
            heartImages[count].sprite = heartSpriteDictionary[entry.Key];
            heartTexts[count].text = entry.Value.ToString();

            count++;
        }

    }
}
