using UnityEngine;
using UnityEngine.Serialization;

public class TransformAnimation : MonoBehaviour
{
    [FormerlySerializedAs("curve")] [SerializeField] AnimationCurve heightAnimation;
    [SerializeField] float rotationAnimation;

    new Transform transform;
    Vector3 initialPosition;

    void Start()
    {
        transform = base.transform;
        initialPosition = transform.localPosition;
    }

    void Update()
    {
        Vector3 position = initialPosition;
        position.y += heightAnimation.Evaluate(Time.time);
        transform.localPosition = position;

        transform.rotation *= Quaternion.AngleAxis(rotationAnimation * Time.deltaTime, Vector3.up);
    }
}
