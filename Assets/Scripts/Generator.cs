using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

public class Generator : MonoBehaviour
{
    public int2 size = new int2(30, 30);
    public int height = 10;
    public float seed;
    public float scale = 10;
    public float humidityScale = 10;
    public float temperatureScale = 10;
    
    [Header("Biomes")]
    public GameObject grassPrefab;
    public GameObject dirtPrefab;
    public GameObject sandPrefab;
    public GameObject icePrefab;
    public GameObject waterPrefab;
    
    private float[,] map;
    private float[,] tempMap;
    private float[,] humMap;

    void Start()
    {
        map = Generate(scale);
        tempMap = Generate(temperatureScale);
        humMap = Generate(humidityScale);
        
        Draw();
    }


    float[,] Generate(float _scale)
    {
        var _map = new float[size.x, size.y];
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                var px = x / _scale + seed;
                var py = y / _scale + seed;
                var value = noise.snoise(new float2(px, py));
                _map[x, y] = (value + 1) / 2; //change range from -1..1 to 0..1
            }
        }

        return _map;
    }

    void Draw()
    {
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                var value = map[x, y];
                var py = Mathf.Floor(value * height);
                
                if(humMap[x, y] >= 0.5f && tempMap[x, y] >= 0.5f) //humid and hot
                    Fill(x, (int)py, y, dirtPrefab, grassPrefab);
                else if(humMap[x, y] >= 0.5f && tempMap[x, y] < 0.5f) //humid and cold
                    Fill(x, (int)py, y, waterPrefab);
                else if(humMap[x, y] < 0.5f && tempMap[x, y] >= 0.5f) //dry and hot
                    Fill(x, (int)py, y, sandPrefab);
                else if(humMap[x, y] < 0.5f && tempMap[x, y] < 0.5f) //dry and cold
                    Fill(x, (int)py, y, icePrefab);
            }
        }
    }

    void Fill(int x, int y, int z, GameObject dirt, GameObject grass = null)
    {
        if(grass != null)
            Instantiate(grass, new Vector3(x, y, z), Quaternion.identity, transform);
        else
            Instantiate(dirt, new Vector3(x, y, z), Quaternion.identity, transform);
        
        for (int py = y - 1; py >= 0; py--)
        {
            Instantiate(dirt, new Vector3(x, py, z), Quaternion.identity, transform);
        }
    }
}