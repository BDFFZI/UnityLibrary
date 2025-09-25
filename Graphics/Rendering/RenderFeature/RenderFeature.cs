using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public interface IRenderFeature
{
    RenderPass SetupPass();
}

[ExecuteAlways]
public class RenderFeature<TPass> : SerializedMonoBehaviour, IRenderFeature
    where TPass : RenderPass, new()
{
    public RenderFeatureSystem RenderFeatureSystem => renderFeatureSystem;

    public virtual RenderPassEvent RenderQueue => RenderPassEvent.BeforeRenderingTransparents;
    public virtual int RenderOrder => 0;

    protected virtual TPass CreatePass()
    {
        return new TPass();
    }

    protected virtual void SetupPass(TPass pass)
    {
        //Null
    }

    protected virtual void OnEnable()
    {
        renderFeatureSystem = GetComponentInParent<RenderFeatureSystem>();
        if (renderFeatureSystem == null)
            throw new NullReferenceException("RenderFeature can only be attached to object that contain RenderFeatureSystem!");

        renderPass = CreatePass();
        renderPass.renderPassEvent = RenderQueue;
        renderPass.OnAwake();
        renderFeatureSystem.AddRenderPass(RenderOrder, this);
    }

    protected virtual void OnDisable()
    {
        renderFeatureSystem.RemoveRenderPass(RenderOrder, this);
        renderPass.OnDestroy();
        renderPass = null;
    }

    [SerializeField] [ReadOnly] RenderFeatureSystem renderFeatureSystem;
    TPass renderPass;

    public RenderPass SetupPass()
    {
        SetupPass(renderPass);
        return renderPass;
    }
}
