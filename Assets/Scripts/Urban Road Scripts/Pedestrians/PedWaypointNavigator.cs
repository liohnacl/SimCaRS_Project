// using RoadArchitect;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using System.ComponentModel;
using UnityEngine;

public class PedWaypointNavigator : MonoBehaviour
{
    PedestrianController controller;
    public PedWaypoint currentWaypoint;
    float direction;

    private void Awake()
    {
        controller = GetComponent<PedestrianController>();
    }


    // Start is called before the first frame update
    void Start()
    {
        //  controller = GetComponent<PedestrianController>();
        controller.SetDestination(currentWaypoint.GetPosition());
        direction = Mathf.RoundToInt(UnityEngine.Random.Range(0, 1));
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.reachedDestination)
        {
            bool shouldBranch = false;

            if(currentWaypoint.branches != null && currentWaypoint.branches.Count > 0)
            {
                shouldBranch = Random.Range(0f, 1f) <= currentWaypoint.branchRatio ? true : false;
            }

            if (shouldBranch)
            {
                currentWaypoint = currentWaypoint.branches[Random.Range(0, currentWaypoint.branches.Count - 1)];
            }
            else
            {
                if(direction == 0)
                {
                    if (currentWaypoint.nextWaypoint != null)
                    {
                        currentWaypoint = currentWaypoint.nextWaypoint;
                    }
                    else
                    {
                        currentWaypoint = currentWaypoint.previousWaypoint;
                        direction=1;
                    }
                }
                else if(direction == 1)
                {
                    if (currentWaypoint.previousWaypoint != null)
                    {
                        currentWaypoint = currentWaypoint.previousWaypoint;
                    }
                    else
                    {
                        currentWaypoint = currentWaypoint.nextWaypoint;
                        direction=0;
                    }
                    
                }
            }
            
            controller.SetDestination(currentWaypoint.GetPosition());
        }
            
    }
}
