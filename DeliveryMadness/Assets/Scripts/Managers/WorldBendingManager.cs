using UnityEngine;
using UnityEngine.Rendering;

[ExecuteAlways]
public class WorldBendingManager : MonoBehaviour
{
    private const string BENDING_FEATURE = "BENDING_ENABLED";

    private void OnEnable()
    {
        RenderPipelineManager.beginCameraRendering += OnBeginCameraRendering;
        RenderPipelineManager.endCameraRendering += OnEndCameraRendering;

    }

    private void OnDisable()
    {
        RenderPipelineManager.beginCameraRendering -= OnBeginCameraRendering;
        RenderPipelineManager.endCameraRendering -= OnEndCameraRendering;
    }
    private void Awake()
    {
        if(Application.isPlaying)
        {
            Shader.EnableKeyword(BENDING_FEATURE);
        }
        else
        {
            Shader.DisableKeyword(BENDING_FEATURE);
        }
    }

    private static void OnBeginCameraRendering (ScriptableRenderContext ctx, Camera cam)
    {
#if UNITY_EDITOR
        cam.cullingMatrix = Matrix4x4.Ortho(-99, 99, -99, 99, 0.001f, 500) * cam.worldToCameraMatrix;
#else
        cam.cullingMatrix = Matrix4x4.Ortho(-20, 20, -5, 15, 0.001f, 80) * cam.worldToCameraMatrix;
#endif
    }

    private static void OnEndCameraRendering(ScriptableRenderContext ctx, Camera cam)
    {
        cam.ResetCullingMatrix();
    }
}
