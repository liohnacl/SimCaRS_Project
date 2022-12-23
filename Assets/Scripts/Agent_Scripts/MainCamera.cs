using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    private Transform carAgent;

    private float yOffset = 5f;
    private float zOffset = 0.5f;
    private float xOffset = -15f;

    // Start is called before the first frame update
    void Start()
    {
        carAgent = GameObject.Find("CarAgent").transform;
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = new Vector3(carAgent.position.x + xOffset, carAgent.position.y + yOffset, carAgent.position.z + zOffset);
        
    }
}
