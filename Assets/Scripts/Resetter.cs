using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resetter : MonoBehaviour {

    public Renderer textureRender;

    public void OnStart()
    {
        textureRender = GetComponent<Renderer>();

        textureRender.sharedMaterial.mainTexture = Texture2D.whiteTexture;
    }
}
