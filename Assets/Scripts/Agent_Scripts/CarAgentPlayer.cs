using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAgentPlayer : MonoBehaviour {

    private CarAgent carAgent;

    private void Awake() {
        carAgent = GetComponent<CarAgent>();
    }

    private void Update() {
        float forwardAmount = Input.GetAxisRaw("Vertical");
        float turnAmount = Input.GetAxisRaw("Horizontal");
        carAgent.SetInputs(forwardAmount, turnAmount);
    }

}
