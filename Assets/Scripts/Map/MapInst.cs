using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapInst : MonoBehaviour
{
    private Camera _camera;
    private float _offsetY = 24;

    [SerializeField]
    private GameObject _jumpPosObj;
    private void Start()
    {
        _camera = Camera.main;
        
        // 找到子对象中叫“Grid/JumpPos”的物体
        _jumpPosObj?.SetActive(false);
    }
    
    void FixedUpdate()
    {
        if (_camera is not null && Camera.main.transform.position.y - transform.position.y > _offsetY)
        {
            Destroy(gameObject);
        }
    }
}
