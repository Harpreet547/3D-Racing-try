using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcadeCarController : MonoBehaviour {

    public float torque = 500f;
    public float brakeTorque = 250f;
    public float turnFactor = 20;
    public float driftFactor = 0.9f;
    public GameObject centerOfMass;

    private Rigidbody rb;
    private InputController inputController;
    private float acceleration;
    private float lastVelocity;
    float rotationAngle = 0;

    private void Start() {
        rb = GetComponent<Rigidbody>();
        inputController = GameManager.instance.inputController;
    }

    private void Update() {
        
    }

    private void FixedUpdate() {
        AddAcceleration();
        AddSteering();
        KillOrthogonalVelocity();
        SetAcceleration();
    }

    private void AddAcceleration() {
        float throttle = torque * inputController.throttleInput;

        float velocityForward = Vector3.Dot(transform.forward, rb.velocity);

        rb.AddForceAtPosition(transform.forward * throttle, centerOfMass.transform.position);
        // rb.AddForce();
    }

    private void SetAcceleration() {
        acceleration = Mathf.Abs((rb.velocity.x - lastVelocity) / Time.deltaTime); //Mathf.Abs((Vector3.Dot(rb.velocity, transform.right) - lastVelocity) / Time.deltaTime);
        lastVelocity =  rb.velocity.x; // Vector3.Dot(rb.velocity, transform.right);
    }

    void KillOrthogonalVelocity() {
        float forwardVelocity = Vector3.Dot(rb.velocity, transform.forward);
        float rightVelocity = Vector3.Dot(rb.velocity, transform.right);
        // Debug.Log(-rightVelocity * driftFactor);
        // rb.velocity = new Vector3(rightVelocity * driftFactor, rb.velocity.y, rb.velocity.z);
        // rb.AddForce(transform.right * -rightVelocity * driftFactor * 5000);
        // rb.AddForce(transform.right * (rb.mass * acceleration) * driftFactor);

        Debug.Log(acceleration * rb.mass);
        float velocity = rb.velocity.x;
        // rb.AddRelativeForce(transform.right * -1 * (acceleration * rb.mass) * driftFactor);
        // rb.velocity = forwardVelocity + rightVelocity * driftFactor;
    }

    private void AddSteering() {
        rotationAngle = inputController.steerInput * turnFactor;
        // Quaternion deltaRotation = Quaternion.Euler(0, rotationAngle, 0);
        // Debug.Log(rotationAngle);
        rb.AddRelativeTorque(transform.up * rotationAngle);
    }
}
