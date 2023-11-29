using UnityEngine;
using UnityEngine.UI;

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
    Sprite emptyHeartSprite;

    [SerializeField]
    Sprite emptyRedHeartSprite;

    [SerializeField]
    Sprite halfRedHeartSprite;

    [SerializeField]
    Sprite redHeartSprite;

    [SerializeField]
    Sprite halfBlueHeartSprite;

    [SerializeField]
    Sprite blueHeartSprite;

    void Start(){
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

            images[i].gameObject.SetActive(true);
            
            if(heart.firstHalfHeartType == HeartType.RedEmpty && heart.secondHalfHeartType == HeartType.RedEmpty){
                images[i].sprite = emptyRedHeartSprite;
            }

            else if(heart.firstHalfHeartType == HeartType.Red && heart.secondHalfHeartType == HeartType.RedEmpty){
                images[i].sprite = halfRedHeartSprite;
            }

            else if(heart.firstHalfHeartType == HeartType.Red && heart.secondHalfHeartType == HeartType.Red){
                images[i].sprite = redHeartSprite;
            }

            else if(heart.firstHalfHeartType == HeartType.Blue && heart.secondHalfHeartType == HeartType.NoHeart){
                images[i].sprite = halfBlueHeartSprite;
            }

            else if(heart.firstHalfHeartType == HeartType.Blue && heart.secondHalfHeartType == HeartType.Blue){
                images[i].sprite = blueHeartSprite;
            }
            
            else{
                images[i].gameObject.SetActive(false);
                images[i].sprite = emptyHeartSprite;
            }
        }
    }
}
