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
    }


    void MoveChildrenInCircle()
    {
        var childCount = transform.childCount;

        for (var i = 0; i < childCount; i++)
        {
            Transform child = transform.GetChild(i);

            // Calculate the angle based on the current time and speed
            var angle = i * degreeRange * (Mathf.PI/180) / childCount;

            // Calculate the position on the circle
            var x = Mathf.Cos(angle) * radius + transform.position.x;
            var y = transform.position.y;
            var z = Mathf.Sin(angle) * radius + transform.position.z;

            // Set the position of the child
            child.position = new Vector3(x, 0f, z);

            // Orient the child's rotation
            Quaternion rotation = Quaternion.LookRotation(-Vector3.up, -(transform.position - child.position));
            child.rotation = rotation;
            child.Rotate(transform.rotation.eulerAngles);
        }
    }
}
