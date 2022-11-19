using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedWaypoint : MonoBehaviour
{
    public PedWaypoint previousWaypoint;
    public PedWaypoint nextWaypoint;

    [Range(0f, 5f)]
    public float width = 1f;
    public List<PedWaypoint> branches = new List<PedWaypoint>();

    [Range(0f, 1f)]
    public float branchRatio = 0.5f;


    public Vector3 GetPosition()
    {
        Vector3 minBound = transform.position + transform.right * width / 2f;
        Vector3 maxBound = transform.position - transform.right * width / 2f;

        return Vector3.Lerp(minBound, maxBound, Random.Range(0f, 1f));
    }
}
