using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardManager : MonoBehaviour
{


    [SerializeField]
    TextMeshProUGUI cardNameTextMeshProUGUI;

    [SerializeField]
    TextMeshProUGUI cardTextTextMeshProUGUI;

    [SerializeField]
    TextMeshProUGUI cardCostTextMeshProUGUI;

    private Card card_;

    public Card card{
        get{
            return card_;
        }
        set{
            card_ = value;
            UpdateAccordingToCard();
        }
    }

    void UpdateAccordingToCard(){

    }
}
