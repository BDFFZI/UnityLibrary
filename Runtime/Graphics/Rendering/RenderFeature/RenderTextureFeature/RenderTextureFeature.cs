using UnityEngine;

public class RenderTextureFeature<TPass> : RenderFeature<TPass>
    where TPass : RenderTexturePass, new()
{
    [SerializeField] RenderTexture renderTarget;
    [SerializeField] bool preview;

    protected override TPass CreatePass()
    {
        TPass pass = base.CreatePass();

        if (renderTarget != null)
            pass.RenderTextureProvider = new AssetTextureProvider(renderTarget);
        else
            pass.RenderTextureProvider = new CameraTextureProvider("_" + GetType().Name);
        return pass;
    }

    protected override void SetupPass(TPass pass)
    {
        base.SetupPass(pass);

        pass.Preview = preview;
    }
}
