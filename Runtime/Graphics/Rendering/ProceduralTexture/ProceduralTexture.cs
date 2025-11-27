using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ProceduralTexture : RenderFeature<ProceduralTexturePass>
{
    public IProceduralTextureCanvas Canvas { get => canvas; set => canvas = value; }
    public override RenderPassEvent RenderQueue => renderQueue;
    public override int RenderOrder => renderOrder;

    [SerializeField] RenderPassEvent renderQueue = RenderPassEvent.BeforeRenderingTransparents;
    [SerializeField] int renderOrder;
    [SerializeField] IProceduralTextureCanvas canvas;
    [SerializeField] IProceduralTextureProcess[] processes;
    [SerializeField] int executeCount = -1;
    [SerializeField] IProceduralTextureSource source;

    protected override void SetupPass(ProceduralTexturePass pass)
    {
        if (executeCount != -1 && Application.isPlaying)
        {
            executeCount--;
            if (executeCount == 0)
            {
                UniTask.Create(async () => {
                    await UniTask.DelayFrame(1);
                    gameObject.SetActive(false);
                });
            }
        }

        base.SetupPass(pass);
        pass.Setup(canvas, source, processes);
    }

    void OnValidate()
    {
        if (processes == null)
            processes = Array.Empty<IProceduralTextureProcess>();
    }
}

public class ProceduralTexturePass : RenderPass
{
    public void Setup(IProceduralTextureCanvas target, IProceduralTextureSource source, IProceduralTextureProcess[] processes)
    {
        this.target = target;
        this.source = source;
        this.processes = processes;
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        if (renderingData.cameraData.isPreviewCamera)
            return;
        if (target == null || target.RequestTexture(context, ref renderingData) == false)
            return;

        RenderTextureDescriptor descriptor = target.TextureDescriptor;
        descriptor.depthBufferBits = 0; //否则创建出来的是深度纹理
        RenderingUtils.ReAllocateIfNeeded(ref swapTexture, descriptor, target.TextureFilterMode, target.TextureWrapMode, name: "_SwapTexture");

        CommandBuffer cmd = CommandBufferPool.Get(target.TextureName);

        RTHandle sourceTexture = target.Texture;
        RTHandle destinationTexture = swapTexture;

        if (source != null)
        {
            cmd.Blit(source.Texture, destinationTexture);
            (sourceTexture, destinationTexture) = (destinationTexture, sourceTexture);
        }

        for (int processIndex = 0; processIndex < processes.Length; processIndex++)
        {
            if (processes[processIndex].enabled)
            {
                processes[processIndex].ProcessTexture(context, ref renderingData, target, cmd, sourceTexture, destinationTexture);
                (sourceTexture, destinationTexture) = (destinationTexture, sourceTexture);
            }
        }

        if (sourceTexture != target.Texture)
            cmd.Blit(sourceTexture, destinationTexture);
        cmd.SetGlobalTexture(target.TextureName, target.Texture);

        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
    }

    IProceduralTextureCanvas target;
    IProceduralTextureSource source;
    IProceduralTextureProcess[] processes;
    RTHandle swapTexture;

    public override void OnDestroy()
    {
        if (swapTexture != null)
            RTHandles.Release(swapTexture);

        base.OnDestroy();
    }
}
