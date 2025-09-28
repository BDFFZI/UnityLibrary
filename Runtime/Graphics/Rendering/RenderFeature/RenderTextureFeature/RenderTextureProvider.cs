using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public interface IRenderTextureProvider
{
    RTHandle RenderTarget { get; }
    void OnCameraSetup(ref RenderingData renderingData);
}

public class AssetTextureProvider : IRenderTextureProvider
{
    public AssetTextureProvider(RenderTexture renderTarget)
    {
        RenderTarget = RTHandles.Alloc(renderTarget);
    }

    public RTHandle RenderTarget { get; }
    public void OnCameraSetup(ref RenderingData renderingData) { }
}

public class CameraTextureProvider : IRenderTextureProvider
{
    public CameraTextureProvider(string renderTargetName)
    {
        renderTarget = RTHandles.Alloc(1, 1, filterMode: FilterMode.Point, wrapMode: TextureWrapMode.Clamp, name: renderTargetName);
    }

    ~CameraTextureProvider()
    {
        RTHandles.Release(RenderTarget);
    }

    public RTHandle RenderTarget => renderTarget;
    public float TextureScale { get; set; } = 1;

    RTHandle renderTarget;

    public void OnCameraSetup(ref RenderingData renderingData)
    {
        RenderTextureDescriptor textureDescriptor = renderingData.cameraData.cameraTargetDescriptor;
        textureDescriptor.depthBufferBits = 0;
        textureDescriptor.width = (int)(textureDescriptor.width * TextureScale);
        textureDescriptor.height = (int)(textureDescriptor.height * TextureScale);
        textureDescriptor.colorFormat = RenderTextureFormat.ARGBFloat;
        RenderingUtils.ReAllocateIfNeeded(ref renderTarget, textureDescriptor, RenderTarget.rt.filterMode, RenderTarget.rt.wrapMode, name: RenderTarget.name);
    }
}
