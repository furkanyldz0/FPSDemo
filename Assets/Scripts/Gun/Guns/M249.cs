using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M249 : MonoBehaviour, IGun {
    public float RELOAD_TIME { 
        get {
            return 2.4f;
        }
    }
    public float RATE_OF_FIRE {
        get {
            return 0.07f;
        }
    }
}
