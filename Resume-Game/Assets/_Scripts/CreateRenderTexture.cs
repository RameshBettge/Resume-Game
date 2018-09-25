using UnityEngine;
using UnityEngine.UI;

public class CreateRenderTexture : MonoBehaviour
{
    [SerializeField]
    RawImage renderView;

    Camera cam;
    private void Awake()
    {
        cam = GetComponent<Camera>();
        Create();
    }

    public void Create()
    {
        RenderTexture newTex = new RenderTexture(Screen.width, Screen.height, -1);
        cam.targetTexture = newTex;
        renderView.texture = newTex;
    }
}
