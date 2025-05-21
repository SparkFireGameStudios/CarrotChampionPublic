using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform frog;
    
    public float offsetY;

    void Start()
    {
        
    }

    void LateUpdate()
    {
        transform.position = new Vector3(0, frog.position.y + offsetY, transform.position.z);
    }
}
