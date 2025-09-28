using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRouteAlgorithm
{
    void Route(Transform origin, Transform destination, ref List<Transform> path);
}
