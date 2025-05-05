using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using GameLogic;

public class MovementUIDisplay : MonoBehaviour
{
    Entity entity_;

    public Entity entity{
        get{
            return entity_;
        }

        set{
            entity_ = value;
            UpdateFromEntity();
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
        UpdateFromEntity();
    }

    private void UpdateFromEntity()
    {
        if(entity == null){
            for (int i = 0; i < images.Length; i++){
                images[i].gameObject.SetActive(false);
            }
            return;
        }

        for (int i = 0; i < images.Length; i++){
            images[i].gameObject.SetActive(i < entity.maxMovement);
            images[i].sprite = i < entity.movementLeft ? spriteMovementFull : spriteMovementEmpty;
        }

    }
}
