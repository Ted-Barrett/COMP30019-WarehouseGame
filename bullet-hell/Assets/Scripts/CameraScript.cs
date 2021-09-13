using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Makes camera look at object from a certain distance and with a certain angle.
/// </summary>
public class CameraScript : MonoBehaviour
{
    public float angleDeg; // x angle of camera
    private float _angleRad; // derived value
    
    public double dist; // distance of camera from target

    public Transform lookAt; // object to look at
    private Vector3 target;

    void Update()
    {
        target = lookAt.position;
        _angleRad = (float) (angleDeg * Math.PI / 180);
        Transform thisTransform = this.transform;
        thisTransform.position = new Vector3(0, (float) (Math.Sin(_angleRad)*dist),(float) (-Math.Cos(_angleRad)*dist)) + target;
        thisTransform.eulerAngles = new Vector3(angleDeg, 0, 0);
    }
}
