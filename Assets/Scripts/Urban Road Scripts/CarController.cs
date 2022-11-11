using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public SpawnManager spawnManager;
    public WaypointNavigator waypointNavigator;
    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";

    private float horizontalInput;
    private float verticalInput;
    private float currentSteerAngle;
    private float currentbreakForce;
    private bool isBreaking;

    
    public bool reachedDestination = false;
    
    private float rotationSpeed = 100f, movementSpeed = 1f;

    [SerializeField] private float stopDistance;
    [SerializeField] private Vector3 destination;

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
        //HandleSteering();
        UpdateWheels();
        HandleDestination();
        speedLimit();
        
        

    }

    public void speedLimit()
    {
        Rigidbody body = GetComponent<Rigidbody>();
        body.velocity = Vector3.ClampMagnitude(body.velocity, 2);
    }

    public void HandleDestination()
    {
        if (transform.position != destination)
        {
            Vector3 destinationDirection = destination - transform.position;
            destinationDirection.y = 0;

            float destinationDistance = destinationDirection.magnitude;

            if (destinationDistance >= stopDistance)
            {
                reachedDestination = false;
                Quaternion targetRotation = Quaternion.LookRotation(destinationDirection);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);

            }
            else
            {
                reachedDestination = true;
            }

            //Vehicle moves forward towards the destination
            HandleMotor(1);
            ApplyBreaking(0);
            
        }
    }

    public void SetDestination(Vector3 destination)
    {
        this.destination = destination;
        reachedDestination = false;
    }

    private void HandleMotor(float power)
    {
        Front_Left_Collider.motorTorque = power * motorForce;
        Front_Right_Collider.motorTorque = power * motorForce;

    }

    private void ApplyBreaking(float power)
    {
        Front_Right_Collider.brakeTorque = currentbreakForce * power;
        Front_Left_Collider.brakeTorque = currentbreakForce * power;
        Rear_Left_Collider.brakeTorque = currentbreakForce * power;
        Rear_Right_Collider.brakeTorque = currentbreakForce * power;
    }

    public void HandleSteering(Vector3 steer)
    {
        currentSteerAngle = maxSteerAngle * 0;
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
    public void OnTriggerEnter(Collider other)
    {
        spawnManager.SpawnTriggerEntered();
    }

}
