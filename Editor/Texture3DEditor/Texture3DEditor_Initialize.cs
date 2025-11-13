using Sirenix.OdinInspector;
using Unity.Burst;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace BDXK
{
    public partial class Texture3DEditor
    {
        enum InitializeType
        {
            Clear,
            Box,
            Sphere,
            Mesh,
        }

        [BurstCompile]
        struct InitializeComputer : IComputer
        {
            public ComputeData ComputeData { get; set; }

            public InitializeType type;
            public float value;

            public void Execute(int index)
            {
                ComputeData.GetPosition(index, out int3 position, out float3 _);

                float mask = 0;
                switch (type)
                {
                    case InitializeType.Clear:
                    {
                        mask = 1;
                        break;
                    }
                    case InitializeType.Box:
                    {
                        bool isInside = position.x != 0 && position.x != ComputeData.width - 1 &&
                                        position.y != 0 && position.y != ComputeData.height - 1 &&
                                        position.z != 0 && position.z != ComputeData.depth - 1;
                        mask = isInside ? 1 : 0;
                        break;
                    }
                    case InitializeType.Sphere:
                    {
                        float3 center = new float3((ComputeData.width - 1) / 2.0f, (ComputeData.height - 1) / 2.0f, (ComputeData.depth - 1) / 2.0f);
                        float radius = (math.min(math.min(ComputeData.width, ComputeData.height), ComputeData.depth) - 1) / 2.0f;
                        float distance = math.distance(position, center);
                        mask = 1 - math.saturate(distance / radius);
                        break;
                    }
                    default:
                        break;
                }

                ComputeData.SetValue(index, value * mask);
            }
        }

        [FoldoutGroup("Initialize", expanded: false)] [SerializeField] [Range(0, 1)]
        float initializeValue = 1;
        [FoldoutGroup("Initialize", expanded: false)] [SerializeField]
        InitializeType initializeType;
        [FoldoutGroup("Initialize")] [SerializeField] [ShowIf("@initializeType == InitializeType.Mesh")]
        Mesh initializeMesh;
        [FoldoutGroup("Initialize", expanded: false)] [Button("Invoke")]
        new void Initialize()
        {
            if (initializeType == InitializeType.Mesh)
            {
                Texture3DUtility.MeshToWorld(canvas, initializeMesh);
                AssetDatabase.SaveAssets();
            }
            else
            {
                InitializeComputer computer = CreateComputer<InitializeComputer>();
                computer.type = initializeType;
                computer.value = initializeValue;
                ApplyComputer(computer);
            }
        }
    }
}
