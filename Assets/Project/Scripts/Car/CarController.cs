using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    // public float maxSteerAngle = 15f;
    public float brakeForce = 800f;

    public Transform centerOfMass;

    private Rigidbody rb;
    private Wheel[] wheels;
    private float steer { get; set; }
    private float throttle { get; set; }
    private float motorTorque;
    private Engine engine;

    // Experimental
    private Differential differential;

    // Start is called before the first frame update
    void Start() {
        wheels = GetComponentsInChildren<Wheel>();
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMass.localPosition;
        engine = GetComponent<Engine>();
        differential = GetComponent<Differential>();
    }

    private void FixedUpdate() {
        SetTorqueForSingleWheel();
    }

    private void Update() {

        // steer = GameManager.instance.inputController.steerInput;
        throttle = GameManager.instance.inputController.throttleInput;
        differential.torque = motorTorque * throttle;
        // foreach(var wheel in wheels) {
        //     // wheel.steerAngle = steer * maxSteerAngle;
        //     wheel.torque = motorTorque;
        //     // wheel.torque = throttle * motorTorque;
        // }
    }

    private void SetTorqueForSingleWheel() {
        motorTorque = engine.CalculatePower(throttle) / GetNumberOfPoweredWheels();
    }

    private int GetNumberOfPoweredWheels() {
        int numberOfPoweredWheels = 0;
        foreach(var wheel in wheels) {
            if(wheel.power) numberOfPoweredWheels++;
        }
        return numberOfPoweredWheels;
    }

}
