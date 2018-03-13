using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class StructureManager : MonoBehaviour {

    public float radius = 5f;
    public int seed = 0;

    public Transform terrain;

    public List<StructureInfo> structures;
    public List<Vector2> emitted;

    public void CreateStructures(float[,] heightMap, float meshHeightMultiplier, AnimationCurve meshHeightCurve)
    {
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);

        var tempList = terrain.Cast<Transform>().ToList();
        foreach (var child in tempList)
        {
            DestroyImmediate(child.gameObject);
        }

        emitted = StructureGenerator.GenerateStructures(heightMap, radius, seed);
        
        foreach(Vector2 localPos in emitted)
        {
            Vector2Int gridPos = Noise.LocalToGrid(localPos, width, height);
            float value = heightMap[gridPos.x, gridPos.y];

            float multipliedHeight = value * meshHeightCurve.Evaluate(value) * meshHeightMultiplier;

            foreach (StructureInfo sinfo in structures)
            {
                if(value >= sinfo.minHeight && value <= sinfo.maxHeight)
                {
                    GameObject structure = Instantiate(sinfo.prefab, terrain);
                    structure.transform.localPosition = new Vector3(localPos.x, multipliedHeight, -localPos.y);
                    break;
                }
            }
        }

    }



    private void OnDrawGizmos()
    {
        foreach (Vector2 sample in emitted)
        {
            // Gizmos.DrawWireSphere(new Vector3(sample.x, 0f, sample.y), 1f);
        }
    }
}

[System.Serializable]
public struct StructureInfo
{
    public GameObject prefab;
    [Range(0f, 1f)]
    public float minHeight;
    [Range(0f, 1f)]
    public float maxHeight;
}