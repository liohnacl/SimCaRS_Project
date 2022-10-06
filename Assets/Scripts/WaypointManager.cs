using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    public Transform agent;
    // Update is called once per frame
    void FixedUpdate()
    {
        Transform agent = GetComponent<Transform>();
        this.gameObject.transform.position = agent.position + agent.TransformDirection(new Vector3(0,0,0.85f));
    }
}
