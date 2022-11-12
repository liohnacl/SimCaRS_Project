using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class SpawnManager : MonoBehaviour
{

    RoadSpawner roadSpawner;
    // Start is called before the first frame update
    void Start()
    {
        roadSpawner = GetComponent<RoadSpawner>();
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SpawnTriggerEntered(){
        roadSpawner.MoveRoad();
        
    }
}
