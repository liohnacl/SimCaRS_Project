using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCameraController : MonoBehaviour
{
    private Transform player;

    private float yOffset = 25f;
    private float zOffset = -60f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("CarDriver").transform;
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = new Vector3(player.position.x, player.position.y + yOffset, player.position.z + zOffset);
        
    }
}
