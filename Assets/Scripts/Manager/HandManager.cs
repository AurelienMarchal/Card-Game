using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    public Hand hand{
        get;
        set;
    }

    [SerializeField]
    GameObject cardPrefab;

    [SerializeField]
    float radius;

    [SerializeField]
    float degreeRange;


    void Update(){
        MoveChildrenInCircle();
        // Update according to hand ?

        // TEST
        hand.cards.Clear();
        var childCount = transform.childCount;

        for (var i = 0; i < childCount; i++){
            Transform child = transform.GetChild(i);
            var cardManager = child.gameObject.GetComponent<CardManager>();
            if(cardManager != null){
                if(cardManager.card != null){
                    hand.cards.Add(cardManager.card);
                }
            }
        }

        //TEST
    }


    void MoveChildrenInCircle()
    {
        var childCount = transform.childCount;

        for (var i = 0; i < childCount; i++)
        {
            Transform child = transform.GetChild(i);

            // Calculate the angle based on the current time and speed
            var angle = (i - childCount/2) * degreeRange * (Mathf.PI/180) / childCount;

            // Calculate the position on the circle
            var x = Mathf.Sin(angle) * radius + transform.position.x;
            var y = transform.position.y;
            var z = Mathf.Cos(angle) * radius + transform.position.z - radius;

            // Set the position of the child
            child.position = new Vector3(x, y, z);

            var circleCenter = new Vector3(transform.position.x, transform.position.y, transform.position.z - radius);

            // Orient the child's rotation
            Quaternion rotation = Quaternion.LookRotation(-Vector3.up, -(circleCenter - child.position));
            child.rotation = rotation;

        }
    }

    void OnDestroy(){
        var childCount = transform.childCount;

        for (var i = 0; i < childCount; i++){
            Transform child = transform.GetChild(i);
            Destroy(child.gameObject);
        }
    }
}
