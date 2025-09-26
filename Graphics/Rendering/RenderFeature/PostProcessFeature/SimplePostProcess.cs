using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SimplePostProcess : PostProcessFeature<SimplePostProcessPass>
{
    [SerializeField] Material material;

    protected override void SetupPass(SimplePostProcessPass pass)
    {
        base.SetupPass(pass);
        pass.MaterialAsset = material;
    }
}

public class SimplePostProcessPass : PostProcessPass
{
    public override Material Material => MaterialAsset;
    public Material MaterialAsset { get; set; }
}
