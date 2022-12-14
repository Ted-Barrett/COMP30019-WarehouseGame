using System;
using UnityEngine;

/// <summary>
/// Makes camera look at object from a certain distance and with a certain angle.
/// </summary>
public class CameraScript : MonoBehaviour
{
    [SerializeField]
    private float angleDeg; // x angle of camera

    [SerializeField]
    private float dist; // distance of camera from target

    [SerializeField]
    private Transform lookAt; // object to look at

    private float shakeDuration = 0f;

    [SerializeField]
    private float maxShakeDuration = 0.5f;

    [SerializeField]
    private float shakeAmount = 0.4f;

    // uses late update so that if tracking something
    // the camera pos will be updated after the object pos
    void LateUpdate()
    {
        Vector3 target = lookAt.position;

        // angle in radians
        float angleRad = (float) (angleDeg * Math.PI / 180);

        Transform thisTransform = this.transform;
        thisTransform.position = new Vector3(0, (float) (Math.Sin(angleRad)*dist),(float) (-Math.Cos(angleRad)*dist)) + target;
        thisTransform.eulerAngles = new Vector3(angleDeg, 0, 0);

        if (shakeDuration > 0)
        {
            this.transform.position += UnityEngine.Random.insideUnitSphere * shakeAmount;
            shakeDuration -= Time.deltaTime;
        }
        else
        {
            shakeDuration = 0f;
        }
    }

    public void Shake()
    {
        shakeDuration = maxShakeDuration;
    }
}
