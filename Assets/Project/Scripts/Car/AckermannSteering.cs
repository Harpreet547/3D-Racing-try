using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AckermannSteering : MonoBehaviour {
    // Keeping these variables in this script so that in future if car has more than 1 steering axis it can be added multiple times.
    [Header("Steering setup")]
    public bool isUsingPredefinedValues = false;
    public float turningRadius;
    public float wheelbase;
    public float rearTrack;

    [Header("Wheels: Only use if you want script to calculate rear track and wheelbase")]
    public Wheel frontLeft;
    public Wheel frontRight;
    public Wheel rearLeft;
    public Wheel rearRight;

    private InputController inputController;

    void Start() {
        inputController = GameManager.instance.inputController;
    }

    void Update() {
        float _rearTrack = isUsingPredefinedValues ? rearTrack : GetRearTrack();
        float _wheelbase = isUsingPredefinedValues ? wheelbase : GetWheelbase();

        float steerAngle = inputController.steerInput;
        if (steerAngle > 0 ) {
		    //rear tracks size is set to 1.5f       wheel base has been set to 2.55f
            frontLeft.steerAngle = Mathf.Rad2Deg * Mathf.Atan(_wheelbase / (turningRadius + (_rearTrack / 2))) * steerAngle;
            frontRight.steerAngle = Mathf.Rad2Deg * Mathf.Atan(_wheelbase / (turningRadius - (_rearTrack / 2))) * steerAngle;
        } else if (steerAngle < 0 ) {                                                          
            frontLeft.steerAngle = Mathf.Rad2Deg * Mathf.Atan(_wheelbase / (turningRadius - (_rearTrack / 2))) * steerAngle;
            frontRight.steerAngle = Mathf.Rad2Deg * Mathf.Atan(_wheelbase / (turningRadius + (_rearTrack / 2))) * steerAngle;

        } else {
            frontLeft.steerAngle = 0;
            frontRight.steerAngle = 0;
        }
    }

    private float GetWheelbase() {
        return Mathf.Abs(Vector3.Distance(frontLeft.transform.position, rearLeft.transform.position));
    }

    private float GetRearTrack() {
        return Mathf.Abs(Vector3.Distance(rearLeft.transform.position, rearRight.transform.position));
    }
}
