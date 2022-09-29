using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldGenerator : MonoBehaviour
{
    [SerializeField]
    GameObject Cube;
    [SerializeField]
    Vector2Int dimension;
    void Start()
    {
        
    }

    public void GeneratePrefaps()
    {
        Transform paranetObj = new GameObject("Terran").transform;
        var terrain = paranetObj.gameObject.AddComponent<Terran>();
        terrain.Grid = new TerranField[dimension.x, dimension.y];
        Collider col = Cube.GetComponent<Collider>() ?? Cube.AddComponent<Collider>();
        var size = col.bounds.size;
  
        for (int i = 0; i < dimension.x; i++)
        {
            for (int j = 0; j < dimension.y; j++)
            {
                var field = Instantiate(Cube, new Vector3(size.x * i, 0, size.z * j), Quaternion.identity, paranetObj);
                field.name = String.Format("field {0}x{1}", i, j);

                var ter = field.AddComponent<TerranField>();
                terrain.Grid[i, j] = ter;
            }
        }
    }
}
