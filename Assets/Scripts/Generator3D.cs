using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Generator3D : MonoBehaviour
{
    public int3 size = new int3(50, 50, 50);
    public float scale = 100;
    public float seed;
    [Range(0, 1)] public float threshold = 0.5f;
    public GameObject tilePrefab;
    
    float[,,] map;
    
    void Start()
    {
        seed = Random.Range(0, 1000);
        Generate();
        Draw();
    }

    void Generate()
    {
        map = new float[size.x, size.y, size.z];
        
        for(int z=0; z<size.z; z++)
        {
            for(int y=0; y<size.y; y++)
            {
                for(int x=0; x<size.x; x++)
                {
                    var px = x / scale + seed;
                    var py = y / scale + seed;
                    var pz = z / scale + seed;
                    var value = noise.snoise(new float3(px, py, pz));
                    map[x, y, z] = (value + 1) / 2; //change range from -1..1 to 0..1
                }
            }
        }
    }

    void Draw()
    {
        for(int z=0; z<size.z; z++)
        {
            for(int y=0; y<size.y; y++)
            {
                for(int x=0; x<size.x; x++)
                {
                    var value = map[x, y, z];
                    if(value > threshold)
                    {
                        Instantiate(tilePrefab, new Vector3(x, y, z), Quaternion.identity, transform);
                    }
                }
            }
        }
    }
}
