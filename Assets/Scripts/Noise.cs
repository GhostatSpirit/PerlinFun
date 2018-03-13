using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noise {
    public struct OctaveInfo
    {
        public int octaves;
        public float persistence;
        public float lacunarity;
        public int seed;

        public OctaveInfo(int _octaves, float _persistence, float _lacunarity)
        {
            octaves = _octaves;
            persistence = _persistence;
            lacunarity = _lacunarity;
            seed = 0;
        }

        public OctaveInfo(int _octaves, float _persistence, float _lacunarity, int _seed)
        {
            octaves = _octaves;
            persistence = _persistence;
            lacunarity = _lacunarity;
            seed = _seed;
        }
    }

    
    public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, float scale)
    {
        float[,] noiseMap = new float[mapWidth, mapHeight];

        if(scale <= 0)
        {
            scale = 0.001f;
        }

        for(int y = 0; y < mapHeight; ++y)
        {
            for(int x = 0; x < mapWidth; x++)
            {
                float sampleX = x / scale;
                float sampleY = y / scale;

                float perlineValue = Mathf.PerlinNoise(sampleX, sampleY);

                noiseMap[x, y] = perlineValue;
            }
        }

        return noiseMap;
    }

    public static float[,] GenerateNoiseMap
        (int mapWidth, int mapHeight, float scale, Vector2 offset, OctaveInfo oinfo)
    {

        float[,] noiseMap = new float[mapWidth, mapHeight];

        System.Random random = new System.Random(oinfo.seed);
        Vector2[] octaveOffsets = new Vector2[oinfo.octaves];
        for(int i = 0; i < oinfo.octaves; ++i)
        {
            float offsetX = random.Next(-10000, 10000) + offset.x;
            float offsetY = random.Next(-10000, 10000) - offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }



        if (scale <= 0)
        {
            scale = 0.001f;
        }

        float halfWidth = mapWidth / 2f;
        float halfHeight = mapHeight / 2f;

        float maxNoise = float.MinValue;
        float minNoise = float.MaxValue;

        float amp = 1f, freq = 1f;

        for (int i = 0; i < oinfo.octaves; ++i)
        {
            for (int y = 0; y < mapHeight; ++y)
            {
                for (int x = 0; x < mapWidth; x++)
                {

                    float sampleX = (x - halfWidth + octaveOffsets[i].x) / scale * freq;
                    float sampleY = (y - halfHeight + octaveOffsets[i].y) / scale * freq ;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY);
                    perlinValue = perlinValue * 2.0f - 1.0f;

                    float newNoise = noiseMap[x, y] + perlinValue * amp;
                    noiseMap[x, y] = newNoise;


                    if (newNoise > maxNoise) maxNoise = newNoise;
                    if (newNoise < minNoise) minNoise = newNoise;
                }
            }

            amp *= oinfo.persistence;
            freq *= oinfo.lacunarity;
        }

        for(int y = 0; y < mapHeight; ++y)
        {
            for(int x = 0; x < mapWidth; ++x)
            {
                noiseMap[x, y] = Mathf.InverseLerp(minNoise, maxNoise, noiseMap[x, y]);
            }
        }

        return noiseMap;
    }

    public static Vector2Int LocalToGrid(Vector2 localPos, int width, int height)
    {
        float halfWidth = width / 2.0f;
        float halfHeight = height / 2.0f;

        Vector2 offset = new Vector2(halfWidth, halfHeight);
        Vector2 gridPos = localPos + offset;

        return new Vector2Int(Mathf.RoundToInt(gridPos.x), Mathf.RoundToInt(gridPos.y));
    }
}
