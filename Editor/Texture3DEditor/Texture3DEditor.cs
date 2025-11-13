using Sirenix.OdinInspector.Editor;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace BDXK
{
    [CreateAssetMenu(menuName = "MarchingCubesBuilder")]
    public partial class Texture3DEditor : OdinEditorWindow
    {
        [SerializeField] Texture3D canvas;

        [MenuItem("Tools/Texture3D Editor")]
        static void CreateWindow()
        {
            GetWindow<Texture3DEditor>().Show();
        }

        struct ComputeData
        {
            public NativeArray<byte> canvas;
            public int width;
            public int height;
            public int depth;

            public void GetPosition(int index, out int3 position, out float3 uv)
            {
                position.z = index / (width * height);
                position.y = index % (width * height) / width;
                position.x = index % (width * height) % width;
                uv = position / new float3(width, height, depth);
            }
            public float GetValue(int index)
            {
                return (float)canvas[index] / byte.MaxValue;
            }
            public void SetValue(int index, float value)
            {
                canvas[index] = (byte)(value * byte.MaxValue);
            }
        }

        interface IComputer : IJobParallelFor
        {
            public ComputeData ComputeData { get; set; }
        }


        TComputer CreateComputer<TComputer>()
            where TComputer : struct, IComputer
        {
            TComputer computer = new TComputer();
            computer.ComputeData = new ComputeData() {
                canvas = canvas.GetPixelData<byte>(0),
                width = canvas.width,
                height = canvas.height,
                depth = canvas.depth
            };
            return computer;
        }
        void ApplyComputer<TComputer>(TComputer computer)
            where TComputer : struct, IComputer
        {
            computer.Schedule(computer.ComputeData.canvas.Length, 16).Complete();
            canvas.SetPixelData(computer.ComputeData.canvas, 0);
            canvas.Apply();
            AssetDatabase.SaveAssets();
        }
    }
}
