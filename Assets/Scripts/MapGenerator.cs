using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapGenerator : MonoBehaviour
{

    public enum DrawMode { NoiseMap, ColorMap, Mesh };
    public DrawMode drawMode;

    public int mapWidth;
    public int mapHeight;
    public float scale;

    public int octaves;
    [Range(0, 1)]
    public float persistence;
    public float lacunarity;

    public int seed;
    public Vector2 offset;

    public float meshHeightMuliplier = 10f;
    public AnimationCurve meshHeightCurve;

    public bool autoUpdate;

    public MapData mapData;

    public TerrainType[] regions;


    List<Vector2> emitted = new List<Vector2>();
    public void GenerateMap()
    {
        Noise.OctaveInfo oinfo = new Noise.OctaveInfo(octaves, persistence, lacunarity, seed);

        float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, scale, offset, oinfo);
        mapData.heightMap = (float[,])noiseMap.Clone();

        Color[] colorMap = new Color[mapWidth * mapHeight];
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                float currentHeight = noiseMap[x, y];
                for (int i = 0; i < regions.Length; i++)
                {
                    if (currentHeight <= regions[i].height)
                    {
                        colorMap[y * mapWidth + x] = regions[i].color;
                        break;
                    }
                }
            }
        }

        MapDisplay display = FindObjectOfType<MapDisplay>();


        Texture2D texture = new Texture2D(mapWidth, mapHeight);
        if(drawMode == DrawMode.NoiseMap)
        {
            texture = TextureGenerator.TextureFromHeightMap(noiseMap);
            display.DrawTexture(texture);
        }
        else if (drawMode == DrawMode.ColorMap)
        {
            texture = TextureGenerator.TextureFromColorMap(colorMap, mapWidth, mapHeight);
            display.DrawTexture(texture);
        }
        else if (drawMode == DrawMode.Mesh)
        {
            texture = TextureGenerator.TextureFromColorMap(colorMap, mapWidth, mapHeight);
            display.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, meshHeightMuliplier, meshHeightCurve),
               texture);

            
            
        }

        
    }

    public void GenerateObjects()
    {
        GetComponent<StructureManager>().CreateStructures(mapData.heightMap, meshHeightMuliplier, meshHeightCurve);
    }

    private void OnEnable()
    {
        seed = Random.Range(-114514, 114514);
        GenerateMap();
        GenerateObjects();
    }

    void OnValidate()
    {
        if (mapWidth < 1)
        {
            mapWidth = 1;
        }
        if (mapHeight < 1)
        {
            mapHeight = 1;
        }
        if (lacunarity < 1)
        {
            lacunarity = 1;
        }
        if (octaves < 0)
        {
            octaves = 0;
        }
        if(scale <= 0f)
        {
            scale = 0.1f;
        }
    }

    
}

[System.Serializable]
public struct TerrainType
{
    public string name;
    public float height;
    public Color color;
}

public struct MapData
{
    public float[,] heightMap;    
}