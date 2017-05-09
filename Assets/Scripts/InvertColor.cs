using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Rendering;

public class InvertColor : MonoBehaviour
{
    public Shader m_IndertShader;
    private Material m_Material;

    public Camera cam;
    private Dictionary<Camera, CommandBuffer> m_Cameras = new Dictionary<Camera, CommandBuffer>();

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // Remove command buffers from all cameras we added into
    private void Cleanup()
    {
        foreach (var cam in m_Cameras)
        {
            if (cam.Key)
            {
                cam.Key.RemoveCommandBuffer(CameraEvent.AfterSkybox, cam.Value);
            }
        }
        m_Cameras.Clear();
        Object.DestroyImmediate(m_Material);
    }

    public void OnEnable()
    {
        Cleanup();
    }

    public void OnDisable()
    {
        Cleanup();
    }

    public void OnWillRenderObject()
    {
        CommandBuffer buf = new CommandBuffer();
        buf.name = "Invert colors";

        if (!m_Material)
        {
            m_Material = new Material(m_IndertShader);
            m_Material.hideFlags = HideFlags.HideAndDontSave;
        }

        var cam = Camera.current;
        if (!cam)
            return;

        m_Cameras[cam] = buf;

        // copy screen into temporary RT
        int screenCopyID = Shader.PropertyToID("_ScreenCopyTexture");
        buf.GetTemporaryRT(screenCopyID, -1, -1, 0, FilterMode.Bilinear);
        buf.Blit(BuiltinRenderTextureType.CurrentActive, screenCopyID);

        int invertID = Shader.PropertyToID("_Temp1");
        buf.GetTemporaryRT(invertID, -1, -1, 0, FilterMode.Bilinear);

        buf.Blit(screenCopyID, invertID);
        buf.ReleaseTemporaryRT(screenCopyID);
        buf.Blit(screenCopyID, invertID, m_Material);

        buf.SetGlobalTexture("_InvertColorTexture", invertID);

        cam.AddCommandBuffer(CameraEvent.AfterEverything, buf);
    }
}
