using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using GameLogic;
using GameLogic.GameState;
using System;

public class MovementUIDisplay : MonoBehaviour
{
    [Obsolete]
    private Entity entity_;

    [Obsolete]
    public Entity entity
    {
        get
        {
            return entity_;
        }

        set
        {
            entity_ = value;
            UpdateAccordingToEntity();
        }
    }

    private EntityState entityState_;

    public EntityState entityState{
        get{
            return entityState_;
        }

        set{
            entityState_ = value;
            UpdateAccordingToEntityState();
        }
    }

    [SerializeField]
    Image[] images;

    [SerializeField]
    Sprite spriteMovementEmpty;

    [SerializeField]
    Sprite spriteMovementFull;
    
    // Start is called before the first frame update
    void Awake(){
        UpdateAccordingToEntityState();
        UpdateAccordingToEntity();
    }
    
    private void UpdateAccordingToEntityState()
    {
        if (entityState == null)
        {
            for (int i = 0; i < images.Length; i++)
            {
                images[i].gameObject.SetActive(false);
            }
            return;
        }

        for (int i = 0; i < images.Length; i++)
        {
            images[i].gameObject.SetActive(i < entityState.maxMovement);
            images[i].sprite = i < entityState.movementLeft ? spriteMovementFull : spriteMovementEmpty;
        }
    }

    private void UpdateAccordingToEntity()
    {
        if (entity == null)
        {
            for (int i = 0; i < images.Length; i++)
            {
                images[i].gameObject.SetActive(false);
            }
            return;
        }

        for (int i = 0; i < images.Length; i++)
        {
            images[i].gameObject.SetActive(i < entity.maxMovement);
            images[i].sprite = i < entity.movementLeft ? spriteMovementFull : spriteMovementEmpty;
        }

    }
}
