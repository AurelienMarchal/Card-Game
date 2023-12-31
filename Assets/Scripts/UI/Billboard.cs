
using UnityEngine;

public class Billboard : MonoBehaviour{

    // Start is called before the first frame update
    private Camera m_Camera;

    private void Start() {
        m_Camera = Camera.main;
    }

    //Orient the camera after all movement is completed this frame to avoid jittering
    void LateUpdate()
    {
        transform.LookAt(transform.position + m_Camera.transform.rotation * Vector3.forward,
            m_Camera.transform.rotation * Vector3.up);
    }
}
