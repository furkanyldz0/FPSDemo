using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M1911 : MonoBehaviour, IGun
{
    public float RELOAD_TIME {
        get {
            return 1.5f;
        }
    }

    public float RATE_OF_FIRE {
        get {
            return .3f;
        }
    }
}
