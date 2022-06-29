using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Differential : MonoBehaviour {

    public Wheel leftWheel;
    public Wheel rightWheel;
    [Range(-1, 1)]
    public float maxSlip = 0;
    public float maxTrans = 0;

    [HideInInspector]
    public float torque = 0;

    void Start() {
        
    }

    void Update() {
        float leftWheelSlip = leftWheel.GetFowardSlip();
        float rightWheelSlip = rightWheel.GetFowardSlip();
        DiffOutput(slipLH: leftWheelSlip, slipRH: rightWheelSlip);
    }

    private void DiffOutput(float slipLH, float slipRH) {
        float[] output = new float[2];
        float slipDifferential = slipRH - slipLH;
        float singleWheelBaseTorque = torque; // 0.5f * 
        float torqueTransfer = 0.0f;
 
        if (maxSlip != 0.0f)
        {
            if (Mathf.Abs(slipDifferential) <= Mathf.Abs(maxSlip))
            {
                torqueTransfer = slipDifferential / maxSlip;
            }
            else
            {
                torqueTransfer = 1.0f * Mathf.Sign(slipDifferential) * Mathf.Sign(maxSlip);
            }
        }
        if(Mathf.Abs(torqueTransfer) > Mathf.Abs(maxTrans))
        {
            torqueTransfer = maxTrans * Mathf.Sign(torqueTransfer);
        }
        float torqueAdjustment = singleWheelBaseTorque * torqueTransfer;
        output[0] = singleWheelBaseTorque - torqueAdjustment;
        output[1] = singleWheelBaseTorque + torqueAdjustment;
        if(output[0] != 0) Debug.Log("Right: " + output[1] + "  Left:  " + output[0]);
        // Debug.Log("Right: " + slipRH + "   Left: " + slipLH);

        leftWheel.torque = output[0];
        rightWheel.torque = output[1];
        // return output;
    }
}
