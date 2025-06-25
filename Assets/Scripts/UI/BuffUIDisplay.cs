using TMPro;
using UnityEngine;
using UnityEngine.UI;

using GameLogic.GameBuff;
using GameLogic.GameState;
using System;

public class BuffUIDisplay : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI buffTextMeshProUGUI;

    [SerializeField]
    TextMeshProUGUI buffNameMeshProUGUI;

    [SerializeField]
    Image backgroundImage;

    [SerializeField]
    Image arrowImage;

    [SerializeField]
    Color potiveColor;

    [SerializeField]
    Color negativeColor;

    [SerializeField]
    Color neutralColor;

    [Obsolete]
    private Buff buff_;

    [Obsolete]
    public Buff buff
    {
        get
        {
            return buff_;
        }
        set
        {
            buff_ = value;
            UpdateFromNewBuff();
        }
    }

    private BuffState buffState_;

    public BuffState buffState{
        get{ 
            return buffState_;
        }
        set{ 
            buffState_ = value; 
            UpdateFromBuffState();
        }
    }

    private void UpdateFromBuffState(){
        if (buffState == null){
            return;
        }
        buffNameMeshProUGUI.text = buffState.name;
        buffTextMeshProUGUI.text = buffState.text;
        arrowImage.gameObject.transform.rotation = Quaternion.Euler(0f, 0f, buffState.isPositive >= 0 ? 90f : -90f);
        backgroundImage.color = buffState.isPositive > 0 ? potiveColor : (buffState.isPositive < 0 ? negativeColor : neutralColor);
    }

    void Update(){
        //UpdateFromBuff();
    }

    [Obsolete]
    private void UpdateFromBuff()
    {
        if (buff_ == null)
        {
            return;
        }
        
        buffTextMeshProUGUI.text = buff.GetText();
        arrowImage.gameObject.transform.rotation = Quaternion.Euler(0f, 0f, buff.IsPositive() >= 0 ? 90f : -90f);
        backgroundImage.color = buff.IsPositive() > 0 ? potiveColor : (buff.IsPositive() < 0 ? negativeColor : neutralColor);
    }

    [Obsolete]
    private void UpdateFromNewBuff()
    {
        if (buff == null)
        {
            return;
        }
        buffTextMeshProUGUI.text = buff.GetText();
        buffNameMeshProUGUI.text = buff.name;
    }
}
