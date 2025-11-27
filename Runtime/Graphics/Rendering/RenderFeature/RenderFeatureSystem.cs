using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[ExecuteAlways]
[DefaultExecutionOrder(-10)]
public class RenderFeatureSystem : MonoBehaviour
{
    public Camera Camera { get => camera; set => camera = value; }

    public void AddRenderPass(int priority, IRenderFeature renderFeature)
    {
        if (renderFeatures.TryGetValue(priority, out List<IRenderFeature> renderPassList) == false)
        {
            renderPassList = new List<IRenderFeature>();
            renderFeatures.Add(priority, renderPassList);
        }

        renderPassList.Add(renderFeature);
    }

    public void RemoveRenderPass(int priority, IRenderFeature renderFeature)
    {
        renderFeatures[priority].Remove(renderFeature);
    }

    [SerializeField] new Camera camera;

    UniversalAdditionalCameraData cameraData;
    SortedList<int, List<IRenderFeature>> renderFeatures;

    void OnEnable()
    {
        if (camera != null)
            cameraData = camera.GetComponent<UniversalAdditionalCameraData>();
        renderFeatures = new SortedList<int, List<IRenderFeature>>();

        RenderPipelineManager.beginCameraRendering -= OnBeginCameraRendering;
        RenderPipelineManager.beginCameraRendering += OnBeginCameraRendering;
    }

    void OnDisable()
    {
        RenderPipelineManager.beginCameraRendering -= OnBeginCameraRendering;
    }

    void OnBeginCameraRendering(ScriptableRenderContext renderContext, Camera camera)
    {
        if (this.camera != null)
        {
            if (camera != this.camera)
                return;
        }
        else
        {
            cameraData = camera.GetUniversalAdditionalCameraData();
        }

        foreach (List<IRenderFeature> renderFeatureList in renderFeatures.Values)
        {
            foreach (IRenderFeature renderFeature in renderFeatureList)
            {
                RenderPass pass = renderFeature.SetupPass();
                cameraData.scriptableRenderer.EnqueuePass(pass);
            }
        }
    }
}
