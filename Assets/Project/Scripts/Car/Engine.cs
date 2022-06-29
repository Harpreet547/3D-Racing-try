using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine : MonoBehaviour {
    public AnimationCurve torqueCurve;
    public float[] gears;
    public float smoothTime = 0.2f;
    public float lerpSmoothTime = 0.2f;
    public float finalDrive = 3.4f;
    public float maxRPM = 5000f;

    [HideInInspector]public bool engineLerp = false;
    private float engineLerpValue;

    private Wheel[] wheels;
    private float engineRPM = 0;
    private int currentGear = 0;
    private float totalPower;
    private float velocity = 0;
    private float KPH;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start() {
        wheels = GetComponentsInChildren<Wheel>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update() {
        ShiftGear();
    }

    public float CalculatePower(float throttle) {
        if(gears.Length == 0) return 0.0f;

        lerpEngine();

        float wheelRPM = GetWheelRPM();

        // engineRPM = Mathf.SmoothDamp(engineRPM, 1000 + (Mathf.Abs(wheelRPM) * 3.6f * gears[currentGear]), ref velocity, smoothTime);
        // totalPower = torqueCurve.Evaluate(engineRPM) * gears[currentGear] * throttle;

        if (engineRPM >= maxRPM){
            setEngineLerp(maxRPM - 1000);
        }
        if(!engineLerp){
            engineRPM = Mathf.SmoothDamp(engineRPM, 1000 + (Mathf.Abs(wheelRPM) *  finalDrive *  (gears[currentGear])), ref velocity , smoothTime );
            totalPower = torqueCurve.Evaluate(engineRPM) * (gears[currentGear]) * finalDrive * throttle ; // Final drive should not be here
        }
        KPH = rb.velocity.magnitude * 3.6f;

        // Debug.Log("Engine RPM: " + engineRPM + " totalPower: " + totalPower + " Velocity: " + velocity);
        return totalPower;
    }

    private float GetWheelRPM() {
        if(wheels.Length == 0) return 0;

        float sum = 0;
        int wheelsConnectedToEngine = 0;
        foreach(var wheel in wheels) {
            if(wheel.power) {
                sum += wheel.GetWheelRPM();
                wheelsConnectedToEngine++;
            }
        }
        // Debug.Log(wheels[0].GetWheelRPM() + "  :  " + sum / wheelsConnectedToEngine);

        return sum / wheelsConnectedToEngine;
    }

    void ShiftGear() {
        bool shiftUP = GameManager.instance.inputController.shiftUp;
        bool shiftDown = GameManager.instance.inputController.shiftDown;

        if(shiftUP && currentGear < gears.Length - 1) {
            currentGear ++;
            return;
        }

        if(shiftDown && currentGear > 0) {
            currentGear--;
        }
    }

    private void setEngineLerp(float num){
        engineLerp = true;
        engineLerpValue = num;
    }

    public void lerpEngine(){
        if(engineLerp){
            engineRPM = Mathf.Lerp(engineRPM,engineLerpValue,lerpSmoothTime * Time.deltaTime );
            //engineRPM = Mathf.SmoothDamp(engineRPM,engineLerpValue,ref velocity, lerpSmoothTime * Time.deltaTime );
            engineLerp = engineRPM <= engineLerpValue + 100 ? false : true;
        }
    }
}
