using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSet : MonoBehaviour
{
    // 透视相机Size基础值 以720*1280为基准
    [SerializeField]
    private float orthographicSizeBase;
    // 基础宽高比基础值 720/1280
    [SerializeField] private float baseRatio;
    
    // 透视相机Size
    [SerializeField]
    private float ratio;
    
    void Start()
    {
        ratio = (float)Screen.height / Screen.width;

        if (ratio >= baseRatio)
        {
            Camera.main!.orthographicSize = orthographicSizeBase * ratio * 0.5f;
        }
        else
        {
            
        }
    }
    
}
