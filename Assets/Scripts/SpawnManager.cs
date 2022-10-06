using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class SpawnManager : MonoBehaviour
{
    public NavMeshSurface surface;

    RoadSpawner roadSpawner;
    // Start is called before the first frame update
    void Start()
    {
        roadSpawner = GetComponent<RoadSpawner>();
        

    }

    // Update is called once per frame
    void Update()
    {
        surface.BuildNavMesh();
    }
    public void SpawnTriggerEntered(){
        roadSpawner.MoveRoad();
        
    }
}
