using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SystemConfig : MonoBehaviour
{
    private void Awake()
    {
        Environment.SetEnvironmentVariable("PRODUCTION", "false");
    }

}
