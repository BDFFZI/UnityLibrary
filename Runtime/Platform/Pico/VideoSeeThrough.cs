using Unity.XR.PXR;
using UnityEngine;

public class VideoSeeThrough : MonoBehaviour
{
    void Start()
    {
        PxrLayerBlend blend = new PxrLayerBlend();
        blend.srcColor = PxrBlendFactor.PxrBlendFactorOne;
        blend.dstColor = PxrBlendFactor.PxrBlendFactorOneMinusSrcAlpha;
        PXR_Plugin.Render.UPxr_SetLayerBlend(true, blend);
        PXR_Manager.EnableVideoSeeThrough = true;
        PXR_MixedReality.EnableVideoSeeThroughEffect(true);
    }
}
