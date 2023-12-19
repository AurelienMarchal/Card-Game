using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[Serializable]
public struct HeartSprite {
    public HeartType heartType;
    public Sprite sprite;
}

public class HealthUIDisplay : MonoBehaviour
{
    
    Health health_;

    public Health health{
        get{
            return health_;
        }

        set{
            health_ = value;
            UpdateFromHealth();
        }
    }
    
    [SerializeField]
    Image[] images;

    [SerializeField]
    HeartSprite[] heartSprites;

    Dictionary<HeartType, Sprite> heartSpriteDictionary;


    void Awake(){
        heartSpriteDictionary = new Dictionary<HeartType, Sprite>();
        foreach(HeartSprite heartSprite in heartSprites){
            heartSpriteDictionary.Add(heartSprite.heartType, heartSprite.sprite);
        }

        UpdateFromHealth();
    }


    void UpdateFromHealth(){
        for (int i = 0; i < images.Length; i++){
            images[i].gameObject.SetActive(false);
        }
        
        if(health == null){
            return;
        }

        for (int i = 0; i < health.hearts.Length; i++){
            var heart = health.hearts[i];

            images[i].gameObject.SetActive(heart != HeartType.NoHeart);

            heartSpriteDictionary.TryGetValue(heart, out Sprite sprite);
            if(sprite != null){
                images[i].sprite = sprite;
            }
        }
    }
}
