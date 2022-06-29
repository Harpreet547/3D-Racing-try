using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour {

    public bool steer;
    public bool invertSteer;
    public bool power;

    public float steerAngle { get; set; }
    public float torque { get; set; }

    private WheelCollider wheelCollider;
    private Transform wheelTransform;
    private TrailRenderer trailRenderer;

    // Start is called before the first frame update
    void Start() {
        wheelCollider = GetComponentInChildren<WheelCollider>();
        wheelTransform = GetComponentInChildren<MeshRenderer>().GetComponent<Transform>();
        trailRenderer = GetComponentInChildren<TrailRenderer>();
    }

    // Update is called once per frame
    void Update() {
        UpdateWheel(wheelCollider, wheelTransform);
    }

    private void FixedUpdate() {
        if(steer) {
            wheelCollider.steerAngle = steerAngle * (invertSteer ? -1 : 1);
        }
        if(power) {
            wheelCollider.motorTorque = torque;
        }

        SetDriftEffects();
    }

    void UpdateWheel(WheelCollider col, Transform wheelTransform) {
        col.GetWorldPose(out Vector3 position, out Quaternion rotation);
        wheelTransform.position = position;
        wheelTransform.rotation = rotation;
    }

    void SetDriftEffects() {
        if(CheckIfWheelSlipping()) StartTrailMarks();
        else StopTrailMarks();
    }

    void StartTrailMarks() {
        if(trailRenderer == null) return;
        trailRenderer.emitting = true;
    }

    void StopTrailMarks() {
        if(trailRenderer == null) return;
        trailRenderer.emitting = false;
    }

    public bool CheckIfWheelSlipping() {
        bool isSlipping = false;
        if(wheelCollider.GetGroundHit(out WheelHit hit)) {
            isSlipping = hit.forwardSlip > 0.5f;
        }
        return isSlipping;
    }

    public float GetWheelRPM() {
        return wheelCollider.rpm;
    }

    public float GetFowardSlip() {
        wheelCollider.GetGroundHit(out WheelHit hit);
        return hit.forwardSlip;
    }
}
