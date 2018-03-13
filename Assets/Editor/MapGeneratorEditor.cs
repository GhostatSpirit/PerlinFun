using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor {

    public override void OnInspectorGUI()
    {
        MapGenerator mapGen = (MapGenerator)target;

        if(DrawDefaultInspector() && mapGen.autoUpdate)
        {
            mapGen.GenerateMap();
        }
        

        if (GUILayout.Button("Generate Map")){
            mapGen.GenerateMap();
        }

        if (GUILayout.Button("Generate Objects"))
        {
            mapGen.GenerateObjects();
        }
    }
}