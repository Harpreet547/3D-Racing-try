using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {

    public string steerAxis = "Horizontal";
    public string throttleAxis = "Vertical";

    public float throttleInput { get; private set; }
    public float steerInput { get; private set; }
    public bool shiftUp { get; private set; }
    public bool shiftDown { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        throttleInput = Input.GetAxis("Vertical");
        steerInput = Input.GetAxis("Horizontal");
        shiftUp = Input.GetButtonDown("ShiftUp");
        shiftDown = Input.GetButtonDown("ShiftDown");
    }
}
