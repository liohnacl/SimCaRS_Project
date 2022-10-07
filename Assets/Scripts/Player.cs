using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public SpawnManager spawnManager;
    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";

    private float horizontalInput;
    private float verticalInput;
    private float currentSteerAngle;
    private float currentbreakForce;
    private bool isBreaking;

    [SerializeField] private float motorForce;
    [SerializeField] private float breakForce;
    [SerializeField] private float maxSteerAngle;

    [SerializeField] private WheelCollider Front_Left_Collider;
    [SerializeField] private WheelCollider Front_Right_Collider;
    [SerializeField] private WheelCollider Rear_Left_Collider;
    [SerializeField] private WheelCollider Rear_Right_Collider;

    [SerializeField] private Transform Front_Left_Wheel;
    [SerializeField] private Transform Front_Right_Wheel;
    [SerializeField] private Transform Rear_Left_Wheel;
    [SerializeField] private Transform Rear_Right_Wheel;

    private void FixedUpdate()
    {
        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
        
    }


    private void GetInput()
    {
        // if(this.gameObject.transform.position.x>RoadSpawner.leftSide || this.gameObject.transform.position.x<RoadSpawner.rightSide){
        horizontalInput = Input.GetAxis(HORIZONTAL);
        verticalInput = Input.GetAxis(VERTICAL);
        isBreaking = Input.GetKey(KeyCode.Space);
    }

    private void HandleMotor()
    {
        Front_Left_Collider.motorTorque = verticalInput * motorForce;
        Front_Right_Collider.motorTorque = verticalInput * motorForce;
        currentbreakForce = isBreaking ? breakForce : 0f;
        ApplyBreaking();       
    }

    private void ApplyBreaking()
    {
        Front_Right_Collider.brakeTorque = currentbreakForce;
        Front_Left_Collider.brakeTorque = currentbreakForce;
        Rear_Left_Collider.brakeTorque = currentbreakForce;
        Rear_Right_Collider.brakeTorque = currentbreakForce;
    }

    private void HandleSteering()
    {
        currentSteerAngle = maxSteerAngle * horizontalInput;
        Front_Left_Collider.steerAngle = currentSteerAngle;
        Front_Right_Collider.steerAngle = currentSteerAngle;
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(Front_Left_Collider, Front_Left_Wheel);
        UpdateSingleWheel(Front_Right_Collider, Front_Right_Wheel);
        UpdateSingleWheel(Rear_Right_Collider, Rear_Right_Wheel);
        UpdateSingleWheel(Rear_Left_Collider, Rear_Left_Wheel);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;       
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        // pos.x= Mathf.Clamp(pos.x + horizontalInput, -0.2f, 9.3f);
        wheelTransform.position = pos;
    }
    public void OnTriggerEnter(Collider other){
        spawnManager.SpawnTriggerEntered();
    }
}
