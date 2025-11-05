using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Hands;
using UnityEngine.XR.Hands.Gestures;

public class GesturesRecognizer : MonoBehaviour
{
    [Flags]
    enum Handedness
    {
        Left = 0b_01,
        Right = 0b_10,
    }

    [Serializable]
    struct Gestures
    {
        public Handedness handedness;
        public ScriptableObject jointsCondition;
    }

    public string GestureName => gestureName;
    public string GestureDescription => gestureDescription;
    public bool IsDisplaying => isDisplaying;

    public UnityEvent OnBeginDisplaying => onBeginDisplaying;
    public UnityEvent OnEndDisplaying => onEndDisplaying;

    [SerializeField] string gestureName;
    [SerializeField] string gestureDescription;
    [SerializeField] Gestures[] gestures;
    [SerializeField] float toleranceTime = 0.5f;
    [SerializeField] UnityEvent onBeginDisplaying;
    [SerializeField] UnityEvent onEndDisplaying;

    XRHandShape[] handShape;
    XRHandPose[] handPose;
    bool leftHandIsDisplaying;
    bool rightHandIsDisplaying;
    bool isDisplaying;
    float unDisplayingTime;
    bool lastIsDisplaying;

    void Awake()
    {
        handShape = new XRHandShape[gestures.Length];
        handPose = new XRHandPose[gestures.Length];

        for (int i = 0; i < gestures.Length; i++)
        {
            handShape[i] = gestures[i].jointsCondition as XRHandShape;
            handPose[i] = gestures[i].jointsCondition as XRHandPose;
        }
    }

    void OnEnable()
    {
        GesturesSystem.Instance.LeftHandJoints.jointsUpdated.AddListener(OnJointsUpdated);
        GesturesSystem.Instance.RightHandJoints.jointsUpdated.AddListener(OnJointsUpdated);
    }

    void OnDisable()
    {
        GesturesSystem.Instance.LeftHandJoints.jointsUpdated.RemoveListener(OnJointsUpdated);
        GesturesSystem.Instance.RightHandJoints.jointsUpdated.RemoveListener(OnJointsUpdated);
    }

    void Update()
    {
        lastIsDisplaying = isDisplaying;

        if (leftHandIsDisplaying | rightHandIsDisplaying)
        {
            isDisplaying = true;
            unDisplayingTime = 0;
        }
        else
        {
            if (isDisplaying && unDisplayingTime < toleranceTime)
                unDisplayingTime += Time.deltaTime;
            else
                isDisplaying = false;
        }

        if (lastIsDisplaying && isDisplaying == false)
            onEndDisplaying.Invoke();
        if (lastIsDisplaying == false && isDisplaying)
            onBeginDisplaying.Invoke();
    }

    void OnJointsUpdated(XRHandJointsUpdatedEventArgs arg0)
    {
        if (arg0.hand.handedness == UnityEngine.XR.Hands.Handedness.Left)
        {
            int i;
            for (i = 0; i < gestures.Length; i++)
            {
                if ((gestures[i].handedness & Handedness.Left) != 0)
                {
                    bool result = CheckConditions(arg0, handShape[i], handPose[i]);
                    if (result)
                    {
                        leftHandIsDisplaying = true;
                        break;
                    }
                }
            }
            if (i == gestures.Length)
                leftHandIsDisplaying = false;
        }

        if (arg0.hand.handedness == UnityEngine.XR.Hands.Handedness.Right)
        {
            int i;
            for (i = 0; i < gestures.Length; i++)
            {
                if ((gestures[i].handedness & Handedness.Right) != 0)
                {
                    bool result = CheckConditions(arg0, handShape[i], handPose[i]);
                    if (result)
                    {
                        rightHandIsDisplaying = true;
                        break;
                    }
                }
            }
            if (i == gestures.Length)
                rightHandIsDisplaying = false;
        }
    }
    bool CheckConditions(XRHandJointsUpdatedEventArgs eventArgs, XRHandShape handShape, XRHandPose handPose)
    {
        if (handShape != null && handShape.CheckConditions(eventArgs))
            return true;
        if (handPose != null && handPose.CheckConditions(eventArgs))
            return true;
        return false;
    }
}
