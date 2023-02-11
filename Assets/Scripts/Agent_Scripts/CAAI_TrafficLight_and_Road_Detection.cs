using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEditor;

public class CAAI_TrafficLight_and_Road_Detection : Agent {

    // [SerializeField] private TrackCheckpoints trackCheckpoints;

    [SerializeField] private Transform spawnPosition;

    //private CarAgent carAgent;
    private Player player;
    private Rigidbody rb;


    private void Awake() {
        //carAgent = GetComponent<CarAgent>();
        player = GetComponent<Player>();
    }

    private void Start() {
      rb = GetComponent<Rigidbody>();
       //     trackCheckpoints.OnCarCorrectCheckpoint += TrackCheckpoints_OnCarCorrectCheckpoint;
    //     trackCheckpoints.OnCarWrongCheckpoint += TrackCheckpoints_OnCarWrongCheckpoint;
   }

    // private void TrackCheckpoints_OnCarWrongCheckpoint(object sender, TrackCheckpoints.CarCheckpointEventArgs e) {
    //     if (e.carTransform == transform) {
    //         AddReward(-0.2f * rb.velocity.magnitude);
    //         EndEpisode();
    //     }
    // }

    // private void TrackCheckpoints_OnCarCorrectCheckpoint(object sender, TrackCheckpoints.CarCheckpointEventArgs e) {
    //     if (e.carTransform == transform) {
    //         AddReward(0.2f * rb.velocity.magnitude);
    //     }
    // }

    public override void OnEpisodeBegin() {
        transform.position = spawnPosition.position;
        transform.forward = spawnPosition.forward;
      
        player.StopCompletely();
        //Debug.Log("Reward: " + GetCumulativeReward());
    }

    public override void CollectObservations(VectorSensor sensor) {
        // Vector3 checkpointForward = transform.forward;
        // float directionDot = Vector3.Dot(transform.forward, checkpointForward);
        // sensor.AddObservation(directionDot);

        sensor.AddObservation(rb.velocity.magnitude);
        if (rb.velocity.magnitude > 1f)
        {
            AddReward(0.05f);
        }
    }

    public override void OnActionReceived(ActionBuffers actions) {
        //Debug.Log(actions.ContinuousActions[1]);
        float forwardAmount = 0f;
        float turnAmount = 0f;
        float isBraking = 0f;

        switch (actions.DiscreteActions[0])
        {
            case 0: forwardAmount = 0f; break;
            case 1: forwardAmount = +1f; break;
            case 2: isBraking = +1f; break;
            case 3: forwardAmount = -1f; break;
        }
        switch (actions.DiscreteActions[1])
        {
            case 0: turnAmount = 0f; break;
            case 1: turnAmount = +1f; break;
            case 2: turnAmount = -1f; break;
        }

        player.SetInputs(forwardAmount, turnAmount, isBraking);


    }

    public override void Heuristic(in ActionBuffers actionsOut) {
        int forwardAction = 0;
        if (Input.GetKey(KeyCode.UpArrow)) forwardAction = 1;
        if (Input.GetKey(KeyCode.DownArrow)) forwardAction = 2;

        int turnAction = 0;
        if (Input.GetKey(KeyCode.RightArrow)) turnAction = 1;
        if (Input.GetKey(KeyCode.LeftArrow)) turnAction = 2;

        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
        discreteActions[0] = forwardAction;
        discreteActions[1] = turnAction;
    }

    private void FixedUpdate(){
    RaycastHit hit;
    if (Physics.Raycast(transform.position, transform.forward, out hit, 50.0f))
   {
        // Color color = hit.transform.gameobject.GetComponent<Lights>().material.color;
        // print("Object colour: " + color);
        Debug.Log("Hit Traffic Light");
   }

    }

    private void OnTriggerEnter(Collider other) {
        float speed = rb.velocity.magnitude;
        if (other.gameObject.tag == "RoadLane") {
            // Hit a Road lane
            AddReward(-0.2f * speed);
            EndEpisode();
            Debug.Log("Hit Road Lane");
            
        }
        else if (other.gameObject.tag == "Goal") {
            // Hit a Goal
            AddReward(0.2f);
            EndEpisode();
           Debug.Log("Goal");   
            
        }

        else if (other.gameObject.tag == "TrafficlightboxRed"){
            AddReward(-0.2f * speed);
            Debug.Log("Stop");
        }
        else if (other.gameObject.tag == "TrafficlightboxYellow"){
            AddReward(-0.2f * speed);
            Debug.Log("Listen");
        }
        else if (other.gameObject.tag == "TrafficlightboxGreen"){
            AddReward(0.2f * speed);
            Debug.Log("Go");
        }
   
    }

    
}