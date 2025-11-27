using Unity.XR.PICO.TOBSupport;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class PicoEnterpriseManager : MonoBehaviour
{
    void Awake()
    {
        PXR_Enterprise.InitEnterpriseService();
        PXR_Enterprise.BindEnterpriseService();
        Debug.Log("企业版初始化完毕");
    }
}
