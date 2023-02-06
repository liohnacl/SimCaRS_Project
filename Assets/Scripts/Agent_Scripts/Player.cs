using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private const int SOLID_OBJECTS_LAYER = -1;

    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";

    private float speed;
    private float horizontalInput;
    private float verticalInput;
    private float currentSteerAngle;
    private float isBreaking;

    Rigidbody body;


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

    private void Update()
    {
        //SetInputs();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
        speedLimiter();
        ApplyBreaking();

    }

    public void speedLimiter()
    {
        body = GetComponent<Rigidbody>();
        body.velocity = Vector3.ClampMagnitude(body.velocity, 5f);
    }

    public void SetInputs(float forwardAmount, float turnAmount, float isBraking)
    {
        horizontalInput = turnAmount;
        verticalInput = forwardAmount;
        isBreaking = isBraking;
    }

    public void SetInputs()
    {
        verticalInput = Input.GetAxisRaw("Vertical");
        horizontalInput = Input.GetAxisRaw("Horizontal");
        if (Input.GetKeyDown(KeyCode.Space)) { isBreaking = 1; } else { isBreaking = 0; }


    }

    private void HandleMotor()
    {
        body = GetComponent<Rigidbody>();

        Front_Left_Collider.motorTorque = verticalInput * motorForce;
        Front_Right_Collider.motorTorque = verticalInput * motorForce;
        Rear_Left_Collider.motorTorque = verticalInput * motorForce;
        Rear_Right_Collider.motorTorque = verticalInput * motorForce;

        if (verticalInput == 0f)
        {
            Front_Right_Collider.brakeTorque = 1000f;
            Front_Left_Collider.brakeTorque =  1000f;
            Rear_Left_Collider.brakeTorque =   1000f;
            Rear_Right_Collider.brakeTorque =  1000f;
        }
               
    }

    private void ApplyBreaking()
    {
        Front_Right_Collider.brakeTorque = isBreaking * breakForce;
        Front_Left_Collider.brakeTorque = isBreaking * breakForce;
        Rear_Left_Collider.brakeTorque = isBreaking * breakForce;
        Rear_Right_Collider.brakeTorque = isBreaking * breakForce;
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

    public void StopCompletely()
    {
        Front_Left_Collider.motorTorque = 0f;
        Front_Right_Collider.motorTorque = 0f;
        Rear_Left_Collider.motorTorque = 0f;
        Rear_Right_Collider.motorTorque = 0f;
        currentSteerAngle = 0f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (SOLID_OBJECTS_LAYER != -1)
        {
            if (collision.gameObject.layer == SOLID_OBJECTS_LAYER)
            {
                speed = Mathf.Clamp(speed, 0f, 20f);
            }
        }
    }

}
