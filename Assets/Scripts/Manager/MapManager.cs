using System;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class MapManager : SingletonMonobehaviour<MapManager>
{
    [Header("两块地图的间隔")]
    public float _mapOffsetY = 24;
    [Header("地图预制体")]
    public GameObject[] _mapPrefabs;
    [Header("地图父物体")]
    public GameObject _mapParent;
    
    private int _lastMapIndex;
    
    private Vector3 _initPosition;

    protected override void Awake()
    {
        base.Awake();
        _initPosition = transform.position;
        
        _mapParent = GameObject.Find("MapParent");
        DontDestroyOnLoad(_mapParent);
    }

    public void CheckPosition()
    {
        if (Camera.main !=null && Camera.main.transform.position.y - transform.position.y >= 0)
        {
            transform.position = new Vector3(
                transform.position.x, 
                transform.position.y + _mapOffsetY, 
                transform.position.z
            );
            SpawnMap();
        }
    }

    private void SpawnMap()
    {
        var randomIndex = Random.Range(0, _mapPrefabs.Length);
        while (randomIndex == _lastMapIndex)
        {
            randomIndex = Random.Range(0, _mapPrefabs.Length);
        }
        _lastMapIndex = randomIndex;
        var map = Instantiate(_mapPrefabs[randomIndex], transform.position, Quaternion.identity, _mapParent.transform);
    }

    public void ResetPosition()
    {
        transform.position = _initPosition;
    }
    
    public void ClearMap()
    {
        foreach (Transform child in _mapParent.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
