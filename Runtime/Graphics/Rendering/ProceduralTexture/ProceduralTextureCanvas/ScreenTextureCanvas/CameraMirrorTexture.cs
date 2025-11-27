using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[ExecuteAlways]
public class CameraMirrorTexture : ScreenRenderTextureCanvas
{
    [SerializeField] [Required] Transform plane;

    List<ShaderTagId> shaderTags;

    void OnEnable()
    {
        shaderTags = new List<ShaderTagId>() {
            new ShaderTagId("SRPDefaultUnlit"),
            new ShaderTagId("UniversalForward"),
            new ShaderTagId("UniversalForwardOnly")
        };
    }

    public override bool RequestTexture(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        if (plane == null)
            return false;

        return base.RequestTexture(context, ref renderingData);
    }

    protected override void InitializeTexture(ScriptableRenderContext context, ref RenderingData renderingData, CommandBuffer cmd)
    {
        context.Submit(); //确保之前的渲染已完成，否则会受SetupCameraProperties干扰

        Camera camera = renderingData.cameraData.camera;

        //计算镜像矩阵用于反转物体
        float3 n = plane.up;
        float3 p = plane.position;
        float d = -math.dot(n, p);
        float4x4 mirrorMatrix = new float4x4(
            1 - 2 * n.x * n.x, -2 * n.y * n.x, -2 * n.z * n.x, -2 * n.x * d,
            -2 * n.x * n.y, 1 - 2 * n.y * n.y, -2 * n.z * n.y, -2 * n.y * d,
            -2 * n.x * n.z, -2 * n.y * n.z, 1 - 2 * n.z * n.z, -2 * n.z * d,
            0, 0, 0, 1
        );
        camera.worldToCameraMatrix = math.mul(camera.worldToCameraMatrix, mirrorMatrix);

        //计算斜截视锥体以去除水下物体
        float3 nVS = camera.worldToCameraMatrix.MultiplyVector(n).normalized;
        float3 pVS = camera.worldToCameraMatrix.MultiplyPoint(p);
        float4 c = new float4(nVS.x, nVS.y, nVS.z, -math.dot(nVS, pVS));
        float4x4 projectionMatrix = camera.projectionMatrix;
        float4x4 inverseProjectionMatrix = math.inverse(projectionMatrix);
        float4 q = math.mul(inverseProjectionMatrix, new float4(math.sign(c.x), math.sign(c.y), 1, 1));
        float4 m3 = -2 * q.z / math.dot(q, c) * c + new float4(0, 0, 1, 0);
        projectionMatrix.c0[2] = m3.x;
        projectionMatrix.c1[2] = m3.y;
        projectionMatrix.c2[2] = m3.z;
        projectionMatrix.c3[2] = m3.w;
        camera.projectionMatrix = projectionMatrix;

        //赋值相机参数
        context.SetupCameraProperties(camera);

        //因为反射矩阵会导致面变逆时针，因此反转面朝向设定
        GL.invertCulling = true;

        {
            base.InitializeTexture(context, ref renderingData, cmd); //SetupCameraProperties会导致渲染目标重置，故追加重设渲染目标的命令。

            //渲染
            camera.TryGetCullingParameters(out ScriptableCullingParameters cullingParameters);
            CullingResults cullingResults = context.Cull(ref cullingParameters);
            DrawingSettings drawingSettings = RenderingUtils.CreateDrawingSettings(shaderTags, ref renderingData, SortingCriteria.CommonOpaque);
            FilteringSettings filteringSettings = new FilteringSettings(RenderQueueRange.opaque);
            context.DrawRenderers(cullingResults, ref drawingSettings, ref filteringSettings);

            //SetupCameraProperties内部分功能会引用相机并延迟执行，故必须显示在相机复原前渲染
            context.Submit();
        }

        //还原面朝向设定
        GL.invertCulling = false;

        //还原相机数据
        camera.ResetWorldToCameraMatrix();
        camera.ResetProjectionMatrix();
        context.SetupCameraProperties(camera);
    }
}
