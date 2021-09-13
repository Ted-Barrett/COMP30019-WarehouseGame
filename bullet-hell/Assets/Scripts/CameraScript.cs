using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Makes camera look at object from a certain distance and with a certain angle.
/// </summary>
public class CameraScript : MonoBehaviour
{
    [SerializeField]
    private float angleDeg; // x angle of camera

    [SerializeField]
    private double dist; // distance of camera from target

    [SerializeField]
    private Transform lookAt; // object to look at

    void Update()
    {
        Vector3 target = lookAt.position;

        // angle in radians
        float angleRad = (float) (angleDeg * Math.PI / 180);

        Transform thisTransform = this.transform;
        thisTransform.position = new Vector3(0, (float) (Math.Sin(angleRad)*dist),(float) (-Math.Cos(angleRad)*dist)) + target;
        thisTransform.eulerAngles = new Vector3(angleDeg, 0, 0);
    }
}
