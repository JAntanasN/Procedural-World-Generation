using Unity.Mathematics;
using UnityEngine;

public class Generator2D : MonoBehaviour
{
    public int2 size = new int2(30, 30);
    [Range(0, 1)]public float threshold = 0.5f;
    public GameObject tilePrefab;
    public float scale = 10;
    public float seed;
    
    float[,] map;
    
    void Start()
    {
        seed = UnityEngine.Random.Range(0, 100);
        Generate();
        Draw();
    }

    void Generate()
    {
        map = new float[size.x, size.y];
        for(int y=0; y<size.y; y++)
        {
            for(int x=0; x<size.x; x++)
            {
                var px = x / scale + seed;
                var py = y / scale + seed;
                var value = noise.snoise(new float2(px, py));
                map[x, y] = (value + 1) / 2; //change range from -1..1 to 0..1
            }
        }
    }

    void Draw()
    {
        for(int y=0; y<size.y; y++)
        {
            for(int x=0; x<size.x; x++)
            {
                var value = map[x, y];
                if(value > threshold)
                {
                    var tile = Instantiate(tilePrefab, new Vector3(x, 0, y), Quaternion.identity);
                    tile.transform.parent = transform;
                }
            }
        }
    }
}
