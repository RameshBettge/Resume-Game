using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateRenderTexture : MonoBehaviour
{
    private void Awake()
    {
        Camera cam = GetComponent<Camera>();
        cam.targetTexture = new RenderTexture(Screen.width, Screen.height, -1);
    }
}
