using UnityEngine;

public class PupilDistanceController : MonoBehaviour
{
    [SerializeField] PupilDistanceManager pupilDistanceManager;
    [SerializeField] Vector2 maxRange = new Vector2(58, 72);
    [SerializeField] float amplitude = 0.5f;

    float offset;

    void Start()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        ActivityProxy.VolumeDownHandler = () => {
            offset -= amplitude;
            return true;
        };
        ActivityProxy.VolumeUpHandler = () => {
            offset += amplitude;
            return true;
        };
#endif
    }

    void Update()
    {
        if (offset != 0)
        {
            float pupilDistance = pupilDistanceManager.PupilDistance;
            pupilDistance += offset;
            pupilDistance = Mathf.Clamp(pupilDistance, maxRange.x, maxRange.y);
            pupilDistanceManager.PupilDistance = pupilDistance;

            offset = 0;
        }
    }
}
