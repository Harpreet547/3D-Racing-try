using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance { get; private set; }
    public InputController inputController { get; private set;}
    private void Awake() {
        instance = this;
        inputController = GetComponentInChildren<InputController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
