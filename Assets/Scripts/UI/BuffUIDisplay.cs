using TMPro;
using UnityEngine;
using UnityEngine.UI;

using GameLogic.GameBuff;

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

    private Buff buff_;

    public Buff buff{
        get{ 
            return buff_;
        }
        set{ 
            buff_ = value; 
            UpdateFromNewBuff();
        }
    }

    void Update(){
        UpdateFromBuff();
    }

    private void UpdateFromBuff()
    {
        if(buff_ == null){
            return;
        }
        buffTextMeshProUGUI.text = buff.GetText();
        arrowImage.gameObject.transform.rotation = Quaternion.Euler(0f, 0f, buff.IsPositive() >= 0  ? 90f : -90f);
        backgroundImage.color = buff.IsPositive() > 0 ? potiveColor : (buff.IsPositive() < 0 ? negativeColor : neutralColor);
    }

    private void UpdateFromNewBuff(){
        if(buff == null){
            return;
        }
        buffTextMeshProUGUI.text = buff.GetText();
        buffNameMeshProUGUI.text = buff.name;
    }
}
