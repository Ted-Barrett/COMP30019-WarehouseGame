using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour {
    private float speed;
    private float rotationSpeed; // Degrees per second
    private Vector3 direction;

    public void Initialise(float speed, float rotationSpeed, Vector3 direction) {
        this.speed = speed;
        this.rotationSpeed = rotationSpeed;
        this.direction = direction.normalized;

        // Point in the direction we're heading
        Quaternion rotationChange = Quaternion.FromToRotation(this.transform.forward, this.direction);
        this.transform.localRotation *= rotationChange;
    }

    private void FixedUpdate() {
        this.transform.position += this.speed * Time.fixedDeltaTime * this.direction;
        transform.Rotate(Vector3.right, rotationSpeed * Time.fixedDeltaTime, Space.Self);
    }

    private void OnTriggerEnter(Collider other) {
        Destroy(other.gameObject);
        Destroy(gameObject);
    }
}
