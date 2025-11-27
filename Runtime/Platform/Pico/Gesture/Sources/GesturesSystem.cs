using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Hands;
using UnityEngine.XR.Interaction.Toolkit.Inputs;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

[DefaultExecutionOrder(-10)]
public class GesturesSystem : MonoBehaviour
{
    public static GesturesSystem Instance { get; private set; }

    public XRHandTrackingEvents LeftHandJoints => leftHandTracking;
    public XRHandTrackingEvents RightHandJoints => rightHandTracking;
    public bool IsRecognizing => isRecognizing;

    public Vector3 GetHandPosition(InteractorHandedness handedness)
    {
        return handedness switch {
            InteractorHandedness.Left => leftHandPosition,
            InteractorHandedness.Right => rightHandPosition,
            InteractorHandedness.None => throw new NotSupportedException(),
            _ => throw new ArgumentOutOfRangeException(nameof(handedness), handedness, null)
        };
    }

    public void OnLeftHandMove(InputAction.CallbackContext context)
    {
        leftHandPosition = context.ReadValue<Vector3>();
    }
    public void OnRightHandMove(InputAction.CallbackContext context)
    {
        rightHandPosition = context.ReadValue<Vector3>();
    }


    [SerializeField] XRHandTrackingEvents leftHandTracking;
    [SerializeField] XRHandTrackingEvents rightHandTracking;
    [SerializeField] XRInputModalityManager inputModalityManager;

    bool isRecognizing;
    Vector3 leftHandPosition;
    Vector3 rightHandPosition;

    void Awake()
    {
        Instance = this;
        isRecognizing = false;
        inputModalityManager.trackedHandModeStarted.AddListener(() => isRecognizing = true);
        inputModalityManager.trackedHandModeEnded.AddListener(() => isRecognizing = false);
    }
}
