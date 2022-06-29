using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiRollBars : MonoBehaviour {

    public WheelCollider wheelColliderLeft;
    public WheelCollider wheelColliderRight;
    public float antiRoll = 5000.0f;

    private Rigidbody rb;
    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        float travelLeft = TravelCalculation(wheelColliderLeft);
        float travelRight = TravelCalculation(wheelColliderRight);
    
        float antiRollForce = (travelLeft - travelRight) * antiRoll;

        AddForceToWheel(wheelColliderLeft, -antiRollForce);
        AddForceToWheel(wheelColliderRight, antiRollForce);
    }

    void AddForceToWheel(WheelCollider wheelCollider, float antiRollForce) {
        if(wheelCollider.GetGroundHit(out WheelHit hit)) {
            rb.AddForceAtPosition(wheelCollider.transform.up * antiRollForce, wheelCollider.transform.position);
        }
    }

    float TravelCalculation(WheelCollider wheelCollider) {
        bool grounded = wheelCollider.GetGroundHit(out WheelHit hit);
        if(grounded) return (-wheelCollider.transform.InverseTransformPoint(hit.point).y - wheelCollider.radius) / wheelCollider.suspensionDistance;
        else return 1.0f;
    }
}
