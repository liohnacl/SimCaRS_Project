using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class CarAgentAI : Agent {

    [SerializeField] private TrackCheckpoints trackCheckpoints;
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
        trackCheckpoints.OnCarCorrectCheckpoint += TrackCheckpoints_OnCarCorrectCheckpoint;
        trackCheckpoints.OnCarWrongCheckpoint += TrackCheckpoints_OnCarWrongCheckpoint;
    }

    private void TrackCheckpoints_OnCarWrongCheckpoint(object sender, TrackCheckpoints.CarCheckpointEventArgs e) {
        if (e.carTransform == transform) {
            AddReward(-1f);
        }
    }

    private void TrackCheckpoints_OnCarCorrectCheckpoint(object sender, TrackCheckpoints.CarCheckpointEventArgs e) {
        if (e.carTransform == transform) {
            AddReward(1f);
        }
    }

    public override void OnEpisodeBegin() {
        transform.position = spawnPosition.position;
        transform.forward = spawnPosition.forward;
        trackCheckpoints.ResetCheckpoints();
        player.StopCompletely();
        //Debug.Log("Reward: " + GetCumulativeReward());
    }

    public override void CollectObservations(VectorSensor sensor) {
        Vector3 checkpointForward = trackCheckpoints.GetNextCheckpoint(transform).transform.forward;
        float directionDot = Vector3.Dot(transform.forward, checkpointForward);
        sensor.AddObservation(directionDot);
    }

    public override void OnActionReceived(ActionBuffers actions) {
        //Debug.Log(actions.ContinuousActions[1]);
        float forwardAmount = 0f;
        float turnAmount = 0f;
        float isBraking = 0f;

        AddReward(-0.01f);

        switch (actions.DiscreteActions[0])
        {
            case 0: forwardAmount = 0f; break;
            case 1: forwardAmount = +1f; break;
            case 2: isBraking = +1f; break;
        }
        switch (actions.DiscreteActions[1])
        {
            case 0: turnAmount = 0f; break;
            case 1: turnAmount = +1f; break;
            case 2: turnAmount = -1f; break;
        }
        //if (actions.ContinuousActions[1] == 0)
        //{
        //    isBraking = 1f;
        //}
        //forwardAmount = actions.ContinuousActions[1];

        //turnAmount = actions.ContinuousActions[0];


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

        //ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        //continuousActions[0] = forwardAction;
        //continuousActions[0] = turnAction;
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Wall") {
            // Hit a Wall
            AddReward(-1);
            Debug.Log("Hit wall");
            //SetReward(0f);
            // EndEpisode();
            
        }
        else if(collision.gameObject.tag=="Civilian"){
            AddReward(-1);
            Debug.Log("Hit Civilian");
        }
        else if(collision.gameObject.tag=="Vehicle"){
            AddReward(-1);
            Debug.Log("Hit other vehicle");
        }
        else if(collision.gameObject.tag=="Trafficlight"){
            AddReward(-1);
            Debug.Log("Hit Traffic Light");
        }
        else if(collision.gameObject.tag=="Roadblock"){
            AddReward(-1);
            Debug.Log("Hit a Road Block");
        }

    }

    private void OnCollisionStay(Collision collision) {
        if (collision.gameObject.tag == "Wall") {
            // Hit a Wall
            AddReward(-1f);
            
        }
         else if(collision.gameObject.tag=="Civilian"){
            AddReward(-1);
            Debug.Log("Hit Civilian");
        }
        else if(collision.gameObject.tag=="Vehicle"){
            AddReward(-1);
            Debug.Log("Hit other vehicle");
        }
        else if(collision.gameObject.tag=="Trafficlight"){
            AddReward(-1);
            Debug.Log("Hit Traffic Light");
        }
        else if(collision.gameObject.tag=="Roadblock"){
            AddReward(-1);
            Debug.Log("Hit a Road Block");
        }

    }

    private void FixedUpdate()
    {
        
        if (rb.velocity.sqrMagnitude <= 0)
        {
            AddReward(-0.01f);
        }
    }

}
