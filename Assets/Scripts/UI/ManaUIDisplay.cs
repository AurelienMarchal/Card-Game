using UnityEngine;
using UnityEngine.UI;


using GameLogic;

public class ManaUIDisplay : MonoBehaviour
{
    Player player_;

    public Player player{
        get{
            return player_;
        }

        set{
            player_ = value;
            UpdateFromPlayer();
        }
    }

    [SerializeField]
    Image[] images;

    [SerializeField]
    Sprite spriteManaEmpty;

    [SerializeField]
    Sprite spriteManaFull;
    
    // Start is called before the first frame update
    void Start(){
        UpdateFromPlayer();
    }

    private void UpdateFromPlayer()
    {
        if(player == null){
            for (int i = 0; i < images.Length; i++){
                images[i].gameObject.SetActive(false);
            }
            return;
        }

        for (int i = 0; i < images.Length; i++){
            images[i].gameObject.SetActive(i < player.maxMana);
            images[i].sprite = i < player.manaLeft ? spriteManaFull : spriteManaEmpty;
        }

    }
}
