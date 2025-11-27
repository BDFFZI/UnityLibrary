using Unity.XR.PICO.TOBSupport;
using UnityEngine;

[DefaultExecutionOrder(-10)]
public class PupilDistanceManager : MonoBehaviour
{
    public float PupilDistance
    {
        get => pupilDistance;
        set
        {
            pupilDistance = value;
            PXR_Enterprise.SetIPD(pupilDistance, i => { Debug.Log(i); });
        }
    }

    [SerializeField] float pupilDistance = 65;

    void Start()
    {
        PupilDistance = pupilDistance;
    }
}
