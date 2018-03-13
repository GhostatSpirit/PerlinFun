using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
[AddComponentMenu("Procedural Terrain/Terrain Meta")]
public class TerrainMeta : ScriptableObject, ISerializationCallbackReceiver {

    public float[,] noiseMap;
    public Texture2D texture;

    [SerializeField]
    [HideInInspector]
    byte[] rawTextureData;
    [SerializeField]
    [HideInInspector]
    int width;
    [SerializeField]
    [HideInInspector]
    int height;

    public void OnAfterDeserialize()
    {
        if(rawTextureData != null && rawTextureData.Length > 0)
        {
            texture.LoadRawTextureData(rawTextureData);
        }
    }

    public void OnBeforeSerialize()
    {
        if(!texture)
        {
            rawTextureData = new byte[0];
            width = 0;
            height = 0;
        } else
        {
            rawTextureData = texture.GetRawTextureData();
            width = texture.width;
            height = texture.height;
        }
    }
}
