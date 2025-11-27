using System;
using System.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.XR.PICO.TOBSupport;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

[DefaultExecutionOrder(-10)]
public class PicoCameraTexture : MonoBehaviour
{
    public Texture2D Texture => texture;

    [SerializeField] int width = 1024;
    [SerializeField] int height = 1024;

    Texture2D texture;
    IntPtr nativePtr;

    unsafe void Awake()
    {
        texture = new Texture2D(width, height, GraphicsFormat.R8G8B8A8_UNorm, TextureCreationFlags.None);
        nativePtr = (IntPtr)texture.GetRawTextureData<byte>().GetUnsafePtr();
    }

    void Start()
    {
        IEnumerator Enumerator()
        {
            yield return new WaitForSeconds(1);
            PXR_Enterprise.SetCameraFrameBufferfor4U(width, height, ref nativePtr, ImageAvailable);
            if (PXR_Enterprise.StartGetImageDatafor4U(PXRCaptureRenderMode.PXRCapture_RenderMode_LEFT, width, height))
                Debug.Log("开始接收相机画面");
        }

        PXR_Enterprise.OpenCameraAsyncfor4U(ret => {
            Debug.Log("相机打开状态：" + ret);
            StartCoroutine(Enumerator());
        });
    }

    void ImageAvailable(Frame obj)
    {
        texture.Apply();
    }
}
