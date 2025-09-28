using Unity.XR.PXR;
using UnityEngine;
using UnityEngine.XR.Hands;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

[DefaultExecutionOrder(-10)]
public class GesturesSystem : MonoBehaviour
{
    public static XRHandTrackingEvents LeftHandJoints => instance.leftHandTracking;
    public static XRHandTrackingEvents RightHandJoints => instance.rightHandTracking;
    public static bool IsRecognizing => instance.isRecognizing;

    static GesturesSystem instance;

    [SerializeField] XRHandTrackingEvents leftHandTracking;
    [SerializeField] XRHandTrackingEvents rightHandTracking;
    [SerializeField] XRInputModalityManager inputModalityManager;

    bool isRecognizing;

    void Awake()
    {
        instance = this;
        isRecognizing = false;
        inputModalityManager.trackedHandModeStarted.AddListener(() => isRecognizing = true);
        inputModalityManager.trackedHandModeEnded.AddListener(() => isRecognizing = false);
    }
}
