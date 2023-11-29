using UnityEngine;
using UnityEngine.UI;

public class MovementUIDisplay : MonoBehaviour
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
    Sprite spriteMovementEmpty;

    [SerializeField]
    Sprite spriteMovementFull;
    
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
            images[i].gameObject.SetActive(i < player.maxMovement);
            images[i].sprite = i < player.movementLeft ? spriteMovementFull : spriteMovementEmpty;
        }

    }
}
