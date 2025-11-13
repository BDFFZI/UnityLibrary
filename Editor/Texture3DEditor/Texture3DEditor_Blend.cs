using System;
using Sirenix.OdinInspector;
using Unity.Burst;
using Unity.Collections;
using UnityEngine;

namespace BDXK
{
    public partial class Texture3DEditor
    {
        enum BlendType
        {
            Multiply
        }

        [BurstCompile]
        struct BlendComputer : IComputer
        {
            public ComputeData ComputeData { get; set; }

            public BlendType blendType;
            public NativeArray<byte> blendData;

            public void Execute(int index)
            {
                float source = (float)blendData[index] / byte.MaxValue;
                float destination = ComputeData.GetValue(index);

                switch (blendType)
                {
                    case BlendType.Multiply:
                        ComputeData.SetValue(index, destination * source);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        [FoldoutGroup("Blend")]
        [SerializeField]
        BlendType blendType;
        [FoldoutGroup("Blend")]
        [SerializeField]
        Texture3D blendTex;
        [FoldoutGroup("Blend", expanded: false)]
        [Button("Invoke")]
        void Blend()
        {
            BlendComputer blendComputer = CreateComputer<BlendComputer>();
            blendComputer.blendType = blendType;
            blendComputer.blendData = blendTex.GetPixelData<byte>(0);
            ApplyComputer(blendComputer);
        }
    }
}
