using UnityEngine;
using UnityEngine.UI;


using GameLogic;
using System;
using GameLogic.GameState;

public class ManaUIDisplay : MonoBehaviour
{
    [Obsolete]
    Player player_;

    [Obsolete]
    public Player player
    {
        get
        {
            return player_;
        }

        set
        {
            player_ = value;
            UpdateFromPlayer();
        }
    }

    PlayerState playerState_;

    public PlayerState playerState
    {
        get
        {
            return playerState_;
        }

        set
        {
            playerState_ = value;

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

    }

    public void UpdateVisuals()
    {

        if (playerState == null)
        {
            for (int i = 0; i < images.Length; i++)
            {
                images[i].gameObject.SetActive(false);
            }
            return;
        }

        //Debug.Log($"Updating Mana Display for PlayerNum {playerState.playerNum}, maxMana : {playerState.maxMana}, manaLeft {playerState.manaLeft}");

        for (int i = 0; i < images.Length; i++)
        {
            images[i].gameObject.SetActive(i < playerState.maxMana);
            images[i].sprite = i < playerState.manaLeft ? spriteManaFull : spriteManaEmpty;
        }
    }

    [Obsolete]
    private void UpdateFromPlayer()
    {
        if (player == null)
        {
            for (int i = 0; i < images.Length; i++)
            {
                images[i].gameObject.SetActive(false);
            }
            return;
        }

        for (int i = 0; i < images.Length; i++)
        {
            images[i].gameObject.SetActive(i < player.maxMana);
            images[i].sprite = i < player.manaLeft ? spriteManaFull : spriteManaEmpty;
        }

    }
}
