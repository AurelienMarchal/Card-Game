using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class CameraFollowingSelectedEntity : MonoBehaviour
{
    
    [SerializeField]
    Transform cameraTransform;

    public float angle;

    [SerializeField]
    float cameraHeight;

    [SerializeField]
    float cameraDistance;

    public GameObject entityGameobject;

    // Update is called once per frame
    void Update(){

        UpdateCameraTransform();
    }

    void UpdateCameraTransform(){

        if(entityGameobject == null){
            return;
        }

        var calculatedAngle = (entityGameobject.transform.rotation.eulerAngles.y - 90f)*Mathf.PI/180f + angle;
        
        var x = cameraDistance * Mathf.Cos(calculatedAngle);

        var z = -cameraDistance * Mathf.Sin(calculatedAngle);
        
        var cameraPos = new Vector3(x, cameraHeight, z) + entityGameobject.transform.position;
        cameraTransform.transform.position = cameraPos;
        cameraTransform.transform.LookAt(entityGameobject.transform);
    }
}
